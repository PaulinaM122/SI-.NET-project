# Base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the solution file and restore dependefockerncies
COPY ["WindFarmMonitoring.sln", "./"]
COPY ["WindFarmMonitoring/WindFarmMonitoring.csproj", "WindFarmMonitoring/"]
RUN dotnet restore "WindFarmMonitoring/WindFarmMonitoring.csproj"

# Copy the entire project directory for build
COPY . .
WORKDIR "/src/WindFarmMonitoring"
RUN dotnet build "WindFarmMonitoring.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "WindFarmMonitoring.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WindFarmMonitoring.dll"]
