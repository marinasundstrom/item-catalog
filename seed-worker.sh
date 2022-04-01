#!sh

CS="Server=localhost,1433;User Id=sa;Password=P@ssw0rd"

echo "Seeding databases"

echo "Seeding Worker"
dotnet run --project ./Worker/Worker/Worker.csproj -- --seed --connection-string "$CS;Database=Worker"
echo "Done"

echo "All done"