using System;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using DDay.iCal.Objects;
using DDay.iCal.DataTypes;
using DDay.iCal.Serialization;

namespace DDay.iCal.Components
{
    /// <summary>
    /// A class that represents an RFC 2445 VJOURNAL component.
    /// </summary>
    [DebuggerDisplay("{Summary}: {(Description.ToString().Length < 32) ? Description.ToString() : Description.ToString().Substring(0, 32)}")]
    public class Journal : RecurringComponent
    {
        #region Public Fields
                             
        [SerializedAttribute]
        public JournalStatus Status;        

        #endregion

        #region Static Public Methods

        static public Journal Create(iCalendar iCal)
        {
            Journal j = (Journal)iCal.Create(iCal, "VJOURNAL");
            j.UID = UniqueComponent.NewUID();
            j.Created = DateTime.Now;
            j.DTStamp = DateTime.Now;

            return j;
        }

        #endregion

        #region Constructors

        public Journal(iCalObject parent) : base(parent)
        {
            this.Name = "VJOURNAL";
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a typed copy of the Journal object.
        /// </summary>
        /// <returns>A typed copy of the Journal object.</returns>
        public Journal Copy()
        {
            return (Journal)base.Copy();
        }

        public override System.Collections.Generic.List<Period> Evaluate(Date_Time FromDate, Date_Time ToDate)
        {
            if (Start != null)
            {
                Period p = new Period(Start);
                if (!Periods.Contains(p))
                    Periods.Add(p);

                return base.Evaluate(FromDate, ToDate);
            }
            return new System.Collections.Generic.List<Period>();
        }

        #endregion
    }
}
