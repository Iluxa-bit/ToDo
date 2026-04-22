
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDo_1.Controllers;
using ToDo_1.ClassesRecord;
using ToDo_1.Models;
using ToDo_1.Logging;



var builder = WebApplication.CreateBuilder();
builder.Services.AddDbContext<ApplicationContext>(opt =>
 opt.UseNpgsql(builder.Configuration.GetConnectionString("MyWebApiConection")));
builder.Services.AddScoped<IObservableLog>(observ => 
observ.GetRequiredService<ApplicationContext>());
var app = builder.Build();



app.UseMiddleware<RequestTimingMiddleware>();
app.MapGet("/", async (ApplicationContext db) =>
{
    var tasks = await db.Logs.FromSqlRaw("SELECT * FROM \"Logs\"").ToListAsync();
    return Results.Json(tasks);
});


app.MapGet("/api/logs/{date}", async (ApplicationContext db, string date) =>
{
try
{
    var logs = await db.Logs.FromSqlRaw("SELECT * FROM \"Logs\" WHERE \"dateTime\"::DATE={0} ::date ", date).ToListAsync();
    return Results.Json(logs);
}
catch
{
    return Results.NotFound(Consts.MESSAGE_ERROR);
    }
});


app.MapGet("/api/tasks", async (ApplicationContext db, ILogger<Program> logger) => {

    
    return Results.Json(await db.Tasks.ToListAsync());
    }); //db - объект, 



app.MapGet("/api/tasks/{id:int}", async (int id, ApplicationContext db) =>
{
    try
    {
    
        // получаем пользовател€ по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);
        // если не найден, отправл€ем статусный код и сообщение об ошибке
        if (task == null) return Results.NotFound(Consts.USER_NOT_FOUND);
        

        // если пользователь найден, отправл€ем его
        return Results.Json(task);
    }
    catch
    {
        return Results.NotFound(Consts.MESSAGE_ERROR);
    }

});



app.MapDelete("/api/tasks/{id:int}", async (int id, ApplicationContext db) =>
{
    try
    {
        // получаем пользовател€ по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);

        // если не найден, отправл€ем статусный код и сообщение об ошибке
        if (task == null) return Results.NotFound(Consts.USER_NOT_FOUND);

        // если пользователь найден, удал€ем его
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.Json(task);
    }
    catch
    {
        return Results.NotFound(Consts.MESSAGE_ERROR);
    }
});



app.MapPost("/api/tasks", async (ApplicationContext db, CreateTaskRecord taskDto) => {

    // устанавливаем id дл€ нового пользовател€


    Purpose task = new Purpose();
    task.Title = taskDto.Title;
    task.Description = taskDto.Description;
    task.CreatedAt = DateTime.UtcNow;
    
    // добавл€ем пользовател€ в список
    await db.Tasks.AddAsync(task); //все пол€, что заполнил пользователь дл€ этой сущности помечаютс€ added, и при выызове SaveChangesAsync

    await db.SaveChangesAsync();//все данные что заполнил пользователь - сохран€тс€ в бд, если он не заполнил значение будет null (или "")
    return Results.Json(task);
});



app.MapPut("/api/tasks/{id:int}", async (UpdateTaskRecord taskDto, ApplicationContext db,int id) => {

    try
    {
        // получаем пользовател€ по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);
        // если не найден, отправл€ем статусный код и сообщение об ошибке
           if (task == null) return Results.NotFound(Consts.USER_NOT_FOUND);
        // если пользователь найден, измен€ем его данные и отправл€ем обратно клиенту

        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.Status = taskDto.Status;
        task.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Results.Json(task);
    }
    catch
    {
        return Results.NotFound(Consts.MESSAGE_ERROR);
    }
});



app.MapPatch("/api/tasks/{id:int}/status", async (ApplicationContext db, int id, PatchStatusRecord patchStatusDto) => {
    try
    {
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (task == null) return Results.NotFound(Consts.USER_NOT_FOUND);
        task.Status = patchStatusDto.status;
        await db.SaveChangesAsync();
        return Results.Json(task);
    }
    catch 
    {
        return Results.NotFound(Consts.MESSAGE_ERROR);
    }


});



app.Run();