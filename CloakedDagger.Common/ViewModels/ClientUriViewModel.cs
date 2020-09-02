using System;
using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.ViewModels
{
    public class ClientUriViewModel
    {
        public Guid Id { get; set; }

        public ClientUriType Type { get; set; }
        
        public string Uri { get; set; }
    }
}