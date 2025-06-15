namespace smoking_control
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell())
            {
#if WINDOWS
                MaximumWidth = 500,
                MaximumHeight = 800,
#endif
            };
        }
    }
}