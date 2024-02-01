#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Enqueuer.Telegram.Notifications/Enqueuer.Telegram.Notifications.csproj", "src/Enqueuer.Telegram.Notifications/"]
RUN dotnet restore "./src/Enqueuer.Telegram.Notifications/./Enqueuer.Telegram.Notifications.csproj"
COPY . .
WORKDIR "/src/src/Enqueuer.Telegram.Notifications"
RUN dotnet build "./Enqueuer.Telegram.Notifications.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Enqueuer.Telegram.Notifications.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enqueuer.Telegram.Notifications.dll"]