using Xunit;
using System;
using System.Globalization;
using System.Threading;

namespace Programmerare.ShortestPaths.Utils
{

    public class TimeMeasurerTest
    {
        private static readonly IFormatProvider _swedishCultureFormatProvider_ = new CultureInfo("sv-SE");
        private const string _swedishDateAndTimeFormatString_ = "yyyy-MM-dd HH:mm:ss"; // e.g. "2018-12-28 23:59:58"

        [Fact]
        public void aGetNumberOfSecondsBetweenTwoDatesTest()
        {
            DateTime startTime = DateTime.Now;
            TimeMeasurer timeMeasurer = TimeMeasurer.Start();
            Thread.Sleep(2900); // 2.9 seconds will be rounded to 2 seconds
            Assert.Equal(2, timeMeasurer.GetSeconds());
        }

        [Fact]
        public void GetNumberOfSecondsBetweenTwoDatesTest()
        {
            DateTime datetime_2018_12_28__23_59_58 = CreateDateTime("2018-12-28 23:59:58");
            DateTime datetime_2018_12_29__00_00_02 = CreateDateTime("2018-12-29 00:00:02");
            DateTime datetime_2018_12_29__00_01_03 = CreateDateTime("2018-12-29 00:01:03");
            DateTime datetime_2018_12_29__03_01_03 = CreateDateTime("2018-12-29 03:01:03"); // three hours since above d3 i.e. 3 * 3600 seconds
            
            Assert.Equal(4, TimeMeasurer.GetNumberOfSecondsBetweenTwoDates(
                    datetime_2018_12_28__23_59_58, 
                    datetime_2018_12_29__00_00_02
                ));

            Assert.Equal(61, TimeMeasurer.GetNumberOfSecondsBetweenTwoDates(
                    datetime_2018_12_29__00_00_02, 
                    datetime_2018_12_29__00_01_03
                ));
            Assert.Equal(-61, // negative since the order is reversed
                TimeMeasurer.GetNumberOfSecondsBetweenTwoDates(
                    datetime_2018_12_29__00_01_03, 
                    datetime_2018_12_29__00_00_02
                ));

            Assert.Equal(3 * 3600, // 3 hours
                TimeMeasurer.GetNumberOfSecondsBetweenTwoDates(
                    datetime_2018_12_29__00_01_03, 
                    datetime_2018_12_29__03_01_03
                ));
        }

        private static DateTime CreateDateTime(string dateAndTimeString)
        {
            return DateTime.ParseExact(dateAndTimeString, _swedishDateAndTimeFormatString_, _swedishCultureFormatProvider_, DateTimeStyles.None);
        }
    }
}