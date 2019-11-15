dotnet ef migrations add MVP --project="../DAL/CoreModel" --context="CatalogDbContext"
dotnet ef database update --project="../DAL/CoreModel" --context="CatalogDbContext"


dotnet ef migrations add MVP --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
dotnet ef database update --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
