using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using TestUtils;
using WebDriversCommon;

namespace MosheAzariaPayoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 1)
                Help();

            string browserName = args[0];

            if (!CommonWebDriver.SupportedBrowsers.Any(x => x.ToLower() == browserName.ToLower()))
                Help();

            Logger.Log("Test Started, running test with ", browserName, "...");

            EngadgetScraper endgScraper = new EngadgetScraper();
            IWebDriver driver = CommonWebDriver.CreateWebDriver(browserName);

            List<EngadgetScraper.Article> popularArticles = endgScraper.GetPopularArticles(driver);
            EngadgetScraper.Article randomArticle = endgScraper.GetRandomArticle(popularArticles, driver);

            GmailHandler gh = new GmailHandler();
            gh.SignIntoGmail(driver);
            /*page objects - gmail and endgadget in webdrivercommon
            
            single(new guid order by)*/
            driver.Quit();
        }

        private static void Help()
        {
            Console.WriteLine("Usage: MosheAzariaPayoTest.exe browserName");
            Console.WriteLine("browserName = IE || Firefox || Chrome");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
            Environment.Exit(-1);
        }
    }
}
