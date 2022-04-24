using Application.AdapterInbound.Contract.Request;
using FluentValidation;

namespace Application.Validators
{
    public class TodoValidator : AbstractValidator<TodoRequest>
    {
        public TodoValidator()
        {
            RuleFor(o => o.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$")
                .WithMessage("Nome com problema");

            RuleFor(o => o.Email)
                .NotEmpty();
                
        }
    }
}
