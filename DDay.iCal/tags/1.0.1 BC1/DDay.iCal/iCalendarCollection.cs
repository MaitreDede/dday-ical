using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using DDay.iCal.Serialization;

namespace DDay.iCal
{
    /// <summary>
    /// A list of iCalendars.
    /// </summary>
#if DATACONTRACT
    [CollectionDataContract(Name = "iCalendarCollection", Namespace = "http://www.ddaysoftware.com/dday.ical/2009/07/")]
#endif
    [Serializable]
    public class iCalendarCollection :
        List<IICalendar>,
        IICalendarCollection
    {
        #region IGetOccurrences Members

        public void ClearEvaluation()
        {
            foreach (IICalendar iCal in this)
                iCal.ClearEvaluation();
        }

        public IList<Occurrence> GetOccurrences(IDateTime dt)
        {
            List<Occurrence> occurrences = new List<Occurrence>();
            foreach (IICalendar iCal in this)
                occurrences.AddRange(iCal.GetOccurrences(dt));
            occurrences.Sort();
            return occurrences;
        }

        public IList<Occurrence> GetOccurrences(DateTime dt)
        {
            List<Occurrence> occurrences = new List<Occurrence>();
            foreach (IICalendar iCal in this)
                occurrences.AddRange(iCal.GetOccurrences(dt));
            occurrences.Sort();
            return occurrences;
        }

        public IList<Occurrence> GetOccurrences(IDateTime startTime, IDateTime endTime)
        {
            List<Occurrence> occurrences = new List<Occurrence>();
            foreach (IICalendar iCal in this)
                occurrences.AddRange(iCal.GetOccurrences(startTime, endTime));
            occurrences.Sort();
            return occurrences;
        }

        public IList<Occurrence> GetOccurrences(DateTime startTime, DateTime endTime)
        {
            List<Occurrence> occurrences = new List<Occurrence>();
            foreach (IICalendar iCal in this)
                occurrences.AddRange(iCal.GetOccurrences(startTime, endTime));
            occurrences.Sort();
            return occurrences;
        }

        public IList<Occurrence> GetOccurrences<T>(IDateTime dt) where T : IRecurringComponent
        {
            List<Occurrence> occurrences = new List<Occurrence>();
            foreach (IICalendar iCal in this)
                occurrences.AddRange(iCal.GetOccurrences<T>(dt));
            occurrences.Sort();
            return occurrences;
        }

        public IList<Occurrence> GetOccurrences<T>(DateTime dt) where T : IRecurringComponent
        {
            List<Occurrence> occurrences = new List<Occurrence>();
            foreach (IICalendar iCal in this)
                occurrences.AddRange(iCal.GetOccurrences<T>(dt));
            occurrences.Sort();
            return occurrences;
        }

        public IList<Occurrence> GetOccurrences<T>(IDateTime startTime, IDateTime endTime) where T : IRecurringComponent
        {
            List<Occurrence> occurrences = new List<Occurrence>();
            foreach (IICalendar iCal in this)
                occurrences.AddRange(iCal.GetOccurrences<T>(startTime, endTime));
            occurrences.Sort();
            return occurrences;
        }

        public IList<Occurrence> GetOccurrences<T>(DateTime startTime, DateTime endTime) where T : IRecurringComponent
        {
            List<Occurrence> occurrences = new List<Occurrence>();
            foreach (IICalendar iCal in this)
                occurrences.AddRange(iCal.GetOccurrences<T>(startTime, endTime));
            occurrences.Sort();
            return occurrences;
        }

        #endregion
    }
}
