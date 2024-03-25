using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Services;
using src.Utils;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CateringServicesController : ControllerBase
    {
        private readonly ICateringServiceService _cateringserviceService;

        public CateringServicesController(ICateringServiceService cateringserviceService)
        {
            _cateringserviceService = cateringserviceService;
        }

        // 404 is only for when there is no such endpoint or resource, not for when there is no data
        // 204 is the correct status code for when there is no data 
        // to return 204, use NoContent() instead of Ok(null)

        [HttpGet]
        public async Task<ActionResult<PagedResult<CateringService>>> GetCateringServices(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string orderBy = null)
        {
            try
            {
                var list = await _cateringserviceService.GetAll(pageNumber, pageSize, searchTerm, orderBy).ConfigureAwait(false);

                if(list == null)
                    return NoContent();

                var total = await _cateringserviceService.GetCount(searchTerm).ConfigureAwait(false);
                var totalPages = (int)Math.Ceiling((double)total / (double)pageSize);

                var result = new PagedResult<CateringService>
                {
                    List = list,
                    TotalPages = totalPages,
                    TotalRecords = total
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CateringService>> GetCateringServiceById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, status = 400, message = "Invalid ID" });

                var response = await _cateringserviceService.GetByID(id).ConfigureAwait(false);

                return response != null ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CateringService>> CreateCateringService([FromBody] CateringService catering_service)
        {
            try
            {
                if (catering_service == null)
                    return BadRequest(new { success = false, status = 400, message = "Invalid Catering Service" });

                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, status = 400, message = "Invalid Catering Service" });

                var catering_serviceByEmail = await _cateringserviceService.GetByEmail(catering_service.Email).ConfigureAwait(false);

                if (catering_serviceByEmail != null)
                    return BadRequest(new { success = false, status = 400, message = "Email already exists" });

                var response = await _cateringserviceService.Create(catering_service).ConfigureAwait(false);

                return response != null ? CreatedAtAction(nameof(GetCateringServiceById), new { id = response.Id }, response) : StatusCode(StatusCodes.Status500InternalServerError, "Failed to create Catering Service");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CateringService>> UpdateCateringService(int id, [FromBody] CateringService catering_service)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, status = 400, message = "Invalid ID" });

                if (catering_service == null)
                    return BadRequest(new { success = false, status = 400, message = "Invalid Catering Service" });

                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, status = 400, message = "Invalid Catering Service" });

                var catering_serviceByEmail = await _cateringserviceService.GetByEmail(catering_service.Email).ConfigureAwait(false);

                if (catering_serviceByEmail != null && catering_serviceByEmail.Id != catering_service.Id)
                    return BadRequest(new { success = false, status = 400, message = "Email already exists" });

                var response = await _cateringserviceService.Update(id, catering_service).ConfigureAwait(false);

                return response != null ? AcceptedAtAction(nameof(GetCateringServiceById), new { id = response.Id }, response) : StatusCode(StatusCodes.Status500InternalServerError, "Failed to update Catering Service");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("state/{id}")]
        public async Task<ActionResult<CateringService>> ChangeState(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, status = 400, message = "Invalid ID" });

                var response = await _cateringserviceService.ChangeState(id).ConfigureAwait(false);

                return response != null ? AcceptedAtAction(nameof(GetCateringServiceById), new { id = response.Id }, response) : StatusCode(StatusCodes.Status500InternalServerError, "Failed to change state");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
