using smoking_control.Api;
using smoking_control.Models;
using smoking_control.Pages;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace smoking_control
{
    public partial class MainPage : ContentPage
    {
        #region ctor, loading
        public MainPage()
        {
            Content = new ActivityIndicator()
            {
                IsRunning = true,
            };
            _data = null!;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_initialized)
                return;
            _initialized = true;

            await Authorization();
        }

        /// <summary>
        /// Trying to authorize via saved token. Or pages::AuthPage
        /// </summary>
        private async Task Authorization()
        {
            string? savedToken = await SecureStorage.Default.GetAsync("token");
            if (savedToken != null)
            {
                try
                {
                    if (await APIClient.Current.AuthModule.VerifyToken(savedToken))
                    {
                        APIClient.Current.Token = savedToken;
                        await LoadData();
                        return;
                    }
                }
                catch (Exception exc)
                {
                    //await ErrorPage.DisplayError(this, exc);
                }
            }

            var authPage = new AuthPage();
            await Navigation.PushModalAsync(authPage);
            authPage.Unloaded += async (s, e) =>
            {
                await LoadData();
            };
        }

        /// <summary>
        /// Trying to load UserData from server. Or create new in editor.
        /// </summary>
        private async Task LoadData()
        {
            UserData? d = null;
            try
            {
                d = await APIClient.Current.DataModule.GetData();
            }
            catch (Exception exc1)
            {
                await ErrorPage.DisplayError(this, exc1);
            }
            if (d == null)
            {
                var dataPage = new UserDataEditorPage();
                await Navigation.PushModalAsync(dataPage);
                dataPage.Unloaded += async (s, e) =>
                {
                    try
                    {
                        this._data = dataPage.CollectData();
                        await APIClient.Current.DataModule.SetData(_data);
                        await Task.Delay(100);
                        _data = (await APIClient.Current.DataModule.GetData())!; // Not null. Additional check, id recv.
                        await Task.Delay(100);
                        _logsToday = (Int32)(await APIClient.Current.LogsModule.GetLogsToday(DateTimeOffset.Now.Offset.Hours));
                        OnLoaded();
                    }
                    catch (Exception exc)
                    {
                        await ErrorPage.DisplayError(this, exc);
                    }
                };
            }
            else
            {
                this._data = d;
                await Task.Delay(100);
                _logsToday = (Int32)(await APIClient.Current.LogsModule.GetLogsToday(DateTimeOffset.Now.Offset.Hours));
                OnLoaded();
            }
        }

        #endregion

        /// <summary>
        /// xaml init. It is called after authorization and 
        /// </summary>
        private void OnLoaded()
        {
            InitializeComponent();

            UpdateLog();

            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                for(;;)
                {
                    await Task.Delay(1000);
                    if (_data.last_input == 0)
                    {
                        if (layoutElapsed.IsVisible)
                        {
                            layoutElapsed.IsVisible = false;
                            layoutCd.IsVisible = false;
                        }
                        continue;
                    }
                    else
                    {
                        if (!layoutElapsed.IsVisible)
                        {
                            layoutElapsed.IsVisible = true;
                            layoutCd.IsVisible = true;
                        }

                        labelElapsed.Text = (DateTime.UtcNow - _lastInput).AsFormattedString();
                        labelCd.Text = (_nextInput - DateTime.UtcNow).AsFormattedString();
                    }
                }
            });
        }

        private void UpdateLog()
        {
            _lastInput = DateTimeOffset.FromUnixTimeSeconds(_data.last_input).DateTime;
            _nextInput = _lastInput.Add(TimeSpan.FromSeconds(_data.interval));
            labelCounter.Text = _logsToday.ToString();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var input = await APIClient.Current.LogsModule.AddLog();
            _data.last_input = input.time;
            App.Logs.Add(new UserLogVM(input));
            App.LogsUpdateRequired = true;
            ++_logsToday;
            UpdateLog();
        }

        private UserData _data;

        private bool _initialized = false;

        private DateTime _lastInput;
        private DateTime _nextInput;

        private int _logsToday = 0;
    }
}
