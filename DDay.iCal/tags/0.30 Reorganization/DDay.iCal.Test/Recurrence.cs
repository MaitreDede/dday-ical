using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Resources;
using System.Web;
using System.Web.UI;
using DDay.iCal.Components;
using DDay.iCal.DataTypes;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace DDay.iCal.Test
{
    [TestFixture]
    public class Recurrence
    {
        private TZID tzid;

        [TestFixtureSetUp]
        public void InitAll()
        {            
            tzid = new TZID("US-Eastern");
        }

        static public void DoTests()
        {
            Recurrence r = new Recurrence();
            r.InitAll();

            r.RECURPARSE1();
            r.RECURPARSE2();
            r.RECURPARSE3();
            r.RECURPARSE4();
            r.RECURPARSE5();
            r.RECURPARSE6();

            r.USHOLIDAYS();
            r.RRULE1();
            r.RRULE2();
            r.RRULE3();
            r.RRULE4();
            r.RRULE5();
            r.RRULE6();
            r.RRULE6_1();
            r.RRULE7();
            r.RRULE8();
            r.RRULE9();
            r.RRULE10();
            r.RRULE11();
            r.RRULE12();
            r.RRULE13();
            r.RRULE14();
            r.RRULE15();
            r.RRULE16();
            r.RRULE17();
            r.RRULE18();
            r.RRULE19();
            r.RRULE20();
            r.RRULE21();
            r.RRULE22();
            r.RRULE23();
            r.RRULE24();
            r.RRULE25();
            r.RRULE26();
            r.RRULE27();
            r.RRULE28();
            r.RRULE29();
            r.RRULE30();
            r.RRULE31();
            r.RRULE32();
            r.RRULE33();
            r.RRULE34();
            r.RRULE35();
            r.RRULE36();
            r.RRULE37();
            r.RRULE38();
            r.RRULE39();
            r.RRULE40();
            r.RRULE41();            
        }

        /// <summary>
        /// See Page 118 of RFC 2445 - RRULE:FREQ=YEARLY;INTERVAL=2;BYMONTH=1;BYDAY=SU;BYHOUR=8,9;BYMINUTE=30
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE1()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE1.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(2006, 1, 1, tzid, iCal), new Date_Time(2011, 1, 1, tzid, iCal));

            Date_Time dt = new Date_Time(2006, 1, 1, 8, 30, 0, tzid, iCal);
            while (dt.Year < 2011)
            {
                if ((dt > evt.Start) &&
                    (dt.Year % 2 == 0) &&
                    (dt.Month == 1) &&
                    (dt.DayOfWeek == DayOfWeek.Sunday))
                {
                    Date_Time dt1 = dt.AddHours(1);
                    Assert.IsTrue(evt.OccursAt(dt), "Event should occur at " + dt);
                    Assert.IsTrue(evt.OccursAt(dt1), "Event should occur at " + dt1);
                }
                else if (dt.Date != evt.Start.Date)
                    Assert.IsFalse(evt.OccursOn(dt), "Event should not occur on " + dt.Local.ToShortDateString());

                dt = dt.AddDays(1);
            }
        }

        /// <summary>
        /// See Page 118 of RFC 2445 - RRULE:FREQ=DAILY;COUNT=10;INTERVAL=2
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE2()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE2.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(2006, 7, 1, tzid, iCal), new Date_Time(2006, 9, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(2006, 07, 18, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 07, 20, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 07, 22, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 07, 24, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 07, 26, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 07, 28, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 07, 30, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 08, 01, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 08, 03, 10, 00, 00, tzid, iCal),
                new Date_Time(2006, 08, 05, 10, 00, 00, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 118 of RFC 2445 - RRULE:FREQ=DAILY;UNTIL=19971224T000000Z
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE3()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE3.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal), new Date_Time(1998, 1, 1, tzid, iCal));

            Date_Time dt = new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal);
            while (dt.Year < 1998)
            {
                if ((dt >= evt.Start) &&
                    (dt < new Date_Time(1997, 12, 24, 0, 0, 0, tzid, iCal)))
                {
                    Assert.IsTrue(evt.OccursAt(dt), "Event should occur at " + dt);
                    Assert.IsTrue(
                        (dt < new Date_Time(1997, 10, 26, tzid, iCal) && dt.TimeZoneInfo.TimeZoneName == "EDT") ||
                        (dt > new Date_Time(1997, 10, 26, tzid, iCal) && dt.TimeZoneInfo.TimeZoneName == "EST"),
                        "Event " + dt + " doesn't occur in the correct time zone (including Daylight & Standard time zones)");
                }
                else Assert.IsFalse(evt.OccursAt(dt), "Event should not occur at " + dt);

                dt = dt.AddDays(1);
            }
        }

        /// <summary>
        /// See Page 118 of RFC 2445 - RRULE:FREQ=DAILY;INTERVAL=2
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE4()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE4.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal), new Date_Time(1997, 12, 4, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 6, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 8, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 20, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 22, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 24, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 26, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 6, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 8, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 20, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 22, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 24, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 26, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 17, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 21, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 23, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 25, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 27, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 3, 9, 0, 0, tzid, iCal)                
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 119 of RFC 2445 - RRULE:FREQ=DAILY;INTERVAL=10;COUNT=5
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE5()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE5.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal), new Date_Time(1998, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 22, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 12, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 119 of RFC 2445 - RRULE:FREQ=DAILY;UNTIL=20000131T090000Z;BYMONTH=1
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE6()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE6.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal), new Date_Time(2000, 12, 31, tzid, iCal));

            Date_Time dt = new Date_Time(1998, 1, 1, 9, 0, 0, tzid, iCal);
            while (dt.Year < 2001)
            {
                if (dt >= evt.Start &&
                    dt.Month == 1 &&
                    dt <= new Date_Time(2000, 1, 31, 9, 0, 0, tzid, iCal))
                    Assert.IsTrue(evt.OccursAt(dt), "Event should occur at " + dt);
                else Assert.IsFalse(evt.OccursAt(dt), "Event should not occur at " + dt);

                dt = dt.AddDays(1);
            }
        }

        /// <summary>
        /// See Page 119 of RFC 2445 - RRULE:FREQ=YEARLY;UNTIL=20000131T150000Z;BYMONTH=1;BYDAY=SU,MO,TU,WE,TH,FR,SA
        /// <note>
        ///     The example was slightly modified to fix a suspected flaw in the design of
        ///     the example RRULEs.  UNTIL is always UTC time, but it expected the actual
        ///     time to correspond to other time zones.  Odd.
        /// </note>        
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE6_1()
        {
            iCalendar iCal1 = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE6.ics");
            iCalendar iCal2 = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE6_1.ics");
            Program.TestCal(iCal1);
            Program.TestCal(iCal2);
            Event evt1 = (Event)iCal1.Events[0];
            Event evt2 = (Event)iCal2.Events[0];

            evt1.Evaluate(new Date_Time(1997, 9, 1), new Date_Time(2000, 12, 31));
            evt2.Evaluate(new Date_Time(1997, 9, 1), new Date_Time(2000, 12, 31));
            Assert.IsTrue(evt1.Periods.Count == evt2.Periods.Count, "RRULE6_1 does not match RRULE6 as it should");
            for (int i = 0; i < evt1.Periods.Count; i++)
                Assert.IsTrue(evt1.Periods[i].Equals(evt2.Periods[i]), "PERIOD " + i + " from RRULE6 (" + evt1.Periods[i].ToString() + ") does not match PERIOD " + i + " from RRULE6_1 (" + evt2.Periods[i].ToString() + ")");
        }

        /// <summary>
        /// See Page 119 of RFC 2445 - RRULE:FREQ=WEEKLY;COUNT=10
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE7()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE7.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal), new Date_Time(1998, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 23, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 21, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 4, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 119 of RFC 2445 - RRULE:FREQ=WEEKLY;UNTIL=19971224T000000Z
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE8()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE8.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal), new Date_Time(1999, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 23, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 21, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 25, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 23, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);

        }

        /// <summary>
        /// See Page 119 of RFC 2445 - RRULE:FREQ=WEEKLY;INTERVAL=2;WKST=SU
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE9()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE9.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal), new Date_Time(1998, 1, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 25, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 23, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 6, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 20, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 119 of RFC 2445 - RRULE:FREQ=WEEKLY;UNTIL=19971007T000000Z;WKST=SU;BYDAY=TU,TH
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE10()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE10.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal), new Date_Time(1999, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 23, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 25, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 2, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 120 of RFC 2445 - RRULE:FREQ=WEEKLY;COUNT=10;WKST=SU;BYDAY=TU,TH
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE11()
        {
            iCalendar iCal1 = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE10.ics");
            iCalendar iCal2 = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE11.ics");
            Program.TestCal(iCal1);
            Program.TestCal(iCal2);
            Event evt1 = (Event)iCal1.Events[0];
            Event evt2 = (Event)iCal2.Events[0];

            evt1.Evaluate(new Date_Time(1997, 9, 1), new Date_Time(1999, 1, 1));
            evt2.Evaluate(new Date_Time(1997, 9, 1), new Date_Time(1999, 1, 1));
            Assert.IsTrue(evt1.Periods.Count == evt2.Periods.Count, "RRULE11 does not match RRULE10 as it should");
            for (int i = 0; i < evt1.Periods.Count; i++)
                Assert.IsTrue(evt1.Periods[i].Equals(evt2.Periods[i]), "PERIOD " + i + " from RRULE10 (" + evt1.Periods[i].ToString() + ") does not match PERIOD " + i + " from RRULE11 (" + evt2.Periods[i].ToString() + ")");
        }

        /// <summary>
        /// See Page 120 of RFC 2445 - RRULE:FREQ=WEEKLY;INTERVAL=2;UNTIL=19971224T000000Z;WKST=SU;BYDAY=MO,WE,FR
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE12()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE12.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 17, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 17, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 27, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 31, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 24, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 26, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 8, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 22, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",                
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 120 of RFC 2445 - RRULE:FREQ=WEEKLY;INTERVAL=2;COUNT=8;WKST=SU;BYDAY=TU,TH
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE13()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE13.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 16, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 120 of RFC 2445 - RRULE:FREQ=MONTHLY;COUNT=10;BYDAY=1FR
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE14()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE14.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 2, 6, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 6, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 4, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 5, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 6, 5, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EDT",
                "EDT"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 120 of RFC 2445 - RRULE:FREQ=MONTHLY;UNTIL=19971224T000000Z;BYDAY=1FR
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE15()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE15.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 5, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 120 of RFC 2445 - RRULE:FREQ=MONTHLY;INTERVAL=2;COUNT=10;BYDAY=1SU,-1SU
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE16()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE16.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 25, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 5, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 5, 31, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EDT",
                "EDT"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 121 of RFC 2445 - RRULE:FREQ=MONTHLY;COUNT=6;BYDAY=-2MO
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE17()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE17.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 22, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 20, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 17, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 22, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 2, 16, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 121 of RFC 2445 - RRULE:FREQ=MONTHLY;BYMONTHDAY=-3
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE18()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE18.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 3, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 28, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 2, 26, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",                
                "EST",
                "EST",
                "EST",
                "EST",
                "EST"                
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 121 of RFC 2445 - RRULE:FREQ=MONTHLY;COUNT=10;BYMONTHDAY=2,15
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE19()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE19.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 3, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 15, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 121 of RFC 2445 - RRULE:FREQ=MONTHLY;COUNT=10;BYMONTHDAY=1,-1
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE20()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE20.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 3, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 31, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 31, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 31, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 2, 1, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",                
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 121 of RFC 2445 - RRULE:FREQ=MONTHLY;INTERVAL=18;COUNT=10;BYMONTHDAY=10,11,12,13,14,15
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE21()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE21.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(2000, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 13, 9, 0, 0, tzid, iCal),
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST"                
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 122 of RFC 2445 - RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=TU
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE22()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE22.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 4, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 23, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 25, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 6, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 20, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 27, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 17, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 24, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 31, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EDT",
                "EDT",                
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST"                
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 122 of RFC 2445 - RRULE:FREQ=YEARLY;COUNT=10;BYMONTH=6,7
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE23()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE23.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(2002, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 6, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 7, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 6, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 7, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 6, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 7, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2000, 6, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2000, 7, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2001, 6, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2001, 7, 10, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 122 of RFC 2445 - RRULE:FREQ=YEARLY;INTERVAL=2;COUNT=10;BYMONTH=1,2,3
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE24()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE24.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(2003, 4, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 3, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 1, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 2, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2001, 1, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2001, 2, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2001, 3, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2003, 1, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2003, 2, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2003, 3, 10, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EST", "Event " + dt + " should occur in the EST TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 122 of RFC 2445 - RRULE:FREQ=YEARLY;INTERVAL=3;COUNT=10;BYYEARDAY=1,100,200
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE25()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE25.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(2007, 1, 1, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 1, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 4, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 7, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(2000, 1, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(2000, 4, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(2000, 7, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(2003, 1, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(2003, 4, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(2003, 7, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(2006, 1, 1, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EST",
                "EDT",
                "EDT",
                "EST",
                "EDT",
                "EDT",
                "EST",
                "EDT",
                "EDT",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 123 of RFC 2445 - RRULE:FREQ=YEARLY;BYDAY=20MO
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE26()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE26.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 5, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 5, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 5, 17, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 123 of RFC 2445 - RRULE:FREQ=YEARLY;BYWEEKNO=20;BYDAY=MO
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE27()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE27.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 5, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 5, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 5, 17, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 123 of RFC 2445 - RRULE:FREQ=YEARLY;BYMONTH=3;BYDAY=TH
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE28()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE28.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 3, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 3, 20, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 3, 27, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 26, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 3, 25, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EST", "Event " + dt + " should occur in the EST TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 123 of RFC 2445 - RRULE:FREQ=YEARLY;BYDAY=TH;BYMONTH=6,7,8
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE29()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE29.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1999, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 6, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 6, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 6, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 6, 26, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 7, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 7, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 7, 17, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 7, 24, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 7, 31, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 14, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 21, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 28, 9, 0, 0, tzid, iCal),                
                new Date_Time(1998, 6, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 6, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 6, 18, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 6, 25, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 7, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 7, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 7, 16, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 7, 23, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 7, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 8, 6, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 8, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 8, 20, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 8, 27, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 6, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 6, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 6, 17, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 6, 24, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 7, 1, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 7, 8, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 7, 15, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 7, 22, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 7, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 8, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 8, 12, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 8, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 8, 26, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 123 of RFC 2445:
        /// EXDATE;TZID=US-Eastern:19970902T090000
        /// RRULE:FREQ=MONTHLY;BYDAY=FR;BYMONTHDAY=13
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE30()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE30.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(2000, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1998, 2, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 11, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1999, 8, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(2000, 10, 13, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EST",
                "EST",
                "EST",
                "EDT",
                "EDT"                
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 124 of RFC 2445 - RRULE:FREQ=MONTHLY;BYDAY=SA;BYMONTHDAY=7,8,9,10,11,12,13
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE31()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE31.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 6, 30, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 8, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 13, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 2, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 4, 11, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 5, 9, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 6, 13, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EDT",
                "EDT",
                "EDT"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 124 of RFC 2445 - RRULE:FREQ=YEARLY;INTERVAL=4;BYMONTH=11;BYDAY=TU;BYMONTHDAY=2,3,4,5,6,7,8
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE32()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE32.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(2004, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1996, 11, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(2000, 11, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(2004, 11, 2, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EST", "Event " + dt + " should occur in the EST TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 124 of RFC 2445 - RRULE:FREQ=MONTHLY;COUNT=3;BYDAY=TU,WE,TH;BYSETPOS=3
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE33()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE33.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(2004, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 4, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 7, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 6, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",
                "EDT",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 124 of RFC 2445 - RRULE:FREQ=MONTHLY;BYDAY=MO,TU,WE,TH,FR;BYSETPOS=-2
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE34()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE34.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 3, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 10, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 11, 27, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 12, 30, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 1, 29, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 2, 26, 9, 0, 0, tzid, iCal),
                new Date_Time(1998, 3, 30, 9, 0, 0, tzid, iCal)
            };

            string[] TimeZones = new string[]
            {
                "EDT",                
                "EST",
                "EST",
                "EST",
                "EST",
                "EST",
                "EST"
            };

            for (int i = 0; i < DateTimes.Length; i++)
            {
                Date_Time dt = (Date_Time)DateTimes[i];
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 125 of RFC 2445 - RRULE:FREQ=HOURLY;INTERVAL=3;UNTIL=19970902T170000Z
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE35()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE35.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 3, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 12, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 15, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 125 of RFC 2445 - RRULE:FREQ=MINUTELY;INTERVAL=15;COUNT=6
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE36()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE36.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 2, tzid, iCal), new Date_Time(1997, 9, 3, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 9, 15, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 9, 30, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 9, 45, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 10, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 10, 15, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 125 of RFC 2445 - RRULE:FREQ=MINUTELY;INTERVAL=90;COUNT=4
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE37()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE37.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 10, 30, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 12, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 13, 30, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 125 of RFC 2445 - RRULE:FREQ=DAILY;BYHOUR=9,10,11,12,13,14,15,16;BYMINUTE=0,20,40
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE38()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE38.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1997, 9, 2, tzid, iCal), new Date_Time(1997, 9, 4, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 9, 2, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 9, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 9, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 10, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 10, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 10, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 11, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 11, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 11, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 12, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 12, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 12, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 13, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 13, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 13, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 14, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 14, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 14, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 15, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 15, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 15, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 16, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 16, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 2, 16, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 9, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 9, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 10, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 10, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 10, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 11, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 11, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 11, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 12, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 12, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 12, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 13, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 13, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 13, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 14, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 14, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 14, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 15, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 15, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 15, 40, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 16, 0, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 16, 20, 0, tzid, iCal),
                new Date_Time(1997, 9, 3, 16, 40, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 125 of RFC 2445 - RRULE:FREQ=MINUTELY;INTERVAL=20;BYHOUR=9,10,11,12,13,14,15,16
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE39()
        {
            iCalendar iCal1 = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE38.ics");
            iCalendar iCal2 = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE39.ics");
            Program.TestCal(iCal1);
            Program.TestCal(iCal2);
            Event evt1 = (Event)iCal1.Events[0];
            Event evt2 = (Event)iCal2.Events[0];

            evt1.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal1), new Date_Time(1997, 9, 3, tzid, iCal1));
            evt2.Evaluate(new Date_Time(1997, 9, 1, tzid, iCal2), new Date_Time(1997, 9, 3, tzid, iCal2));
            Assert.IsTrue(evt1.Periods.Count == evt2.Periods.Count, "RRULE39 does not match RRULE38 as it should");
            for (int i = 0; i < evt1.Periods.Count; i++)
                Assert.IsTrue(evt1.Periods[i].Equals(evt2.Periods[i]), "PERIOD " + i + " from RRULE38 (" + evt1.Periods[i].ToString() + ") does not match PERIOD " + i + " from RRULE39 (" + evt2.Periods[i].ToString() + ")");
        }

        /// <summary>
        /// See Page 125 of RFC 2445 - RRULE:FREQ=WEEKLY;INTERVAL=2;COUNT=4;BYDAY=TU,SU;WKST=MO
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE40()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE40.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 8, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 10, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 24, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// See Page 125 of RFC 2445 - RRULE:FREQ=WEEKLY;INTERVAL=2;COUNT=4;BYDAY=TU,SU;WKST=SU
        /// This is the same as RRULE40, except WKST is SU, which changes the results.
        /// </summary>
        [Test, Category("Recurrence")]
        public void RRULE41()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Recurrence\RRULE41.ics");
            Program.TestCal(iCal);
            Event evt = iCal.Events[0];
            evt.Evaluate(new Date_Time(1996, 1, 1, tzid, iCal), new Date_Time(1998, 12, 31, tzid, iCal));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(1997, 8, 5, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 17, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 19, 9, 0, 0, tzid, iCal),
                new Date_Time(1997, 8, 31, 9, 0, 0, tzid, iCal)
            };

            foreach (Date_Time dt in DateTimes)
            {
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);
                Assert.IsTrue(dt.TimeZoneInfo.TimeZoneName == "EDT", "Event " + dt + " should occur in the EDT TimeZone");
            }

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// Tests the iCal holidays downloaded from apple.com
        /// </summary>
        [Test, Category("Recurrence")]
        public void USHOLIDAYS()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\General\USHolidays.ics");

            Assert.IsNotNull(iCal, "iCalendar was not loaded.");
            Hashtable items = new Hashtable();
            items["Christmas"] = new Date_Time(2006, 12, 25);
            items["Thanksgiving"] = new Date_Time(2006, 11, 23);
            items["Veteran's Day"] = new Date_Time(2006, 11, 11);
            items["Halloween"] = new Date_Time(2006, 10, 31);
            items["Daylight Saving Time Ends"] = new Date_Time(2006, 10, 29);
            items["Columbus Day"] = new Date_Time(2006, 10, 9);
            items["Labor Day"] = new Date_Time(2006, 9, 4);
            items["Independence Day"] = new Date_Time(2006, 7, 4);
            items["Father's Day"] = new Date_Time(2006, 6, 18);
            items["Flag Day"] = new Date_Time(2006, 6, 14);
            items["John F. Kennedy's Birthday"] = new Date_Time(2006, 5, 29);
            items["Memorial Day"] = new Date_Time(2006, 5, 29);
            items["Mother's Day"] = new Date_Time(2006, 5, 14);
            items["Cinco de Mayo"] = new Date_Time(2006, 5, 5);
            items["Earth Day"] = new Date_Time(2006, 4, 22);
            items["Easter"] = new Date_Time(2006, 4, 16);
            items["Tax Day"] = new Date_Time(2006, 4, 15);
            items["Daylight Saving Time Begins"] = new Date_Time(2006, 4, 2);
            items["April Fool's Day"] = new Date_Time(2006, 4, 1);
            items["St. Patrick's Day"] = new Date_Time(2006, 3, 17);
            items["Washington's Birthday"] = new Date_Time(2006, 2, 22);
            items["President's Day"] = new Date_Time(2006, 2, 20);
            items["Valentine's Day"] = new Date_Time(2006, 2, 14);
            items["Lincoln's Birthday"] = new Date_Time(2006, 2, 12);
            items["Groundhog Day"] = new Date_Time(2006, 2, 2);
            items["Martin Luther King, Jr. Day"] = new Date_Time(2006, 1, 16);
            items["New Year's Day"] = new Date_Time(2006, 1, 1);

            Hashtable occurred = new Hashtable();
            foreach (DictionaryEntry de in items)
            {
                occurred[de.Key] = false;
            }

            foreach (Event evt in iCal.Events)
            {
                evt.Evaluate(new Date_Time(2006, 1, 1), new Date_Time(2006, 12, 31));
                if (items.ContainsKey(evt.Summary.ToString()))
                {
                    Date_Time dt = (Date_Time)items[evt.Summary.ToString()];
                    if (evt.OccursOn(dt))
                        occurred[evt.Summary.ToString()] = true;
                }
            }

            foreach (DictionaryEntry de in occurred)
            {
                if ((bool)de.Value == false)
                    Assert.Fail(de.Key + " holiday was not expected.");
            }
        }

        /// <summary>
        /// Tests recurrence rule parsing in English.
        /// </summary>
        [Test, Category("Recurrence")]
        public void RECURPARSE1()
        {
            iCalendar iCal = new iCalendar();
            
            Event evt = Event.Create(iCal);
            evt.Summary = "Test event";
            evt.Start = new Date_Time(2006, 10, 1, 9, 0, 0);
            evt.Duration = new TimeSpan(1, 0, 0);
            evt.AddRecurrence(new Recur("Every 3rd month on the last tuesday and wednesday"));

            evt.Evaluate(new Date_Time(2006, 10, 1), new Date_Time(2007, 4, 30));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(2006, 10, 1, 9, 0, 0),
                new Date_Time(2006, 10, 25, 9, 0, 0),
                new Date_Time(2006, 10, 31, 9, 0, 0),
                new Date_Time(2007, 1, 30, 9, 0, 0),
                new Date_Time(2007, 1, 31, 9, 0, 0),
                new Date_Time(2007, 4, 24, 9, 0, 0),
                new Date_Time(2007, 4, 25, 9, 0, 0)
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// Tests recurrence rule parsing in English.
        /// </summary>
        [Test, Category("Recurrence")]
        public void RECURPARSE2()
        {
            iCalendar iCal = new iCalendar();

            Event evt = Event.Create(iCal);
            evt.Summary = "Test event";
            evt.Start = new Date_Time(2006, 10, 1, 9, 0, 0);
            evt.Duration = new TimeSpan(1, 0, 0);
            evt.AddRecurrence(new Recur("Every day at 6:00PM"));

            evt.Evaluate(new Date_Time(2006, 10, 1), new Date_Time(2006, 10, 6));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(2006, 10, 1, 9, 0, 0),
                new Date_Time(2006, 10, 1, 18, 0, 0),
                new Date_Time(2006, 10, 2, 18, 0, 0),
                new Date_Time(2006, 10, 3, 18, 0, 0),
                new Date_Time(2006, 10, 4, 18, 0, 0),
                new Date_Time(2006, 10, 5, 18, 0, 0),
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// Tests recurrence rule parsing in English.
        /// </summary>
        [Test, Category("Recurrence")]
        public void RECURPARSE3()
        {
            iCalendar iCal = new iCalendar();

            Event evt = Event.Create(iCal);
            evt.Summary = "Test event";
            evt.Start = new Date_Time(2006, 1, 1, 9, 0, 0);
            evt.Duration = new TimeSpan(1, 0, 0);
            evt.AddRecurrence(new Recur("Every other month, on day 21"));

            evt.Evaluate(new Date_Time(2006, 1, 1), new Date_Time(2006, 12, 31));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(2006, 1, 1, 9, 0, 0),
                new Date_Time(2006, 1, 21, 9, 0, 0),
                new Date_Time(2006, 3, 21, 9, 0, 0),
                new Date_Time(2006, 5, 21, 9, 0, 0),
                new Date_Time(2006, 7, 21, 9, 0, 0),
                new Date_Time(2006, 9, 21, 9, 0, 0),
                new Date_Time(2006, 11, 21, 9, 0, 0)                
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// Tests recurrence rule parsing in English.
        /// </summary>
        [Test, Category("Recurrence")]
        public void RECURPARSE4()
        {
            iCalendar iCal = new iCalendar();

            Event evt = Event.Create(iCal);
            evt.Summary = "Test event";
            evt.Start = new Date_Time(2006, 1, 1, 9, 0, 0);
            evt.Duration = new TimeSpan(1, 0, 0);
            evt.AddRecurrence(new Recur("Every 10 minutes for 5 occurrences"));

            evt.Evaluate(new Date_Time(2006, 1, 1), new Date_Time(2006, 1, 31));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(2006, 1, 1, 9, 0, 0),
                new Date_Time(2006, 1, 1, 9, 10, 0),
                new Date_Time(2006, 1, 1, 9, 20, 0),
                new Date_Time(2006, 1, 1, 9, 30, 0),
                new Date_Time(2006, 1, 1, 9, 40, 0)
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// Tests recurrence rule parsing in English.        
        /// </summary>
        [Test, Category("Recurrence")]
        public void RECURPARSE5()
        {
            iCalendar iCal = new iCalendar();

            Event evt = Event.Create(iCal);
            evt.Summary = "Test event";
            evt.Start = new Date_Time(2006, 1, 1, 9, 0, 0);
            evt.Duration = new TimeSpan(1, 0, 0);
            evt.AddRecurrence(new Recur("Every 10 minutes until 1/1/2006 9:50"));

            evt.Evaluate(new Date_Time(2006, 1, 1), new Date_Time(2006, 1, 31));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(2006, 1, 1, 9, 0, 0),
                new Date_Time(2006, 1, 1, 9, 10, 0),
                new Date_Time(2006, 1, 1, 9, 20, 0),
                new Date_Time(2006, 1, 1, 9, 30, 0),
                new Date_Time(2006, 1, 1, 9, 40, 0),
                new Date_Time(2006, 1, 1, 9, 50, 0)
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        /// <summary>
        /// Tests recurrence rule parsing in English.        
        /// </summary>
        [Test, Category("Recurrence")]
        public void RECURPARSE6()
        {
            iCalendar iCal = new iCalendar();

            Event evt = Event.Create(iCal);
            evt.Summary = "Test event";
            evt.Start = new Date_Time(2006, 1, 1, 9, 0, 0);
            evt.Duration = new TimeSpan(1, 0, 0);
            evt.AddRecurrence(new Recur("Every month on the first sunday, at 5:00PM, and at 7:00PM"));

            evt.Evaluate(new Date_Time(2006, 1, 1), new Date_Time(2006, 3, 31));

            Date_Time[] DateTimes = new Date_Time[]
            {
                new Date_Time(2006, 1, 1, 9, 0, 0),
                new Date_Time(2006, 1, 1, 17, 0, 0),
                new Date_Time(2006, 1, 1, 19, 0, 0),
                new Date_Time(2006, 2, 5, 17, 0, 0),
                new Date_Time(2006, 2, 5, 19, 0, 0),
                new Date_Time(2006, 3, 5, 17, 0, 0),
                new Date_Time(2006, 3, 5, 19, 0, 0)
            };

            foreach (Date_Time dt in DateTimes)
                Assert.IsTrue(evt.OccursAt(dt), "Event should occur on " + dt);

            Assert.IsTrue(evt.Periods.Count == DateTimes.Length, "There should be exactly " + DateTimes.Length + " occurrences; there were " + evt.Periods.Count);
        }

        
    }
}
