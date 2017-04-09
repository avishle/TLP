using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLP.Models;

namespace TLP.DAL
{
    public interface IAuctionRespository
    {
        IQueryable<Auction> Auctions { get; }
        Auction Save(Auction auction);
         List<Auction> RetrieveAvailableAuctions();
    }
}
