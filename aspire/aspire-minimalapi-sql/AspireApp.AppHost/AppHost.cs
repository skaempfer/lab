var builder = DistributedApplication.CreateBuilder(args);

var dbpw = builder.AddParameter("sqldb-password","Foobar12345");

var sql = builder
    .AddSqlServer("sqldb", password: dbpw, port: 51350)
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("Aspire");

builder.AddProject<Projects.AspireApp_MinimalApi>("minimalapi")
    .WithReference(sql)
    .WaitFor(sql);

builder.Build().Run();
