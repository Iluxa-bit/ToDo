namespace ToDo_1.Models
{
    public class UpdateTaskDto
    {
        
            public string Title { get; set; }="";

            public string? Description { get; set; }

            public StatusDto Status { get; set; }
        

    }
}
