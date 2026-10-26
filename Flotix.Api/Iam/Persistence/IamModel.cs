using Flotix.Api.Iam.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Iam.Persistence;

public static class IamModel
{
    public static ModelBuilder ApplyIamModel(this ModelBuilder model)
    {
        model.Entity<User>().HasIndex(u => u.Email).IsUnique();
        return model;
    }
}
