namespace Order.Domain.Events;

public record OrderCreatedEvent(Models.Order Order) : IDomainEvent;
