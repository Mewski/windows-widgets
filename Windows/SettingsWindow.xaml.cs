using MicaWPF.Controls;
using MicaWPF.Core.Enums;
using MicaWPF.Core.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace WindowsWidgets
{
    public partial class SettingsWindow : MicaWindow
    {
        private List<Window> _widgets;

        public SettingsWindow()
        {
            InitializeComponent();

            /*
            var wallpaper = new System.Drawing.Bitmap(SystemInformation.GetWallpaperPath());
            var pixelated = ImageUtilities.PixelateImage(wallpaper, new System.Drawing.Rectangle(0, 0, wallpaper.Width, wallpaper.Height), 480);
            var colors = ImageUtilities.GetColorPalette(pixelated);

            var image = new Image();
            image.Source = Imaging.CreateBitmapSourceFromHBitmap(pixelated.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            TheGrid.Children.Add(image);
             
            var color = colors[32];
            */

            var color = Color.FromRgb(139, 213, 255);
            Application.Current.Resources["WidgetBackground"] = new SolidColorBrush(Color.FromArgb(50, color.R, color.G, color.B));
            Application.Current.Resources["WidgetContent"] = new SolidColorBrush(Color.FromArgb(35, color.R, color.G, color.B));
            Application.Current.Resources["WidgetText"] = new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
            Application.Current.Resources["WidgetSubtext"] = new SolidColorBrush(Color.FromArgb(125, color.R, color.G, color.B));

            _widgets = new List<Window>()
            {
                new ClockWidget(),
                //new WeatherWidget()
            };

            foreach (var widget in _widgets)
            {
                widget.Show();
                var methodInfo = widget.GetType().GetMethod("RefreshWidget");
                if (methodInfo != null)
                {
                    methodInfo.Invoke(widget, null);
                }
            }

            SendWidgetsToBack();

            var refreshTimer = new DispatcherTimer();
            refreshTimer.Interval = TimeSpan.FromSeconds(5);
            refreshTimer.Tick += (sender, e) =>
            {
                foreach (var widget in _widgets)
                {
                    var methodInfo = widget.GetType().GetMethod("RefreshWidget");
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(widget, null);
                    }
                }
            };
            refreshTimer.Start();
        }

        private void SendWidgetsToBack()
        {
            foreach (var widget in _widgets)
            {
                WindowAttributeManager.SendToBack(widget);
            }
        }

        private void ShowWindow()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Minimized:
                    Hide();
                    ShowInTaskbar = false;
                    break;
                case WindowState.Normal:
                    Show();
                    ShowInTaskbar = true;
                    break;
            }
        }

        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuShowWindow_Click(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void MenuSendToBack_Click(object sender, RoutedEventArgs e)
        {
            SendWidgetsToBack();
        }

        private void TaskbarIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }
    }
}