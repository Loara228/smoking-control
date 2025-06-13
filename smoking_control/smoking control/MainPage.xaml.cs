using smoking_control.Api;
using smoking_control.Models;
using smoking_control.Pages;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace smoking_control
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            _data = null!;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_initialized)
                return;
            _initialized = true;

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
                catch(Exception exc)
                {
                    await ErrorPage.DisplayError(this, exc);
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
                        await Task.Delay(500);
                        _data = (await APIClient.Current.DataModule.GetData())!; // Not null. Additional check, id recv.
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
                OnLoaded();
            }
        }

        private void OnLoaded()
        {
            labelTest.Text = _data.ToString();
        }

        private bool _initialized = false;
        private UserData _data;

    }
}
