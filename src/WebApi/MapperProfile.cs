using AutoMapper;
using Weather.Domain.Interfaces;
using Weather.WebApi.ViewModels;

namespace Weather.WebApi
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<IForecast, ForecastVM>()
                .ForMember(z => z.Provider, opt => opt.Ignore());
        }
    }
}
