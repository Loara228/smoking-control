using smoking_control.Api;
using smoking_control.Models;

namespace smoking_control.Pages;

public class LogsPage : ContentPage
{
	public LogsPage()
    {
        _initialized = false;

        Content = new ActivityIndicator()
		{
			IsRunning = true,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_initialized)
        {
            _initialized = true;
            await LoadLogs();
            InitUI();
        }
        if (App.LogsUpdateRequired)
        {
            App.LogsUpdateRequired = false;
            await LoadLogs();
        }
    }

    private async Task LoadLogs()
    {
        App.Logs.Clear();

        (await APIClient.Current.LogsModule.GetLogs(0, 50))
            .Select(x => new UserLogVM(x))
            .ToList()
            .ForEach(x => App.Logs.Add(x));
    }

    private void InitUI()
    {
        var btnUpdate = new Button() { Text = "Update" };
        btnUpdate.Clicked += async (s, e) => { await LoadLogs(); };
        Content = new ListView()
        {
            ItemsSource = App.Logs,
            Header = btnUpdate,
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid()
                {
                    ColumnDefinitions = new ColumnDefinitionCollection()
                    {
                        new ColumnDefinition(GridLength.Star), // datetime
                        new ColumnDefinition(GridLength.Auto), // button
                    },
                    RowDefinitions = new RowDefinitionCollection()
                    {
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(GridLength.Auto),
                    }
                };

                var label1 = new Label() { FontSize = 16, VerticalOptions = LayoutOptions.End, Padding = new Thickness(10, 0, 0, 0)};
                var label2 = new Label() { FontSize = 13, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(10, 0, 0, 0), TextColor = Colors.Gray };
                var button1 = new Button() { Text = "Delete"};

                label1.SetBinding(Label.TextProperty, "Elapsed");
                label2.SetBinding(Label.TextProperty, "DateTimeFormatted");

                button1.Clicked += async (s, e) =>
                {
                    UserLogVM item = ((s as Button)!.BindingContext as UserLogVM)!;
                };

                grid.Add(label1, 0, 0);
                grid.Add(label2, 0, 1);
                grid.Add(button1, 1, 0);
                grid.SetRowSpan(button1, 2);
                return new ViewCell()
                {
                    View = grid,
                };
            })
        };
    }

	private bool _initialized;
}

public class UserLogVM
{
    public UserLogVM(UserLog model)
    {
        this._logModel = model;
    }

    public string DateTimeFormatted
    {
        get => DateTimeOffset.FromUnixTimeSeconds(_logModel.time).DateTime.ToString("HH:mm\tdd.MM.yyyy");
    }

    public string Elapsed
    {
        get {
            var formatted = (DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(_logModel.time)).AsFormattedString(true);
            if (formatted == "-")
                return "now";
            return formatted + " ago";
        }
    }

    private UserLog _logModel;
}