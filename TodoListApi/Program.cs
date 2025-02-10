using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoApi.Item;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8, 0, 2)),
    mysqlOptions => 
    {
        mysqlOptions.EnableRetryOnFailure(maxRetryCount: 10);
        mysqlOptions.CommandTimeout(500); // Set command timeout to 60 seconds
    }));
// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Load the connection string from appsettings.json and inject the TodoContext into the builder
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8, 0, 2))));

//Add the Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use the CORS policy
app.UseCors(MyAllowSpecificOrigins);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API");
    c.RoutePrefix = string.Empty;
});

app.MapGet("/tasks", async (ToDoDbContext db)=>{
    return Results.Ok(await db.Items.ToListAsync());
});
app.MapGet("/tasks/{id}", async (ToDoDbContext db, int id) => { return Results.Ok(await db.Items.FindAsync(id)); });
app.MapPost("/tasks", async (ToDoDbContext db, Item item) =>
{
    db.Items.Add(item);  
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

app.MapPut("/tasks/{id}", async (ToDoDbContext db, int id,  Item item) => {
    var existingItem = await db.Items.FindAsync(id);
    if (existingItem == null)
        return Results.NotFound();
    existingItem.Name = item.Name;
    existingItem.IsComplete = item.IsComplete;

    //Set the Id to ensure it matches
    existingItem.Id = id;

    db.Items.Update(existingItem);
    await db.SaveChangesAsync();
    return Results.Ok(existingItem);
});


app.MapDelete("/tasks/{id}", async (ToDoDbContext db,int id) => {
    var existingItem = await db.Items.FindAsync(id);
    if(existingItem==null)
        return Results.NotFound();
    db.Remove(existingItem);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("/", ()=>"Todo server is running");

app.Run();

