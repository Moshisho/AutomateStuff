using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MyAutomation
{
    public partial class MyAutomation : Form
    {
        private readonly string harmonyUrl = "http://finger/eharmony/web/frmLogin.asp?toLng=RTL&selLng=Hebrew";
        private  static string curChooseSite;

        public MyAutomation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ChooseSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            curChooseSite = ChooseSite.SelectedItem.ToString();

            //AddInputForSite(curChooseSite);
        }

        private void AddInputForSite(string curChooseSite)
        {
            throw new NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (curChooseSite)
            {
                case "finger harmony":
                    string message = RunFingerHarmonyAutomation();
                    MessageBox.Show(message);
                    break;
                default:
                    return;
            }
        }

        private string RunFingerHarmonyAutomation()
        {
            string msg = "";
            ChromeDriver crBrowser = new ChromeDriver();

            crBrowser.Navigate().GoToUrl(harmonyUrl);
            IWebElement userId = crBrowser.FindElementById("TXTUSERID");
            IWebElement passwd = crBrowser.FindElementById("TXTPASSWD");
            ReadOnlyCollection<IWebElement> radioName = crBrowser.FindElements(By.Name("RADGRLOGIN"));
            IWebElement enterBtn = crBrowser.FindElementById("BTNENTER");

            radioName[1].Click();
            Thread.Sleep(500);

            userId.SendKeys("משה עזריה");
            passwd.SendKeys("36675155");
            
            Thread.Sleep(500);
            enterBtn.Click();
            Thread.Sleep(1500);

            IWebElement idkun = crBrowser.FindElementById("mn_0").FindElement(By.XPath(@"./div[2]//td"));
            idkun.Click();

            msg = CalculatePageHours(msg, crBrowser);

            crBrowser.Dispose();

            return msg;
        }

        //refactor this.
        private string CalculatePageHours(string msg, ChromeDriver crBrowser)
        {
            IWebDriver tableiframe = crBrowser.SwitchTo().Frame("myIframe");

            ReadOnlyCollection<IWebElement> workTimeRows = tableiframe.FindElements(By.ClassName("gridtextbold"));
            Collection<string> workTimeRowsFiltered = new Collection<string>();

            foreach (IWebElement e in workTimeRows)
            {
                if (e.GetAttribute("Id") != null)
                    if (e.GetAttribute("Id").Contains("row"))
                        workTimeRowsFiltered.Add(e.Text);
            }

            List<int> minutes = null;
            foreach (string s in workTimeRowsFiltered)
            {
                DateTime date;
                DateTime.TryParse(s.Remove(10), out date);
                if (date.Day.Equals(20)) //doesn't work
                    minutes = new List<int>();
                else if (minutes == null)
                    continue;
                else if (DateTime.Today.Date == date)
                    continue;
                else if (s.Contains("מאושר"))
                    continue;

                Match m = Regex.Match(s, @"(\(-?\d\d\:\d\d\))");
                string[] hoursMinutes = m.Value.Split(':');
                int min;
                int hours;
                int.TryParse(hoursMinutes[1].Remove(2), out min);
                int.TryParse(hoursMinutes[0].Remove(0, 1), out hours);

                min += hours * 60;

                if (m.ToString().Contains('-'))
                    min = -min;
                
                minutes.Add(min);
            }
            TimeSpan totMins = TimeSpan.FromMinutes((double)minutes.Take(minutes.Count).Sum());

            // TODO user and password input.
            // TODO Add support for 2 pages.
            // TODO add timed message box for waiting for results.

            msg = "Total work hours result: " + totMins.ToString();
            return msg;
        }
    }
}
