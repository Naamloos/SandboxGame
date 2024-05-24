using System;
using System.Linq;
using System.Reflection;

namespace SandboxGame.Engine
{
    public class LaunchArgumentParser
    {
        string[] args;
        public LaunchArgumentParser(string[] args)
        {
            this.args = args;
        }

        public LaunchArgs Parse()
        {
            var launchArgs = new LaunchArgs();

            var options = typeof(LaunchArgs).GetProperties().Where(x => x.GetCustomAttribute<LaunchArgAttribute>() != null)
                .Select(prop => (prop.GetCustomAttribute<LaunchArgAttribute>().OptionName, prop));

            for(var i = 0; i < args.Length; i++)
            {
                if (!args[i].StartsWith("--"))
                {
                    Console.WriteLine("Unknown Argument: {0}", args[i]);
                    continue;
                }

                var parsedArg = args[i].Substring(2);
                var foundOption = options.FirstOrDefault(x => x.Item1 == parsedArg);
                if(foundOption == default)
                {
                    Console.WriteLine("Unknown Argument: {0}", args[i]);
                    continue;
                }

                if(foundOption.prop.PropertyType == typeof(bool))
                {
                    // option was found, inverted prop value
                    foundOption.prop.SetValue(launchArgs, !((bool)foundOption.prop.GetValue(launchArgs)));
                    Console.WriteLine("Found valid command line argument: {0}", args[i]);
                    continue;
                }

                if(foundOption.prop.PropertyType == typeof(string))
                {
                    if (args.Length == i + 1 || args[i + 1].StartsWith("--"))
                    {
                        Console.WriteLine("No data given for argument {0}", args[i]);
                        continue;
                    }

                    Console.WriteLine("Found valid command line argument: {0} and set it's value to: {1}", args[i], args[i+1]);

                    foundOption.prop.SetValue(launchArgs, args[i + 1]);
                    i++;
                    continue;
                }

                Console.WriteLine("Unknown Argument: {0}", args[i]);
            }

            return launchArgs;
        }
    }

    public class LaunchArgs
    {
        [LaunchArg("unlock-fps")] // so --unlock-fps, takes no argument
        public bool UnlockFPS { get; set; } = false;

        [LaunchArg("force-new-world")]
        public bool ForceNewWorldGen { get; set; } = false;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LaunchArgAttribute : Attribute
    {
        public string OptionName {  get; private set; }

        public LaunchArgAttribute(string optionName) 
        {
            OptionName = optionName;
        }
    }
}
