using BLL.Resources.Auction;
using FluentValidation;

namespace BLL.Validators.Auction
{
    public class CreateAuctionResourceValidator : AbstractValidator<CreateAuctionResource>
    {
        public CreateAuctionResourceValidator()
        {
            RuleFor(m => m.Name).NotEmpty().Length(2, 36);
            RuleFor(m => m.Description).Length(2, 128).When(m => !string.IsNullOrEmpty(m.Description));
        }
    }
}