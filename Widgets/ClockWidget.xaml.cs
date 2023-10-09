using System.Windows;

namespace WindowsWidgets
{
    public partial class ClockWidget : Window
    {
        public ClockWidget()
        {
            InitializeComponent();

            WindowAttributeManager.EnableRoundCorners(this);
            WindowAttributeManager.SetNoActivate(this);
            WindowAttributeManager.SetClickThrough(this);
            WindowAttributeManager.HideBorder(this);

            ChicagoClock.SetClockInformation();
            NewYorkClock.SetClockInformation();
            LosAngelasClock.SetClockInformation();
            GreenwichClock.SetClockInformation();
        }

        public void RefreshWidget()
        {
            Left = SystemParameters.PrimaryScreenWidth - Width - 20;
            Top = 20;
        }
    }
}
