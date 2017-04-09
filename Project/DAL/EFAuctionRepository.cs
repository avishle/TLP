using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLP.Models;

namespace TLP.DAL
{
    public class EFAuctionRepository : IAuctionRespository
    {
        DataAccessLayer dal = new DataAccessLayer();
        public IQueryable<Auction> Auctions
        {
            get
            {
                return dal.Auctions;
            }
        }

        public Auction Save(Auction auction)
        {
            if (auction.a_id == 0)
            {
                dal.Auctions.Add(auction);

            }
            else
            {
                dal.Entry(auction).State = System.Data.Entity.EntityState.Modified;
            }
            dal.SaveChanges();
            return auction;
        }
        public  List<Auction> RetrieveAvailableAuctions()
        {
            List<Auction> availableAuctions = (from x in dal.Auctions
                                               where x.deadLine >= DateTime.Today
                                               select x).ToList<Auction>();
            return availableAuctions;
        }
    }
}