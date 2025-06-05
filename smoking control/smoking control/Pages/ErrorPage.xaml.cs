using System.Threading.Tasks;

namespace smoking_control.Pages;

public partial class ErrorPage : ContentPage
{
	public ErrorPage(Exception exc)
	{
		InitializeComponent();
		excOutput.Text = exc.ToString();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}