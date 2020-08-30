using System;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.ViewModels;
using OwlTin.Common.Exceptions;

namespace CloakedDagger.Common.Domain
{
    public class ClientUri : IDomainModel
    {
        public Guid Id { get; internal set; }

        public string Key => Id.ToString();
        
        public ClientUriType Type { get;  internal set; }
        
        public string Uri { get; internal set; }

   

        public static void ValidateUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new EntityValidationException("Uri must not be blank!");
            }
        }
        public override bool Equals(object? obj)
        {
            var otherClientUri = obj as ClientUri;

            if (null == otherClientUri)
            {
                return false;
            }

            return Id.Equals(otherClientUri.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}