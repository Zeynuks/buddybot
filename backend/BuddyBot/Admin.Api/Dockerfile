FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "backend/BuddyBot/Admin.Api/Admin.Api.csproj"
WORKDIR "/src/backend/BuddyBot/Admin.Api"
RUN dotnet publish "Admin.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Admin.Api.dll"]
