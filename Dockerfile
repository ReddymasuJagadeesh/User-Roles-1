# =========================================
# STAGE 1: BUILD (using .NET 10 SDK)
# =========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# =========================================
# STAGE 2: RUNTIME (using .NET 10 ASP.NET)
# =========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

# App will listen on port 8080 inside container
EXPOSE 8080

# Copy published output from build stage
COPY --from=build /app/publish .

# Environment variables for containers
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Start the application
ENTRYPOINT ["dotnet", "UserRoles.dll"]
