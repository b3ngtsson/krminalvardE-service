using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Bookings
    {
        public int BookingId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int PersonOfInterest { get; set; }
        public int User { get; set; }
    }
}