namespace SSW_Clean.Application.Features.TodoItems.Commands.CompleteTodoItem;

public class CompleteTodoItemCommandValidator : AbstractValidator<CompleteTodoItemCommand>
{
    public CompleteTodoItemCommandValidator()
    {
        RuleFor(p => p.TodoItemId)
            .NotEmpty();
    }
}