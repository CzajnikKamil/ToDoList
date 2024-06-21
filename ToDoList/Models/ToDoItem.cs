using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsCompleted { get; set; }
    }
}
