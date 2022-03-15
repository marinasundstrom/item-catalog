using System.ComponentModel.DataAnnotations;

namespace Catalog.WebApi.Controllers;

public class UpdateCommentDto
{
    [Required]
    public string Text { get; set; } = null!;
}
