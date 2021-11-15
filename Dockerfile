FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ./build ./build
COPY ./cog/Cog.Core/*.csproj ./cog/Cog.Core/
COPY ./cog/Cog.DAL/*.csproj ./cog/Cog.DAL/

#DAL dependencies
COPY ./src/DAL/CatalogModel/*.csproj ./src/DAL/CatalogModel/
COPY ./src/DAL/OrganizationModel/*.csproj ./src/DAL/OrganizationModel/

#Connector dependencies
COPY ./src/Connectors/Connectors.Common/*.csproj ./src/Connectors/Connectors.Common/
COPY ./src/Connectors/Connectors.Atlassian/*.csproj ./src/Connectors/Connectors.Atlassian/
COPY ./src/Connectors/Connectors.Atlassian.Jira/*.csproj ./src/Connectors/Connectors.Atlassian.Jira/

#Project dependencies
COPY ./src/API/*.csproj ./src/API/
COPY ./src/Services/*.csproj ./src/Services/
COPY ./src/Common/*.csproj ./src/Common/
COPY ./src/Mailer/*.csproj ./src/Mailer/

RUN dotnet restore "./src/API/API.csproj"
COPY cog/. ./cog
COPY src/. ./src
WORKDIR "/app/src/API"
RUN dotnet build "API.csproj" -c Release -o /out/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /out/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /out/publish ./  
RUN mkdir -p wwwroot
ENTRYPOINT ["dotnet", "Tayra.API.dll"]
