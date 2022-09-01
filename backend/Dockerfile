FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /backend

# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore "./src/Library.Api/Library.Api.csproj"
RUN dotnet publish "./src/Library.Api/Library.Api.csproj" -c release -o /app --no-restore

# Unit Test
FROM build AS unittest
WORKDIR /backend/src/Library.UnitTests
CMD ["dotnet", "test"]

# Build runtime image
FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Library.Api.dll"]