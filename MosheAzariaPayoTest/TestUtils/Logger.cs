using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TestUtils
{
    public class Logger
    {
        private static object lockObj = new object();

        public static string LogPath
        {
            get
            {
                return "log.txt";
            }
        }

        public Logger()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        public static void Log(params object[] message)
        {
            StackFrame frame = new StackFrame(2);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sb.Append("] [");
            sb.Append(type.Name);
            sb.Append("] [");
            sb.Append(name);
            sb.Append("] ");
            foreach (object mo in message)
            {
                sb.Append(mo.ToString());
                sb.Append(" ");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);

            lock (lockObj)
            {
                using (StreamWriter sw = File.AppendText(LogPath))
                {
                    sw.Write(sb.ToString());
                }
                Console.WriteLine(sb.ToString());
            }
        }
    }
}
