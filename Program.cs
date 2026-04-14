using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ToDo_1.ClassesDTO;
using ToDo_1.Logging;
using ToDo_1.Models;



var builder = WebApplication.CreateBuilder();
builder.Services.AddDbContext<ApplicationContext>(opt =>
 opt.UseNpgsql(builder.Configuration.GetConnectionString("MyWebApiConection")));
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));

var app = builder.Build();

app.MapGet("/api/tasks", async (ApplicationContext db, ILogger<Program> logger) => {

    logger.LogInformation("‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘‘");
    return Results.Json(await db.Tasks.ToListAsync());
    }); //db - объект, 

app.MapGet("/api/tasks/{id}", async (int id, ApplicationContext db) =>
{
    try
    {
    
        // получаем пользовател€ по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);
        // если не найден, отправл€ем статусный код и сообщение об ошибке
        if (task == null) return Results.NotFound(new { message = "ѕользователь не найден" });
        

        // если пользователь найден, отправл€ем его
        return Results.Json(task);
    }
    catch
    {
        return Results.NotFound(new { message = "Ќекорректные данные" });
    }

});

app.MapDelete("/api/tasks/{id}", async (int id, ApplicationContext db) =>
{
    try
    {
        // получаем пользовател€ по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);

        // если не найден, отправл€ем статусный код и сообщение об ошибке
        if (task == null) return Results.NotFound(new { message = "ѕользователь не найден" });

        // если пользователь найден, удал€ем его
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.Json(task);
    }
    catch
    {
        return Results.NotFound(new { message = "Ќекорректные данные" });
    }
});

app.MapPost("/api/tasks", async (ApplicationContext db, CreateTaskDto taskDto) => {

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

app.MapPut("/api/tasks/{id}", async (UpdateTaskDto taskDto, ApplicationContext db,int id) => {

    try
    {
        // получаем пользовател€ по id
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(u => u.Id == id);
        // если не найден, отправл€ем статусный код и сообщение об ошибке
           if (task == null) return Results.NotFound(new { message = "ѕользователь не найден" });
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
        return Results.NotFound(new { message = "Ќекорректные данные" });
    }
});

app.MapPatch("/api/tasks/{id}/status", async (ApplicationContext db, int id, PatchStatusDto patchStatusDto) => {
    try
    {
        Purpose? task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (task == null) return Results.NotFound(new { message = "ѕользователь не найден" });
        task.Status = patchStatusDto.status;
        await db.SaveChangesAsync();
        return Results.Json(task);
    }
    catch 
    {
        return Results.NotFound(new { message = "Ќекорректные данные" });
    }


});

app.Run();

