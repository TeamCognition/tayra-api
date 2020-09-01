dotnet ef migrations add AddProfileMetricsTable --project="../DAL/OrganizationModel" --context="OrganizationDbContext"

dotnet ef migrations remove --project="../DAL/OrganizationModel" --context="OrganizationDbContext" //DOESNT WORK
