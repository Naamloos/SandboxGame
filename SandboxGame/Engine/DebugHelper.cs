using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine
{
    public static class DebugHelper
    {
        private static Dictionary<string, string> debugStrings = new();

        public static void SetDebugValues(string name, string value)
        {
            if(debugStrings.ContainsKey(name))
            {
                debugStrings[name] = value;
                return;
            }

            debugStrings.Add(name, value);
        }

        public static string GetDebugString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"[ DEBUG ]");

            foreach(var value in debugStrings)
            {
                sb.AppendLine($"{value.Key}: {value.Value}");
            }

            return sb.ToString();
        }
    }
}
