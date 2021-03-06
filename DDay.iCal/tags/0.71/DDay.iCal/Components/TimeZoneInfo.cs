using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using DDay.iCal.Components;
using DDay.iCal.DataTypes;
using DDay.iCal.Serialization;

namespace DDay.iCal.Components
{    
    /// <summary>
    /// A class that contains time zone information, and is usually accessed
    /// from an iCalendar object using the <see cref="DDay.iCal.iCalendar.GetTimeZone"/> method.        
    /// </summary>
    public class TimeZoneInfo : RecurringComponent
    {
        #region Disabled Properties and Methods

        public override List<Alarm> Alarms
        {
            get
            {
                return base.Alarms;
            }
            set
            {
            }
        }

        public override Binary[] Attach
        {
            get
            {
                return base.Attach;
            }
            set
            {
            }
        }

        public override Cal_Address[] Attendee
        {
            get
            {
                return base.Attendee;
            }
            set
            {
            }
        }

        public override TextCollection[] Categories
        {
            get
            {
                return base.Categories;
            }
            set
            {
            }
        }

        public override string Category
        {
            get
            {
                return base.Category;
            }
            set
            {
            }
        }

        public override Text Class
        {
            get
            {
                return base.Class;
            }
            set
            {
            }
        }

        public override Text[] Comment
        {
            get
            {
                return base.Comment;
            }
            set
            {
            }
        }

        public override Text[] Contact
        {
            get
            {
                return base.Contact;
            }
            set
            {
            }
        }

        public override iCalDateTime Created
        {
            get
            {
                return base.Created;
            }
            set
            {
            }
        }

        public override Text Description
        {
            get
            {
                return base.Description;
            }
            set
            {
            }
        }

        public override RecurrenceDates[] ExDate
        {
            get
            {
                return base.ExDate;
            }
            set
            {
            }
        }

        public override RecurrencePattern[] ExRule
        {
            get
            {
                return base.ExRule;
            }
            set
            {
            }
        }

        public override iCalDateTime Last_Modified
        {
            get
            {
                return base.Last_Modified;
            }
            set
            {
            }
        }

        public override Cal_Address Organizer
        {
            get
            {
                return base.Organizer;
            }
            set
            {
            }
        }

        public override Integer Priority
        {
            get
            {
                return base.Priority;
            }
            set
            {
            }
        }

        public override iCalDateTime Recurrence_ID
        {
            get
            {
                return base.Recurrence_ID;
            }
            set
            {
            }
        }

        public override Text[] Related_To
        {
            get
            {
                return base.Related_To;
            }
            set
            {
            }
        }

        public override RequestStatus[] Request_Status
        {
            get
            {
                return base.Request_Status;
            }
            set
            {
            }
        }

        public override Integer Sequence
        {
            get
            {
                return base.Sequence;
            }
            set
            {
            }
        }

        public override Text Summary
        {
            get
            {
                return base.Summary;
            }
            set
            {
            }
        }

        public override Text UID
        {
            get
            {
                return base.UID;
            }
            set
            {
            }
        }

        public override URI Url
        {
            get
            {
                return base.Url;
            }
            set
            {
            }
        }

        public override List<AlarmOccurrence> PollAlarms()
        {
            return null;
        }

        public override List<AlarmOccurrence> PollAlarms(iCalDateTime Start, iCalDateTime End)
        {
            return null;
        }

        public override void AddAlarm(Alarm alarm)
        {
        }

        public override void AddAttendee(Cal_Address attendee)
        {
        }

        public override void AddCategory(string categoryName)
        {
        }

        public override void AddComment(string comment)
        {
        }

        public override void AddExceptionPattern(RecurrencePattern recur)
        {
        }

        public override void AddSingleException(iCalDateTime dt)
        {
        }

        protected override void EvaluateExDate(iCalDateTime FromDate, iCalDateTime ToDate)
        {
        }

        protected override void EvaluateExRule(iCalDateTime FromDate, iCalDateTime ToDate)
        {
        }

        #endregion

        #region Private Fields

