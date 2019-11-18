dotnet ef migrations add MVP --project="../DAL/CatalogModel" --context="CatalogDbContext"
dotnet ef database update --project="../DAL/CatalogModel" --context="CatalogDbContext"
