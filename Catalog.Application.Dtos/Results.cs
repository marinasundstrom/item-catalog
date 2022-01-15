using System;

namespace Catalog.Application;

public record class Results<T>(IEnumerable<T> Items, int TotalCount);