using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;


namespace UIAutomationTry
{
    public class GetWindowsHandler
    {
        public AutomationElementCollection GetAllDesktopWindows() 
        {
            AutomationElementCollection winCollection = AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition);

            foreach (AutomationElement ele in winCollection) 
            {
                Console.WriteLine("--------Element");
                Console.WriteLine("AutomationId: {0}", ele.Current.AutomationId);
                Console.WriteLine("Name: {0}", ele.Current.Name);
                Console.WriteLine("ClassName: {0}", ele.Current.ClassName);
                Console.WriteLine("ControlType: {0}", ele.Current.ControlType.ProgrammaticName);
                Console.WriteLine("IsEnabled: {0}", ele.Current.IsEnabled);
                Console.WriteLine("IsOffscreen: {0}", ele.Current.IsOffscreen);
                Console.WriteLine("ProcessId: {0}", ele.Current.ProcessId);
            }

            int processID = 4432; //look up at your applications processID with your windows taskexplorer under processes (PID)
            Condition yourCondition = new PropertyCondition(AutomationElement.ProcessIdProperty, processID);
            AutomationElement mainWindow = AutomationElement.RootElement.FindFirst(TreeScope.Element | TreeScope.Children, yourCondition);

            //string buttonId; your Button ID
            //Condition yourCondition2 = new PropertyCondition(AutomationElement.AutomationId, buttonId);
            //AutomationElement yourButton = rootElement.FindFirst(TreeScope.Element | TreeScope.Descendants, yourCondition2);

            return winCollection;
        }
        
    }
}
