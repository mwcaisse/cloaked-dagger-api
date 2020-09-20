using System;
using System.Collections.Generic;
using OwlTin.Common.Exceptions;

namespace CloakedDagger.Common.Domain
{
    public class Resource : IDomainModel
    {
        public Guid Id { get; }
        
        public string Key => Id.ToString();
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        private readonly List<Scope> _availableScopes;

        public IEnumerable<Scope> AvailableScopes => _availableScopes.AsReadOnly();

        public Resource(string name, string description = null)
        {
            ValidateName(name);

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        public void Rename(string name)
        {
            ValidateName(name);

            Name = name;
        }

        public void Redescribe(string description)
        {
            Description = description;
        }

        public void AddScope(Scope scope)
        {
            _availableScopes.Add(scope);
        }

        public void RemoveScope(Scope scope)
        {
            _availableScopes.Remove(scope);
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new EntityValidationException("Name must not be blank.");
            }

            if (name.Length > 250)
            {
                throw new EntityValidationException("Name must not be more than 250 characters.");
            }
        }

        public override bool Equals(object? obj)
        {
            var otherResource = obj as Resource;

            if (null == otherResource)
            {
                return false;
            }

            return Id.Equals(otherResource.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}