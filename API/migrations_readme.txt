dotnet ef migrations add MVP --project="../DAL/CoreModel" --context="CoreDbContext"
dotnet ef database update --project="../DAL/CoreModel" --context="CoreDbContext"


dotnet ef migrations add MVP --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
dotnet ef database update --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
