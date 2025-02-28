using FluentValidation;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Entities.Bid;

namespace SolaERP.Persistence.Validations.Bid;

public class BidSaveValidation : AbstractValidator<BidMainDto>
{
    private readonly IBidRepository _bidRepository;

    public BidSaveValidation(IBidRepository bidRepository)
    {
        _bidRepository = bidRepository;

        // RuleFor(x => x)
        //     .MustAsync(BeUniqueRFQVendor)
        //     .WithMessage("Bu RFX üçün artıq həmin təçhizatçı tərəfindən təklif yaradılmışdır");
        
        RuleFor(x => new { x.RFQMainId, x.VendorCode })
            .MustAsync(async (x, cancellationToken) => 
                await _bidRepository.GetBidCheckExistsByRFQMainIdAndVendorCode(x.RFQMainId, x.VendorCode))
            .WithMessage("Bu RFX üçün artıq həmin təçhizatçı tərəfindən təklif yaradılmışdır");
    }
    
    // private async Task<bool> BeUniqueRFQVendor(BidMainDto bidMain, CancellationToken cancellationToken)
    // {
    //     var data = await _bidRepository.GetBidByRFQMainIdAndVendorCode(bidMain.RFQMainId, bidMain.VendorCode);
    //     return data.Count > 0;
    // }
}