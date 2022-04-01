#!sh

CS="Server=localhost,1433;User Id=sa;Password=P@ssw0rd"

echo "Seeding databases"

echo "Seeding IdentityService"
dotnet run --project ./IdentityService/IdentityService/IdentityService.csproj -- --seed --connection-string "$CS;Database=IdentityServer"
echo "Done"

echo "Seeding Catalog"
dotnet run --project ./AppService/WebApi/WebApi.csproj -- --seed --connection-string "$CS;Database=Catalog"
echo "Done"

echo "Seeding Notifications"
dotnet run --project ./Notifications/Notifications/Notifications.csproj -- --seed --connection-string "$CS;Database=Notifications"
echo "Done"

echo "Seeding Messenger"
dotnet run --project ./Messenger/Messenger/Messenger.csproj -- --seed --connection-string "$CS;Database=Messenger"
echo "Done"

echo "Seeding Worker"
dotnet run --project ./Worker/Worker/Worker.csproj -- --seed --connection-string "$CS;Database=Worker"
echo "Done"

echo "All done"