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

        App.Logs.CollectionChanged += Logs_CollectionChanged;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_initialized)
        {
            await Reload();
            InitUI();
            _initialized = true;
        }
        if (App.LogsUpdateRequired)
        {
            App.LogsUpdateRequired = false;
        }
    }

    private async Task Reload()
    {
        App.Logs.Clear();
        try
        {
            (await APIClient.Current.LogsModule.GetLogs(0, 50))
                .Select(x => new UserLogVM(x))
                .ToList()
                .ForEach(x => App.Logs.Add(x));
        }
        catch(Exception exc)
        {
            await ErrorPage.DisplayError(this, exc);
        }
    }

    private async Task TryLoadMoreLogs()
    {
        try
        {
            (await APIClient.Current.LogsModule.GetLogs(App.Logs.Count, 50))
                .Select(x => new UserLogVM(x))
                .ToList()
                .ForEach(x => App.Logs.Add(x));
        }
        catch (Exception exc)
        {
            await ErrorPage.DisplayError(this, exc);
        }
    }

    private void Logs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (!_initialized)
            return;
    }

    private void InitUI()
    {
        var btnTextStyle = (Style)Application.Current!.Resources["ButtonText"];

        var btnLoadMore = new Button() { 
            Text = "Load",
            Style = (Style)Application.Current!.Resources["ButtonText"],
            HeightRequest = 50
        };
        var activityIndicator = new ActivityIndicator()
        {
            HeightRequest = 50,
            WidthRequest = 50,
            IsRunning = true,
            IsVisible = false
        };
        btnLoadMore.Clicked += async (s, e) =>
        {
            btnLoadMore.IsVisible = false;
            activityIndicator.IsVisible = true;
            await Task.Delay(500);
            await TryLoadMoreLogs();
            btnLoadMore.IsVisible = true;
            activityIndicator.IsVisible = false;
        };

        Content = new ListView()
        {
            Footer = new VerticalStackLayout()
            {
                Children = { btnLoadMore, activityIndicator }
            },
            SelectionMode = ListViewSelectionMode.None,
            ItemsSource = App.Logs,
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
                var button1 = new Button() { Text = "Delete", Style = btnTextStyle };

                label1.SetBinding(Label.TextProperty, "Elapsed");
                label2.SetBinding(Label.TextProperty, "DateTimeFormatted");

                button1.Clicked += async (s, e) =>
                {
                    UserLogVM log = ((s as Button)!.BindingContext as UserLogVM)!;
                    try
                    {
                        try
                        {
                            await APIClient.Current.LogsModule.DeleteLog(log.Id);
                        }
                        catch (Exception exc)
                        {
                            await ErrorPage.DisplayError(this, exc);
                        }
                        App.Logs.Remove(log);
                        --App.LogsCounter;
                        App.MainUpdateRequired = true;
                    }
                    catch(Exception exc)
                    {
                        await ErrorPage.DisplayError(this, exc);
                    }
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

    public int Id
    {
        get => _logModel.id;
    }

    private UserLog _logModel;
}