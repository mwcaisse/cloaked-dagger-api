using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CloakedDagger.Common.Domain;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Mapper
{
    public static class ClientMapper
    {
        
        private static readonly MapperConfiguration _mapperConfiguration = new MapperConfiguration(config =>
        {
            config.CreateMap<Client, ClientViewModel>();
            config.CreateMap<ClientUri, ClientUriViewModel>();
        });

        private static readonly IMapper _mapper = new AutoMapper.Mapper(_mapperConfiguration);
        
        public static ClientViewModel ToViewModel(this Client client)
        {
            return _mapper.Map<Client, ClientViewModel>(client);
        }

        public static IEnumerable<ClientViewModel> ToViewModel(this IEnumerable<Client> clients)
        {
            return clients.Select(x => x.ToViewModel());
        }
    }
}