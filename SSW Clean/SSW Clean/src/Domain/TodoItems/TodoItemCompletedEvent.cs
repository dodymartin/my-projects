using SSW_Clean.Domain.Common.Base;

namespace SSW_Clean.Domain.TodoItems;
public record TodoItemCompletedEvent(TodoItem Item) : DomainEvent;
