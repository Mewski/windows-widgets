using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WindowsWidgets
{
    public partial class Clock : UserControl
    {
        private DispatcherTimer _timer;
        private TimeZoneInfo _timeZone;

        public static readonly DependencyProperty CityNameProperty = DependencyProperty.Register("CityName", typeof(string), typeof(Clock), new PropertyMetadata("Greenwich"));
        public string CityName
        {
            get { return (string)GetValue(CityNameProperty); }
            set { SetValue(CityNameProperty, value); }
        }

        public static readonly DependencyProperty TimeZoneIdProperty = DependencyProperty.Register("TimeZoneId", typeof(string), typeof(Clock), new PropertyMetadata("Greenwich Standard Time"));
        public string TimeZoneId
        {
            get { return (string)GetValue(TimeZoneIdProperty); }
            set { SetValue(TimeZoneIdProperty, value); }
        }

        public Clock()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

            SetClockInformation();
            SetupTimer();
        }

        private void SetupTimer()
        {
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateClockHands();
        }

        private void UpdateClockHands()
        {
            var timeZoneDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZone);

            double minuteHandAngle = (timeZoneDateTime.Minute * 6) + (0.1 * timeZoneDateTime.Second); 
            double hourHandAngle = (timeZoneDateTime.Hour % 12 * 30) + (0.5 * timeZoneDateTime.Minute);

            RotateTransform minuteHandTransform = new RotateTransform(minuteHandAngle);
            MinuteHand.RenderTransform = minuteHandTransform;

            RotateTransform hourHandTransform = new RotateTransform(hourHandAngle);
            HourHand.RenderTransform = hourHandTransform;
        }

        public void SetClockInformation()
        {
            var foundTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
            var foundDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, foundTimeZone);
            _timeZone = foundTimeZone;

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var yesterday = today.AddDays(-1);

            if (foundDateTime.Day == today.Day)
            {
                TemporalLabel.Content = "Today";
            }

            if (foundDateTime.Day == tomorrow.Day)
            {
                TemporalLabel.Content = "Tomorrow";
            }

            if (foundDateTime.Day == yesterday.Day)
            {
                TemporalLabel.Content = "Yesterday";
            }

            var localTimeZone = TimeZoneInfo.Local;
            var localDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, localTimeZone);

            var offset = foundTimeZone.GetUtcOffset(foundDateTime) - localTimeZone.GetUtcOffset(localDateTime);
            var sign = offset.TotalHours >= 0 ? "+" : "-";
            var unit = (offset.Minutes == 0) ? "HRS" : (offset.TotalHours == 1) ? "HR" : string.Empty;
            OffsetLabel.Content = sign + Math.Abs(offset.Hours) + (offset.Minutes != 0 ? ":" + String.Format("{0:00}", Math.Abs(offset.Minutes)) : "") + unit;

            UpdateClockHands();
        }
    }
}