﻿namespace Catalog.IdentityService.Domain.Common.Interfaces;

public interface IHasDomainEvent
{
    public List<DomainEvent> DomainEvents { get; set; }
}