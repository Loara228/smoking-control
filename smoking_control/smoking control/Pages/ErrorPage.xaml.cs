using System.Threading.Tasks;

namespace smoking_control.Pages;

public partial class ErrorPage : ContentPage
{
	public ErrorPage(Exception exc, bool critical)
	{
		this._critical = critical;
		InitializeComponent();
		excOutput.Text = exc.ToString();
		if (_critical)
		{
			btn.Text = "quit";
		}
	}

	public static async Task DisplayError(ContentPage self, Exception exc)
	{
		var errPage = new ErrorPage(exc, true);
		await self.Navigation.PushModalAsync(errPage);
		await Task.Delay(-1);
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if (_critical)
        {
			Application.Current!.Quit();
        }
		else
        {
            await Navigation.PopModalAsync();
        }
    }

	private bool _critical;
}