using System;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebConsole
{
    public class ConsoleWeb
    {
        static IWebDriver driver;
        static IWebElement webElement;

        static Commands commands;
        static string input;
        static string id;

        static bool isId, isClass, isName;

        public static void Main(string[] args)
        {
            driver = new Driver();
            commands = new Commands();

            for (int i = 0; i < commands.cmds.Length; i++)
            {
                Console.WriteLine(commands.cmds[i]);
            }

            getInput();
        }


        static void getInput()
        {
            input = Console.ReadLine();
            scanInput();
        }

        static void scanInput()
        {
            string[] words = input.Split();

            if (words.Length == 1)
            {
                if (words[0].ToLower() == "commands")
                {
                    for (int i = 0; i < commands.cmds.Length; i++)
                    {
                        Console.WriteLine(commands.cmds[i]);
                        Thread.Sleep(20);
                    }
                }
            }

            else if (words.Length == 2) {

                if (words[0].ToLower() == "web") {
                    string http = "https://";

                    if (words[1].StartsWith(http)) {
                        driver.Url = words[1];
                    }
                    else {
                        driver.Url = "https://" + words[1];
                    }
                }

                if (words[0].ToLower() == "html")
                {
                    if (words[1].ToLower() == "remove")
                    {
                        webElement = null;
                    }

                    else if (words[1].ToLower() == "elemcl")
                    {
                        IJavaScriptExecutor javaScriptExecutor = driver as IJavaScriptExecutor;
                        if (isId) {
                            if (webElement != null) {
                                webElement = null;
                                javaScriptExecutor.ExecuteScript("var id = '" + id + "'; " +
                                    "var element = document.getElementById(id); " +
                                    "element.parentNode.removeChild(element)");
                            }
                            else {
                                Console.WriteLine("the ID: " + id + " does not exist and current action cannot proceed");
                            }
                        }
                        else if (isClass) {
                            if (webElement != null) {
                                webElement = null;
                                javaScriptExecutor.ExecuteScript(
                                    "var id = '." + id + "'; " +
                                    "var element = document.querySelector(id);" +
                                    "element.parentNode.removeChild(element)");
                            }
                            else {
                                Console.WriteLine("the CLASS_NAME: " + id + " does not exist and current action cannot proceed");
                            }
                        }
                        else if (isName) {
                            if (webElement != null) {
                                webElement = null;
                                javaScriptExecutor.ExecuteScript(
                                    "var id = '." + id + "'; " +
                                    " var element = document.querySelector(id);" +
                                    " element.parentNode.removeChild(element)");
                            }
                            else {
                                Console.WriteLine("the NAME: " + id + " does not exist and current action cannot proceed");
                            }
                        }
                    }
                }
            }
            else if (words.Length == 3)
            {
                if (words[0].ToLower() == "html")
                {
                    if (words[1].ToLower() == "getid") {
                        try {
                            webElement = driver.FindElement(By.Id(words[2]));
                            id = words[2];
                            isId = true; isClass = false; isName = false;
                        }
                        catch(Exception e) {
                            Console.WriteLine("cannot find element with ID: " + words[2]);
                        }
                    }
                    else if (words[1].ToLower() == "getclass") {
                        try {
                            webElement = driver.FindElement(By.ClassName(words[2]));
                            id = words[2];
                            isId = false; isClass = true; isName = false;
                        }
                        catch (Exception e) {
                            Console.WriteLine("cannot find element with CLASS_NAME: " + words[2]);
                        }
                    }
                    else if (words[1].ToLower() == "getname") {
                        try {
                            webElement = driver.FindElement(By.Name(words[2]));
                            id = words[2];
                            isId = false; isClass = false; isName = true;
                        }
                        catch(Exception e) {
                            Console.WriteLine("cannot find element with NAME: " + words[2]);
                        }
                    }

                    else if (words[1].ToLower() == "text"){
                        if (webElement != null) {
                            try {
                                webElement.Clear();
                                webElement.SendKeys(words[2]);
                            } catch(Exception e) {
                                try {
                                    IJavaScriptExecutor javaScriptExecutor = driver as IJavaScriptExecutor;
                                    if (isId) {
                                        javaScriptExecutor.ExecuteScript("var id = '" + id + "'; " +
                                            "var element = document.getElementById(id); " +
                                            "element.textContent = '" + words[2] + "'");
                                    }
                                    else if (isClass) {
                                        javaScriptExecutor.ExecuteScript("var id = '." + id + "'; " +
                                            "var element = document.querySelector(id); " +
                                            "element.textContent = '" + words[2] + "'");
                                    }
                                    else if (isName) {
                                        javaScriptExecutor.ExecuteScript("var id = '." + id + "'; " +
                                            "var element = document.querySelector(id); " +
                                            "element.textContent = '" + words[2] + "'");
                                    }

                                }catch(Exception e1) {}
                            }
                        }
                        else {
                            Console.WriteLine("cannot set text of null");
                        }
                    }
                }
            }
            else if (words.Length > 3)
            {
                string text = "";
                if (words[0].ToLower() == "html" && words[1].ToLower() == "text") {
                    for (int i = 0; i < words.Length-2; i++) {
                        text = text + words[i+2] + " ";

                        if (webElement != null) {
                            try {
                                webElement.SendKeys(text);
                            } catch(Exception e) {
                                try  {
                                    IJavaScriptExecutor javaScriptExecutor = driver as IJavaScriptExecutor;
                                    if (isId)  {
                                        javaScriptExecutor.ExecuteScript("var id = '" + id + "'; " +
                                            "var element = document.getElementById(id); " +
                                            "element.textContent = '" + text + "'");
                                    }
                                    else if (isClass) {
                                        javaScriptExecutor.ExecuteScript("var id = '." + id + "'; " +
                                            "var element = document.querySelector(id); " +
                                            "element.textContent = '" + text + "'");
                                    }
                                    else if (isName) {
                                        javaScriptExecutor.ExecuteScript("var id = '." + id + "'; " +
                                            "var element = document.querySelector(id); " +
                                            "element.textContent = '" + text + "'");
                                    }
                                }
                                catch (Exception e1){}
                            }
                        }
                    }
                }
                text = "";
            }

            getInput();
        }
    }

    class Driver : ChromeDriver
    {
        public Driver()
        {
            this.NetworkConditions = new ChromeNetworkConditions()
            { DownloadThroughput = 0, UploadThroughput = 0, Latency = TimeSpan.FromMilliseconds(1), IsOffline = false };
        }
    }
}
