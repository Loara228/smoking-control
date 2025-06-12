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

            var authPage = new AuthPage();
            await Navigation.PushModalAsync(authPage);
            authPage.Unloaded += async (s, e) =>
            {
                UserData? d = null;
                try
                {
                    d = await APIClient.Current.DataModule.GetData();
                }
                catch(Exception exc1)
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
                        }
                        catch(Exception exc)
                        {
                            await ErrorPage.DisplayError(this, exc);
                        }
                    };
                }
            };
        }

        private bool _initialized = false;
        private UserData _data;

    }
}
