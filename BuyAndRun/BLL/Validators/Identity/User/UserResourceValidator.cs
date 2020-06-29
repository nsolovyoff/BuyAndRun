using BLL.Resources.Identity.User;
using FluentValidation;

namespace BLL.Validators.Identity.User
{
    public class UserResourceValidator : AbstractValidator<UserResource>
    {
        public UserResourceValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.UserName).NotEmpty().Length(2, 16);
            RuleFor(m => m.RegisteredAt).NotEmpty();
            RuleFor(m => m.ImageUrl).NotEmpty();

            RuleFor(m => m.Name).Length(2, 32).When(m => string.IsNullOrEmpty(m.Name));
            RuleFor(m => m.Email).EmailAddress().When(m => string.IsNullOrEmpty(m.Email));

            RuleFor(m => m.Roles).InjectValidator().When(m => m.Roles != null);
        }
    }
}
