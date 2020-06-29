using BLL.Resources.Category;
using FluentValidation;

namespace BLL.Validators.Category
{
    public class CreateCategoryResourceValidator : AbstractValidator<CreateCategoryResource>
    {
        public CreateCategoryResourceValidator()
        {
            RuleFor(m => m.Title).NotEmpty().Length(2, 128);
            RuleFor(m => m.ParentAuction).NotEmpty().InjectValidator();
            RuleFor(m => m.StartedBy).NotEmpty().InjectValidator();
        }
    }
}
