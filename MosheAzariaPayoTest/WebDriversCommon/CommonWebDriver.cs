using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDriversCommon
{
    public class CommonWebDriver
    {
        public static List<string> SupportedBrowsers = new List<string>
        {
            "IE",
            "Firefox",
            "Chrome"
        };

        public static IWebDriver CreateWebDriver(string browser)
        {
            switch (browser.ToLower())
            {
                case "ie":
                case "internet explorer":
                case "internetexplorer":
                    return new InternetExplorerDriver();
                case "firefox":
                    return new FirefoxDriver();
                case "chrome":
                default:
                    return new ChromeDriver();
            }
        }
    }
}
