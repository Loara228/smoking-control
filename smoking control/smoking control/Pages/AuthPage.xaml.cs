using System.Threading.Tasks;

namespace smoking_control.Pages;

public partial class AuthPage : ContentPage
{
	public AuthPage()
	{
		InitializeComponent();
		textbox1.TextChanged += (s, e) => UnmarkInputs();
		textbox2.TextChanged += (s, e) => UnmarkInputs();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if (!CheckFormat())
		{
            return;
        }
        SetActivity(loading: true);
        await Task.Delay(100);
        if (await Auth())
        {
			await Navigation.PopModalAsync();
			return;
		}

        SetActivity(loading: false);
        MarkInputs("Authorization failed");
    }

	private async Task<bool> Auth()
    {
		try
		{
            return await API.Current.Authorize(textbox1.Text.Trim(), textbox2.Text.Trim());
        }
		catch(ApiException e)
		{
			return false;
		}
		catch(Exception e)
		{
			await Navigation.PushModalAsync(new Pages.ErrorPage(e));
		}
		return false;
    }

	private bool CheckFormat()
	{
		if (string.IsNullOrWhiteSpace(textbox1.Text))
		{
            MarkInputs("Username cannot be empty");
			return false;
        }
		if (string.IsNullOrWhiteSpace(textbox2.Text))
        {
            MarkInputs("Password cannot be empty");
            return false;
        }
		if (textbox1.Text.Length > 20)
        {
            MarkInputs("Username must be at least 20 characters long");
			return false;
        }
		if (textbox2.Text.Length > 64)
        {
            MarkInputs("Password is too long");
            return false;
        }

		foreach (char c in _not_allowed_chars)
		{
			if (textbox1.Text.Contains(c) || textbox2.Text.Contains(c))
            {
                MarkInputs("Username or password cannot contain:\n" + string.Join(' ', _not_allowed_chars));
                return false;
			}
		}
		return true;
    }

	private void MarkInputs(string hint)
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

	public string resultToken = "empy";

	private bool _marked;
	private readonly char[] _not_allowed_chars = ['/', '\\', '$', '?', '&'];
	private Color _tbColor = Colors.White;

}