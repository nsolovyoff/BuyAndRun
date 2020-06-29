using BLL.Resources.Identity.Role;
using FluentValidation;

namespace BLL.Validators.Identity.Role
{
    public class RoleResourceValidator : AbstractValidator<RoleResource>
    {
        public RoleResourceValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
