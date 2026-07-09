using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Menu
{
    public class MenuBulkRealtimeUpdateDTO
    {
        public List<MenuRealtimeUpdateDTO> MenuItems { get; set; }= new List<MenuRealtimeUpdateDTO>();
    }
}
