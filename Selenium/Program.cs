using System;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DeleteThemes();
            Console.ReadLine();
        }

        public static void DebindThemes()
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

        public static void DeleteThemes()
        {
            ChromeOptions options = new ChromeOptions();
            IWebDriver webDriver = new ChromeDriver(options);
            webDriver.Navigate().GoToUrl("http://localhost/ZJG.GisClient/default.aspx?f=mm");
            webDriver.Manage().Timeouts().ImplicitWait = new TimeSpan(10000 * 50000);
            IWebElement element = webDriver.FindElement(By.CssSelector("[data-navid=\"manege_config\"]"));
            //checkbox_false_full
            //subject-config 
            element.Click();




            var elements = webDriver.FindElements(By.CssSelector(".subject-config .checkbox_false_full"));
            elements.ToList().ForEach((checkbox) =>
            {
                checkbox.Click();
                Task.Delay(500).Wait();
            });
            //icon-wh-trash

            IWebElement trashBinElement = webDriver.FindElement(By.CssSelector(".subject-config .icon-wh-trash"));


            trashBinElement.Click();


            var confirmDialog = webDriver.SwitchTo().Alert();
            Task.Delay(200).Wait();
            confirmDialog.Accept();





        }

    }
}
