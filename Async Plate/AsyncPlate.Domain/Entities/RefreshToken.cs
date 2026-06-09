using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Domain.Entities
{
    public class RefreshToken
    {
        //id , refreshtoken , createdat , expiredat, isrevoked

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RefreshTokenValue { get; set; } = string.Empty; //req
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiredAt { get; set; } = DateTime.UtcNow.AddDays(90);

        public bool IsRevoked { get; set; } = false;
        public bool IsExpired { get; set; } = false;

        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;



    }
}
