namespace smoking_control
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
#if WINDOWS

            SecureStorage.Default.Remove("token");
#elif ANDROID
            // сам не в восторге :/
            new Thread(() =>
            {
                SecureStorage.Default.Remove("token");
            }).Start();
            Thread.Sleep(500);
#endif
            Application.Current!.Quit();
        }
    }
}
