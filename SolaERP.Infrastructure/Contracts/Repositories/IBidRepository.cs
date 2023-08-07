using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IBidRepository
    {
        Task<List<BidAll>> GetAllAsync(BidAllFilter filter);
        Task<List<BidMainLoad>> GetMainLoadAsync(int bidMainId);
        Task<int> AddMainAsync(BidMain entity);

    }
}
