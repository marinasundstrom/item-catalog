﻿namespace Catalog.Notifications.Application.Common.Models;

public record class Results<T>(IEnumerable<T> Items, int TotalCount);