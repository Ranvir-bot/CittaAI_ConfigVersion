# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["ConfigurationVersioning.Api.csproj", "./"]

RUN dotnet restore "ConfigurationVersioning.Api.csproj"

COPY . .

RUN dotnet publish "ConfigurationVersioning.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

CMD ["sh", "-c", "dotnet ConfigurationVersioning.Api.dll --urls http://0.0.0.0:${PORT:-8080}"]