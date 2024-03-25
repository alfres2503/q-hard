using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Services;
using src.Utils;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociatesController : ControllerBase
    {
        private readonly IAssociateService _associateService;

        public AssociatesController(IAssociateService associateService)
        {
            _associateService = associateService;
        }

        // 404 is only for when there is no such endpoint or resource, not for when there is no data
        // 204 is the correct status code for when there is no data 
        // to return 204, use NoContent() instead of Ok(null)

        [HttpGet]
        public async Task<ActionResult<PagedResult<Associate>>> GetAssociates(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string orderBy = null)
        {
            try
            {
                var list = await _associateService.GetAll(pageNumber, pageSize, searchTerm, orderBy).ConfigureAwait(false);

                if(list == null)
                    return NoContent();

                var total = await _associateService.GetCount(searchTerm).ConfigureAwait(false);
                var totalPages = (int)Math.Ceiling((double)total / (double)pageSize);

                var result = new PagedResult<Associate>
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
        public async Task<ActionResult<Associate>> GetAssociateById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, status = 400, message = "Invalid ID" });

                var response = await _associateService.GetByID(id).ConfigureAwait(false);

                return response != null ? Ok(response) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Associate>> CreateAssociate([FromBody] Associate associate)
        {
            try
            {
                if (associate == null)
                    return BadRequest(new { success = false, status = 400, message = "Invalid associate" });

                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, status = 400, message = "Invalid associate" });

                var associateByEmail = await _associateService.GetByEmail(associate.Email).ConfigureAwait(false);

                if (associateByEmail != null)
                    return BadRequest(new { success = false, status = 400, message = "Email already exists" });

                var associateByIDCard = await _associateService.GetByIdCard(associate.IdCard).ConfigureAwait(false);

                if (associateByIDCard != null)
                    return BadRequest(new { success = false, status = 400, message = "ID Card already exists" });

                var response = await _associateService.Create(associate).ConfigureAwait(false);

                return response != null ? CreatedAtAction(nameof(GetAssociateById), new { id = response.Id }, response) : StatusCode(StatusCodes.Status500InternalServerError, "Failed to create associate");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Associate>> UpdateAssociate(int id, [FromBody] Associate associate)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, status = 400, message = "Invalid ID" });

                if (associate == null)
                    return BadRequest(new { success = false, status = 400, message = "Invalid associate" });

                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, status = 400, message = "Invalid associate" });

                var memberByEmail = await _associateService.GetByEmail(associate.Email).ConfigureAwait(false);

                if (memberByEmail != null && memberByEmail.Id != associate.Id)
                    return BadRequest(new { success = false, status = 400, message = "Email already exists" });

                var memberByIDCard = await _associateService.GetByIdCard(associate.IdCard).ConfigureAwait(false);

                if (memberByIDCard != null && memberByIDCard.Id != associate.Id)
                    return BadRequest(new { success = false, status = 400, message = "ID Card already exists" });

                var response = await _associateService.Update(id, associate).ConfigureAwait(false);

                return response != null ? AcceptedAtAction(nameof(GetAssociateById), new { id = response.Id }, response) : StatusCode(StatusCodes.Status500InternalServerError, "Failed to update associate");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("state/{id}")]
        public async Task<ActionResult<Associate>> ChangeState(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, status = 400, message = "Invalid ID" });

                var response = await _associateService.ChangeState(id).ConfigureAwait(false);

                return response != null ? AcceptedAtAction(nameof(GetAssociateById), new { id = response.Id }, response) : StatusCode(StatusCodes.Status500InternalServerError, "Failed to change state");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
