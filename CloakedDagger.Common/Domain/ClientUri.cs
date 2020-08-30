using System;
using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.Domain
{
    public class ClientUri
    {
        public ClientUriType Type { get; private set; }
        
        public string Uri { get; private set; }

        public override bool Equals(object? obj)
        {
            var otherClientUri = obj as ClientUri;

            if (null == otherClientUri)
            {
                return false;
            }

            return otherClientUri.Type == this.Type &&
                   otherClientUri.Uri == this.Uri;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Type, Uri);
        }
    }
}