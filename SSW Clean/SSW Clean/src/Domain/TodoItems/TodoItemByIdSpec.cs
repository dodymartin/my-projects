﻿using Ardalis.Specification;

namespace SSW_Clean.Domain.TodoItems;
public sealed class TodoItemByIdSpec : SingleResultSpecification<TodoItem>
{
    public TodoItemByIdSpec(TodoItemId todoItemId)
    {
        Query.Where(t => t.Id == todoItemId);
    }
}