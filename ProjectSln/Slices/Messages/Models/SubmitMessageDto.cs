using FluentMigrator.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Messages.Models
{
    public record SubmitMessageDto
    {
        public DateTime Date { get; } = DateTime.UtcNow;
        
        [Required(ErrorMessage = "Id of the sender is required")]
        public string SenderId { get; init; }
        [Required(ErrorMessage = "Message body cannot be null")]
        public string Body { get; init; }
    }
}
