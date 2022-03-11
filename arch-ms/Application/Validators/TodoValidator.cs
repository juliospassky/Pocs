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
                .NotEmpty()
                .Matches("^(?(\"\")(\"\".+?\"\"@)|((0-9a-zA-Z)(?<=[0-9a-zA-Z])@))(?([)([(\\d{1,3}.){3}\\d{1,3}])|(([0-9a-zA-Z][-\\w][0-9a-zA-Z].)+[a-zA-Z]{2,6}))$");
        }
    }
}
