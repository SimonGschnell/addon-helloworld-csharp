FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /app
COPY StremioAddonExample/ .
RUN dotnet restore
RUN dotnet publish -c Release -o /publish -p:ErrorOnDuplicatePublishOutputFiles=false
WORKDIR /publish
ENTRYPOINT ["dotnet", "StremioAddonExample.dll"]
