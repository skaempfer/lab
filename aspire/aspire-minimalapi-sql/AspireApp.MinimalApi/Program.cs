using Microsoft.EntityFrameworkCore;
using AspireApp.MinimalApi.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (from shared project)
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext with SQL Server connection string from Aspire
// Use the connection string injected by Aspire (from the referenced SQL resource)
// Fallback to appsettings.json for local development
var sqlConnectionString = builder.Configuration.GetConnectionString("Aspire")
    ?? throw new InvalidOperationException("Connection string 'opn' is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

app.MapDefaultEndpoints(); // From shared Extensions

app.MapGet("/", () => "Hello from Minimal API!");

// --- People Endpoints ---

// Get all People
app.MapGet("/people", async (AppDbContext db) =>
{
    var people = await db.People.ToListAsync();
    return Results.Ok(people);
});

// Create Person
app.MapPost("/people", async (Person person, AppDbContext db) =>
{
    db.Add(person);
    await db.SaveChangesAsync();
    return Results.Created($"/people/{person.Name}", person);
});

// Update Person
app.MapPut("/people/{name}", async (string name, Person updated, AppDbContext db) =>
{
    var person = await db.Set<Person>().FirstOrDefaultAsync(p => p.Name == name);
    if (person is null) return Results.NotFound();
    person.Age = updated.Age;
    person.Name = updated.Name;
    await db.SaveChangesAsync();
    return Results.Ok(person);
});

// Delete Person
app.MapDelete("/people/{name}", async (string name, AppDbContext db) =>
{
    var person = await db.Set<Person>().FirstOrDefaultAsync(p => p.Name == name);
    if (person is null) return Results.NotFound();
    db.Remove(person);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
