using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flotix.Api.Shared.Web;

/// <summary>
/// json-server style filters (<c>?vehicleId=v-1&amp;status=Abierto</c>): every query key that names a public
/// scalar property becomes an equality condition; unknown keys are ignored and values are always SQL parameters.
/// </summary>
internal static class QueryFilter
{
    public static Expression<Func<T, bool>>? FromQuery<T>(IQueryCollection query, JsonSerializerOptions options)
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        Expression? body = null;

        foreach (var (key, values) in query)
        {
            var property = typeof(T).GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property is null || property.IsDefined(typeof(JsonIgnoreAttribute))) continue;
            if (property.PropertyType != typeof(string) && !property.PropertyType.IsValueType) continue;

            Expression condition = TryConvert(values.ToString(), property.PropertyType, options, out var value)
                ? Expression.Equal(
                    Expression.Property(parameter, property),
                    Expression.Convert(Expression.Field(Expression.Constant(new StrongBox<object?>(value)), "Value"), property.PropertyType))
                : Expression.Constant(false);

            body = body is null ? condition : Expression.AndAlso(body, condition);
        }

        return body is null ? null : Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static bool TryConvert(string raw, Type type, JsonSerializerOptions options, out object? value)
    {
        value = raw;
        if (type == typeof(string)) return true;

        // Raw JSON first (numbers, booleans), then as a JSON string (enums, dates).
        foreach (var json in (string[])[raw, JsonSerializer.Serialize(raw)])
        {
            try
            {
                value = JsonSerializer.Deserialize(json, type, options);
                return true;
            }
            catch (JsonException)
            {
            }
        }

        return false;
    }
}
