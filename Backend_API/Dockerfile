# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Backend_API/*.csproj .
RUN dotnet restore
COPY Backend_API/. .
RUN dotnet publish  --os linux --arch x64 -c Release -o /app

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Backend_API.dll"]