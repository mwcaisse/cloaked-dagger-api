using System;
using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common.Domain.Events.Client;

namespace CloakedDagger.Common.Repositories
{
    public interface IClientEventRepository
    {
        /// <summary>
        /// Fetches all events for the given client in the order that they occurred.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IEnumerable<ClientDomainEvent> GetClientEvents(Guid clientId);

        /// <summary>
        /// Saves the list of client events
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="events"></param>
        void SaveClientEvents(Guid clientId, IEnumerable<ClientDomainEvent> events);
    }
}