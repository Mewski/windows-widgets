using System.Windows;

namespace WindowsWidgets
{
    public partial class WeatherWidget : Window
    {
        public WeatherWidget()
        {
            InitializeComponent();

            WindowAttributeManager.EnableRoundCorners(this);
            WindowAttributeManager.SetNoActivate(this);
            WindowAttributeManager.SetClickThrough(this);
            WindowAttributeManager.HideBorder(this);
        }

        public void RefreshWidget()
        {
            Left = SystemParameters.PrimaryScreenWidth - Width - 20;
            Top = 260;
        }
    }
}
