dotnet ef migrations add BETA65 --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
dotnet ef database update --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
