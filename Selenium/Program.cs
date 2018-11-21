using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;

namespace SeleniumDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {

            BatchAddSingleResourceTheme();
            //var list = ListAction();
            //AddResource(list);

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
            element.Click();
            var elements = webDriver.FindElements(By.CssSelector(".subject-config .checkbox_false_full"));
            elements.ToList().ForEach((checkbox) =>
            {
                checkbox.Click();
                Task.Delay(500).Wait();
            });
            IWebElement trashBinElement = webDriver.FindElement(By.CssSelector(".subject-config .icon-wh-trash"));
            trashBinElement.Click();
            var confirmDialog = webDriver.SwitchTo().Alert();
            Task.Delay(200).Wait();
            confirmDialog.Accept();
        }

        public static void AddResource(IEnumerable<ResourceItem> items)
        {
            IWebDriver webDriver = new ChromeDriver();
            webDriver.Navigate().GoToUrl("http://localhost/ZJG.GisClient/default.aspx?f=mm");
            webDriver.Manage().Timeouts().ImplicitWait = new TimeSpan(10000 * 50000);
            IWebElement ztpzElement = webDriver.FindElement(By.CssSelector("[data-navid=\"manege_register\"]"));
            ztpzElement.Click();

            items.ToList().ForEach((item) =>
            {
                //registerResource_add
                var addButtonElement = webDriver.FindElement(By.CssSelector(".registerResource_add"));
                addButtonElement.Click();
                Task.Delay(1000).Wait();
                var zymcInput = webDriver.FindElement(By.CssSelector(".vectorPanel input.txtName"));

                var urlInput = webDriver.FindElement(By.CssSelector(".vectorPanel input.txtURL"));

                //urlInput

                IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
                ////js.ExecuteScript()
                ////js.ExecuteScript(   " document.getElement")
                ///



                //js.ExecuteScript(String.Format("$('.vectorPanel input.textName')[0].value='{0}'", "adsfs"));
                js.ExecuteScript($"$(\".vectorPanel input.txtURL\")[0].value=\"{item.Url}\"");

                js.ExecuteScript($"$(\".vectorPanel input.txtName\")[0].value=\"{item.CnName}\"");
                Task.Delay(1000).Wait();


                var saveButtonElement = webDriver.FindElement(By.CssSelector(".addResource .saveResource"));
                saveButtonElement.Click();

                Task.Delay(3000).Wait();
                ;

                //urlInput.
            });




        }

        public static void DeleleResource()
        {
            ChromeOptions options = new ChromeOptions();
            IWebDriver webDriver = new ChromeDriver(options);
            webDriver.Navigate().GoToUrl("http://localhost/ZJG.GisClient/default.aspx?f=mm");
            webDriver.Manage().Timeouts().ImplicitWait = new TimeSpan(10000 * 50000);
            IWebElement resourceTabEntry = webDriver.FindElement(By.CssSelector("[data-navid=\"manege_config\"]"));
            // checkbox_false_full

            resourceTabEntry.Click();

            webDriver.Manage().Timeouts().ImplicitWait = new TimeSpan(10000 * 20000);

            //main-content checkbox_false_full

            var loaded = webDriver.FindElement(By.CssSelector(".data-source .main-content .checkbox_false_full"));

            var checkboxes = webDriver.FindElements(By.CssSelector(".data-source .checkbox_false_full"));


            checkboxes.ToList().ForEach((checkbox) =>
            {
                checkbox.Click();
                Task.Delay(500);
            });
            IWebElement trashBinElement = webDriver.FindElement(By.CssSelector(".data-source .icon-wh-trash"));
            trashBinElement.Click();
            var confirmDialog = webDriver.SwitchTo().Alert();
            Task.Delay(200).Wait();
            confirmDialog.Accept();
        }

        public static List<FileInfo> GetFileInfos(string root, string extension)
        {

            List<FileInfo> returnFileInfos = new List<FileInfo>();

            var rootDi = new DirectoryInfo(root);
            var files = rootDi.GetFiles($@"*.{extension}");

            returnFileInfos.AddRange(files);




            var subDirectories = rootDi.GetDirectories();



            subDirectories.ToList().ForEach((subDir) =>
            {
                returnFileInfos.AddRange(GetFileInfos(subDir.FullName, extension));
            });


            //rootDi.GetFiles().fi

            //rootDi.GetDirectories().

            return returnFileInfos;
        }


        public static IEnumerable<ResourceItem> ListAction()
        {
            var infos = GetFileInfos(@"\\192.168.0.4\arcgisserver\directories\arcgissystem\arcgisinput\ZJG", "mxd");
            var list = infos.Select((info) =>
            {
                var fullname = info.FullName;
                var split = fullname.Split('\\');
                var projName = split[7];
                var servname = split[8].Split('.')[0];
                var mxdName = split[11].Split('.')[0];
                var item = new ResourceItem()
                {
                    Url = $"http://192.168.0.4:6080/arcgis/rest/services/{projName}/{servname}/MapServer",
                    CnName = mxdName
                };
                return item;
            });


            return list;


        }

        public static void BatchAddSingleResourceTheme()
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




            Task.Delay(2000).Wait();

            var addThemePopPanelElement = webDriver.FindElement(By.CssSelector(".subject-config .icon-dialog-add"));





            var firstTree = webDriver.FindElements(By.CssSelector(".main-content li.level0"))[0];
            firstTree.FindElements(By.CssSelector("li.level1")).ToList().ForEach((li) =>
            {

                var cnname = li.FindElement(By.CssSelector(".node_name")).Text;



                addThemePopPanelElement.Click();
                var newThemePanel = webDriver.FindElement(By.CssSelector(".new-subject-popup"));


                var js = (IJavaScriptExecutor)webDriver;



                var obj = js.ExecuteScript($"$('input.subject-name')[0].value='{cnname}'");
                //var themeNameInput = newThemePanel.FindElement(By.CssSelector("input.subject-name"));
                Task.Delay(500).Wait();


                var saveButton = newThemePanel.FindElement(By.CssSelector("input.save"));
                saveButton.Click();
                Task.Delay(2000).Wait();




                //li.FindElement(By.CssSelector(".checkbox_false_full")).Click();

            });

        }


    }






    public class ResourceItem
    {
        public string Url { get; set; }

        public string CnName { get; set; }
    }
}
