using smoking_control.Api;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace smoking_control.Pages;

public partial class AuthPage : ContentPage
{
    public AuthPage()
    {
        InitializeComponent();
        textbox1.TextChanged += (s, e) => UnmarkInputs();
        textbox2.TextChanged += (s, e) => UnmarkInputs();
        textbox2.Completed += (s, e) => Button_Clicked(s, e);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (!CheckFormat())
        {
            return;
        }
        SetActivity(loading: true);
        await Task.Delay(100);
        string message = "Authorization failed";
        if (CurrentPageType == PageType.Authorization)
        {
            if (await Auth())
            {
                await Navigation.PopModalAsync();
                await SaveToken();
                return;
            }
        }
        else if (CurrentPageType == PageType.Registration)
        {
            var r = await Register();
            if (r.result)
            {
                await Auth();
                await SaveToken();
                await Navigation.PopModalAsync();
                return;
            }
            message = r.failMessage;
        }

        SetActivity(loading: false);
        MarkInputs(message);
    }

    private async Task SaveToken()
    {
        if (cb1.IsChecked)
            await SecureStorage.Default.SetAsync("token", APIClient.Current.Token);
    }

    private async Task<bool> Auth()
    {
        try
        {
            return await APIClient.Current.AuthModule.Auth(textbox1.Text.Trim(), textbox2.Text.Trim());
        }
        catch (ApiException e)
        {
            return false;
        }
        catch (Exception e)
        {
            await Navigation.PushModalAsync(new ErrorPage(e, false));
        }
        return false;
    }

    private async Task<(bool result, string failMessage)> Register()
    {
        try
        {
            return await APIClient.Current.AuthModule.Register(textbox1.Text.Trim(), textbox2.Text.Trim());
        }
        catch (ApiException e)
        {
            await Navigation.PushModalAsync(new ErrorPage(e, false));
            return (false, "something wrong");
        }
        catch (Exception e)
        {
            await Navigation.PushModalAsync(new ErrorPage(e, false));
            return (false, "something wrong");
        }
    }

    private bool CheckFormat()
    {
        const string USERNAME_PATTERN = @"^[A-Za-z0-9]+$";
        const string PASSWORD_PATTERN = @"^[a-zA-Z0-9!@#\$%\^&\*$$_\+=$$$$\{\};:<>\|./?,\-]+$";


        if (string.IsNullOrWhiteSpace(textbox1.Text))
            return MarkInputs("Username cannot be empty");
        if (string.IsNullOrWhiteSpace(textbox2.Text))
            return MarkInputs("Password cannot be empty");

        if (textbox1.Text.Length > 20)
            return MarkInputs("Username must be at least 20 characters long");
        if (textbox2.Text.Length > 50)
            return MarkInputs("Password is too long");
        if (textbox1.Text.Length < 2)
            return MarkInputs("Username is too short");
        if (textbox2.Text.Length < 8)
            return MarkInputs("Password is too short");

        if (!Regex.IsMatch(textbox1.Text, USERNAME_PATTERN))
            return MarkInputs("Invalid characters in the username");
        if (!Regex.IsMatch(textbox2.Text, PASSWORD_PATTERN))
            return MarkInputs("Invalid characters in the password");

        return true;
    }

    private bool MarkInputs(string hint)
    {
        _marked = true;
        labelHint.Text = hint;
        labelHint.IsVisible = true;

        if (Application.Current?.Resources.TryGetValue("Error", out object color) == true)
        {
            _tbColor = textbox1.TextColor;
            textbox1.TextColor = (Color)color;
            textbox2.TextColor = (Color)color;
        }
        return false;
    }

    private void UnmarkInputs()
    {
        if (_marked)
        {
            _marked = false;
            labelHint.IsVisible = false;
            textbox1.TextColor = _tbColor;
            textbox2.TextColor = _tbColor;
        }
    }

    private void SetActivity(bool loading)
    {
        activityIndicator.IsVisible = loading;
        textbox1.IsEnabled = !loading;
        textbox2.IsEnabled = !loading;
        button1.IsVisible = !loading;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (CurrentPageType == PageType.Authorization)
            CurrentPageType = PageType.Registration;
        else
            CurrentPageType = PageType.Authorization;
    }

    protected enum PageType : int
    {
        Authorization = 0,
        Registration = 1
    }

    protected PageType CurrentPageType
    {
        get => _curPageType;
        set
        {
            _curPageType = value;
            if (value == PageType.Authorization)
            {
                labelSwitcher.Text = "Don't have an account yet?";
                button1.Text = "Sign in";
            }
            else if (value == PageType.Registration)
            {
                labelSwitcher.Text = "Already have an account?";
                button1.Text = "Continue";
            }
        }
    }

    private PageType _curPageType;
    private bool _marked;
    private Color _tbColor = Colors.White;
}