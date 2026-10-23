using AutomationFrameWork.PageElements.Corporate_Roles;
using AutomationFrameWork.Utilities;
using Microsoft.Playwright;

namespace AutomationFrameWork.PageActions.Corporate_Roles;

public class LoginPages : LoginObjects
{
    private readonly TestContext _context;

    public LoginPages(TestContext context, IPage page) : base(page)
    {
        _context = context;
    }

    public async Task NavigateAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException(
                "URL cannot be null or empty", nameof(url));

        await _context.Page.GotoAsync(url);

        await SignInWithAcc.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });
    }

    public async Task SaveAuthStateAsync()
    {
        await _context.Page.Context.StorageStateAsync(
            new() { Path = "auth.json" });
    }

    public async Task LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required",
                nameof(username));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required",
                nameof(password));

        await SignInWithAcc.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });

        await SignInWithAcc.ClickAsync();

        await EmailTextField.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });

        await EmailTextField.FillAsync(username);

        await PasswordField.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });

        await PasswordField.FillAsync(password);

        // Submit the login form.
        // If Enter does not submit it, use the actual submit-button locator.
        await PasswordField.PressAsync("Enter");

        await HoverOnProfile.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });

        Console.WriteLine("** Corporate login successful");
    }

    public async Task LogoutAsync()
    {
        await HoverOnProfile.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });
        await HoverOnProfile.HoverAsync();
        await ClickOnLogoutBtn.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });

        await ClickOnLogoutBtn.ClickAsync();

        // Wait until the sign-in screen returns.
        await SignInWithAcc.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });

        Console.WriteLine("** Corporate logout successful");
    }
}
