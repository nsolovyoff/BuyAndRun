using BLL.Resources.Identity.User;
using FluentValidation;

namespace BLL.Validators.Identity.User
{
    public class CreateUserResourceValidator : AbstractValidator<CreateUserResource>
    {
        public CreateUserResourceValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().Length(2, 16);
            RuleFor(m => m.Password).NotEmpty().Length(2, 32);
            RuleFor(m => m.ImageUrl).NotEmpty();
            RuleFor(m => m.Name).Length(2, 32).When(m => string.IsNullOrEmpty(m.Name));
            RuleFor(m => m.Email).EmailAddress().When(m => string.IsNullOrEmpty(m.Email));
        }
    }
}
