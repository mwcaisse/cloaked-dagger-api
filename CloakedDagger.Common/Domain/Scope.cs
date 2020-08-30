using System;
using System.Security.Cryptography;
using CloakedDagger.Common.ViewModels;
using OwlTin.Common.Exceptions;

namespace CloakedDagger.Common.Domain
{
    public class Scope : IDomainModel
    {
        public string Name { get; }
        
        public string Description { get; private set; }

        public string Key => Name;
        
        public Scope(string name, string description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new EntityValidationException("Name must not be blank!");
            }
            
            Name = name;
            Description = description;
        }

        public void Redescribe(string description)
        {
            this.Description = description;
        }
        
        public override bool Equals(object? obj)
        {
            var otherScope = obj as Scope;

            if (null == otherScope)
            {
                return false;
            }

            return string.Equals(otherScope.Name, Name, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}