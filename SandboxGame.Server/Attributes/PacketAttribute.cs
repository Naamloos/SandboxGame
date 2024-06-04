using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketAttribute : Attribute
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public PacketAttribute(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
