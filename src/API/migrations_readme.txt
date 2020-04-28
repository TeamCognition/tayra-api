dotnet ef migrations add TaskCategoryUpdate --project="../DAL/CatalogModel" --context="CatalogDbContext"
dotnet ef database update --project="../DAL/CatalogModel" --context="CatalogDbContext"

dotnet ef migrations remove --project="../DAL/CatalogModel" --context="CatalogDbContext"
