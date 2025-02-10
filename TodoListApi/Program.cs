using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoApi.Item;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8, 0, 2))
));
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

//Add the Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API");
    c.RoutePrefix = string.Empty;
});

// Use the CORS policy
app.UseCors(MyAllowSpecificOrigins);

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

app.MapGet("/api", ()=>"Todo server is running");
Console.WriteLine("db connectionstring" + builder.Configuration.GetConnectionString("ToDoDB"));
app.Run();
