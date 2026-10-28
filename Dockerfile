FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /app
COPY FlotixPlatform.API/*.csproj FlotixPlatform.API/
RUN dotnet restore ./FlotixPlatform.API
COPY . .
RUN dotnet publish ./FlotixPlatform.API -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=builder /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FlotixPlatform.API.dll"]
