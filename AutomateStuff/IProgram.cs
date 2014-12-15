using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateStuff
{
    interface IProgram
    {
        public string Name { get; }

        public object Run(object obj);
    }
}
