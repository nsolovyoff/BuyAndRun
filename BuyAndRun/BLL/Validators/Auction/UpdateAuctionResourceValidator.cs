using BLL.Resources.Auction;
using FluentValidation;

namespace BLL.Validators.Auction
{
    public class UpdateAuctionResourceValidator : AbstractValidator<UpdateAuctionResource>
    {
        public UpdateAuctionResourceValidator()
        {
            RuleFor(m => m.Name).Length(2, 36).When(m => !string.IsNullOrEmpty(m.Name));
            RuleFor(m => m.Description).Length(2, 128).When(m => !string.IsNullOrEmpty(m.Description));
        }
    }
}
