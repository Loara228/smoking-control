using System.Threading.Tasks;

namespace smoking_control.Pages;

public partial class AuthPage : ContentPage
{
	public AuthPage(API api)
	{
		this._api = api;
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if (!CheckFormat())
		{
			MarkFormat();
            return;
        }
        SetActivity(loading: true);
        await Task.Delay(1000);
        if (await Auth())
        {
			await Navigation.PopModalAsync();
			return;
		}

        SetActivity(loading: false);
    }

	private async Task<bool> Auth()
    {
		try
		{
            return await _api.Authorize(textbox1.Text.Trim(), textbox2.Text.Trim());
        }
		catch(Exception e)
		{
			await Navigation.PushModalAsync(new Pages.ErrorPage(e));
		}
		return false;
    }

	private bool CheckFormat()
	{
		if (string.IsNullOrWhiteSpace(textbox1.Text) || string.IsNullOrWhiteSpace(textbox2.Text))
			return false;
		return true;
	}

	private void MarkFormat()
	{
		// todo
		// unmarkFormat()
	}

	private void SetActivity(bool loading)
	{
		activityIndicator.IsVisible = loading;
        textbox1.IsEnabled = !loading;
        textbox2.IsEnabled = !loading;
        button1.IsVisible = !loading;
    }

	public string resultToken = "empy";
	private API _api;
}