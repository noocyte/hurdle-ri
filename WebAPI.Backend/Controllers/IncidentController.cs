using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Shared.Models;
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

        [Route("api/{company}/incident/")]
        public async Task<IHttpActionResult> GetAll(string company)
        {
            var result = await _repository.GetAllAsync(company);
            return HandleResponse(result);
        }

        [Route("api/{company}/incident/{id}")]
        public async Task<IHttpActionResult> GetOne(string company, string id)
        {
            var result = await _repository.GetByIdAsync(company, id);
            return HandleResponse(result);
        }

        [Route("api/{company}/incident/{id}")]
        public async Task<IHttpActionResult> Put(string company, string id, [FromBody] Incident obj)
        {
            var dto = new IncidentDto(company, id) {ETag = "*"}; // todo consider etag in header, not impl. now
            return await CreateOrUpdateAsync(obj, dto, _repository.UpdateAsync);
        }

        private async Task<IHttpActionResult> CreateOrUpdateAsync(Incident obj, IncidentDto dto,
            Func<IncidentDto, Task<TableResponse<IncidentDto>>> createOrUpdateFunc)
        {
            Mapper.Map(obj, dto);
            var result = await createOrUpdateFunc(dto);
            return HandleResponse(result);
        }

        private IHttpActionResult HandleResponse(TableResponse<IncidentDto> result)
        {
            if (!result.Success) return StatusCode(result.Status);

            var dto = result.Result;
            var incident = Mapper.Map<Incident>(dto);
            return Ok(incident);
        }
    }
}