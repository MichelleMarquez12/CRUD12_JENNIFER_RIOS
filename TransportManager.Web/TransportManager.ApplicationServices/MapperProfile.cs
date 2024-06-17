using TransportManager.Core.Transports;
using AutoMapper;

namespace TransportManager.ApplicationServices
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Journeys, JourneyDto>();
            CreateMap<JourneyDto, Journeys>();

            CreateMap<Passengers, PassengerDto>();
            CreateMap<PassengerDto, Passengers>();

            CreateMap<Tickets, TicketDto>();
            CreateMap<TicketDto, Tickets>();
        }
    }
}
