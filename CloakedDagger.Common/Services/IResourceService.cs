using System;
using System.Collections;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IResourceService
    {

        public Resource Get(Guid id);

        public IEnumerable GetAll();

        public Resource Create(ResourceViewModel resource);

        public Resource Update(ResourceViewModel resource);
        
        public void Delete(Guid id);

    }
}