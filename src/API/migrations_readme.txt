//CATALOG DB 
dotnet ef migrations add Initial --project="../DAL/CatalogModel" --context="CatalogDbContext"
dotnet ef migrations remove --project="../DAL/CatalogModel" --context="CatalogDbContext"
dotnet ef database update --project="../DAL/CatalogModel" --context="CatalogDbContext"

//ORGANIZATION DB 
dotnet ef migrations add DeleteOldMetricTables --project="../DAL/OrganizationModel" --context="OrganizationDbContext"

//AUTH DB
dotnet ef database update --project="../Auth" --context="OpeniddictDbContext"
