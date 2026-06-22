using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        //appuser is an identity user :every time i use this i will use all of identityuser table
        //so => inheritance not 1:1 
        //one appuser can be either a customer or a kitchenchef but not both

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }



        public Customer? Customer { get; set; } //? => it is the may side in mapping
        public KitchenChef? KitchenChef { get; set; }//? => it is the may side in
        public Admin? Admin { get; set; }//? => it is the may side in

        public ICollection<OneTimeToken> OneTimeTokens { get; set; } = new List<OneTimeToken>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
    public enum UserType
    {
        Customer,
        KitchenChef,
        Admin
    }
}
