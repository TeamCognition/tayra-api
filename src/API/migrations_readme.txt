dotnet ef migrations add MVP --project="../DAL/CoreModel" --context="CatalogDbContext"
dotnet ef database update --project="../DAL/CoreModel" --context="CatalogDbContext"


dotnet ef migrations add BETA-28 --project="../DAL/CatalogModel" --context="CatalogDbContext"
dotnet ef database update --project="../DAL/CatalogModel" --context="CatalogDbContext"
