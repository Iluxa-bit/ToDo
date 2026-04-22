namespace ToDo_1.ClassesRecord
{
   
    public record struct UpdateTaskRecord(string Title, string? Description, StatusRecord Status);
}
