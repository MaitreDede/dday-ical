using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DDay.iCal.DataTypes
{
    /// <summary>
    /// An iCalendar representation of the <c>RRULE</c> property.
    /// </summary>
    public partial class RecurrencePattern : iCalDataType
    {
        #region Private Fields

        public System.Globalization.CultureInfo _Culture;
        public System.Globalization.Calendar _Calendar;

        private FrequencyType _Frequency;
        private iCalDateTime _Until;
        private int _Count = int.MinValue;
        private int _Interval = int.MinValue;
        private List<int> _BySecond = new List<int>();
        private List<int> _ByMinute = new List<int>();
        private List<int> _ByHour = new List<int>();
        private List<DaySpecifier> _ByDay = new List<DaySpecifier>();
        private List<int> _ByMonthDay = new List<int>();
        private List<int> _ByYearDay = new List<int>();
        private List<int> _ByWeekNo = new List<int>();
        private List<int> _ByMonth = new List<int>();
        private List<int> _BySetPos = new List<int>();
        private DayOfWeek _Wkst = DayOfWeek.Monday;
        private List<iCalDateTime> _StaticOccurrences = new List<iCalDateTime>();
        private RecurrenceRestrictionType? _RestrictionType = null;
        private RecurrenceEvaluationModeType? _EvaluationMode = null;

        #endregion

        #region Public Properties

        public FrequencyType Frequency
        {
            get { return _Frequency; }
            set { _Frequency = value; }
        }

        public iCalDateTime Until
        {
            get { return _Until; }
            set { _Until = value; }
        }

        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        public int Interval
        {
            get
            {
                if (_Interval == int.MinValue)
                    return 1;
                return _Interval;
            }
            set { _Interval = value; }
        }

        public List<int> BySecond
        {
            get { return _BySecond; }
            set { _BySecond = value; }
        }

        public List<int> ByMinute
        {
            get { return _ByMinute; }
            set { _ByMinute = value; }
        }

        public List<int> ByHour
        {
            get { return _ByHour; }
            set { _ByHour = value; }
        }

        public List<DaySpecifier> ByDay
        {
            get { return _ByDay; }
            set { _ByDay = value; }
        }

        public List<int> ByMonthDay
        {
            get { return _ByMonthDay; }
            set { _ByMonthDay = value; }
        }

        public List<int> ByYearDay
        {
            get { return _ByYearDay; }
            set { _ByYearDay = value; }
        }

        public List<int> ByWeekNo
        {
            get { return _ByWeekNo; }
            set { _ByWeekNo = value; }
        }

        public List<int> ByMonth
        {
            get { return _ByMonth; }
            set { _ByMonth = value; }
        }

        public List<int> BySetPos
        {
            get { return _BySetPos; }
            set { _BySetPos = value; }
        }

        public DayOfWeek Wkst
        {
            get { return _Wkst; }
            set { _Wkst = value; }
        }

        public List<iCalDateTime> StaticOccurrences
        {
            get { return _StaticOccurrences; }
            set { _StaticOccurrences = value; }
        }
                
        public RecurrenceRestrictionType RestrictionType
        {
            get
            {
                // NOTE: Fixes bug #1924358 - Cannot evaluate Secondly patterns
                if (_RestrictionType != null &&
                    _RestrictionType.HasValue)
                    return _RestrictionType.Value;
                else if (iCalendar != null)
                    return iCalendar.RecurrenceRestriction;
                else
                    return RecurrenceRestrictionType.Default;
            }
            set { _RestrictionType = value; }
        }

        public RecurrenceEvaluationModeType? EvaluationMode
        {
            get
            {
                // NOTE: Fixes bug #1924358 - Cannot evaluate Secondly patterns
                if (_EvaluationMode != null &&
                    _EvaluationMode.HasValue)
                    return _EvaluationMode.Value;
                else if (iCalendar != null)
                    return iCalendar.RecurrenceEvaluationMode;
                else
                    return RecurrenceEvaluationModeType.Default;
            }
            set { _EvaluationMode = value; }
        }

        #endregion

        #region Constructors

        public RecurrencePattern()
        {            
            _Calendar = System.Globalization.CultureInfo.CurrentCulture.Calendar;
        }
        public RecurrencePattern(string value)
            : this()
        {
            CopyFrom((RecurrencePattern)Parse(value));
        }

        #endregion

        #region Overrides

        public override void CopyFrom(object obj)
        {
            base.CopyFrom(obj);
            if (obj is RecurrencePattern)
            {
                RecurrencePattern r = (RecurrencePattern)obj;

                Frequency = r.Frequency;
                Until = r.Until;
                Count = r.Count;
                Interval = r.Interval;
                BySecond = new List<int>(r.BySecond);
                ByMinute = new List<int>(r.ByMinute);
                ByHour = new List<int>(r.ByHour);
                ByDay = new List<DaySpecifier>(r.ByDay);
                ByMonthDay = new List<int>(r.ByMonthDay);
                ByYearDay = new List<int>(r.ByYearDay);
                ByWeekNo = new List<int>(r.ByWeekNo);
                ByMonth = new List<int>(r.ByMonth);
                BySetPos = new List<int>(r.BySetPos);
                Wkst = r.Wkst;
                _RestrictionType = r._RestrictionType;
                _EvaluationMode = r._EvaluationMode;                
            }
            base.CopyFrom(obj);
        }

        public override bool Equals(object obj)
        {
            if (obj is RecurrencePattern)
            {
                RecurrencePattern r = (RecurrencePattern)obj;
                if (!ArrayEquals(r.ByDay.ToArray(), ByDay.ToArray()) ||
                    !ArrayEquals(r.ByHour.ToArray(), ByHour.ToArray()) ||
                    !ArrayEquals(r.ByMinute.ToArray(), ByMinute.ToArray()) ||
                    !ArrayEquals(r.ByMonth.ToArray(), ByMonth.ToArray()) ||
                    !ArrayEquals(r.ByMonthDay.ToArray(), ByMonthDay.ToArray()) ||
                    !ArrayEquals(r.BySecond.ToArray(), BySecond.ToArray()) ||
                    !ArrayEquals(r.BySetPos.ToArray(), BySetPos.ToArray()) ||
                    !ArrayEquals(r.ByWeekNo.ToArray(), ByWeekNo.ToArray()) ||
                    !ArrayEquals(r.ByYearDay.ToArray(), ByYearDay.ToArray()))
                    return false;
                if (r.Count != Count) return false;
                if (r.Frequency != Frequency) return false;
                if (r.Interval != Interval) return false;
                if (r.Until != null)
                {
                    if (!r.Until.Equals(Until))
                        return false;
                }
                else if (Until != null)
                    return false;
                if (r.Wkst != Wkst) return false;
                return true;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hashCode = 
                ByDay.GetHashCode() ^ ByHour.GetHashCode() ^ ByMinute.GetHashCode() ^
                ByMonth.GetHashCode() ^ ByMonthDay.GetHashCode() ^ BySecond.GetHashCode() ^
                BySetPos.GetHashCode() ^ ByWeekNo.GetHashCode() ^ ByYearDay.GetHashCode() ^
                Count.GetHashCode() ^ Frequency.GetHashCode();

            if (Interval.Equals(1))
                hashCode ^= 0x1;
            else hashCode ^= Interval.GetHashCode();

            hashCode ^= Until.GetHashCode();
            hashCode ^= Wkst.GetHashCode();
            return hashCode;
        }

        private bool ArrayEquals(Array a1, Array a2)
        {
            // NOTE: fixes a bug where arrays weren't properly compared
            if (a1 == null ||
                a2 == null)
            {
                if (a1 == a2)
                    return true;
                else return false;
            }
            if (!a1.Length.Equals(a2.Length))
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (!a1.GetValue(i).Equals(a2.GetValue(i)))
                    return false;
            return true;
        }

        public override bool TryParse(string value, ref object obj)
        {
            RecurrencePattern r = (RecurrencePattern)obj;
                        
            Match match = Regex.Match(value, @"FREQ=(SECONDLY|MINUTELY|HOURLY|DAILY|WEEKLY|MONTHLY|YEARLY);?(.*)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                // Parse the frequency type
                r.Frequency = (FrequencyType)Enum.Parse(typeof(FrequencyType), match.Groups[1].Value, true);

                // NOTE: fixed a bug where the group 2 match
                // resulted in an empty string, which caused
                // an error.
                if (match.Groups[2].Success &&
                    match.Groups[2].Length > 0)
                {
                    string[] keywordPairs = match.Groups[2].Value.Split(';');
                    foreach (string keywordPair in keywordPairs)
                    {
                        string[] keyValues = keywordPair.Split('=');                        
                        string keyword = keyValues[0];
                        string keyValue = keyValues[1];

                        switch (keyword.ToUpper())
                        {
                            case "UNTIL": r.Until = new iCalDateTime(keyValue); break;
                            case "COUNT": r.Count = Convert.ToInt32(keyValue); break;
                            case "INTERVAL": r.Interval = Convert.ToInt32(keyValue); break;
                            case "BYSECOND": AddInt32Values(r.BySecond, keyValue); break;
                            case "BYMINUTE": AddInt32Values(r.ByMinute, keyValue); break;
                            case "BYHOUR": AddInt32Values(r.ByHour, keyValue); break;
                            case "BYDAY":
                                {
                                    string[] days = keyValue.Split(',');
                                    foreach (string day in days)
                                        r.ByDay.Add(new DaySpecifier(day));
                                } break;
                            case "BYMONTHDAY": AddInt32Values(r.ByMonthDay, keyValue); break;
                            case "BYYEARDAY": AddInt32Values(r.ByYearDay, keyValue); break;
                            case "BYWEEKNO": AddInt32Values(r.ByWeekNo, keyValue); break;
                            case "BYMONTH": AddInt32Values(r.ByMonth, keyValue); break;
                            case "BYSETPOS": AddInt32Values(r.BySetPos, keyValue); break;
                            case "WKST": r.Wkst = GetDayOfWeek(keyValue); break;                                
                        }
                    }
                }
            }
            else if ((match = Regex.Match(value, @"every\s+(?<Interval>other|\d+)?\w{0,2}\s*(?<Freq>second|minute|hour|day|week|month|year)s?,?\s*(?<More>.+)", RegexOptions.IgnoreCase)).Success)
            {
                if (match.Groups["Interval"].Success)
                {
                    int interval;
                    if (!int.TryParse(match.Groups["Interval"].Value, out interval))
                        r.Interval = 2; // "other"
                    else r.Interval = interval;
                }
                else r.Interval = 1;

                switch (match.Groups["Freq"].Value.ToLower())
                {
                    case "second": r.Frequency = FrequencyType.Secondly; break;
                    case "minute": r.Frequency = FrequencyType.Minutely; break;
                    case "hour": r.Frequency = FrequencyType.Hourly; break;
                    case "day": r.Frequency = FrequencyType.Daily; break;
                    case "week": r.Frequency = FrequencyType.Weekly; break;
                    case "month": r.Frequency = FrequencyType.Monthly; break;
                    case "year": r.Frequency = FrequencyType.Yearly; break;
                }

                string[] values = match.Groups["More"].Value.Split(',');
                foreach (string item in values)
                {
                    if ((match = Regex.Match(item, @"(?<Num>\d+)\w\w\s+(?<Type>second|minute|hour|day|week|month)", RegexOptions.IgnoreCase)).Success ||
                        (match = Regex.Match(item, @"(?<Type>second|minute|hour|day|week|month)\s+(?<Num>\d+)", RegexOptions.IgnoreCase)).Success)
                    {
                        int num;
                        if (int.TryParse(match.Groups["Num"].Value, out num))
                        {
                            switch (match.Groups["Type"].Value.ToLower())
                            {
                                case "second":
                                    r.BySecond.Add(num);
                                    break;
                                case "minute":
                                    r.ByMinute.Add(num);
                                    break;
                                case "hour":
                                    r.ByHour.Add(num);
                                    break;
                                case "day":
                                    switch (r.Frequency)
                                    {
                                        case FrequencyType.Yearly:
                                            r.ByYearDay.Add(num);
                                            break;
                                        case FrequencyType.Monthly:
                                            r.ByMonthDay.Add(num);
                                            break;
                                    }
                                    break;
                                case "week":
                                    r.ByWeekNo.Add(num);
                                    break;
                                case "month":
                                    r.ByMonth.Add(num);
                                    break;
                            }
                        }
                    }
                    else if ((match = Regex.Match(item, @"(?<Num>\d+\w{0,2})?(\w|\s)+?(?<First>first)?(?<Last>last)?\s*((?<Day>sunday|monday|tuesday|wednesday|thursday|friday|saturday)\s*(and|or)?\s*)+", RegexOptions.IgnoreCase)).Success)
                    {
                        int num = int.MinValue;
                        if (match.Groups["Num"].Success)
                        {
                            if (int.TryParse(match.Groups["Num"].Value, out num))
                            {
                                if (match.Groups["Last"].Success)
                                {
                                    // Make number negative
                                    num *= -1;
                                }
                            }
                        }
                        else if (match.Groups["Last"].Success)
                            num = -1;
                        else if (match.Groups["First"].Success)
                            num = 1;

                        foreach (Capture capture in match.Groups["Day"].Captures)
                        {                            
                            DaySpecifier ds = new DaySpecifier((DayOfWeek)Enum.Parse(typeof(DayOfWeek), capture.Value, true));
                            ds.Num = num;
                            r.ByDay.Add(ds);
                        }                        
                    }
                    else if ((match = Regex.Match(item, @"at\s+(?<Hour>\d{1,2})(:(?<Minute>\d{2})((:|\.)(?<Second>\d{2}))?)?\s*(?<Meridian>(a|p)m?)?", RegexOptions.IgnoreCase)).Success)
                    {
                        int hour, minute, second;
                        
                        if (int.TryParse(match.Groups["Hour"].Value, out hour))
                        {
                            // Adjust for PM
                            if (match.Groups["Meridian"].Success && 
                                match.Groups["Meridian"].Value.ToUpper().StartsWith("P"))
                                hour += 12;
                            
                            r.ByHour.Add(hour);

                            if (match.Groups["Minute"].Success &&
                                int.TryParse(match.Groups["Minute"].Value, out minute))
                            {
                                r.ByMinute.Add(minute);
                                if (match.Groups["Second"].Success &&
                                    int.TryParse(match.Groups["Second"].Value, out second))
                                    r.BySecond.Add(second);
                            }
                        }
                    }
                    else if ((match = Regex.Match(item, @"^\s*until\s+(?<DateTime>.+)$", RegexOptions.IgnoreCase)).Success)
                    {
                        DateTime dt = DateTime.Parse(match.Groups["DateTime"].Value);
                        DateTime.SpecifyKind(dt, DateTimeKind.Utc);

                        r.Until = new iCalDateTime(dt);
                    }
                    else if ((match = Regex.Match(item, @"^\s*for\s+(?<Count>\d+)\s+occurrences\s*$", RegexOptions.IgnoreCase)).Success)
                    {
                        int count;
                        if (!int.TryParse(match.Groups["Count"].Value, out count))
                            return false;
                        else r.Count = count;
                    }
                }
            }
            else return false;

            CheckMutuallyExclusive("COUNT", "UNTIL", r.Count, r.Until);
            CheckRange("INTERVAL", r.Interval, 1, int.MaxValue);
            CheckRange("COUNT", r.Count, 1, int.MaxValue);
            CheckRange("BYSECOND", r.BySecond, 0, 59);
            CheckRange("BYMINUTE", r.ByMinute, 0, 59);
            CheckRange("BYHOUR", r.ByHour, 0, 23);
            CheckRange("BYMONTHDAY", r.ByMonthDay, -31, 31);
            CheckRange("BYYEARDAY", r.ByYearDay, -366, 366);
            CheckRange("BYWEEKNO", r.ByWeekNo, -53, 53);
            CheckRange("BYMONTH", r.ByMonth, 1, 12);
            CheckRange("BYSETPOS", r.BySetPos, -366, 366);
            return true;
        }

        /// <summary>
        /// Returns a typed copy of the object.
        /// </summary>
        /// <returns>A typed copy of the object.</returns>
        public new RecurrencePattern Copy()
        {
            return (RecurrencePattern)base.Copy();
        }

        #endregion

        #region Static Methods
        static public DayOfWeek GetDayOfWeek(string value)
        {
            switch (value.ToUpper())
            {
                case "SU": return DayOfWeek.Sunday;
                case "MO": return DayOfWeek.Monday;
                case "TU": return DayOfWeek.Tuesday;
                case "WE": return DayOfWeek.Wednesday;
                case "TH": return DayOfWeek.Thursday;
                case "FR": return DayOfWeek.Friday;
                case "SA": return DayOfWeek.Saturday;
            }
            throw new ArgumentException(value + " is not a valid iCal day-of-week indicator.");
        }

        static protected void AddInt32Values(List<int> array, string value)
        {
            string[] values = value.Split(',');
            foreach (string v in values)
                array.Add(Convert.ToInt32(v));
        }
        #endregion

        #region Protected Methods

        protected void EnsureByXXXValues(iCalDateTime StartDate)
        {
            // If the frequency is weekly, and
            // no day of week is specified, use
            // the original date's day of week.
            // NOTE: fixes RRULE7 and RRULE8 handling
            if (Frequency == FrequencyType.Weekly &&
                ByDay.Count == 0)
                this.ByDay.Add(new DaySpecifier(StartDate.Value.DayOfWeek));            
            if (Frequency > FrequencyType.Secondly &&
                this.BySecond.Count == 0 &&
                StartDate.HasTime /* NOTE: Fixes a bug where all-day events have BySecond/ByMinute/ByHour added incorrectly */)
                this.BySecond.Add(StartDate.Value.Second);
            if (Frequency > FrequencyType.Minutely &&
                this.ByMinute.Count == 0 &&
                StartDate.HasTime /* NOTE: Fixes a bug where all-day events have BySecond/ByMinute/ByHour added incorrectly */)
                this.ByMinute.Add(StartDate.Value.Minute);
            if (Frequency > FrequencyType.Hourly &&                
                this.ByHour.Count == 0 &&
                StartDate.HasTime /* NOTE: Fixes a bug where all-day events have BySecond/ByMinute/ByHour added incorrectly */)
                this.ByHour.Add(StartDate.Value.Hour);
            // If neither BYDAY, BYMONTHDAY, or BYYEARDAY is specified,
            // default to the current day of month
            // NOTE: fixes RRULE23 handling, added BYYEARDAY exclusion
            // to fix RRULE25 handling
            if (Frequency > FrequencyType.Weekly &&
                this.ByMonthDay.Count == 0 && 
                this.ByYearDay.Count == 0 && 
                this.ByDay.Count == 0) 
                this.ByMonthDay.Add(StartDate.Value.Day);
            // If neither BYMONTH nor BYYEARDAY is specified, default to
            // the current month
            // NOTE: fixes RRULE25 handling
            if (Frequency > FrequencyType.Monthly &&
                this.ByYearDay.Count == 0 && 
                this.ByDay.Count == 0 &&
                this.ByMonth.Count == 0)
                this.ByMonth.Add(StartDate.Value.Month);
        }

        protected void EnforceEvaluationRestrictions()
        {
            RecurrenceEvaluationModeType? evaluationMode = EvaluationMode;
            RecurrenceRestrictionType? evaluationRestriction = RestrictionType;

            if (evaluationRestriction != RecurrenceRestrictionType.NoRestriction)
            {
                switch (evaluationMode)
                {
                    case RecurrenceEvaluationModeType.AdjustAutomatically:
                        switch (Frequency)
                        {
                            case FrequencyType.Secondly:
                                {
                                    switch (evaluationRestriction)
                                    {
                                        case RecurrenceRestrictionType.Default:
                                        case RecurrenceRestrictionType.RestrictSecondly: Frequency = FrequencyType.Minutely; break;
                                        case RecurrenceRestrictionType.RestrictMinutely: Frequency = FrequencyType.Hourly; break;
                                        case RecurrenceRestrictionType.RestrictHourly: Frequency = FrequencyType.Daily; break;
                                    }
                                } break;
                            case FrequencyType.Minutely:
                                {
                                    switch (evaluationRestriction)
                                    {
                                        case RecurrenceRestrictionType.RestrictMinutely: Frequency = FrequencyType.Hourly; break;
                                        case RecurrenceRestrictionType.RestrictHourly: Frequency = FrequencyType.Daily; break;
                                    }
                                } break;
                            case FrequencyType.Hourly:
                                {
                                    switch (evaluationRestriction)
                                    {
                                        case RecurrenceRestrictionType.RestrictHourly: Frequency = FrequencyType.Daily; break;
                                    }
                                } break;
                            default: break;
                        } break;
                    case RecurrenceEvaluationModeType.ThrowException:
                    case RecurrenceEvaluationModeType.Default:
                        switch (Frequency)
                        {
                            case FrequencyType.Secondly:
                                {
                                    switch (evaluationRestriction)
                                    {
                                        case RecurrenceRestrictionType.Default:
                                        case RecurrenceRestrictionType.RestrictSecondly:
                                        case RecurrenceRestrictionType.RestrictMinutely:
                                        case RecurrenceRestrictionType.RestrictHourly:
                                            throw new EvaluationEngineException();
                                    }
                                } break;
                            case FrequencyType.Minutely:
                                {
                                    switch (evaluationRestriction)
                                    {
                                        case RecurrenceRestrictionType.RestrictMinutely: 
                                        case RecurrenceRestrictionType.RestrictHourly:
                                            throw new EvaluationEngineException();
                                    }
                                } break;
                            case FrequencyType.Hourly:
                                {
                                    switch (evaluationRestriction)
                                    {
                                        case RecurrenceRestrictionType.RestrictHourly:
                                            throw new EvaluationEngineException();
                                    }
                                } break;
                            default: break;
                        } break;                        
                }
            }             
        }

        #region Calculating Occurrences

        protected List<iCalDateTime> GetOccurrences(iCalDateTime StartDate, iCalDateTime FromDate, iCalDateTime ToDate, int Count)
        {
            List<iCalDateTime> DateTimes = new List<iCalDateTime>();

            // If the Recur is restricted by COUNT, we need to evaluate just
            // after any static occurrences so it's correctly restricted to a
            // certain number. NOTE: fixes bug #13 and bug #16
            if (Count > 0)
            {
                FromDate = StartDate;
                foreach (iCalDateTime dt in StaticOccurrences)
                {
                    if (FromDate < dt)
                        FromDate = dt;
                }
            }

            // Handle "UNTIL" values that are date-only. If we didn't change values here, "UNTIL" would
            // exclude the day it specifies, instead of the inclusive behaviour it should exhibit.
            if (Until != null && !Until.HasTime)
                Until.Value = new DateTime(Until.Year, Until.Month, Until.Day, 23, 59, 59, Until.Value.Kind);

            // Ignore recurrences that occur outside our time frame we're looking at
            if ((Until != null && FromDate > Until) ||
                ToDate < StartDate)
                return DateTimes;

            // Narrow down our time range further to avoid over-processing
            if (Until != null && Until < ToDate)
                ToDate = Until;
            if (StartDate > FromDate)
                FromDate = StartDate;

            // If the interval is greater than 1, then we need to ensure that the StartDate occurs in one of the
            // "active" days/weeks/months/years/etc. to ensure that we properly "step" through the interval.
            // NOTE: Fixes bug #1741093 - WEEKLY frequency eval behaves strangely
            {
                long difference = DateUtils.DateDiff(Frequency, StartDate, FromDate, Wkst);
                while (difference % Interval > 0)
                {
                    FromDate = DateUtils.AddFrequency(Frequency, FromDate, -1);
                    difference--;
                }                
            }

            // If the start date has no time, then our "From" date should not either 
            // NOTE: Fixes bug #1876582 - All-day holidays are sometimes giving incorrect times
            if (!StartDate.HasTime)
            {
                FromDate = new iCalDateTime(FromDate.Year, FromDate.Month, FromDate.Day, StartDate.Hour, StartDate.Minute, StartDate.Second);
                FromDate.IsUniversalTime = StartDate.IsUniversalTime;
                FromDate.HasTime = false;
            }

            while (
                FromDate <= ToDate &&
                (
                    Count == int.MinValue ||
                    DateTimes.Count <= Count)
                )
            {
                // Retrieve occurrences that occur on our interval period
                if (BySetPos.Count == 0 && IsValidDate(FromDate) && !DateTimes.Contains(FromDate.Value))
                    DateTimes.Add(FromDate.Copy());

                // Retrieve "extra" occurrences that happen within our interval period
                if (Frequency > FrequencyType.Secondly)
                {
                    foreach (iCalDateTime dt in GetExtraOccurrences(FromDate, ToDate))
                    {
                        // Don't add duplicates
                        if (!DateTimes.Contains(dt))
                            DateTimes.Add(dt.Copy());
                    }
                }

                IncrementDate(ref FromDate);
            }

            return DateTimes;
        }

        public void IncrementDate(ref iCalDateTime dt)
        {
            IncrementDate(ref dt, this.Interval);
        }

        public void IncrementDate(ref iCalDateTime dt, int Interval)
        {
            iCalDateTime old = dt.Copy();
            switch (Frequency)
            {
                case FrequencyType.Secondly: dt = old.AddSeconds(Interval); break;
                case FrequencyType.Minutely: dt = old.AddMinutes(Interval); break;
                case FrequencyType.Hourly: dt = old.AddHours(Interval); break;
                case FrequencyType.Daily: dt = old.AddDays(Interval); break;
                case FrequencyType.Weekly:
                    // How the week increments depends on the WKST indicated (defaults to Monday)
                    // So, basically, we determine the week of year using the necessary rules,
                    // and we increment the day until the week number matches our "goal" week number.
                    // So, if the current week number is 36, and our Interval is 2, then our goal
                    // week number is 38.
                    // NOTE: fixes RRULE12 eval.
                    int current = _Calendar.GetWeekOfYear(old.Value, System.Globalization.CalendarWeekRule.FirstFourDayWeek, Wkst),
                        lastLastYear = _Calendar.GetWeekOfYear(new DateTime(old.Year-1, 12, 31, 0, 0, 0, DateTimeKind.Local), System.Globalization.CalendarWeekRule.FirstFourDayWeek, Wkst),
                        last = _Calendar.GetWeekOfYear(new DateTime(old.Year, 12, 31, 0, 0, 0, DateTimeKind.Local), System.Globalization.CalendarWeekRule.FirstFourDayWeek, Wkst),
                        goal = current + Interval;

                    // If the goal week is greater than the last week of the year, wrap it!
                    if (goal > last)
                        goal = goal - last;
                    else if (goal <= 0)
                        goal = lastLastYear + goal;

                    int interval = Interval > 0 ? 1 : -1;
                    while (current != goal)
                    {
                        old = old.AddDays(interval);
                        current = _Calendar.GetWeekOfYear(old.Value, System.Globalization.CalendarWeekRule.FirstFourDayWeek, Wkst);
                    }

                    dt = old;
                    break;
                case FrequencyType.Monthly: dt = old.AddDays(-old.Day + 1).AddMonths(Interval); break;
                case FrequencyType.Yearly: dt = old.AddDays(-old.DayOfYear + 1).AddYears(Interval); break;
                default: throw new Exception("FrequencyType.NONE cannot be evaluated. Please specify a FrequencyType before evaluating the recurrence.");
            }
        }

        #endregion

        #region Calculating Extra Occurrences

        protected List<iCalDateTime> GetExtraOccurrences(iCalDateTime StartDate, iCalDateTime AbsEndDate)
        {
            List<iCalDateTime> DateTimes = new List<iCalDateTime>();
            iCalDateTime EndDate = new iCalDateTime(StartDate);
            
            // FIXME: is there a reason for this?
            //AbsEndDate = AbsEndDate.AddSeconds(-1);

            IncrementDate(ref EndDate, 1);
            EndDate = EndDate.AddSeconds(-1);
            if (EndDate > AbsEndDate)
                EndDate = AbsEndDate;

            return CalculateChildOccurrences(StartDate, EndDate);
        }

        /// <summary>
        /// NOTE: fixes RRULE25
        /// </summary>
        /// <param name="TC"></param>
        protected void FillYearDays(TimeCalculation TC)
        {
            DateTime baseDate = new DateTime(TC.StartDate.Value.Year, 1, 1);
            foreach (int day in TC.YearDays)
            {
                DateTime currDate;
                if (day > 0)
                    currDate = baseDate.AddDays(day - 1);
                else if (day < 0)
                    currDate = baseDate.AddYears(1).AddDays(day);
                else throw new Exception("BYYEARDAY cannot contain 0");

                TC.Month = currDate.Month;
                TC.Day = currDate.Day;
                FillHours(TC);
            }
        }

        /// <summary>
        /// NOTE: fixes RRULE26
        /// </summary>
        /// <param name="TC"></param>
        protected void FillByDay(TimeCalculation TC)
        {       
            // If BYMONTH is specified, offset each day into those months,
            // otherwise, use Jan. 1st as a reference date.
            // NOTE: fixes USHolidays.ics eval
            List<int> months = new List<int>();
            if (TC.Recur.ByMonth.Count == 0)
                months.Add(1);
            else months = TC.Months;

            foreach (int month in months)
            {
                DateTime baseDate = new DateTime(TC.StartDate.Value.Year, month, 1);
                foreach (DaySpecifier day in TC.ByDays)
                {
                    DateTime curr = baseDate;

                    int inc = (day.Num < 0) ? -1 : 1;
                    if (day.Num != int.MinValue &&
                        day.Num < 0)
                    {
                        // Start at end of year, or end of month if BYMONTH is specified
                        if (TC.Recur.ByMonth.Count == 0)
                            curr = curr.AddYears(1).AddDays(-1);
                        else curr = curr.AddMonths(1).AddDays(-1);
                    }

                    while (curr.DayOfWeek != day.DayOfWeek)
                        curr = curr.AddDays(inc);

                    if (day.Num != int.MinValue)
                    {
                        for (int i = 1; i < day.Num; i++)
                            curr = curr.AddDays(7 * inc);

                        TC.Month = curr.Month;
                        TC.Day = curr.Day;
                        FillHours(TC);
                    }
                    else
                    {
                        while (
                            (TC.Recur.Frequency == FrequencyType.Monthly &&
                            curr.Month == TC.StartDate.Value.Month) ||
                            (TC.Recur.Frequency == FrequencyType.Yearly &&
                            curr.Year == TC.StartDate.Value.Year))
                        {
                            TC.Month = curr.Month;
                            TC.Day = curr.Day;
                            FillHours(TC);
                            curr = curr.AddDays(7);
                        }
                    }
                }
            }
        }

        protected void FillMonths(TimeCalculation TC)
        {
            foreach (int month in TC.Months)
            {
                TC.Month = month;
                FillDays(TC);
            }
        }

        protected void FillDays(TimeCalculation TC)
        {            
            foreach (int day in TC.Days)
            {
                TC.Day = day;
                FillHours(TC);
            }
        }

        protected void FillHours(TimeCalculation TC)
        {
            if (TC.Hours.Count > 0)
            {
                foreach (int hour in TC.Hours)
                {
                    TC.Hour = hour;
                    FillMinutes(TC);
                }
            }
            else
            {
                TC.Hour = 0;
                FillMinutes(TC);
            }
        }

        protected void FillMinutes(TimeCalculation TC)
        {
            if (TC.Minutes.Count > 0)
            {
                foreach (int minute in TC.Minutes)
                {
                    TC.Minute = minute;
                    FillSeconds(TC);
                }
            }
            else
            {
                TC.Minute = 0;
                FillSeconds(TC);
            }
        }

        protected void FillSeconds(TimeCalculation TC)
        {
            if (TC.Seconds.Count > 0)
            {
                foreach (int second in TC.Seconds)
                {
                    TC.Second = second;
                    TC.Calculate();
                }
            }
            else
            {
                TC.Second = 0;
                TC.Calculate();
            }
        }

        protected List<iCalDateTime> CalculateChildOccurrences(iCalDateTime StartDate, iCalDateTime EndDate)
        {
            TimeCalculation TC = new TimeCalculation(StartDate, EndDate, this);                        
            switch (Frequency)
            {
                case FrequencyType.Yearly:
                    FillYearDays(TC);
                    FillByDay(TC);
                    FillMonths(TC);
                    break;
                case FrequencyType.Weekly: 
                    // Weeks can span across months, so we must
                    // fill months (Note: fixes RRULE10 eval)                    
                    FillMonths(TC);
                    break;
                case FrequencyType.Monthly:
                    FillDays(TC);
                    FillByDay(TC);
                    break;
                case FrequencyType.Daily:
                    FillHours(TC);
                    break;
                case FrequencyType.Hourly:
                    FillMinutes(TC);
                    break;
                case FrequencyType.Minutely:
                    FillSeconds(TC);
                    break;
                default:
                    throw new NotSupportedException("CalculateChildOccurrences() is not supported for a frequency of " + Frequency.ToString());                    
            }

            // Apply the BYSETPOS to the list of child occurrences
            // We do this before the dates are filtered by Start and End date
            // so that the BYSETPOS calculates correctly.
            // NOTE: fixes RRULE33 eval
            if (BySetPos.Count != 0)
            {
                List<iCalDateTime> newDateTimes = new List<iCalDateTime>();
                foreach (int pos in BySetPos)
                {
                    if (Math.Abs(pos) <= TC.DateTimes.Count)
                    {
                        if (pos > 0)
                            newDateTimes.Add(TC.DateTimes[pos - 1]);
                        else if (pos < 0)
                            newDateTimes.Add(TC.DateTimes[TC.DateTimes.Count + pos]);
                    }
                }

                TC.DateTimes = newDateTimes;
            }

            // Filter dates by Start and End date
            for (int i = TC.DateTimes.Count - 1; i >= 0; i--)
            {
                if ((iCalDateTime)TC.DateTimes[i] < StartDate ||
                    (iCalDateTime)TC.DateTimes[i] > EndDate)
                    TC.DateTimes.RemoveAt(i);
            }

            return TC.DateTimes;
        }

        #endregion        

        #endregion

        #region Public Methods

        public List<iCalDateTime> Evaluate(iCalDateTime StartDate, iCalDateTime FromDate, iCalDateTime ToDate)
        {
            List<iCalDateTime> DateTimes = new List<iCalDateTime>();
            DateTimes.AddRange(StaticOccurrences);

            // Create a temporary recurrence for populating 
            // missing information using the 'StartDate'.
            RecurrencePattern r = new RecurrencePattern();
            r.CopyFrom(this);

            // Enforce evaluation engine rules
            r.EnforceEvaluationRestrictions();

            // Fill in missing, necessary ByXXX values
            r.EnsureByXXXValues(StartDate);
                        
            // Get the occurrences
            foreach (iCalDateTime occurrence in r.GetOccurrences(StartDate, FromDate.Copy(), ToDate.Copy(), r.Count))
            {
                // NOTE:
                // Used to be DateTimes.AddRange(r.GetOccurrences(FromDate.Copy(), ToDate, r.Count))
                // By doing it this way, fixes bug #19.
                if (!DateTimes.Contains(occurrence))
                    DateTimes.Add(occurrence);
            }

            // Limit the count of returned recurrences
            if (Count != int.MinValue &&
                DateTimes.Count > Count)
                DateTimes.RemoveRange(Count, DateTimes.Count - Count);

            // Process the UNTIL, and make sure the DateTimes
            // occur between FromDate and ToDate
            for (int i = DateTimes.Count - 1; i >= 0; i--)
            {
                iCalDateTime dt = (iCalDateTime)DateTimes[i];
                if (dt > ToDate ||
                    dt < FromDate)
                    DateTimes.RemoveAt(i);
            }

            // Assign missing values
            foreach (iCalDateTime dt in DateTimes)
                dt.MergeWith(StartDate);

            // Ensure that DateTimes have an assigned time if they occur less than daily
            foreach (iCalDateTime dt in DateTimes)
            {
                if (Frequency < FrequencyType.Daily)
                    dt.HasTime = true;
            }
            
            return DateTimes;
        }

        /// <summary>
        /// [Deprecated]: Use IsValidDate() instead.
        /// </summary>
        public bool CheckValidDate(iCalDateTime dt) { return IsValidDate(dt); }

        /// <summary>
        /// Returns true if <paramref name="dt"/> is a date/time that aligns to (occurs within)
        /// the recurrence pattern of this Recur, false otherwise.
        /// </summary>
        public bool IsValidDate(iCalDateTime dt)
        {
            if (BySecond.Count != 0 && !BySecond.Contains(dt.Value.Second)) return false;
            if (ByMinute.Count != 0 && !ByMinute.Contains(dt.Value.Minute)) return false;
            if (ByHour.Count != 0 && !ByHour.Contains(dt.Value.Hour)) return false;
            if (ByDay.Count != 0)
            {
                bool found = false;
                foreach (DaySpecifier bd in ByDay)
                {
                    if (bd.CheckValidDate(this, dt))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;
            }
            if (ByWeekNo.Count != 0)
            {
                bool found = false;
                int lastWeekOfYear = _Calendar.GetWeekOfYear(new DateTime(dt.Value.Year, 12, 31), System.Globalization.CalendarWeekRule.FirstFourDayWeek, Wkst);
                int currWeekNo = _Calendar.GetWeekOfYear(dt.Value, System.Globalization.CalendarWeekRule.FirstFourDayWeek, Wkst);
                foreach (int WeekNo in ByWeekNo)
                {
                    if ((WeekNo > 0 && WeekNo == currWeekNo) ||
                        (WeekNo < 0 && lastWeekOfYear + WeekNo + 1 == currWeekNo))
                        found = true;
                }
                if (!found)
                    return false;
            }
            if (ByMonth.Count != 0 && !ByMonth.Contains(dt.Value.Month)) return false;
            if (ByMonthDay.Count != 0)
            {
                // Handle negative days of month (count backwards from the end)
                // NOTE: fixes RRULE18 eval
                bool found = false;
                int DaysInMonth = _Calendar.GetDaysInMonth(dt.Value.Year, dt.Value.Month);
                foreach (int Day in ByMonthDay)
                {
                    if ((Day > 0) && (Day == dt.Value.Day))
                        found = true;
                    else if ((Day < 0) && (DaysInMonth + Day + 1 == dt.Value.Day))
                        found = true;
                }

                if (!found)
                    return false;
            }
            if (ByYearDay.Count != 0)
            {
                // Handle negative days of year (count backwards from the end)
                // NOTE: fixes RRULE25 eval
                bool found = false;
                int DaysInYear = _Calendar.GetDaysInYear(dt.Value.Year);
                DateTime baseDate = new DateTime(dt.Value.Year, 1, 1);

                foreach (int Day in ByYearDay)
                {
                    if (Day > 0 && dt.Value.Date == baseDate.AddDays(Day - 1))
                        found = true;
                    else if (Day < 0 && dt.Value.Date == baseDate.AddYears(1).AddDays(Day))
                        found = true;
                }
                if (!found)
                    return false;
            }
            return true;
        }

        #endregion

        #region Helper Classes

        protected class TimeCalculation
        {
            public iCalDateTime StartDate;
            public iCalDateTime EndDate;
            public RecurrencePattern Recur;
            public int Year;
            public List<DaySpecifier> ByDays;
            public List<int> YearDays;
            public List<int> Months;
            public List<int> Days;
            public List<int> Hours;
            public List<int> Minutes;
            public List<int> Seconds;
            public int Month;
            public int Day;
            public int Hour;
            public int Minute;
            public int Second;
            public List<iCalDateTime> DateTimes;

            public TimeCalculation(iCalDateTime StartDate, iCalDateTime EndDate, RecurrencePattern Recur)
            {
                this.StartDate = StartDate;
                this.EndDate = EndDate;
                this.Recur = Recur;

                Year = StartDate.Value.Year;
                Month = StartDate.Value.Month;
                Day = StartDate.Value.Day;
                Hour = StartDate.Value.Hour;
                Minute = StartDate.Value.Minute;
                Second = StartDate.Value.Second;

                YearDays = new List<int>(Recur.ByYearDay);
                ByDays = new List<DaySpecifier>(Recur.ByDay);
                Months = new List<int>(Recur.ByMonth);
                Days = new List<int>(Recur.ByMonthDay);
                Hours = new List<int>(Recur.ByHour);
                Minutes = new List<int>(Recur.ByMinute);
                Seconds = new List<int>(Recur.BySecond);
                DateTimes = new List<iCalDateTime>();

                // Only check what months and days are possible for
                // the week's period of time we're evaluating
                // NOTE: fixes RRULE10 evaluation                
                if (Recur.Frequency == FrequencyType.Weekly)
                {                    
                    if (Months.Count == 0)
                    {
                        Months.Add(StartDate.Value.Month);
                        if (StartDate.Value.Month != EndDate.Value.Month)
                            Months.Add(EndDate.Value.Month);
                    }
                    if (Days.Count == 0)
                    {
                        DateTime dt = StartDate.Value;
                        while (dt < EndDate.Value)
                        {
                            Days.Add(dt.Day);
                            dt = dt.AddDays(1);
                        }
                        Days.Add(EndDate.Value.Day);
                    }
                }
                else
                {
                    if (Months.Count == 0) Months.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });                                
                    if (Days.Count == 0) Days.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 });
                }
            }

            public iCalDateTime CurrentDateTime
            {
                get
                {
                    iCalDateTime dt = null;
                    // Account for negative days of month (count backwards from the end of the month)
                    // NOTE: fixes RRULE18 evaluation
                    if (Day > 0)
                        // Changed from DateTimeKind.Local to StartDate.Kind
                        // NOTE: fixes bug #20
                        dt = new iCalDateTime(new DateTime(Year, Month, Day, Hour, Minute, Second, DateTimeKind.Utc));
                    else
                        dt = new iCalDateTime(new DateTime(Year, Month, 1, Hour, Minute, Second, DateTimeKind.Utc).AddMonths(1).AddDays(Day));

                    // Inherit time zone info, etc. from the start date
                    dt.MergeWith(StartDate);
                    return dt;
                }
            }

            public void Calculate()
            {
                try
                {
                    // Make sure our day falls in the valid date range
                    if (Recur.IsValidDate(CurrentDateTime) &&
                        // Ensure the DateTime hasn't already been calculated (NOTE: fixes RRULE34 eval)
                        !DateTimes.Contains(CurrentDateTime))
                        DateTimes.Add(CurrentDateTime);
                }
                catch (ArgumentOutOfRangeException ex) { }
            }
        }

        #endregion
    }

    public enum FrequencyType
    {
        None,
        Secondly,
        Minutely,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly
    };

    public enum RecurrenceRestrictionType
    {
        Default, /// Same as RestrictSecondly.
        NoRestriction, /// Does not restrict recurrence evaluation - WARNING: this may cause very slow performance!
        RestrictSecondly, /// Disallows use of the SECONDLY frequency for recurrence evaluation
        RestrictMinutely, /// Disallows use of the MINUTELY and SECONDLY frequencies for recurrence evaluation
        RestrictHourly /// Disallows use of the HOURLY, MINUTELY, and SECONDLY frequencies for recurrence evaluation
    }

    public enum RecurrenceEvaluationModeType
    {
        Default,             /// Same as ThrowException.
        AdjustAutomatically, /// Automatically adjusts the evaluation to the next-best frequency based on the restriction type.
                             /// For example, if the restriction were IgnoreSeconds, and the frequency were SECONDLY, then
                             /// this would cause the frequency to be adjusted to MINUTELY, the next closest thing.
        ThrowException       /// This will throw an exception if a recurrence rule is evaluated that does not meet the minimum
                             /// restrictions.  For example, if the restriction were IgnoreSeconds, and a SECONDLY frequency
                             /// were evaluated, an exception would be thrown.
    }
}
