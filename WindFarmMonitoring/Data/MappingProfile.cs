using AutoMapper;
using WindFarmMonitoring.Dto;
using WindFarmMonitoring.Models;

namespace WindFarmMonitoring
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SensorData, SensorDataReadDto>();
        }
    }
}