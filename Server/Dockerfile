# Docs : https://aka.ms/customizecontainer

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER app
WORKDIR /app

# Cette phase est utilisée pour générer le projet de service
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Server.csproj", "."]
RUN dotnet restore "./Server.csproj"
COPY . .
RUN dotnet publish "./Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Cette phase est utilisée en production ou lors de l’exécution à partir de VS en mode normal (par défaut quand la configuration de débogage n’est pas utilisée)
FROM base AS final
WORKDIR /app
RUN --mount=type=secret,id=tmdbapikey
RUN --mount=type=secret,id=tmdbapireadtoken
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.dll", "with-docker"]

# Commandes utilisées pour lancer le projet :
# docker build . -t nobespo-aspdotnet-app
# docker run -it -p 5259:8080 --rm --name dockerize-nobespo-aspdotnet-app nobespo-aspdotnet-app 