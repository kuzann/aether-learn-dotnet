using Aether.SampleApplication.Context;
using Aether.SampleApplication.Entities;
using Aether.SampleApplication.Identity;
using Aether.SampleApplication.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Aether.SampleApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly SampleAppDbContext _dbContext;
        private readonly DbSet<Song> _dbSet;

        public SongController(SampleAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Song>();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var songs = _dbSet.ToList();
            return Ok(songs);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok(_dbSet.FirstOrDefault(s => s.Id == id));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] SongRequest request)
        {
            _dbSet.Add(new Song(request));
            return Ok(_dbContext.SaveChanges());
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] SongRequest request)
        {
            var song = _dbSet.Find(id);
            if (song != null)
            {
                _dbSet.Update(new Song(request));
            }
            return Ok(_dbContext.SaveChanges());
        }

        //policy based
        //[Authorize(Policy = IdentityData.AdminUserPolicyName)]
        //[HttpDelete]
        //public IActionResult Delete([FromRoute] int id)
        //{
        //    var song = _dbSet.Find(id);
        //    if (song != null)
        //    {
        //        _dbSet.Remove(song);
        //    }
        //    return Ok(_dbContext.SaveChanges());
        //}

        //claim based
        [Authorize]
        [RequiresClaimAttributes("admin","true")]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var song = _dbSet.Find(id);
            if (song != null)
            {
                _dbSet.Remove(song);
            }
            return Ok(_dbContext.SaveChanges());
        }
    }
}
