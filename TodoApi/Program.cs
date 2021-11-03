using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Model;
using TodoApi.ViewModel;

var builder = WebApplication.CreateBuilder(args);

//Configuration database inMemory
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));
//Alow show database exception
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/todo/itens", async (TodoModel request, AppDbContext db) => 
{
    var data = request.MapTo(); 
    if (!request.IsValid)
        return Results.BadRequest(request.Notifications);

    db.Todos.Add(data);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/todo/itens", async (AppDbContext db) =>  
{

    var itens = await db.Todos.ToListAsync();
    return itens is not null 
        ? Results.Ok(itens)
        : Results.NotFound();
});


app.MapGet("todo/itens/{id}", async (int id, AppDbContext db) => 
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapDelete("todo/itens/{id}", async (int id, AppDbContext db) => 
{
    var item = await db.Todos.FindAsync(id);
    if(item is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});
            
app.Run();