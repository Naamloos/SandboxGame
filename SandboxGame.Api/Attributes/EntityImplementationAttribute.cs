using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityImplementationAttribute : Attribute
    {
        public string UniqueIdentifier { get; }
        public float Width { get; }
        public float Height { get; }

        public EntityImplementationAttribute(string uniqueIdentifier, float width, float height)
        {
            UniqueIdentifier = uniqueIdentifier;
            Width = width;
            Height = height;
        }
    }
}
