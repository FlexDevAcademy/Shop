using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Helpers
{
    public static class Helpers
    {
        public static async Task<Profile> FindUserProfileWithShoppingBag(HttpContext httpContext, 
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            var profile = context.Profiles.FirstOrDefault(p => p.Email == user.Email);
            var userProfile = await context.Profiles
                .Include(p => p.ShoppingBag)
                .ThenInclude(s => s.Items)
                .FirstAsync(s => s.Id == profile.Id);
            return userProfile;
        }
    }
}
