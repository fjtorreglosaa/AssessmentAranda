1. Modificar el ConnectionStrings del archivo appsettings.json del proyecto WebApi y apuntar defaultConnection a base de datos local.
2. Ejecutar migraciones en una terminal: dotnet ef migrations add InitialModel -p Assessment.Data -s Assessment.WebApi -o Migrations
3. Actualizar base de datos en una terminal: dotnet ef database update -p Assessment.WebApi
