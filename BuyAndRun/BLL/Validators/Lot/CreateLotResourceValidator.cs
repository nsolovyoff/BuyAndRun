using BLL.Resources.Lot;
using FluentValidation;

namespace BLL.Validators.Lot
{
    public class CreateLotResourceValidator : AbstractValidator<CreateLotResource>
    {
        public CreateLotResourceValidator()
        {
            RuleFor(m => m.Description).NotEmpty().Length(2, 512);
            RuleFor(m => m.Title).NotEmpty().Length(2, 64);
            RuleFor(m => m.BuyNowPrice).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Expiring).NotEmpty()/*.GreaterThan(DateTime.Now)*/;
            RuleFor(m => m.CategoryId).NotEmpty();
            RuleFor(m => m.User).NotEmpty().InjectValidator();
        }
    }
}
