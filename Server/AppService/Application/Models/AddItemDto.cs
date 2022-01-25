﻿using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.Models;

public class AddItemDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}