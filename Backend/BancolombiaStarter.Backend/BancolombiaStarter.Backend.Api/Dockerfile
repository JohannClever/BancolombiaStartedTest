# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
USER app
WORKDIR /app
EXPOSE 7112

# Use the .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY ["BancolombiaStarter.Backend.Api/BancolombiaStarter.Backend.Api.csproj", "BancolombiaStarter.Backend.Api/"]
COPY ["./BancolombiaStarter.Backend.Infrastructure/BancolombiaStarter.Backend.Infrastructure.csproj", "BancolombiaStarter.Backend.Infrastructure/"]
COPY ["./BancolombiaStarter.Backend.Domain/BancolombiaStarter.Backend.Domain.csproj", "BancolombiaStarter.Backend.Domain/"]

RUN dotnet restore "./BancolombiaStarter.Backend.Api/BancolombiaStarter.Backend.Api.csproj"



# Set the working directory and build the application
WORKDIR "/src/BancolombiaStarter.Backend.Api"
RUN dotnet build "./BancolombiaStarter.Backend.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BancolombiaStarter.Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime base image and copy the published app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the timezone
ENV TZ=America/Bogota

# Set the DOTNET_RUNNING_IN_CONTAINER environment variable
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Start the application
ENTRYPOINT ["dotnet", "BancolombiaStarter.Backend.Api.dll"]
