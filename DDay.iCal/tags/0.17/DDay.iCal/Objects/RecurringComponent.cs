using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DDay.iCal.Components;
using DDay.iCal.DataTypes;

namespace DDay.iCal.Objects
{
    /// <summary>
    /// An iCalendar component that recurs.
    /// </summary>
    /// <remarks>
    /// This component automatically handles
    /// <see cref="RRULE"/>s, <see cref="RDATE"/>s, <see cref="EXRULE"/>s, and
    /// <see cref="EXDATE"/>s, as well as the <see cref="DTSTART"/>
    /// for the recurring item (all recurring items must have a DTSTART).
    /// </remarks>
    public class RecurringComponent : UniqueComponent
    {
        #region Public Fields

        [DefaultValueType("DATE-TIME")] public Date_Time DTStart;
        public Date_Time EvalStart;
        public Date_Time EvalEnd;
        public RDate[] ExDate;
        public Recur[] ExRule;
        public RDate[] RDate;
        public Recur[] RRule;
        public Date_Time RecurID;        

        #endregion

        #region Private Fields

        private List<Period> m_Periods;
        private List<Alarm> m_Alarms;

        #endregion

        #region Public Properties

        /// <summary>
        /// A collection of <see cref="Period"/> objects that contain the dates and times
        /// when each item occurs/recurs.
        /// </summary>
        public List<Period> Periods
        {
            get { return m_Periods; }
            set { m_Periods = value; }
        }

        /// <summary>
        /// An alias to the DTStart field (i.e. start date/time).
        /// </summary>
        public Date_Time Start
        {
            get { return DTStart; }
            set { DTStart = value; }
        }

        /// <summary>
        /// A list of <see cref="Alarm"/>s for this recurring component.
        /// </summary>
        public List<Alarm> Alarms
        {
            get { return m_Alarms; }
            set { m_Alarms = value; }
        }

        #endregion

        #region Constructors

        public RecurringComponent(iCalObject parent) : base(parent)
        {
            Periods = new List<Period>();
            Alarms = new List<Alarm>();
        }
        public RecurringComponent(iCalObject parent, string name) : base(parent, name)
        {
            Periods = new List<Period>();
            Alarms = new List<Alarm>();
        }

        #endregion

        #region Public Overridables

        /// <summary>
        /// Evaluates this item to determine the dates and times for which it occurs/recurs.
        /// This method only evaluates items which occur/recur between <paramref name="FromDate"/>
        /// and <paramref name="ToDate"/>; therefore, if you require a list of items which
        /// occur outside of this range, you must specify a <paramref name="FromDate"/> and
        /// <paramref name="ToDate"/> which encapsulate the date(s) of interest.
        /// <note type="caution">
        ///     For events with very complex recurrence rules, this method may be a bottleneck
        ///     during processing time, especially when this method is called for a large number
        ///     of items, in sequence, or for a very large time span.
        /// </note>
        /// </summary>
        /// <param name="FromDate">The beginning date of the range to evaluate.</param>
        /// <param name="ToDate">The end date of the range to evaluate.</param>
        /// <returns>
        ///     An <see cref="ArrayList"/> containing a <see cref="Date_Time"/> object for
        ///     each date/time this item occurs/recurs.
        /// </returns>
        virtual public List<Period> Evaluate(Date_Time FromDate, Date_Time ToDate)
        {
            // Evaluate extra time periods, without re-evaluating ones that were already evaluated
            if ((EvalStart == null && EvalEnd == null) ||
                (ToDate == EvalStart) ||
                (FromDate == EvalEnd))
            {
                EvaluateRRule(FromDate, ToDate);
                EvaluateRDate(FromDate, ToDate);
                EvaluateExRule(FromDate, ToDate);
                EvaluateExDate(FromDate, ToDate);
                if (EvalStart == null || EvalStart > FromDate)
                    EvalStart = FromDate.Copy();
                if (EvalEnd == null || EvalEnd < ToDate)
                    EvalEnd = ToDate.Copy();
            }

            if (EvalStart != null && FromDate < EvalStart)
                Evaluate(FromDate, EvalStart);
            if (EvalEnd != null && ToDate > EvalEnd)
                Evaluate(EvalEnd, ToDate);

            Periods.Sort();

            // Evaluate all Alarms for this component.
            foreach (Alarm alarm in Alarms)
                alarm.Evaluate(this);

            return Periods;
        }

        public List<Alarm.AlarmOccurrence> PollAlarms()
        {
            return PollAlarms(null);
        }

