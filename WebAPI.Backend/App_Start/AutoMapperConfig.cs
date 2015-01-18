using AutoMapper;
using Shared.Models;
using WebAPI.Backend.Models;

namespace WebAPI.Backend.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Incident, IncidentDto>();
            Mapper.CreateMap<IncidentDto, Incident>();
        }
    }
}