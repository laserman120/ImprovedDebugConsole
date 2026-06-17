using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImprovedDebugConsole.Managers
{
    public enum ParameterType
    {
        BinaryOnOff,
        ItemIdentifier,
        CharacterType,
        WeatherType,
        CustomEnum,
        Integer,
        Float,
        FreeText
    }

    public class ConsoleParameter
    {
        public string Label { get; set; }
        public ParameterType Type { get; set; }
        public bool IsOptional { get; set; }
        public Func<string, List<string>> SuggestionSource { get; set; }

        public ConsoleParameter(string label, ParameterType type, bool isOptional = false, Func<string, List<string>> customSource = null)
        {
            Label = label;
            Type = type;
            IsOptional = isOptional;
            SuggestionSource = customSource ?? GetDefaultSourceForType(type);
        }

        private Func<string, List<string>> GetDefaultSourceForType(ParameterType type)
        {
            switch (type)
            {
                case ParameterType.BinaryOnOff:
                    return (input) => new List<string> { "on", "off", "true", "false" };
                default:
                    return (input) => null;
            }
        }
    }

    public class CommandDescriptor
    {
        public string CommandName { get; set; }
        public List<ConsoleParameter> Parameters { get; set; } = new List<ConsoleParameter>();
    }
}
