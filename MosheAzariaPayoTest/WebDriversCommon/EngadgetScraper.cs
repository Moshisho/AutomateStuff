using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace WebDriversCommon
{
    public class EngadgetScraper
    {
        public const string Homepage = "http://endgadget.com";

        public List<Article> GetPopularArticles(IWebDriver driver)
        {
            return GetAllArticlesWithCommentsNumber(driver).Where(x => x.commentsCount >= 10).ToList();
        }

        public List<Article> GetAllArticlesWithCommentsNumber(IWebDriver driver)
        {
            List<Article> articles = new List<Article>();

            driver.Navigate().GoToUrl(Homepage);

            List<IWebElement> articleElements = driver.FindElements(By.TagName("article")).ToList();
            articleElements.AddRange(GetListItemsArticleElements(driver.FindElement(By.Id("hot-topics"))));
            //articleElements.AddRange(GetListItemsArticleElements(driver.FindElement(By.Id("dynamic-lede"))));
            //articleElements.AddRange(GetListItemsArticleElements(driver.FindElement(By.Id("review-list"))));

            foreach (IWebElement article in articleElements)
            {
                if (article.GetAttribute("class") == "feature") //no comments num
                    continue;

                int comments = GetCommentsCount(article);
                string linkToArticle = GetLinkForArticle(article);
                articles.Add(new Article(linkToArticle, comments));
            }

            return articles;
        }

        public Article GetRandomArticle(List<Article> articles, IWebDriver driver)
        {
            return new Article(driver, articles.OrderBy(g => Guid.NewGuid()).First().link);
        }

        private int GetCommentsCount(IWebElement article)
        {
            Match m = Regex.Match(article.Text, @"(?<cnum>\d+) COMMENTS");

            if (m.Success)
                return int.Parse(m.Groups["cnum"].Value);
            else if (article.FindElements(By.CssSelector("[class='livefyre-commentcount confab-comment-count-mr'")).Count > 0)
                return int.Parse(article.FindElement(By.CssSelector("[class='livefyre-commentcount confab-comment-count-mr'")).Text);
            else if (article.FindElements(By.CssSelector("[class='confab-comment-count-rail livefyre-commentcount'")).Count > 0)
                return int.Parse(article.FindElement(By.CssSelector("[class='confab-comment-count-rail livefyre-commentcount'")).Text);
            else
                throw new Exception("Could not find comments number.");
        }

        private string GetLinkForArticle(IWebElement article)
        {
            return article.FindElement(By.TagName("a")).GetAttribute("href");
        }

        private List<IWebElement> GetListItemsArticleElements(IWebElement webElement)
        {
            return webElement.FindElements(By.TagName("li")).ToList();
        }

        public class Article
        {
            public string header;
            public string body;
            public List<string> popularComments;
            public int commentsCount;
            public int maxLikes;
            public string link;

            public Article(string aLink, int comments)
            {
                commentsCount = comments;
                link = aLink;
            }

            public Article(IWebDriver driver, string aLink)
            {
                link = aLink;
                driver.Navigate().GoToUrl(aLink);
                header = driver.FindElement(By.CssSelector("[class='header container'")).Text;
                body = driver.FindElement(By.ClassName("article-content")).Text.
                    Remove(driver.FindElement(By.ClassName("article-content")).Text.IndexOf("Please login to comment"));
                popularComments = GetPopularComments(driver, out maxLikes);
            }

            private List<string> GetPopularComments(IWebDriver driver, out int maxlikes)
            {
                if (driver.FindElements(By.ClassName("confab-load-more")).Count > 0)
                    driver.FindElement(By.ClassName("confab-load-more")).Click();

                List<string> popComments = new List<string>();

                maxlikes = 0;
                foreach (IWebElement comment in driver.FindElements(By.ClassName("confab-comment")))
                {
                    int currentLikes = int.Parse(comment.FindElement(By.ClassName("confab-like")).Text);
                    if (currentLikes > maxlikes)
                    {
                        maxLikes = currentLikes;
                        popComments.Clear();
                        popComments.Add(comment.FindElement(By.ClassName("confab-comment-body")).Text);
                    }
                    else if (currentLikes == maxlikes)
                        popComments.Add(comment.FindElement(By.ClassName("confab-comment-body")).Text);
                }
                return popComments;
            }
        }
    }
}
