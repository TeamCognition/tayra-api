FROM mcr.microsoft.com/dotnet/core/sdk:3.1
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
COPY src/API/*.csproj ./src/API/
COPY src/Services/*.csproj ./src/Services/
COPY src/Common/*.csproj ./src/Common/
COPY src/Mailer/*.csproj ./src/Mailer/

#restore
RUN dotnet restore ./src/API/

#copy everything else and build app
COPY /cog/. ./cog
COPY /src/. ./src
WORKDIR /app/src/API/
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=0 /app/src/API/out ./  
RUN mkdir -p wwwroot
EXPOSE 80 443
ENTRYPOINT ["dotnet", "Tayra.API.dll"]
