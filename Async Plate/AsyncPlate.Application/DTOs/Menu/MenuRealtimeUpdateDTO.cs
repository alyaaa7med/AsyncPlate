using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Menu
{
    public class MenuRealtimeUpdateDTO
    {

        public string MenuItemId { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public bool HasOffer { get; set; }
        public decimal FinalPrice { get; set; }

        //if the product is deleted from the menu => we need to send the update to the frontend
        public bool IsDeleted { get; set; }

        //may category be updated in the future or deleted => so we also need to send the update
        //deleted => restrict so no need for taking consideration for this 
        //update => i think changing catgory of the product not happen 
        //public string CategoryId;
        //publi string NewCatgoryName;


    }
}
