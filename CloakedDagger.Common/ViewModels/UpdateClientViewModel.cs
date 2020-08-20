using System;

namespace CloakedDagger.Common.ViewModels
{
    public class UpdateClientViewModel : CreateClientViewModel
    {
        public Guid ClientId { get; set; }
    }
}