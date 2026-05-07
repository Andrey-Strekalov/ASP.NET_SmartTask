using SmartTask.Models;
using SmartTask.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
namespace SmartTask.Models;

public class User : IHasUpdatedAt
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = " Имя должно содержать от 3 до 100 символов")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    public string? Avatar { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Settings { get; set; } // JSON с настройками
                                          // Навигационные свойства

    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}