using SSW_Clean.Domain.Common.Base;

namespace SSW_Clean.Domain.TodoItems;
public record TodoItemCreatedEvent(TodoItem Item) : DomainEvent;
