using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateStuff
{
    class Program : IProgram
    {
        static void Main(string[] args)
        {
            

        }
        public string IProgram.Name
        {
            get { return "StamProg"; }
        }
        public object IProgram.Run(object obj) 
        {
            return obj;
        }
    }
}
