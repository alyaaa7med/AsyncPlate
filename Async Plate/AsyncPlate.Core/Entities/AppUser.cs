using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public  class AppUser : IdentityUser
    {
        //appuser is an identity user :every time i use this i will use all of identityuser table
        //so => inheritance not 1:1 
        //one appuser can be either a customer or a kitchenchef but not both

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address {get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; } 
        public string? RefreshToken { get; set; }

        //nav props
        //virtual for lazy loading as we will use this not too much and we want to load it when we need it
        //n+1 problem will not happen as we will load it when we need it and not all the time
        public virtual Customer? Customer { get; set; } //? => it is the may side in mapping
        public virtual KitchenChef? KitchenChef { get; set; }//? => it is the may side in mapping

    }
}
