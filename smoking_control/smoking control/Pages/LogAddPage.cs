using smoking_control.Api;
using smoking_control.Models;
using System.Threading.Tasks;

namespace smoking_control.Pages;

public class LogAddPage : ContentPage
{
	public LogAddPage(bool displayWarning = true)
	{
        this.Loaded += async (_, _) => await OnLoaded();
		this._warning = displayWarning;

        var b1m = new Button() { Text = "1 minute", WidthRequest = 100 };
        var b2m = new Button() { Text = "2 minutes", WidthRequest = 100 };
        var b4m = new Button() { Text = "4 minutes", WidthRequest = 100 };
        var b8m = new Button() { Text = "8 minutes", WidthRequest = 100 };
        var b12m = new Button() { Text = "12 minutes", WidthRequest = 100 };
        var b30m = new Button() { Text = "30 minutes", WidthRequest = 100 };
        var b60m = new Button() { Text = "1 hour", WidthRequest = 100 };

        b1m.Clicked += async (_, _) => await IncInterval(1);
        b2m.Clicked += async (_, _) => await IncInterval(2);
        b4m.Clicked += async (_, _) => await IncInterval(4);
        b8m.Clicked += async (_, _) => await IncInterval(8);
        b12m.Clicked += async (_, _) => await IncInterval(12);
        b30m.Clicked += async (_, _) => await IncInterval(30);
        b60m.Clicked += async (_, _) => await IncInterval(60);

        _label1 = new Label()
        {
            Text = "",
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 18
        };

        Content = new VerticalStackLayout()
        {
            Padding = 10,
            Spacing = 20,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                b1m,
                b2m,
                b4m,
                b8m,
                b12m,
                b30m,
                b60m,
                _label1
            }
        };
    }

    private async Task OnLoaded()
    {
        const string WARN_MSG =
            "Are you sure? You can try it:\n" +
            "- Chewing gum\n" +
            "- Exercise\n" +
            "- Deep Breathing\n" +
            "- Drinking Water or tea";

        if (_warning)
            if (!await DisplayAlert("🤔", WARN_MSG, "Yes", "No"))
            {
                await Navigation.PopModalAsync();
                return;
            }

        await IncInterval(2, false);
        await Task.Delay(100);
        await PushLog();

        _label1.Text = "The minimum interval has been increased by 2 minutes.\nWould you like to add more?";
    }

	private async Task PushLog()
    {
        UserLog input = null!;
        try
        {
            input = await APIClient.Current.LogsModule.AddLog();
        }
        catch (Exception exc)
        {
            await ErrorPage.DisplayError(this, exc);
        }
        App.UsrData.last_input = input.time;
        App.Logs.Insert(0, new UserLogVM(input));
        ++App.LogsCounter;
		App.MainUpdateRequired = true;
    }

	private async Task IncInterval(int minutes, bool pop = true)
	{
		try
        {
            App.UsrData.interval = await APIClient.Current.DataModule.IncInterval(minutes * 60);
			App.MainUpdateRequired = true;
            if (pop)
                await Navigation.PopModalAsync();
        }
		catch(Exception exc)
		{
			await ErrorPage.DisplayError(this, exc);
		}
	}

    private Label _label1;
	private bool _warning;
}