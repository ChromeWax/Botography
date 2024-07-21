using Codice.Client.BaseCommands;
using Codice.CM.Client.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.BugReporter
{
	public class BugReportConstants
	{
		public const string CLICKUP_TASKS_URL_PRE = "https://api.clickup.com/api/v2/list/";
		public const string CLICKUP_TASKS_URL_SUF = "/task/";
		public const string CLICKUP_TAGS_URL = "https://api.clickup.com/api/v2/space/90100510074/tag/";
		public const string CLICKUP_SPACE_URL_PRE = "https://api.clickup.com/api/v2/space/";
		public const string CLICKUP_FOLDERS_URL_SUF = "/folder/";
		public const string CLICKUP_ATTACHMENT_URL_PRE = "https://api.clickup.com/api/v2/task/";
		public const string CLICKUP_ATTACHMENT_URL_SUF = "/attachment/";
		public const string CLICKUP_LIST_URL_SUF = "/list/";
		public const string API_KEY = "pk_48278367_FNG6GKW8JJDG5PZ222TCAGQIGJHXK2O1";
		public const string BOTOGRAPHY_SPACE_ID = "90100510074";
		public const string BOTOGRAPHY_BACKLOG_LIST_ID = "901002956296";
		public const float MAX_BUTTON_WIDTH = 150;
		public const float MIN_TOGGLE_HEIGHT = 30;
		public const float MAX_STEP_BUTTON_WIDTH = 50;
		public const float MAX_X_BUTTON_WIDTH = 15;
		public const float STEP_LABEL_WIDTH = 30;
		public const float MAX_TEXT_AREA_WIDTH = 400;
		public const float TEXT_AREA_HEIGHT = 70;
		public const int TAGS_PER_ROW = 3;
		public const int MIN_YEAR = 2023;
		public static readonly int MAX_YEAR = DateTime.Now.Year;
		public const float MAX_POPUP_HEIGHT = 500;
		public static readonly List<string> IGNORE_TAGS = new() { "bug" };



		// Tickets Window
		public static readonly Color DEFAULT_STATUS_COLOR = Color.white;
		public const float TITLE_COL_WIDTH = 200;
		public const float STATUS_COL_WIDTH = 100;
		public const float ASSIGNEE_COL_WIDTH = 200;
		public const float DATE_COL_WIDTH = 100;
		public const float URL_COL_WIDTH = 300;
		public const float TICKET_WINDOW_WIDTH = TITLE_COL_WIDTH + STATUS_COL_WIDTH + ASSIGNEE_COL_WIDTH + DATE_COL_WIDTH + URL_COL_WIDTH + 10;
		public const float TICKET_WINDOW_HEIGHT = 600;
		public const float CHAR_PER_WIDTH = 0.175f;

		public const string TODO_DESC = "TO DO - Work has not begun on this ticket. It may still have an assignee who is expected to work on it.";
		public const string INPROGRESS_DESC = "IN-PROGRESS - The listed assignee is actively working on the ticket.";
		public const string READYREVIEW_DESC = "READY REVIEW - The assignee has worked the ticket. It's awaiting testing by a director. Work on these tickets is typically finished with the potential for a few more issues to work out.";
		public const string COMPLETED_DESC = "COMPLETED - This is a finished state. The work for this ticket has been reviewed and tested. However, it has not made it into a build yet.";
		public const string ACCEPTED_DESC = "ACCEPTED - This is a finished state. The work for this ticket has been approved and included in a build.";
		public const string CLOSED_DESC = "CLOSED - This is a finished state. This ticket has been was part of a previous sprint. No more work is planned for it.";
		public const string REJECTED_DESC = "REJECTED - This ticket has been assessed, and it was determined that the issue has been fixed already, the issue is not reproducable, the issue is not a bug, or it otherwise did not need to be worked. Check the ticket itself for the reason it was rejected.";

		public static readonly List<string> STATUS_ORDER = new()
		{
			"TO DO",
			"IN-PROGRESS",
			"READY REVIEW",
			"COMPLETED",
			"ACCEPTED",
			"REJECTED",
			"CLOSED"
		};

		public const float TIME_BETWEEN_LOADING_DOTS = 1;
	}
}
