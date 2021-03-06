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
    public class Journal
    {
        private TZID tzid;

        [TestFixtureSetUp]
        public void InitAll()
        {
            tzid = new TZID("US-Eastern");
        }

        static public void DoTests()
        {
            Journal j = new Journal();
            j.InitAll();
            j.JOURNAL1();
            j.JOURNAL2();
        }

        [Test, Category("Journal")]
        public void JOURNAL1()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Journal\JOURNAL1.ics");
            Program.TestCal(iCal);
            DDay.iCal.Components.Journal j = (DDay.iCal.Components.Journal)iCal.Journals[0];

            Assert.IsNotNull(j, "Journal entry was null");
            Assert.IsTrue(j.Status == JournalStatus.DRAFT, "Journal entry should have been in DRAFT status, but it was in " + j.Status.ToString() + " status.");
            Assert.IsTrue(j.Class.Value == "PUBLIC", "Journal class should have been PUBLIC, but was " + j.Class + ".");
            Assert.IsNull(j.DTStart);
        }

        [Test, Category("Journal")]
        public void JOURNAL2()
        {
            iCalendar iCal = iCalendar.LoadFromFile(@"Calendars\Journal\JOURNAL2.ics");
            Program.TestCal(iCal);
            DDay.iCal.Components.Journal j = (DDay.iCal.Components.Journal)iCal.Journals[0];

            Assert.IsNotNull(j, "Journal entry was null");
            Assert.IsTrue(j.Status == JournalStatus.FINAL, "Journal entry should have been in FINAL status, but it was in " + j.Status.ToString() + " status.");
            Assert.IsTrue(j.Class.Value == "PRIVATE", "Journal class should have been PRIVATE, but was " + j.Class + ".");
            Assert.IsTrue(j.Organizer.CommonName.Value == "JohnSmith", "Organizer common name should have been JohnSmith, but was " + j.Organizer.CommonName.ToString());
            Assert.IsTrue(j.Organizer.SentBy.Value.AbsoluteUri == "mailto:jane_doe@host.com", "Organizer should have had been SENT-BY 'mailto:jane_doe@host.com'; it was sent by '" + j.Organizer.SentBy.Value.AbsoluteUri + "'");
            Assert.IsTrue(j.Organizer.DirectoryEntry.Value.OriginalString == "ldap://host.com:6666/o=3DDC%20Associates,c=3DUS??(cn=3DJohn%20Smith)", "Organizer's directory entry should have been 'ldap://host.com:6666/o=3DDC%20Associates,c=3DUS??(cn=3DJohn%20Smith)', but it was '" + j.Organizer.DirectoryEntry.Value.OriginalString + "'");
            Assert.IsNull(j.DTStart);
        }
    }
}
