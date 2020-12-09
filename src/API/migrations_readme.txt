dotnet ef migrations add Initial --project="../DAL/CatalogModel" --context="CatalogDbContext"
dotnet ef database update --project="../DAL/CatalogModel" --context="CatalogDbContext"

dotnet ef migrations remove --project="../DAL/CatalogModel" --context="CatalogDbContext"
