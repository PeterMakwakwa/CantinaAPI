using Asp.Versioning;
using CantinaAPI.CustomActionFilters;
using CantinaAPI.Models;
using CantinaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CantinaAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets reviews by item ID.
        /// </summary>
        /// <param name="itemId">The ID of the item.</param>
        /// <returns>A list of reviews for the specified item.</returns>
        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetReviewsByItemId(int itemId)
        {
            var reviews = await _service.GetReviewsByItemIdAsync(itemId);
            if (reviews ==null)
            {
                return NotFound();
            }
            return Ok(reviews);
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="review">The review to create.</param>
        /// <returns>The created review.</returns>
        [HttpPost]
        public async Task<ActionResult<ReviewModel>> CreateReview([FromBody] ReviewModel review)
        {
            if (review == null)
            {
                return BadRequest("Review cannot be null.");
            }

            await _service.AddReviewAsync(review);
            return CreatedAtAction(nameof(GetReviewsByItemId), new { itemId = review.ItemId }, review);
        }
    }
}