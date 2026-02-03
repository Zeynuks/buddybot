FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/BuddyBot/Scheduler/Scheduler.csproj", "Scheduler/"]
COPY ["backend/BuddyBot/Application/Application.csproj", "Application/"]
COPY ["backend/BuddyBot/Domain/Domain.csproj", "Domain/"]
COPY ["backend/BuddyBot/Contracts/Contracts.csproj", "Contracts/"]
COPY ["backend/BuddyBot/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Scheduler/Scheduler.csproj"
COPY backend/BuddyBot/. .
WORKDIR "/src/Scheduler"
RUN dotnet build "./Scheduler.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Scheduler.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scheduler.dll"]
