using BLL.Resources.Auction;
using FluentValidation;

namespace BLL.Validators.Auction
{
    public class AuctionResourceValidator : AbstractValidator<AuctionResource>
    {
        public AuctionResourceValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().Length(2, 64);
            RuleFor(m => m.Description).Length(2, 128).When(m => !string.IsNullOrEmpty(m.Description));
            RuleFor(m => m.IsActive).NotEmpty();

            RuleFor(m => m.Moderators).InjectValidator().When(m => m.Moderators != null);
        }
    }
}
