# Use official .NET SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app

# Copy the project files and restore dependencies
COPY ["Web.Api/Web.Api.csproj", "Web.Api/"]
RUN dotnet restore "Web.Api/Web.Api.csproj"

# Copy the rest of the application files and build the app
COPY . .
WORKDIR "/app/Web.Api"
RUN dotnet publish -c Release -o /app/out

# Use ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose port 8080
EXPOSE 8080

# Set environment variables to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
ENTRYPOINT ["dotnet", "Web.Api.dll", "--environment=Development"]