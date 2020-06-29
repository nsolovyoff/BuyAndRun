using BLL.Resources.Category;
using FluentValidation;

namespace BLL.Validators.Category
{
    public class UpdateCategoryResourceValidator : AbstractValidator<UpdateCategoryResource>
    {
        public UpdateCategoryResourceValidator()
        {
            RuleFor(m => m.Title).Length(2, 128).When(m => !string.IsNullOrEmpty(m.Title));
            RuleFor(m => m.ParentAuction).InjectValidator().When(m => m.ParentAuction != null);
        }
    }
}
