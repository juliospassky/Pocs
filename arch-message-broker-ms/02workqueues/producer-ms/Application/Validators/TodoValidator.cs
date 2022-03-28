using Application.Adapter.Contract.Request;
using FluentValidation;

namespace Application.Validators
{
    public class TodoValidator : AbstractValidator<TodoRequest>
    {
        public TodoValidator()
        {
            RuleFor(o => o.Name)
                .NotEmpty()
                .WithMessage("Nome com problema");

            RuleFor(o => o.Email)
                .NotEmpty();
        }
    }
}
