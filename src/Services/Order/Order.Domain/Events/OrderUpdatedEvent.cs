﻿namespace Order.Domain.Events;

public record OrderUpdatedEvent(Models.Order Order) : IDomainEvent;
