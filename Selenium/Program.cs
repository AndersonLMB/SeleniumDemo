using System;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArguments("disable-infobars");
            options.AddArguments("--disable-notifications");
            options.AddArguments("--auto-open-devtools-for-tabs");
            IWebDriver webDriver = new ChromeDriver(options);
            webDriver.Navigate().GoToUrl("http://localhost/ZJG.GisClient/default.aspx?f=mm");
            webDriver.Manage().Timeouts().ImplicitWait = new TimeSpan(10000 * 50000);
            IWebElement element = webDriver.FindElement(By.CssSelector("[data-navid=\"manege_config\"]"));
            element.Click();
            var elements = webDriver.FindElements(By.CssSelector(".subject-organize .main-content a.level0"));
            elements.ToList().ForEach((item) =>
            {
                item.Click();
                Task.Delay(1000).Wait();
                var leftArrowElement = webDriver.FindElement(By.CssSelector(".config2organize .icon-triangle-left"));
                leftArrowElement.Click();

                var confirmDialog = webDriver.SwitchTo().Alert();
                confirmDialog.Accept();
                Task.Delay(1000).Wait();
            });
            Console.ReadLine();
        }
    }
}
