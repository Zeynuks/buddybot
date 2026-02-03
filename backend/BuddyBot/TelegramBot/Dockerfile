FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8084
EXPOSE 8085

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/BuddyBot/TelegramBot/TelegramBot.csproj", "TelegramBot/"]
COPY ["backend/BuddyBot/Application/Application.csproj", "Application/"]
COPY ["backend/BuddyBot/Domain/Domain.csproj", "Domain/"]
COPY ["backend/BuddyBot/Contracts/Contracts.csproj", "Contracts/"]
COPY ["backend/BuddyBot/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "TelegramBot/TelegramBot.csproj"
COPY backend/BuddyBot/. .
WORKDIR "/src/TelegramBot"
RUN dotnet build "./TelegramBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TelegramBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramBot.dll"]
