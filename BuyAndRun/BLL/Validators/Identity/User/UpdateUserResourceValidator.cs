using BLL.Resources.Identity.User;
using FluentValidation;

namespace BLL.Validators.Identity.User
{
    public class UpdateUserResourceValidator : AbstractValidator<UpdateUserResource>
    {
        public UpdateUserResourceValidator()
        {
            RuleFor(m => m.Name).Length(2, 32).When(m => string.IsNullOrEmpty(m.Name));
            RuleFor(m => m.Email).EmailAddress().When(m => string.IsNullOrEmpty(m.Email));
        }
    }
}
