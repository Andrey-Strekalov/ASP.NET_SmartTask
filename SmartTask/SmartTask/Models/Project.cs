using SmartTask.Models;
using SmartTask.Repositories;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace SmartTask.Models
{
    public class Project : IHasUpdatedAt
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название проекта обязательно")]
        [StringLength(100, ErrorMessage = "Название не может превышать 100 символов")]
 public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [RegularExpression("^([A-Fa-f0-9]{6})$", ErrorMessage = "Цвет должен быть в формате HEX(RRGGBB)")]
 public string Color { get; set; } = "808080"; // Серый по умолчанию
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Внешний ключ
        public int OwnerId { get; set; }
        // Навигационные свойства
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}