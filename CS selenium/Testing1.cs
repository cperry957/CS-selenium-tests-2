using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Selenium_Project
{
    class CS_Selenium_Espn
    {
        IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver("D:\\webdrivers");
        }


        // API - Get Window Title 
        public String getCurrWindowTitle()
        {
            String windowTitle = driver.Title;
            return windowTitle;
        }

        // API - Get Window Handle 
        public String getMainWinHandle(IWebDriver driver)
        {
            return driver.CurrentWindowHandle;
        }

        // The test website open-ups number of pop-ups 
        // API - Close all the pop-ups and return to the primary window 
        public Boolean switchToPopOutAndClick(String currWindowHandle)
        {
            IList<string> totWindowHandles = new List<string>(driver.WindowHandles);

            foreach (String WindowHandle in totWindowHandles)
            {

                if (!WindowHandle.Equals(currWindowHandle))
                {
                    driver.SwitchTo().Window(WindowHandle);
                    IWebElement lastclickNavigate = driver.FindElement(By.XPath("/html/body/div[2]/div/div/section/div/div/section/div/section/section[1]/div/ul/li[2]/div[2]/div/button"));

                    lastclickNavigate.Click();
                    System.Threading.Thread.Sleep(5000);
                }
            }

            driver.SwitchTo().Window(currWindowHandle);
            if (driver.WindowHandles.Count == 1)
                return true;
            else
                return false;
        }


        public void Test_ScreenShotRemoteBrowser()
        {
            try
            {
                //Making driver to navigate
                driver.Navigate().GoToUrl("https://www.espn.com/");
                //Take the screenshot
                Screenshot image = ((ITakesScreenshot)driver).GetScreenshot();
                //Save the screenshot
                image.SaveAsFile("D:/temp/Screenshot.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail("Failed with Exception: " + e);
            }

        }



        [Test]
        public void test1()
        {

            driver.Url = "https://www.espn.com/";

            string searchText = driver.FindElement(By.XPath("/html/body/div[5]/section/section/div/section[3]/div[1]/section/ul/li[1]/a")).Text;

            string fileName = @"D:\Temp\top_highlight.txt";

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file     
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes(searchText);
                    fs.Write(title, 0, title.Length);
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }


        [Test]
        public void test2()
        {

            driver.Url = "https://www.espn.com/";
            // take a screen shot of the page
            Test_ScreenShotRemoteBrowser();
        }


        [Test]
        public void test3()
        {

            driver.Url = "https://www.espn.com/";
            // take a screen shot of the page
            Test_ScreenShotRemoteBrowser();

            // click on dropdown profile
            IWebElement clickNavigate = driver.FindElement(By.XPath("//*[@id=\"global-user-trigger\"]"));
            clickNavigate.Click();
            // click on favorites 
            clickNavigate = driver.FindElement(By.XPath("html/body/div[5]/div[2]/header/div[2]/ul/li[2]/div/div/ul[2]/li/div/div/a"));
            clickNavigate.Click();
            // wait for a 3 seconds for page to load
            System.Threading.Thread.Sleep(3000);

            // get the current page to work on
            String windowTitle = getCurrWindowTitle();
            // get the main page that we were working on
            String mainWindow = getMainWinHandle(driver);
            // move to the current page and click on a favorit sport
            switchToPopOutAndClick(mainWindow);
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }

    }
}