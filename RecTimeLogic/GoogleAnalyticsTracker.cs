using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleAnalytics.Core;

namespace RecTimeLogic
{
    public class GoogleAnalyticsTracker
    {
        private TrackerManager _trackerManager;
        private Tracker _tracker;

        public GoogleAnalyticsTracker(string appVersion, string userId)
        {
            _trackerManager = new TrackerManager(new PlatformInfoProvider()
            {
                AnonymousClientId = userId,
                ScreenResolution = new Dimensions(1920, 1080),
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko",
                UserLanguage = "en-us",
                ViewPortResolution = new Dimensions(1920, 1080)
            });

            _tracker = _trackerManager.GetTracker("UA-90438934-1");
            _tracker.AppName = "RecTime";
            _tracker.AppVersion = appVersion;
        }

        public Task SendView(string screenName)
        {
            return Task.Run(() => _tracker.SendView(screenName));
        }

        public Task SendEvent(string category, string action, string label, long value)
        {
            return Task.Run(() => _tracker.SendEvent(category, action, label, value));
        }
    }
}
