using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace Botography.BugReporter
{
    using static BugReportConstants;
    public class BugTicketsWindow : EditorWindow
    {
		bool initialized = false;
		bool gotTicks = false;
		Dictionary<string, List<BugTicket>> tickets = new();
		Vector2 scrollPos;
		GUIStyle headerStyle = new GUIStyle(EditorStyles.label);
		Dictionary<string, bool> statusToggles = new();
		string search = "";
		string loading = "Loading";

		float prevTime;
		float now;

		public static void ShowWindow()
		{
			EditorWindow w = GetWindow(typeof(BugTicketsWindow));
			w.titleContent = new GUIContent("BUGography - Current Tickets");
		}

		private void OnGUI()
		{
			if (!initialized)
			{
				prevTime = Time.realtimeSinceStartup;
				now = Time.realtimeSinceStartup;
				try
				{
					headerStyle.fontStyle = FontStyle.BoldAndItalic;
					headerStyle.fontSize += 4;
					statusToggles.Add("TO DO", true);
					statusToggles.Add("IN-PROGRESS", true);
					statusToggles.Add("READY REVIEW", true);
					statusToggles.Add("COMPLETED", true);
					statusToggles.Add("ACCEPTED", true);
					statusToggles.Add("REJECTED", true);
					statusToggles.Add("CLOSED", false);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					Close();
				}
			}
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

			#region Toggles
			EditorGUILayout.BeginHorizontal();

			Dictionary<string, bool> newTogs = new();
			foreach (KeyValuePair<string, bool> statToggle in statusToggles)
			{
				newTogs.Add(statToggle.Key, GUILayout.Toggle(statToggle.Value, statToggle.Key, GUILayout.MinHeight(MIN_TOGGLE_HEIGHT)));
			}
			statusToggles = newTogs;

			EditorGUILayout.EndHorizontal();
			#endregion Toggles

			#region Search
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Search:", GUILayout.MaxWidth(45));
			search = EditorGUILayout.TextField(search, GUILayout.MaxWidth(MAX_TEXT_AREA_WIDTH));
			EditorGUILayout.EndHorizontal();
			#endregion Search

			#region Table Headers
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField(new GUIContent("Title", "The title of the ticket that should be a brief description of the bug."), headerStyle, GUILayout.Width(TITLE_COL_WIDTH));
			DrawVertLine();
			EditorGUILayout.LabelField(new GUIContent("Status", $"The status of the ticket. The following is a glossary of the different statuses:\n{TODO_DESC}\n{INPROGRESS_DESC}\n{READYREVIEW_DESC}\n{COMPLETED_DESC}\n{ACCEPTED_DESC}\n{CLOSED_DESC}\n{REJECTED_DESC}"), headerStyle, GUILayout.Width(STATUS_COL_WIDTH));
			DrawVertLine();
			EditorGUILayout.LabelField(new GUIContent("Assignee", "The programmer assigned to work on the ticket."), headerStyle, GUILayout.Width(ASSIGNEE_COL_WIDTH));
			DrawVertLine();
			EditorGUILayout.LabelField(new GUIContent("ETA", "The estimated date that the ticket will be finished by. These often change over time. If a ticket has a status of TO DO or IN-PROGRESS and the ETA has passed, the date will appear red."), headerStyle, GUILayout.Width(DATE_COL_WIDTH));
			DrawVertLine();
			EditorGUILayout.LabelField(new GUIContent("URL", "The URL for the ticket. Navigate to this in a browser to see the ticket in full, including notes, further descriptions, priority, etc."), headerStyle, GUILayout.Width(URL_COL_WIDTH));
			DrawVertLine();

			EditorGUILayout.EndHorizontal();
			DrawHorizLine();
			#endregion Table Headers

			if (!initialized)
			{
				BugReporter.GetBugTickets((dict) =>
				{
					foreach (KeyValuePair<string, List<BugTicket>> pair in dict)
					{
						if (!tickets.ContainsKey(pair.Key))
						{
							tickets.Add(pair.Key, new());
						}
						tickets[pair.Key].AddRange(pair.Value);
					}
					gotTicks = true;
				});
				initialized = true;
			}

			#region Table Contents
			if (gotTicks)
			{

				foreach (string stat in STATUS_ORDER)
				{
					if (!statusToggles[stat] || !tickets.ContainsKey(stat))
					{
						continue;
					}

					foreach (BugTicket tick in tickets[stat])
					{
						string searchLower = search.ToLower();
						string titleLower = tick.Title.ToLower();
						if (!titleLower.Contains(searchLower))
						{
							continue;
						}

						GUIStyle statusStyle = new GUIStyle();
						GUIStyle dateStyle = new GUIStyle();

						EditorGUILayout.BeginHorizontal();

						if (tick.Title.Length > CHAR_PER_WIDTH * TITLE_COL_WIDTH - 2)
						{
							string t = $"{tick.Title.Substring(0, (int)((CHAR_PER_WIDTH * TITLE_COL_WIDTH) - 5))}...";
							EditorGUILayout.LabelField(new GUIContent(t, tick.Title), GUILayout.Width(TITLE_COL_WIDTH));
						}
						else
						{
							EditorGUILayout.LabelField(tick.Title, GUILayout.Width(TITLE_COL_WIDTH));
						}
						DrawVertLine();

						statusStyle.normal.textColor = tick.Color;
						EditorGUILayout.LabelField(tick.Status.ToUpper(), statusStyle, GUILayout.Width(STATUS_COL_WIDTH));
						DrawVertLine();

						EditorGUILayout.LabelField(tick.Assignee, GUILayout.Width(ASSIGNEE_COL_WIDTH));
						DrawVertLine();

						if (tick.DateString != null && tick.DateString != "")
						{
							DateTime time = tick.Date;
							if (time < DateTime.Now && (tick.Status.ToUpper() == "TO DO" || tick.Status.ToUpper() == "IN-PROGRESS"))
							{
								dateStyle.normal.textColor = Color.red;
							}
							else
							{
								dateStyle.normal.textColor = Color.white;
							}
							EditorGUILayout.LabelField(tick.DateString, dateStyle, GUILayout.Width(DATE_COL_WIDTH));
						}
						else
						{
							dateStyle.normal.textColor = Color.white;
							dateStyle.fontStyle = FontStyle.Italic;
							EditorGUILayout.LabelField("none", dateStyle, GUILayout.Width(DATE_COL_WIDTH));
						}

						DrawVertLine();

						EditorGUILayout.TextArea(tick.Url, GUILayout.Width(URL_COL_WIDTH));
						DrawVertLine();

						EditorGUILayout.EndHorizontal();
						DrawHorizLine();
					}
				}
			}
			else
			{
				EditorGUILayout.LabelField(loading);
				if (now - prevTime >= TIME_BETWEEN_LOADING_DOTS)
				{
					int index = loading.IndexOf(".");
					if (index != -1 && loading.Substring(index).Length >= 3)
					{
						loading = "Loading";
					}
					else
					{
						loading += ".";
					}
					prevTime = Time.realtimeSinceStartup;
				}
			}
			#endregion Table Contents

			EditorGUILayout.EndScrollView();

			now = Time.realtimeSinceStartup;
		}

		private void DrawVertLine()
		{
			Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(5));
			r.width = 1;
			r.y -= 2;
			r.x += 4 / 2;
			r.height += 6;
			EditorGUI.DrawRect(r, Color.grey);
		}

		private void DrawHorizLine()
		{
			EditorGUILayout.BeginHorizontal();

			Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(10));
			r.height = 1;
			r.y += 9 / 2;
			r.x -= 2;
			r.width += 6;
			EditorGUI.DrawRect(r, Color.grey);

			EditorGUILayout.EndHorizontal();
		}
	}
}
