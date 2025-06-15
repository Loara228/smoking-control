using smoking_control.Pages;
using System.Collections.ObjectModel;

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

        public static ObservableCollection<UserLogVM> Logs
        {
            get; set;
        } = new ObservableCollection<UserLogVM>();

        public static int LogsCounter
        {
            get; set;
        } = 0;

        public static bool LogsUpdateRequired = false;
        public static bool MainUpdateRequired = false;
    }
}