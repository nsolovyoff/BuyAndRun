using BLL.Resources.Lot;
using FluentValidation;

namespace BLL.Validators.Lot
{
    public class MakeBidResourceValidator : AbstractValidator<LotResource>
    {
        public MakeBidResourceValidator()
        {
            RuleFor(m => m.Bid).NotEmpty();
        }
    }
}