using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using WebAPI.Backend.Models;

namespace WebAPI.Backend.Controllers
{
    public class IncidentController : ApiController
    {
        private readonly IncidentRepository _repository;

        public IncidentController() // todo ninject
        {
            _repository = new IncidentRepository(null);
        }

        [Route("api/incident/{company}/{id}")]
        public async Task<IHttpActionResult> Get(string company, string id)
        {
            var result = await _repository.GetByIdAsync(company, id);
            if (!result.Success) return StatusCode(result.Status);

            var dto = result.Result;
            var incident = Mapper.Map<Incident>(dto);
            return Ok(incident);
        }

        [Route("api/incident/{company}/{id}")]
        public async Task<IHttpActionResult> Post(string company, string id, [FromBody] Incident obj)
        {
            var dto = new IncidentDto(company, id);
            Mapper.Map(obj, dto);

            var result = await _repository.CreateAsync(dto);
            if (result.Success)
                return Ok(result.Result);
            return StatusCode(result.Status);
        }

        [Route("api/incident/{company}/{id}")]
        public async Task<IHttpActionResult> Patch(string company, string id, [FromBody] Incident obj)
        {
            var dto = new IncidentDto(company, id) {ETag = "*"};
            Mapper.Map(obj, dto);

            var result = await _repository.UpdateAsync(dto);
            if (result.Success)
                return Ok(result.Result);
            return StatusCode(result.Status);
        }
    }
}