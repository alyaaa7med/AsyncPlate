using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.ProductExtra
{
    public class AddProductExtraDTO
    {
        public List<string> ExtraProductIds { get; set; } = []; //list i will use its properties , ienumerable will not fit
    }
}
