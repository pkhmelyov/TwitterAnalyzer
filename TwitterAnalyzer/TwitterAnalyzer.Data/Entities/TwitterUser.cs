using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TwitterAnalyzer.Data.Entities
{
    public class TwitterUser : IdentityUser
    {
        public string OAuthToken { get; set; }
        public string OAuthTokenSecret { get; set; }
        public ulong TwitterId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<TwitterUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
