using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Botography.BugReporter
{
	using static BugReportConstants;
	public class BugReportWindow : EditorWindow
	{
		[SerializeField] string submitterName = "";
		[SerializeField] string taskName = "";
		[SerializeField] string newTagName = "";
		[SerializeField] string expected = "";
		[SerializeField] string actual = "";
		[SerializeField] int month = 1;
		[SerializeField] int day = 1;
		[SerializeField] int yearIndex = 0;
		string newTaskUrl = "";
		[SerializeField] int daysInMonth = 0;
		[SerializeField] List<string> steps = new();
		Dictionary<string, bool> tagsDict = new();
		[SerializeField] List<string> tagsStrings = new();
		[SerializeField] List<bool> tagsBools = new();
		[SerializeField] List<string> attachments = new();
		bool showTags = false;
		bool submitted = false;
		[SerializeField] int versionIndex = 0;
		Vector2 scrollPos;

		#region Buttons
		string submitBtn = "Submit";
		string refreshBtn = "Refresh";
		string tagsBtn;
		string ticketsBtn = "Show Current Tickets";
		string addTagBtn = "Add New Tag";
		string fileButton = "Attach File";
		#endregion Buttons

		#region Date Lists
		List<GUIContent> months = new List<GUIContent>()
		{
			new GUIContent("January"),
			new GUIContent("February"),
			new GUIContent("March"),
			new GUIContent("April"),
			new GUIContent("May"),
			new GUIContent("June"),
			new GUIContent("July"),
			new GUIContent("August"),
			new GUIContent("September"),
			new GUIContent("October"),
			new GUIContent("November"),
			new GUIContent("December")
		};

		List<GUIContent> years = new();
		[SerializeField] List<GUIContent> days = null;
		#endregion Date Lists

		[MenuItem("Window/Botography/BUGography")]
		public static void ShowWindow()
		{
			EditorWindow w = GetWindow(typeof(BugReportWindow));
			w.titleContent = new GUIContent("BUGography");
		}

		private void OnGUI()
		{
			if (submitted)
			{
				GUILayout.Label("SUCCESS! Your bug report was submitted. Below is the URL for the ticket:");
				EditorGUILayout.TextArea(newTaskUrl);
				GUILayout.Space(30);
				if (GUILayout.Button("Create New Task", GUILayout.MaxWidth(MAX_BUTTON_WIDTH)))
				{
					Refresh();
				}
				return;
			}

			#region Submitter Name
			submitterName = EditorGUILayout.TextField(new GUIContent("Your Name:", "Your name is a string of characters that identify or refer to you."), submitterName);
			#endregion Submitter Name

			#region Refresh Button
			if (GUILayout.Button(refreshBtn, GUILayout.MaxWidth(MAX_BUTTON_WIDTH)))
			{
				Refresh();
			}
			#endregion Refresh Button

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

			#region Version Dropdown
			List<GUIContent> content = new();
			foreach (string v in GameConstants.GAME_VERSIONS)
			{
				content.Add(new GUIContent(v));
			}
			content.Add(new GUIContent("In-Engine"));
			versionIndex = EditorGUILayout.Popup(new GUIContent("Version:", "The version of the application that the bug was discovered in.\nNote: Please try to recreate all of your bugs in a recent build if you can."), versionIndex, content.ToArray());
			#endregion Version Dropdown

			#region Title
			GUILayout.Label(new GUIContent("Title:", "A brief title of the bug you're experiencing."));
			taskName = EditorGUILayout.TextField(taskName);
			#endregion Title

			#region Date Observed


			GUILayout.Label(new GUIContent("Date Observed:", "The date that you observed the bug.\nThis is important because we need to be able to look back and see if the problem has been fixed since the report was submitted."));
			GUILayout.BeginHorizontal();

			GUILayout.Space(40);
			GUILayout.Label("Year:", GUILayout.MaxWidth(35));
			yearIndex = EditorGUILayout.Popup(yearIndex, years.ToArray(), GUILayout.MaxWidth(60));
			GUILayout.Space(15);
			GUILayout.Label("Month:", GUILayout.MaxWidth(45));
			month = EditorGUILayout.Popup(month - 1, months.ToArray(), GUILayout.MaxWidth(100)) + 1;
			int newDaysInMonth = DateTime.DaysInMonth(int.Parse(years[yearIndex].ToString()), month);
			if (day > newDaysInMonth)
			{
				day = 1;
			}
			if (newDaysInMonth != daysInMonth)
			{
				int daysDiff = newDaysInMonth - daysInMonth;
				int mod = -daysDiff / Math.Abs(daysDiff);
				daysInMonth = newDaysInMonth;
				for (int i = daysDiff; i != 0; i += mod)
				{
					if (daysDiff > 0)
					{
						days.Add(new GUIContent((days.Count + 1).ToString()));
					}
					else
					{
						days.RemoveAt(days.Count - 1);
					}
				}
			}
			GUILayout.Space(15);
			GUILayout.Label("Day:", GUILayout.MaxWidth(35));
			day = EditorGUILayout.Popup(day - 1, days.ToArray(), GUILayout.MaxWidth(45)) + 1;

			GUILayout.EndHorizontal();
			#endregion Date Observed

			#region Steps
			GUILayout.Label(new GUIContent("Steps to Reproduce:", "The steps taken from the time the application was started to the moment the bug occurred.\nDetailed steps are always appreciated, but sometimes it's better to summarize steps."));
			for (int i = 0; i < steps.Count; i++)
			{
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("X", GUILayout.MaxWidth(MAX_X_BUTTON_WIDTH)))
				{
					GUILayout.EndHorizontal();
					steps.RemoveAt(i);
					break;
				}
				GUILayout.Label($"{i + 1}.", GUILayout.MaxWidth(STEP_LABEL_WIDTH), GUILayout.MinWidth(STEP_LABEL_WIDTH));
				steps[i] = EditorGUILayout.TextField(steps[i]);
				if (i > 0)
				{
					GUI.SetNextControlName($"Up{i}");
					if (GUILayout.Button("Up", GUILayout.MaxWidth(MAX_STEP_BUTTON_WIDTH)))
					{
						GUI.FocusControl($"Up{i}");
						string temp = steps[i];
						steps[i] = steps[i - 1];
						steps[i - 1] = temp;
					}
				}

				if (i < steps.Count - 1)
				{
					GUI.SetNextControlName($"Down{i}");
					if (GUILayout.Button("Down", GUILayout.MaxWidth(MAX_STEP_BUTTON_WIDTH)))
					{
						GUI.FocusControl($"Down{i}");
						string temp = steps[i];
						steps[i] = steps[i + 1];
						steps[i + 1] = temp;
					}
				}

				GUILayout.EndHorizontal();
			}

			if (GUILayout.Button("Add Step", GUILayout.MaxWidth(MAX_BUTTON_WIDTH)))
			{
				steps.Add("");
			}
			#endregion Steps

			#region Expected
			GUILayout.Label(new GUIContent("Expected Behavior:", "How the application is supposed to behave."));
			GUIStyle style = new GUIStyle(EditorStyles.textField);
			style.wordWrap = true;
			expected = EditorGUILayout.TextField(expected, style, GUILayout.MaxWidth(MAX_TEXT_AREA_WIDTH), GUILayout.Height(TEXT_AREA_HEIGHT));
			#endregion Expected

			#region Actual
			GUILayout.Label(new GUIContent("Actual Behavior:", "How the application actually behaves. Another interpretation of this is the actual bug you're observing."));
			actual = EditorGUILayout.TextField(actual, style, GUILayout.MaxWidth(MAX_TEXT_AREA_WIDTH), GUILayout.Height(TEXT_AREA_HEIGHT));
			#endregion Actual

			#region Tags
			if (showTags)
			{
				tagsBtn = "Hide Tags";
			}
			else
			{
				tagsBtn = "Select Tags";
			}

			if (GUILayout.Button(tagsBtn, GUILayout.MaxWidth(MAX_BUTTON_WIDTH)))
			{
				showTags = !showTags;
			}

			if (showTags)
			{
				EditorGUILayout.BeginVertical();

				Dictionary<string, bool> newTags = new Dictionary<string, bool>();
				int tagCount = 0;
				GUILayout.BeginHorizontal();
				foreach (KeyValuePair<string, bool> t in tagsDict)
				{
					if (tagCount > 0 && tagCount % TAGS_PER_ROW == 0)
					{
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
					}
					newTags.Add(t.Key, GUILayout.Toggle(t.Value, t.Key, GUILayout.MinHeight(MIN_TOGGLE_HEIGHT)));
					tagCount++;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				tagsDict = newTags;
				newTagName = EditorGUILayout.TextField(newTagName, GUILayout.MaxWidth(100));
				if (GUILayout.Button(addTagBtn, GUILayout.MaxWidth(MAX_BUTTON_WIDTH)))
				{
					if (string.IsNullOrWhiteSpace(newTagName))
					{
						Debug.LogWarning("Cannot create tag that is null or whitespace.");
					}
					else
					{
						if (BugReporter.CreateTag(newTagName))
						{
							newTagName = "";
							LoadTags();
						}
					}
				}
				GUILayout.EndHorizontal();

				EditorGUILayout.EndVertical();
			}
			#endregion Tags

			#region Attachments
			if (GUILayout.Button(fileButton, GUILayout.MaxWidth(MAX_BUTTON_WIDTH)))
			{
				string newFile = EditorUtility.OpenFilePanel("BUGography - Attach File", "", "");
				if (!newFile.Equals("") && newFile != null)
				{
					attachments.Add(newFile);
				}
			}

			for (int i = 0; i < attachments.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("X", GUILayout.MaxWidth(MAX_X_BUTTON_WIDTH)))
				{
					GUILayout.EndHorizontal();
					attachments.RemoveAt(i);
					break;
				}
				EditorGUILayout.LabelField(Path.GetFileName(attachments[i]));
				EditorGUILayout.EndHorizontal();
			}
			#endregion Attachments

			#region Submit Button
			if (GUILayout.Button(submitBtn, GUILayout.MaxWidth(MAX_BUTTON_WIDTH)))
			{
				int fails = CheckFields();
				if (fails == 0)
				{
					string version = versionIndex == GameConstants.GAME_VERSIONS.Length ? "In-Engine" : GameConstants.GAME_VERSIONS[versionIndex];
					DateTime dt = new DateTime(int.Parse(years[yearIndex].text), month, day);
					List<string> ts = new();
					foreach (KeyValuePair<string, bool> t in tagsDict)
					{
						if (t.Value)
						{
							ts.Add(t.Key);
						}
					}
					JObject obj = BugReporter.CreateTask(submitterName, taskName, version, dt, ts.ToArray(), GetSteps(), expected, actual, attachments);
					if (obj != null)
					{
						submitted = true;
						newTaskUrl = obj["url"].ToString();
					}
				}
				else
				{
					StringBuilder errMessage = new StringBuilder();
					errMessage.Append("Bug Report Error\nCheck your fields:\n");

					if (string.IsNullOrWhiteSpace(submitterName))
					{
						errMessage.Append("    -Name of submitter not included\n");
						fails--;
					}
					if (string.IsNullOrWhiteSpace(taskName))
					{
						errMessage.Append("    -Title of task not included\n");
						fails--;
					}
					if (string.IsNullOrWhiteSpace(expected))
					{
						errMessage.Append("    -Expected behavior not included\n");
						fails--;
					}
					if (string.IsNullOrWhiteSpace(actual))
					{
						errMessage.Append("    -Actual behavior/bug description not included\n");
						fails--;
					}
					if (steps.Count == 0)
					{
						errMessage.Append("    -Report must include at least one step\n");
						fails--;
					}
					if (fails > 0)
					{
						errMessage.Append("    -Empty steps found\n");
					}

					Debug.LogError(errMessage.ToString());
				}
			}
			#endregion Submit Button

			GUILayout.Space(40);

			#region Tickets
			if (GUILayout.Button(ticketsBtn, GUILayout.MaxWidth(MAX_BUTTON_WIDTH)))
			{
				GetWindow<BugTicketsWindow>().Show();
			}
			#endregion Tickets

			EditorGUILayout.EndScrollView();
		}

		private string GetSteps()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < steps.Count; i++)
			{
				sb.Append($"{i + 1}. {steps[i]}");

				if (i < steps.Count - 1)
				{
					sb.Append("\\n");
				}
			}

			return sb.ToString();
		}

		private void LoadTags()
		{
			//Debug.Log("Loading Tags");
			List<string> tags = BugReporter.GetTags();
			tags.Sort();
			Dictionary<string, bool> newTags = new Dictionary<string, bool>();
			foreach (string t in tags)
			{
				if (tagsDict.ContainsKey(t))
				{
					newTags.Add(t, tagsDict[t]);
				}
				else
				{
					newTags.Add(t, false);
				}
			}
			tagsDict = newTags;
		}

		private void OnEnable()
		{
			string json = EditorPrefs.GetString("BUGography");
			EditorJsonUtility.FromJsonOverwrite(json, this);

			for (int i = 0; i < tagsStrings.Count; i++)
			{
				tagsDict.Add(tagsStrings[i], tagsBools[i]);
			}

			for (int i = MAX_YEAR; i > MIN_YEAR - 1; i--)
			{
				years.Add(new GUIContent(i.ToString()));
			}
			
			LoadTags();
		}

		private void OnDisable()
		{
			tagsStrings = new();
			tagsBools = new();
			foreach (KeyValuePair<string, bool> t in tagsDict)
			{
				tagsStrings.Add(t.Key);
				tagsBools.Add(t.Value);
			}

			string json = EditorJsonUtility.ToJson(this);
			EditorPrefs.SetString("BUGography", json);
		}

		private void Refresh()
		{
			taskName = "";
			newTagName = "";
			expected = "";
			actual = "";
			newTaskUrl = "";
			steps = new();
			tagsDict = new();
			attachments = new();
			versionIndex = 0;
			submitted = false;
			showTags = false;

			LoadTags();
		}

		private int CheckFields()
		{
			bool emptyStep = false;
			foreach (string s in steps)
			{
				if (string.IsNullOrWhiteSpace(s))
				{
					emptyStep = true;
					break;
				}
			}

			return (string.IsNullOrWhiteSpace(submitterName) ? 1 :  0)
				+ (string.IsNullOrWhiteSpace(taskName) ? 1 : 0)
				+ (string.IsNullOrWhiteSpace(expected) ? 1 : 0)
				+ (string.IsNullOrWhiteSpace(actual) ? 1 : 0)
				+ ((steps.Count == 0) ? 1 : 0)
				+ (emptyStep ? 1 : 0);
		}
	}
}
