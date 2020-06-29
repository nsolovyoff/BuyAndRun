using BLL.Resources.Lot;
using FluentValidation;

namespace BLL.Validators.Lot
{
    public class UpdateLotResourceValidator : AbstractValidator<UpdateLotResource>
    {
        public UpdateLotResourceValidator()
        {
            RuleFor(m => m.Description).NotEmpty().Length(2, 512);
        }
    }
}
