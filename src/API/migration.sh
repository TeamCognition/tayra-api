dotnet ef migrations add AddLogMeta --project="../DAL/OrganizationModel" --context="OrganizationDbContext"

dotnet ef migrations add Initial --project="../DAL/OrganizationModel" --context="OrganizationDbContext"

dotnet ef database update --project="../Auth" --context="OpeniddictDbContext"
