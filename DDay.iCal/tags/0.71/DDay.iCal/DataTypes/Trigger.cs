using System;
using System.Collections.Generic;
using System.Text;
using DDay.iCal.Components;

namespace DDay.iCal.DataTypes
{
    /// <summary>
    /// A class that is used to specify exactly when an <see cref="Alarm"/> component will trigger.
    /// Usually this date/time is relative to the component to which the Alarm is associated.
    /// </summary>    
    public class Trigger : iCalDataType
    {
        public enum TriggerRelation
        {
            Start,
            End
        }

        #region Private Fields

        private iCalDateTime m_DateTime;
        private Duration m_Duration;
        private TriggerRelation m_Related = TriggerRelation.Start;
                
        #endregion

        #region Public Properties

        public iCalDateTime DateTime
        {
            get { return m_DateTime; }
            set
            {
                m_DateTime = value;
                if (m_DateTime != null)
                {
                    // NOTE: this, along with the "Duration" setter, fixes the bug tested in
                    // TODO11(), as well as this thread: https://sourceforge.net/forum/forum.php?thread_id=1926742&forum_id=656447

                    // DateTime and Duration are mutually exclusive
                    Duration = null;

                    // Do not allow timeless date/time values
                    m_DateTime.HasTime = true;
                    AddParameter("VALUE", "DATE-TIME");
                }
                else Parameters.Remove("VALUE");
            }
        }

        public Duration Duration
        {
            get { return m_Duration; }
            set
            {
                m_Duration = value;
                if (m_Duration != null)
                {
                    // NOTE: see above.

                    // DateTime and Duration are mutually exclusive
                    DateTime = null;
                }
            }
        }

        public TriggerRelation Related
        {
            get
            {       
                if (Parameters.ContainsKey("RELATED"))
                {
                    Parameter p = (Parameter)Parameters["RELATED"];
                    if (p.Values.Count > 0)
                        m_Related = (TriggerRelation)Enum.Parse(typeof(TriggerRelation), p.Values[0].ToString(), true);
                }                
                return m_Related;
            }
            set
            {
                m_Related = value;
            }
        }

        public bool IsRelative
        {
            get { return m_Duration != null; }
        }

        #endregion

        #region Constructors

        public Trigger() { }
        public Trigger(TimeSpan ts)
        {
            Duration = ts;
        }
        public Trigger(string value)
            : this()
        {
            CopyFrom(Parse(value));
        }

        #endregion

        #region Overrides

        public override void CopyFrom(object obj)
        {
            base.CopyFrom(obj);
            if (obj is Trigger)
            {
                Trigger t = (Trigger)obj;
                DateTime = t.DateTime;
                Duration = t.Duration;
            }
            base.CopyFrom(obj);
        }

        public override bool TryParse(string value, ref object obj)
        {
            Trigger t = (Trigger)obj;
            
            if (ValueType() == typeof(iCalDateTime))
            {
                t.DateTime = new iCalDateTime();
                object dt = t.DateTime;
                return t.DateTime.TryParse(value, ref dt);
            }
            else
            {
                t.Duration = new Duration();
                object d = t.Duration;
                return t.Duration.TryParse(value, ref d);
            }
        }

        #endregion
    }
}
