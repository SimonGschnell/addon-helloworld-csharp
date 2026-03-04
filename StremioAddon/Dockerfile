FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
LABEL maintainer="SimonGschnell"
WORKDIR /app
COPY StremioAddon/ .
RUN dotnet restore
RUN dotnet publish -c Release -o /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "StremioAddon.dll"]
