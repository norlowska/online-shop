using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineShop.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string imie { get; set; }
        public string Nazwisko { get; set; }
        public string Adres { get; set; }
        public string miasto { get; set; }
        public string Województwo { get; set; }
        public string kod_pocztowy { get; set; }
        public string ulica { get; set; }
        public string nr { get; set; }
        public string Kraj { get; set; }

        public bool newsletter { get; set; }

        public int Ograniczeni { get; set; }





        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }
    }
}