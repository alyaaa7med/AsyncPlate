using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Supplier
{
    public class SupplierFilterDTO
    {
        //it is for the query parameters to make the code reusable and clean



        //filtering
        public string? Name { get; set; }


        //pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
