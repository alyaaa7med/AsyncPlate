using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class KitchenChef : AppUser 
    {
        //only one chef per station, and he has a status. 
        //public string Station { get; set; } = string.Empty; //"Grill", "Salad", "Fryer"
        //public ChefStatus Status { get; set; }
        //public int ActiveTicketsCount { get; set; } // Number of orders currently being prepped
    }
    //public enum ChefStatus
    //{
    //    Available,
    //    Busy,
    //    OnBreak
    //}
}
