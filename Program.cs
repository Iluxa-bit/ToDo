using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using ToDo_1.Models;
using ToDo_1.Data;



var builder = WebApplication.CreateBuilder();
builder.Services.AddDbContext<ApplicationContext>(opt =>
 opt.UseNpgsql(builder.Configuration.GetConnectionString("MyWebApiConection")));

var app = builder.Build();

app.MapGet("/api/tasks", async (ApplicationContext db) => Results.Json(await db.Tasks.ToListAsync())); //db - объект, 

app.MapGet("/api/tasks/{id}", async (int id, ApplicationContext db) =>
{
    try
    {
        // получаем пользователя по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);
        // если не найден, отправляем статусный код и сообщение об ошибке
        if (task == null) return Results.NotFound(new { message = "Пользователь не найден" });

        // если пользователь найден, отправляем его
        return Results.Json(task);
    }
    catch
    {
        return Results.NotFound(new { message = "Некорректные данные" });
    }

});

app.MapDelete("/api/tasks/{id}", async (int id, ApplicationContext db) =>
{
    try
    {
        // получаем пользователя по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);

        // если не найден, отправляем статусный код и сообщение об ошибке
        if (task == null) return Results.NotFound(new { message = "Пользователь не найден" });

        // если пользователь найден, удаляем его
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.Json(task);
    }
    catch
    {
        return Results.NotFound(new { message = "Некорректные данные" });
    }
});

app.MapPost("/api/tasks", async (ApplicationContext db, CreateTaskDto taskDto) => {

    // устанавливаем id для нового пользователя
    Purpose task = new Purpose();
    task.Title = taskDto.Title;
    task.Description = taskDto.Description;
    task.CreatedAt = DateTime.UtcNow;
    
    // добавляем пользователя в список
    await db.Tasks.AddAsync(task); //все поля, что заполнил пользователь для этой сущности помечаются added, и при выызове SaveChangesAsync

    await db.SaveChangesAsync();//все данные что заполнил пользователь - сохранятся в бд, если он не заполнил значение будет null (или "")
    return Results.Json(task);
});

app.MapPut("/api/tasks/{id}", async (UpdateTaskDto taskDto, ApplicationContext db,int id) => {

    try
    {
        // получаем пользователя по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);
        // если не найден, отправляем статусный код и сообщение об ошибке
           if (task == null) return Results.NotFound(new { message = "Пользователь не найден" });
        // если пользователь найден, изменяем его данные и отправляем обратно клиенту

        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.Status = taskDto.Status;
        task.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Results.Json(task);
    }
    catch
    {
        return Results.NotFound(new { message = "Некорректные данные" });
    }
});

app.MapPatch("/api/tasks/{id}/status", async (ApplicationContext db, int id, PatchStatusDto patchStatusDto) => {
    try
    {
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (task == null) return Results.NotFound(new { message = "Пользователь не найден" });
        task.Status = patchStatusDto.status;
        await db.SaveChangesAsync();
        return Results.Json(task);
    }
    catch 
    {
        return Results.NotFound(new { message = "Некорректные данные" });
    }


});

app.Run();

