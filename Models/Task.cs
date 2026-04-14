using System.ComponentModel.DataAnnotations;
using ToDo_1.ClassesDTO;

namespace ToDo_1.Models
{
    
        public class Purpose
        {
            public int Id { get; set; }

            [Required]
            [MaxLength(200)]
            public string Title { get; set; } = "";

            public string? Description { get; set; }

           public  StatusDto Status{ get; set; } = StatusDto.NotStarted;
            public DateTime CreatedAt { get; set; }

            public DateTime UpdatedAt { get; set; }
        }
    
}
