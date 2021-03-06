using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using DDay.iCal.Objects;
using DDay.iCal.DataTypes;
using DDay.iCal.Serialization;

namespace DDay.iCal.Components
{
    /// <summary>
    /// A class that represents an RFC 2445 VTODO component.
    /// </summary> 
    [DebuggerDisplay("{Summary} - {Status}")]
    public class Todo : RecurringComponent
    {
        #region Public Fields
       
        [Serialized]
        public Binary[] Attach;
        [Serialized]
        public Cal_Address[] Attendee;
        [Serialized]
        public TextCollection[] Categories;
        [Serialized]
        public Text Class;
        [Serialized]
        public Text[] Comment;
        [Serialized, DefaultValueType("DATE-TIME")]
        public Date_Time Completed;
        [Serialized]
        public Text[] Contact;
        [Serialized, DefaultValueType("DATE-TIME")]
        public Date_Time Created;
        [Serialized]
        public Text Description;        
        [Serialized, DefaultValueType("DATE-TIME")]
        public Date_Time Due;
        [Serialized, DefaultValue("P")]
        public Duration Duration;
        [Serialized]
        public Geo Geo;
        [Serialized]
        public Text Location;
        [Serialized]
        public Cal_Address Organizer;
        [Serialized]
        public Integer PercentComplete;
        [Serialized]
        public Integer Priority;
        [Serialized]
        public Text[] RelatedTo;
        [Serialized]
        public RequestStatus[] RequestStatus;
        [Serialized]
        public TextCollection[] Resources;
        [Serialized, DefaultValue("NEEDS_ACTION\r\n")]
        public TodoStatus Status;
        [Serialized]
        public Text Summary;
        [Serialized]
        public Uri Url;

        #endregion

        #region Constructors

        public Todo(iCalObject parent)
            : base(parent)
        {
            this.Name = "VTODO";
        }

        #endregion

        #region Static Public Methods

        static public Todo Create(iCalendar iCal)
        {
            Todo t = (Todo)iCal.Create(iCal, "VTODO");
            t.UID = UniqueComponent.NewUID();

            return t;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Use this method to determine if a todo item has been completed.
        /// This takes into account recurrence items and the previous date
        /// of completion, if any.
        /// </summary>
        /// <param name="DateTime">The date and time to test.</param>
        /// <returns>True if the todo item has been completed</returns>
        public bool IsCompleted(Date_Time currDt)
        {
            if (Status == TodoStatus.COMPLETED)
            {
                if (Completed == null)
                    return true;

                foreach (Period p in Periods)
                {
                    if (p.StartTime > Completed && // The item has recurred after it was completed
                        currDt >= p.StartTime)     // and the current date is after or on the recurrence date.
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns 'True' if the todo item is Active as of <paramref name="currDt"/>.
        /// An item is Active if it requires action of some sort.
        /// </summary>
        /// <param name="currDt">The date and time to test.</param>
        /// <returns>True if the item is Active as of <paramref name="currDt"/>, False otherwise.</returns>
        public bool IsActive(Date_Time currDt)
        {
            if (DTStart == null)
                return !IsCompleted(currDt) && !IsCancelled();
            else if (currDt >= DTStart)
                return !IsCompleted(currDt) && !IsCancelled();
            else return false;
        }

        /// <summary>
        /// Returns True if the todo item was cancelled.
        /// </summary>
        /// <returns>True if the todo was cancelled, False otherwise.</returns>
        public bool IsCancelled()
        {
            return Status == TodoStatus.CANCELLED;
        }

        #endregion

        #region Overrides

        public override List<Period> Evaluate(Date_Time FromDate, Date_Time ToDate)
        {
            // Add the event itself, before recurrence rules are evaluated
            if (DTStart != null)
                Periods.Add(new Period(DTStart));

            return base.Evaluate(FromDate, ToDate);
        }

        /// <summary>
        /// Automatically derives property values based on others it
        /// contains, to provide a more "complete" object.
        /// </summary>
        /// <param name="e"></param>        
        public override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Automatically determine Duration from Due, or Due from Duration
            if (DTStart != null)
            {
                if (Due != null && Duration == null)
                    Duration = new Duration(Due - DTStart);
                else if (Due == null && Duration != null)
                    Due = DTStart + Duration;                
            }
        }

        #endregion
    }
}
