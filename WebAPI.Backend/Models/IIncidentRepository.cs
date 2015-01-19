using System.Threading.Tasks;

namespace WebAPI.Backend.Models
{
    public interface IIncidentRepository
    {
        Task<TableResponse<IncidentDto>> GetByIdAsync(string company, string id);
        Task<TableResponse<IncidentDto>> GetAllAsync(string company);
        Task<TableResponse<IncidentDto>> CreateAsync(IncidentDto incident);
        Task<TableResponse<IncidentDto>> UpdateAsync(IncidentDto incident);
    }
}