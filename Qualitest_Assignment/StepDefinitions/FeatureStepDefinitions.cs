using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace Qualitest_Assignment.StepDefinitions
{
    // Coding is done using Specflow BDD,Selenium,C# and nUnit.
    [Binding]
    public sealed class FeatureStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private IWebDriver driver;
        private static int keyForLowestPricedProduct;

        [BeforeScenario]
        public void Setup()
        {
            //One time setup for driver launch and to visit URL
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://cms.demo.katalon.com/");
            driver.Manage().Window.Maximize();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0, 500);"); // scroll to see items on screen
        }

        [Given(@"I add four random items to my cart")]
        public void GivenIAddFourRandomItemsToMyCart()
        {
            //Add random items,here I am adding first 4 items from the display
            IWebElement item1 = driver.FindElement(By.XPath("//*[@id=\"main\"]/div[2]/ul/li[2]/div/a[2]"));
            item1.Click(); 
            IWebElement item2 = driver.FindElement(By.XPath("//*[@id=\"main\"]/div[2]/ul/li[1]/div/a[2]"));
            item2.Click();
            IWebElement item3 = driver.FindElement(By.XPath("//*[@id=\"main\"]/div[2]/ul/li[3]/div/a[2]"));
            item3.Click();
            IWebElement item4 = driver.FindElement(By.XPath("//*[@id=\"main\"]/div[2]/ul/li[4]/div/a[2]"));
            item4.Click();
            Thread.Sleep(3000); //without this, the flow is so fast,adding is getting skipped
        }

        [When(@"I view my cart")]
        public void WhenIViewMyCart()
        {
            //Go to Cart
            IWebElement cart = driver.FindElement(By.CssSelector("a[href='https://cms.demo.katalon.com/cart/']"));
            cart.Click();
        }

        [Then(@"I find total four items listed in my cart")]
        public void ThenIFindTotalFourItemsListedInMyCart()
        {
            //Retrieve items count
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0, 300);"); // scroll to see items on screen
            //Total items in cart
            List<IWebElement> elements= driver.FindElements(By.CssSelector("[class*='woocommerce-cart-form__cart-item']")).ToList();

            Assert.AreEqual(4, elements.Count());
        }

        [When(@"I search for lowest priced item")]
        public void WhenISearchForLowestPricedItem()
        {
            //Find lowest priced product
            List<IWebElement> elementsInCart = driver.FindElements(By.CssSelector("[class*='woocommerce-cart-form__cart-item']")).ToList();

            // Store product key and price
            Dictionary<int, int> myItem = new Dictionary<int, int>();

            for(int i = 1; i <= elementsInCart.Count(); i++)
            {
                IWebElement item1 = driver.FindElement(By.XPath("//*[@id=\"post-8\"]/div/div/form/table/tbody/tr[" + i+"]/td[4]/span"));
                int price =  int.Parse(item1.Text.Split(".")[0].Split("$")[1]); // Extract price(Only the number)
                myItem.Add(i, price);
            }

            keyForLowestPricedProduct = myItem.First(kvp => kvp.Value == myItem.Values.Min()).Key;
            
            var lowestPricedProductToRemove = driver.FindElement(By.XPath("//*[@id=\"post-8\"]/div/div/form/table/tbody/tr["+ keyForLowestPricedProduct + "]/td[3]/a"));

            Console.WriteLine($"lowest item to remove {lowestPricedProductToRemove.Text}");

        }

        [When(@"I am able to remove the lowest priced item from my cart")]
        public void WhenIAmAbleToRemoveTheLowestPricedItemFromMyCart()
        {
            //Remove lowest priced product
            var removeItem = driver.FindElement(By.XPath("//*[@id=\"post-8\"]/div/div/form/table/tbody/tr["+ keyForLowestPricedProduct + "]/td[1]/a"));
            removeItem.Click();
            Thread.Sleep(2000); 
        }

        [Then(@"I am able to verify three items in my cart")]
        public void ThenIAmAbleToVerifyThreeItemsInMyCart()
        {
            //Retrive items count from cart
            List<IWebElement> elements = driver.FindElements(By.CssSelector("[class*='woocommerce-cart-form__cart-item']")).ToList();
            Assert.AreEqual(3, elements.Count());
        }

        [AfterScenario]
        public void TearDown()
        {
            //Close browser once execution done.
            driver.Quit();
        }
    }
}