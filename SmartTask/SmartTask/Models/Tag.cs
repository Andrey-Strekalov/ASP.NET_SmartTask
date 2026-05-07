using SmartTask.Models;
using SmartTask.Repositories;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SmartTask.Models
{
    public class Tag : IHasUpdatedAt
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название тега обязательно")]
        [StringLength(50, ErrorMessage = "Название тега не может превышать 50 символов")]
        public string Name { get; set; }
        // Внешний ключ
        public int OwnerId { get; set; }
        // Навигационные свойства
        public User Owner { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        // Связь многие-ко-многим с Task
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}