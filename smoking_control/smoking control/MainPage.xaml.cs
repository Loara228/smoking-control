using System.Runtime.CompilerServices;

namespace smoking_control
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Loaded += async (s, e) => {
                var page = new Pages.AuthPage();
                await Navigation.PushModalAsync(page);
                page.Unloaded += (s, e) =>
                {
                    token = page.resultToken;
                };

            };
            // 192.168.0.148:8080
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            label1.Text = token.ToString();
        }

        private string token = "empty";
    }

}
