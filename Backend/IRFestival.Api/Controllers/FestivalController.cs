using IRFestival.Api.Data;
using IRFestival.Api.Domain;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IRFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FestivalController : ControllerBase
    {
        private readonly FestivalDbContext _ctx;

        private readonly TelemetryClient telemetryClient;

        public FestivalController(FestivalDbContext context, TelemetryClient telemetryClient)
        {
            _ctx = context;
            this.telemetryClient = telemetryClient;
        }

        [HttpGet("LineUp")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Schedule))]
        public async Task<ActionResult> GetLineUp()
        {
            //var lineUp = await _ctx.Schedules.Include(x => x.Items)
            //                                              .ThenInclude(x => x.Artist)
            //                                              .Include(x => x.Items)
            //                                              .ThenInclude(x => x.Stage)
            //                                              .FirstOrDefaultAsync();
            throw new ApplicationException("LineUp failed!");
            return Ok(FestivalDataSource.Current.LineUp);
            //return Ok(lineUp);
        }

        [HttpGet("Artists")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Artist>))]
        public async Task<ActionResult> GetArtists(bool? withRatings)
        {
            if (withRatings.HasValue && withRatings.Value)
                telemetryClient.TrackEvent($"list of artists with ratings");
            else
                telemetryClient.TrackEvent($"List of artists without ratings");
            return base.Ok(FestivalDataSource.Current.Artists);



            var artists = await _ctx.Artists.ToListAsync();
            return Ok(artists);
        }

        [HttpGet("Stages")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Stage>))]
        public async Task<ActionResult> GetStages()
        {
            var stages = await _ctx.Stages.ToListAsync();
            return Ok(stages);
        }

        [HttpPost("Favorite")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ScheduleItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> SetAsFavorite(int id)
        {
            var schedule = await _ctx.ScheduleItems.FirstOrDefaultAsync(si => si.Id == id);
            if (schedule != null)
            {
                schedule.IsFavorite = !schedule.IsFavorite;
                return Ok(schedule);
            }
            return NotFound();
        }

    }
}