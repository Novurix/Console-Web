using System;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace web
{
    public class ConsoleWeb
    {
        static IWebDriver driver;
        static IWebElement element;

        static string input;
        static string webPage;

        static string id;
        static bool isID;

        static Commands commands;

        public static void Main(string[] args)
        {
            commands = new Commands();

            ChromeDriver chromeDriver = new ChromeDriver();
            chromeDriver.NetworkConditions = new ChromeNetworkConditions()
            { DownloadThroughput = 0, UploadThroughput = 0, Latency = TimeSpan.FromMilliseconds(1), IsOffline=false};

            driver = chromeDriver;


            Console.WriteLine(" ");
            for (int i = 0; i < commands.commands.Length; i++)
            {
                Console.WriteLine(commands.commands[i]);
            }
            Console.WriteLine(" ");
            
            getInput();
        }

        static void getInput()
        {
            Console.ForegroundColor = ConsoleColor.White;
            input = Console.ReadLine();
            init();
        }

        static void init()
        {
            string[] words = input.Split();

            if (words.Length == 2 && element == null)
            {
                // FINDING A WEBSITE

                if (words[0].ToLower() == "web" && words[1].StartsWith("https://"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    driver.Url = words[1];
                }
                else if (words[0].ToLower() == "web" && !words[1].StartsWith("https://"))
                {
                    string url = "https://"+words[1];
                    Console.ForegroundColor = ConsoleColor.Green;
                    driver.Url = url;
                }
            }
            else if (words.Length == 2 && element != null)
            {
                // Checking if a selected element exists to prevent moving to another site

                string getToLower = words[0].ToLower();
                if (getToLower == "web" && words[1].StartsWith("https://"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("you've assigned an element to " + driver.Url + " to continue, type 'elem delete'");
                }

                // Deleting selected element

                if (getToLower == "elem" && words[1].ToLower() == "delete")
                {
                    element = null;
                }

                // Clearing input

                else if (getToLower == "elem" && words[1].ToLower() == "tclear")
                {
                    if (element != null)
                    {
                        try {
                            element.Clear();
                        }
                        catch (Exception e) { Console.WriteLine(e.GetBaseException()); }
                    }
                }

                // Clicking a button

                else if (words[0].ToLower() == "elem" && words[1].ToLower() == "click")
                {
                    try {
                        element.Click();
                    }
                    catch(Exception e){}
                }
            }

            if (words.Length == 3)
            {
                // FINDING ELEMENTS

                if (words[0].ToLower() == "findweb" && words[1].ToLower() == "id")
                {
                    element = driver.FindElement(By.Id(words[2]));
                    isID = true; 
                }

                if (words[0].ToLower() == "findweb" && words[1].ToLower() == "class")
                {
                    element = driver.FindElement(By.ClassName(words[2]));
                    isID = false;
                    id = words[2];
                }

                // Changing text of an element

                else if (words[0].ToLower() == "elem" && words[1].ToLower() == "text")
                {
                    if (element != null)
                    {
                        IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                        //element.SendKeys(words[2]);
                        if (isID)
                        {
                            try
                            {
                                js.ExecuteScript("document.getElementById('" + id + "').textContent = '" + words[2] + "';");
                            }
                            catch (Exception e)
                            {
                                try {
                                    element.SendKeys(words[2]);
                                } catch (Exception e1) {}
                            }
                        }
                        else
                        {

                            try {
                                js.ExecuteScript("document.querySelector('" + id + "').textContent = '" + words[2] + "';");
                            }
                            catch (Exception e) {
                                try {
                                    element.SendKeys(words[2]);
                                } catch (Exception e1) {}
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("no element has been assigned");
                    }
                }
            }

            else if (words.Length == 1)
            {
                if (words[0].ToLower() == "commands")
                {
                    Console.WriteLine(" ");
                    for (int i = 0; i < commands.commands.Length; i++)
                    {
                        Console.WriteLine(commands.commands[i]);
                        Thread.Sleep(20);
                    }
                    Console.WriteLine(" ");
                }
            }

            getInput();
        }
    }

    class Commands
    {
        public string[] commands =
        {
            "commands - shows list of commands",
            "web <URL> - goes to url",
            "findweb id <ID> - gets an element by it's <ID> in the website",
            "findweb class <CLASS_NAME> - gets an element by it's <CLASS_NAME> in the website",
            "elem delete - removes the focus of a selected element",
            "elem text <TEXT> - sets the text of a selected element",
            "elem tclear - clears the text of a selected element",
            "elem click - clicks the selected element"
        };
    }
}
