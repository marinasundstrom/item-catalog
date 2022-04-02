using System;

namespace Catalog.Messenger.Application.Common.Models;

public record class Results<T>(IEnumerable<T> Items, int TotalCount);