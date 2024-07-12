using OpenQA.Selenium;

namespace spotify_cli_cs.Components
{
    class SpotifySearchInputField : TextInputField
    {
        public override void OnEnter()
        {
            System.Diagnostics.Debug.WriteLine("on enter but on the child class or smth idk");

            // open search menu
            var driver = SpotifyCLI.driver;
            
            var searchBtn = driver?.FindElement(By.XPath("//a[@href='/search']"));
            searchBtn?.Click();

            Thread.Sleep(500);

            var searchInput = driver?.FindElement(By.XPath("//input[@data-testid='search-input']"));
            searchInput?.Click();

            // clear any existing content of searchInput
            searchInput?.SendKeys(Keys.Control + "a");
            searchInput?.SendKeys(Keys.Delete);

            // send the content into searchInput using SendKeys()
            searchInput?.SendKeys(content);
        }
    }
}