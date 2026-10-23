using Microsoft.Playwright;

namespace AutomationFrameWork.PageElements.Corporate_Roles
{
    public class LoginObjects
    {
        private readonly IPage _page;
        public LoginObjects(IPage page)
        {
            _page = page;
        }
        public ILocator SignInWithAcc => _page.Locator("//a[text()='Sign in']");

        public ILocator EmailTextField =>
            _page.Locator("[id='email']");
        public ILocator PasswordField =>
            _page.Locator("[name='password']");

        public ILocator HoverOnProfile => _page.Locator("[alt='user profile']");

        public ILocator ClickOnLogoutBtn =>
            _page.Locator("#logoutBtn");


    }
}