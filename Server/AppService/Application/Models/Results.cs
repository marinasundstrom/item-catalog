using System;

namespace Catalog.Application.Models;

public record class Results<T>(IEnumerable<T> Items, int TotalCount);