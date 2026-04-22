using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ToDo_1.ClassesDTO;

namespace ToDo_1.Models
{

    [Index("Title", "Description")]
        public class Purpose
        {
            public int Id { get; set; }

            [Required]
            [MaxLength(200)]
            public string Title { get; set; } = "";

            public string? Description { get; set; }

           public  StatusRecord Status{ get; set; } = StatusRecord.NotStarted;
            public DateTime CreatedAt { get; set; }

            public DateTime UpdatedAt { get; set; }
        }
    
}
