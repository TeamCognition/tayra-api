dotnet ef migrations add ChangeClaimBundleRelationships --project="../DAL/OrganizationModel" --context="OrganizationDbContext"

dotnet ef migrations remove --project="../DAL/OrganizationModel" --context="OrganizationDbContext" //DOESNT WORK
