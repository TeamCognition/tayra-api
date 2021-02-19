FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ./build ./build
COPY ./cog/Cog.Core/*.csproj ./cog/Cog.Core/
COPY ./cog/Cog.DAL/*.csproj ./cog/Cog.DAL/

#DAL dependencies
COPY ./src/DAL/CatalogModel/*.csproj ./src/DAL/CatalogModel/
COPY ./src/DAL/OrganizationModel/*.csproj ./src/DAL/OrganizationModel/

#Project dependencies
COPY src/Auth/*.csproj ./src/Auth/
COPY src/Services/*.csproj ./src/Services/
COPY src/Common/*.csproj ./src/Common/

#restore
RUN dotnet restore ./src/Auth/

#copy everything else and build app
COPY /cog/. ./cog
COPY /src/. ./src
WORKDIR /app/src/Auth/
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=0 /app/src/Auth/out ./  
RUN mkdir -p wwwroot
EXPOSE 80 443
ENTRYPOINT ["dotnet", "Tayra.Auth.dll"]
