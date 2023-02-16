using Microsoft.AspNetCore.Identity;
using RealtimeGames.Shared.Models;

namespace RealtimeGames.Server.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Score Score { get; set; } = default!;
    }
}