        /// <summary>
        /// Polls <see cref="Alarm"/>s for occurrences within the <see cref="Evaluate"/>d
        /// time frame of this <see cref="RecurringComponent"/>.  For each evaluated
        /// occurrence if this component, each <see cref="Alarm"/> is polled for its
        /// corresponding alarm occurrences.
        /// <para>
        /// <example>
        /// The following is an example of polling alarms for an event.
        /// <code>
        /// iCalendar iCal = iCalendar.LoadFromUri(new Uri("http://somesite.com/calendar.ics"));
        /// Event evt = iCal.Events[0];
        ///
        /// // Evaluate this recurring event for the month of January, 2006.
        /// evt.Evaluate(
        ///     new Date_Time(2006, 1, 1, "US-Eastern", iCal),
        ///     new Date_Time(2006, 1, 31, "US-Eastern", iCal));
        /// 
        /// // Poll all alarms that occurred in January, 2006.
        /// List<Alarm.AlarmOccurrence> alarms = evt.PollAlarms(null);
        /// 
        /// // Here, you would eliminate alarms that the user has already dismissed.
        /// // This information should be stored somewhere outside of the .ics file.
        /// // You can use the component's UID, and the AlarmOccurence date/time 
        /// // as the primary key for each alarm occurrence.
        /// 
        /// foreach(Alarm.AlarmOccurrence alarm in alarms)        
        ///     MessageBox.Show(alarm.Component.Summary + "\n" + alarm.Alarm.DateTime);        
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="Start">The earliest allowable alarm occurrence to poll, or <c>null</c>.</param>
        /// <returns>A List of <see cref="Alarm.AlarmOccurrence"/> objects, one for each occurrence of the <see cref="Alarm"/>.</returns>
        virtual public List<Alarm.AlarmOccurrence> PollAlarms(Date_Time Start)
        {
            List<Alarm.AlarmOccurrence> Occurrences = new List<Alarm.AlarmOccurrence>();
            foreach (Alarm alarm in Alarms)            
                Occurrences.AddRange(alarm.Poll(Start));
            return Occurrences;
        }

        #endregion

        #region Protected Overridables

        /// <summary>
        /// Evaulates the RRule component, and adds each specified Period
        /// to the <see cref="Periods"/> collection.
        /// </summary>
        /// <param name="FromDate">The beginning date of the range to evaluate.</param>
        /// <param name="ToDate">The end date of the range to evaluate.</param>
        virtual protected void EvaluateRRule(Date_Time FromDate, Date_Time ToDate)
        {
            // Handle RRULEs
            if (RRule != null)
            {
                foreach (Recur rrule in RRule)
                {
                    List<Date_Time> DateTimes = rrule.Evaluate(DTStart, FromDate, ToDate);
                    foreach (Date_Time dt in DateTimes)
                    {
                        Period p = new Period(dt);

                        if (!Periods.Contains(p))
                            this.Periods.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// Evalates the RDate component, and adds each specified DateTime or
        /// Period to the <see cref="Periods"/> collection.
        /// </summary>
        /// <param name="FromDate">The beginning date of the range to evaluate.</param>
        /// <param name="ToDate">The end date of the range to evaluate.</param>
        virtual protected void EvaluateRDate(Date_Time FromDate, Date_Time ToDate)
        {
            // Handle RDATEs
            if (RDate != null)
            {
                foreach (RDate rdate in RDate)
                {
                    ArrayList Items = rdate.Evaluate(DTStart, FromDate, ToDate);
                    foreach (object obj in Items)
                    {
                        Period p = null;
                        if (obj is Period)
                            p = (Period)obj;
                        else if (obj is Date_Time)                        
                            p = new Period((Date_Time)obj);                            

                        if (p != null && !Periods.Contains(p))
                            Periods.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// Evaulates the ExRule component, and excludes each specified DateTime
        /// from the <see cref="Periods"/> collection.
        /// </summary>
        /// <param name="FromDate">The beginning date of the range to evaluate.</param>
        /// <param name="ToDate">The end date of the range to evaluate.</param>
        virtual protected void EvaluateExRule(Date_Time FromDate, Date_Time ToDate)
        {
            // Handle EXRULEs
            if (ExRule != null)
            {
                foreach (Recur exrule in ExRule)
                {
                    List<Date_Time> DateTimes = exrule.Evaluate(DTStart, FromDate, ToDate);
                    foreach (Date_Time dt in DateTimes)
                    {
                        Period p = new Period(dt);
                        if (this.Periods.Contains(p))
                            this.Periods.Remove(p);
                    }
                }
            }
        }

        /// <summary>
        /// Evalates the ExDate component, and excludes each specified DateTime or
        /// Period from the <see cref="Periods"/> collection.
        /// </summary>
        /// <param name="FromDate">The beginning date of the range to evaluate.</param>
        /// <param name="ToDate">The end date of the range to evaluate.</param>
        virtual protected void EvaluateExDate(Date_Time FromDate, Date_Time ToDate)
        {
            // Handle EXDATEs
            if (ExDate != null)
            {
                foreach (RDate exdate in ExDate)
                {
                    ArrayList Items = exdate.Evaluate(DTStart, FromDate, ToDate);
                    foreach (object obj in Items)
                    {
                        Period p = null;
                        if (obj is Period)
                            p = (Period)obj;
                        else if (obj is Date_Time)
                            p = new Period((Date_Time)obj);

                        // If no time was provided for the ExDate, then it excludes the entire day
                        if (!p.StartTime.HasTime || (p.EndTime != null && !p.EndTime.HasTime))
                            p.MatchesDateOnly = true;

                        if (p != null)
                        {
                            while (Periods.Contains(p))
                                Periods.Remove(p);
                        }
                    }
                }
            }
        }

        #endregion

        #region Overrides

        public override void AddChild(iCalObject child)
        {
            if (child is Alarm)
                Alarms.Add((Alarm)child);
            base.AddChild(child);
        }

        #endregion
    }
}
