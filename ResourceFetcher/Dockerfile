FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
LABEL maintainer="SimonGschnell"
RUN apk add --no-cache tzdata
WORKDIR /app
COPY ResourceFetcher/ ResourceFetcher/
COPY StremioAddonExample/ StremioAddonExample/
WORKDIR /app/ResourceFetcher
RUN dotnet restore
RUN dotnet publish -c Release -o /publish -p:ErrorOnDuplicatePublishOutputFiles=false
WORKDIR /publish
ENTRYPOINT ["dotnet", "ResourceFetcher.dll"]