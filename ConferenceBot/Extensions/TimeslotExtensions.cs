﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceBot.Model;

namespace ConferenceBot.Extensions
{
    public static class TimeslotExtensions
    {
        public static Timeslot[] FindSpeaker(this Timeslot[] timeslots, string speaker)
        {
            foreach (var timeslot in timeslots)
                timeslot.Sessions = timeslot.Sessions.Where(s => s.Presenters.Any(p => p.Name.IndexOf(speaker,
                                                                                           StringComparison
                                                                                               .InvariantCultureIgnoreCase) >= 0)).ToArray();

            return timeslots;
        }

        public static Timeslot[] FindTitle(this Timeslot[] timeslots, string title)
        {
            foreach (var timeslot in timeslots)
                timeslot.Sessions = timeslot.Sessions.Where(s => s.Title.IndexOf(title,
                                                                     StringComparison
                                                                         .InvariantCultureIgnoreCase) >= 0).ToArray();

            return timeslots;
        }

        public static Timeslot[] FindRoom(this Timeslot[] timeslots, string room)
        {
            foreach (var timeslot in timeslots)
                timeslot.Sessions = timeslot.Sessions.Where(s => s.Room.Name.IndexOf(room,
                                                                     StringComparison
                                                                         .InvariantCultureIgnoreCase) >= 0).ToArray();

            return timeslots;
        }

        public static Timeslot[] FindTime(this Timeslot[] timeslots, TimeSpan time, bool isNext)
        {
            var result = new List<Timeslot>();

            var days = timeslots.Select(t => t.Date.Date).Distinct();

            if (isNext)
            {
                var today = TimeZoneInfo
                    .ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"))
                    .Date;
                days = days.Where(d => d.Date == today);
            }

            foreach (var day in days)
            {
                var timeslot = timeslots.Where(t => t.Date.Date == day).FirstOrDefault(t => t.Date.TimeOfDay >= time && t.Sessions.Any());

                if (timeslot != null)
                    result.Add(timeslot);
            }

            return result.ToArray();
        }

        public static Timeslot[] FindDate(this Timeslot[] timeslots, DateTime startDate, DateTime endDate)
        {
            return timeslots.Where(t => t.Date >= startDate && t.Date <= endDate && t.Sessions.Any()).ToArray();
        }

        public static Timeslot[] FindKeynote(this Timeslot[] timeslots)
        {
            return new[] { timeslots.First() };
        }

        public static Timeslot[] FindLocknote(this Timeslot[] timeslots)
        {
            return new[] { timeslots.Last() };
        }
    }
}
