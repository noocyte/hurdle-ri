using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using WebAPI.Backend.Models;

namespace WebAPI.Backend.Controllers
{
    public class IncidentController : ApiController
    {
        private readonly IIncidentRepository _repository;

        public IncidentController(IIncidentRepository repository)
        {
            _repository = repository;
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
            return await CreateOrUpdateAsync(obj, dto, _repository.CreateAsync);
        }

        [Route("api/incident/{company}/{id}")]
        public async Task<IHttpActionResult> Patch(string company, string id, [FromBody] Incident obj)
        {
            var dto = new IncidentDto(company, id) {ETag = "*"}; // consider etag in header, not impl. now
            return await CreateOrUpdateAsync(obj, dto, _repository.UpdateAsync);
        }

        private async Task<IHttpActionResult> CreateOrUpdateAsync(Incident obj, IncidentDto dto,
            Func<IncidentDto, Task<TableResponse<IncidentDto>>> createOrUpdateFunc)
        {
            Mapper.Map(obj, dto);

            var result = await createOrUpdateFunc(dto);
            if (result.Success)
                return Ok(result.Result);
            return StatusCode(result.Status);
        }
    }
}