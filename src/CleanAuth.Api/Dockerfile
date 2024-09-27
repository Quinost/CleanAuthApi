FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CleanAuth.Api/CleanAuth.Api.csproj", "src/CleanAuth.Api/"]
RUN dotnet restore "src/CleanAuth.Api/CleanAuth.Api.csproj"
COPY . .
RUN dotnet publish "src/CleanAuth.Api/CleanAuth.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CleanAuth.Api.dll"]