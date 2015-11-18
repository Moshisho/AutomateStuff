using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDriversCommon
{
    public class GmailHandler
    {
        public const string Homepage = "http://gmail.com";

        public void SignIntoGmail(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(Homepage);

            //enter user mail miron.jango
            // id=Email

        }
    }
}
