using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.Models;

public class PostCommentDto
{
    [Required]
    public string Text { get; set; } = null!;
}