        private UTC_Offset m_TZOffsetFrom;            
        private UTC_Offset m_TZOffsetTo;            
        private Text[] m_TZName;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the name of the current Time Zone.
        /// <example>
        ///     The following are examples:
        ///     <list type="bullet">
        ///         <item>EST</item>
        ///         <item>EDT</item>
        ///         <item>MST</item>
        ///         <item>MDT</item>
        ///     </list>
        /// </example>
        /// </summary>
        public string TimeZoneName
        {
            get
            {
                if (TZName.Length > 0)
                    return TZName[0].Value;
                return string.Empty;
            }
            set
            {
                if (TZName == null)
                    TZName = new Text[1];
                TZName[0] = new Text(value);
                TZName[0].Name = "TZNAME";
            }
        }

        [Serialized]
        public UTC_Offset TZOffsetFrom
        {
            get { return m_TZOffsetFrom; }
            set { m_TZOffsetFrom = value; }
        }

        [Serialized]
        public UTC_Offset TZOffsetTo
        {
            get { return m_TZOffsetTo; }
            set { m_TZOffsetTo = value; }
        }

        [Serialized]
        public Text[] TZName
        {
            get { return m_TZName; }
            set { m_TZName = value; }
        }

        #region Overrides

        /// <summary>
        /// Force the DTSTART into a local date-time value.
        /// 
        /// From RFC 2445:
        /// The mandatory "DTSTART" property gives the effective onset date and 
        /// local time for the time zone sub-component definition. "DTSTART" in
        /// this usage MUST be specified as a local DATE-TIME value.
        /// 
        /// Also from RFC 2445:
        /// The date with local time form is simply a date-time value that does
        /// not contain the UTC designator nor does it reference a time zone. For
        /// example, the following represents Janurary 18, 1998, at 11 PM:
        /// 
        /// DTSTART:19980118T230000
        /// </summary>
        [Serialized, DefaultValueType("DATE-TIME"), DisallowedTypes("DATE", "DATE-TIME")]
        public override iCalDateTime DTStart
        {
            get
            {
                return base.DTStart;
            }
            set
            {
                base.DTStart = value;
            }
        }

        #endregion

        #endregion

        #region Constructors

        public TimeZoneInfo() : base()
        {
            base.Sequence = null; // TimeZoneInfo does not allow for sequence numbers
        }
        public TimeZoneInfo(iCalObject parent) : base(parent)
        {
            base.Sequence = null; // TimeZoneInfo does not allow for sequence numbers
        }
        public TimeZoneInfo(string name, iCalObject parent)
            : base(parent)
        {
            base.Sequence = null; // TimeZoneInfo does not allow for sequence numbers
            this.Name = name;
        }

        #endregion

        #region Overrides

        internal override List<Period> Evaluate(iCalDateTime FromDate, iCalDateTime ToDate)
        {
            List<Period> periods = base.Evaluate(FromDate, ToDate);
            // Add the initial specified date/time for the time zone entry
            periods.Insert(0, new Period(Start, null));
            return periods;
        }

        /// <summary>
        /// Returns a typed copy of the TimeZoneInfo object.
        /// </summary>
        /// <returns>A typed copy of the TimeZoneInfo object.</returns>
        public new TimeZoneInfo Copy()
        {
            return (TimeZoneInfo)base.Copy();
        }

        /// <summary>
        /// Creates a copy of the <see cref="TimeZoneInfo"/> object.
        /// </summary>
        public override iCalObject Copy(iCalObject parent)
        {
            // Create a copy
            iCalObject obj = base.Copy(parent);

            // Copy the name, since the .ctor(iCalObject) constructor
            // doesn't handle it
            obj.Name = this.Name;
            
            return obj;
        }

        public override bool Equals(object obj)
        {
            TimeZoneInfo tzi = obj as TimeZoneInfo;
            if (tzi != null)
            {
                return object.Equals(TZName, tzi.TZName) &&
                    object.Equals(TZOffsetFrom, tzi.TZOffsetFrom) &&
                    object.Equals(TZOffsetTo, tzi.TZOffsetTo);
            }
            return base.Equals(obj);
        }
                              
        #endregion
    }    
}
