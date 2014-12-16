using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateStuff
{
    interface IProgram
    {
        string Name { get; }

        object Run(object obj);
    }
}
