using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TLP.Models
{
    public class Auction
    {
        [Key]
        public int a_id { get; set; }
        public String auctionname { get; set; }
        public DateTime deadline { get; set; }

    }
}