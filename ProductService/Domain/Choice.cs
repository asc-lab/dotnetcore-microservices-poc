using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Domain
{
    public class Choice
    {
        public string Code { get; private set; }
        public string Label { get; private set; }

        public ChoiceQuestion Question { get; private set; }

        public Choice()
        { }

        public Choice(string code, string label)
        {
            Code = code;
            Label = label;
        }
    }
}
