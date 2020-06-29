using BLL.Resources.Category;
using FluentValidation;

namespace BLL.Validators.Category
{
    public class CategoryResourceValidator : AbstractValidator<CategoryResource>
    {
        public CategoryResourceValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Title).NotEmpty().Length(2, 128);

            RuleFor(m => m.ParentAuction).NotEmpty().InjectValidator();
        }
    }
}
