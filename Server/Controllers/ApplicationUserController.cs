using RealtimeGames.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealtimeGames.Shared.Models;
using RealtimeGames.Server.Areas.Identity.Models;

namespace RealtimeGames.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RealtimeGamesDbContext _context;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, RealtimeGamesDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<Player>> GetPlayer(string userName)
        {
            var user = await _context.Users.Include(u => u.Score).SingleOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return NotFound();
            }

            return new Player { Name = user.UserName!, Score = user.Score };
        }

        [HttpGet("rankings")]
        public async Task<ActionResult<List<List<Player>>>> GetPlayerRankings([FromQuery] string gameName)
        {
            if (string.IsNullOrEmpty(gameName))
            {
                return NotFound();
            }

            var users = await _context.Users.Include(u => u.Score).ToListAsync();
            return users.Where(user => user.Score[gameName] > 0)
                .GroupBy(user => user.Score[gameName])
                .OrderByDescending(group => group.Key)
                .Take(10)
                .Select(group => group.Select(user => new Player() { Name = user.UserName!, Score = user.Score }).ToList())
                .ToList();
        }

        [HttpPatch("{userName}")]
        public async Task<IActionResult> UpdateScores(string userName, Score score)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            user.Score = score;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}