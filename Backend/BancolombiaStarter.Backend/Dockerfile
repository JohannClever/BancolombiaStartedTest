# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7112

# Use the .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["StackOverFlight.ErrorReport.Api/StackOverFlight.ErrorReport.Api.csproj", "StackOverFlight.ErrorReport.Api/"]
COPY ["StackOverFlight.ErrorReport.Infrastructure/StackOverFlight.ErrorReport.Infrastructure.csproj", "StackOverFlight.ErrorReport.Infrastructure/"]
COPY ["StackOverFlight.ErrorReport.Domain/StackOverFlight.ErrorReport.Domain.csproj", "StackOverFlight.ErrorReport.Domain/"]

RUN dotnet restore "StackOverFlight.ErrorReport.Api/StackOverFlight.ErrorReport.Api.csproj"

# Copy the rest of the application files
COPY . .

# Ensure the directory for the SQL file exists and copy it
RUN mkdir -p /app/Db/bin/Debug/Dbo/
COPY ["StackOverFlight.ErrorReport.Db/Dbo/StoreProcedures/Sp_Insert_Report.sql", "/app/Db/bin/Debug/Dbo/StoreProcedures/"]

# Set the working directory and build the application
WORKDIR "/src/StackOverFlight.ErrorReport.Api"
RUN dotnet build "StackOverFlight.ErrorReport.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "StackOverFlight.ErrorReport.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime base image and copy the published app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the timezone
ENV TZ=America/Bogota

# Set the DOTNET_RUNNING_IN_CONTAINER environment variable
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Start the application
ENTRYPOINT ["dotnet", "StackOverFlight.ErrorReport.Api.dll"]
