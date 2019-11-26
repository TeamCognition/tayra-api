dotnet ef migrations add MVP-2 --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
dotnet ef database update --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
