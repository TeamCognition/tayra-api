dotnet ef migrations add ItemSoftDelete --project="../DAL/OrganizationModel" --context="OrganizationDbContext"

dotnet ef migrations remove --project="../DAL/OrganizationModel" --context="OrganizationDbContext" //DOESNT WORK
