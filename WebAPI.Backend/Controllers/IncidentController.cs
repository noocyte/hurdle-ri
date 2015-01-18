using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage.Table;
using WebAPI.Backend.Models;

namespace WebAPI.Backend.Controllers
{
    public class IncidentController : ApiController
    {
        private readonly IncidentRepository _repository;

        public IncidentController() // ninject!!
        {
            _repository = new IncidentRepository(null);
        }

        [Route("api/incident/{company}/{id}")]
        public async Task<IHttpActionResult> Get(string company, string id)
        {
            var result = await _repository.GetByIdAsync(company, id);
            if (result.Success)
                return Ok(result.Result);
            return StatusCode(result.Status);
        }

        [Route("api/incident/{company}/{id}")]
        public async Task<IHttpActionResult> Post(string company, string id, [FromBody] Incident obj)
        {
            var dto = new IncidentDto(company, id) // automapper!
            {
                Deadline = obj.Deadline,
                Description = obj.Description,
                Title = obj.Title,
                Status = obj.Status
            };

            var result = await _repository.CreateAsync(dto);
            if (result.Success)
                return Ok(result.Result);
            return StatusCode(result.Status);
        }
    }
}