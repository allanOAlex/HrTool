# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80

# Copy csproj and restore as distinct layers
COPY . ./

# Copy everything else and build
WORKDIR API/GSG.API
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/API/GSG.API/out .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "GSG.API.dll"]