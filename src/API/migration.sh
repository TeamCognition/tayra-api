dotnet ef migrations add BETA23 --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
dotnet ef database update --project="../DAL/OrganizationModel" --context="OrganizationDbContext"
