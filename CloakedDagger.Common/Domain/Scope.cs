using System;

namespace CloakedDagger.Common.Domain
{
    public class Scope
    {
        public string Name { get; private set; }
        
        public string Description { get; private set; }

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