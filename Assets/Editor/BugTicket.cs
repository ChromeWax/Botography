using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using UnityEngine;

namespace Botography.BugReporter
{
    using static BugReportConstants;
    public class BugTicket
    {
        public string Title;
        public string Status;
        public string Url;
        public Color Color { get; private set; }
        public string DateString;
        public string Assignee;
        public long DateMil;
        public DateTime Date;

		public BugTicket(string title, string status, string colorHex, string date, string assignee, string url)
        {
            Title = title;
            Status = status.ToUpper();
            Color c;
            ColorUtility.TryParseHtmlString(colorHex, out c);
            Color = c;
            if (date == null || date == "")
            {
                DateString = "";
            }
            else
            {
				try
				{
					DateMil = long.Parse(date);
                    Date = DateTimeOffset.FromUnixTimeMilliseconds(DateMil).Date;
					DateString = Date.ToShortDateString();
				}
				catch (FormatException)
				{
					DateString = date;
					Date = DateTime.Parse(date);
					DateMil = Date.Millisecond;
				}
			}
            Assignee = assignee;
            Url = url;
        }
    }
}