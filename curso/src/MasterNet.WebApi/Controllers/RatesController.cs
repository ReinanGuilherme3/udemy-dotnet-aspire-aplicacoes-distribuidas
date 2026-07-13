using Core.MediatOR.Contracts;
using MasterNet.Application.Contracts;
using MasterNet.Application.Core;
using MasterNet.Application.Ratings.GetRatings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static MasterNet.Application.Ratings.GetRatings.GetRatingsQuery;

namespace MasterNet.WebApi.Controllers;

[ApiController]
[Route("api/ratings")]
public class RatesController : ControllerBase
{
    private readonly IMediatOR _sender;
    private readonly IRatingServiceHttpClient _ratingServiceHttpClient;

    public RatesController(IMediatOR sender, IRatingServiceHttpClient ratingServiceHttpClient)
    {
        _sender = sender;
        _ratingServiceHttpClient = ratingServiceHttpClient;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedList<RatingResponse>>> PaginationRatings
    (
        [FromQuery] GetRatingsRequest request,
        CancellationToken cancellationToken
    )
    {
        var query = new GetRatingsQueryRequest
        {
            RatingsRequest = request
        };
        var results = await _sender.Send(query, cancellationToken);
        return results.IsSuccess ? Ok(results.Value) : NotFound();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SendRating([FromBody] SendRatingRequest request, CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(request.Id) || request.Rating < 1 || request.Rating > 5)
        {
            return BadRequest("Rating must be between 1 and 5.");
        }
        await _ratingServiceHttpClient.SendRating(request.Id, request.Rating);
        return Ok();
    }

}

public record SendRatingRequest(string Id, int Rating);