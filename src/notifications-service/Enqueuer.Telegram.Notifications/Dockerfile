# Setup
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Restore all dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore

# Build the application
FROM build AS publish
WORKDIR "/src/notifications-service/Enqueuer.Telegram.Notifications"
RUN dotnet publish --no-restore -c Release -o /app /p:UseAppHost=false

# Run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Enqueuer.Telegram.Notifications.dll"]