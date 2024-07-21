using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.IO;

namespace Botography.BugReporter
{
	using static BugReportConstants;
	public class BugReporter
	{
		public static JObject CreateTask(string submitterName, string taskName,
			string version,
			DateTime dayObserved,
			string[] tags,
			string steps,
			string expectedBehavior,
			string actualBehavior,
			List<string> attachments)
		{
			submitterName = submitterName.Replace("\"", "\\\"");
			taskName = taskName.Replace("\"", "\\\"");
			for (int i = 0; i < tags.Length; i++)
			{
				tags[i] = tags[i].Replace("\"", "\\\"");
			}
			steps = steps.Replace("\"", "\\\"");
			expectedBehavior = expectedBehavior.Replace("\"", "\\\"");
			actualBehavior = actualBehavior.Replace("\"", "\\\"");

			StringBuilder sb = new StringBuilder();
			string description = $"__Day Observed:__ {dayObserved.ToString("MM/dd/yyyy")}\\n__Version:__ {version}\\n\\n__Steps To Reproduce:__\\n{steps}\\n\\n__Expected Behavior:__ {expectedBehavior}\\n__Actual Behavior:__ {actualBehavior}\\n\\n__Submitted By:__ {submitterName}";

			sb.Append("[");
			for (int i = 0; i < tags.Length; i++)
			{
				sb.Append($"\"{tags[i]}\"");
				sb.Append(",");
			}
			sb.Append("\"bug\"]");

			string body = $"{{\"name\": \"{taskName}\",\"markdown_description\": \"{description}\",\"tags\": {sb}, \"status\": \"To Do\"}}";
			UnityWebRequest r = UnityWebRequest.Post($"{CLICKUP_TASKS_URL_PRE}{BOTOGRAPHY_BACKLOG_LIST_ID}{CLICKUP_TASKS_URL_SUF}", body, "application/json");

			r.method = "POST";
			r.timeout = 30;
			r.SetRequestHeader("Authorization", API_KEY);

			r.SendWebRequest();

			while (!r.isDone) { }

			if (r.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError($"{r.result}: {r.downloadHandler.text}");
				Debug.Log(body);
				return null;
			}

			JObject res = JObject.Parse(r.downloadHandler.text);

			AddAttachments(res["id"].ToString(), attachments);

			return res;
		}

		public static void AddAttachments(string taskId, List<string> attachments)
		{
			if (attachments.Count == 0)
			{
				return;
			}

			//foreach (string att in attachments)
			//{
			//	byte[] bytes = File.ReadAllBytes(att);
			//	string name =  Path.GetFileName(att);
			//	MultipartFormDataContent postData = new MultipartFormDataContent();
			//	postData.Add(new ByteArrayContent(bytes), name);
				
			//	UnityWebRequest r = UnityWebRequest.Post($"{CLICKUP_ATTACHMENT_URL_PRE}{taskId}{CLICKUP_ATTACHMENT_URL_SUF}", postData.ToString(), "multipart/form-data");
			//	r.timeout = 30;
			//	r.SetRequestHeader("Authorization", API_KEY);
				
			//	r.SendWebRequest();
			//	while (!r.isDone) { }

			//	if (r.result == UnityWebRequest.Result.ProtocolError)
			//	{
			//		Debug.LogError($"{r.result}: {r.downloadHandler.text}");
			//	}
			//}
		}

		public static List<string> GetTags()
		{
			UnityWebRequest r = UnityWebRequest.Get(CLICKUP_TAGS_URL);
			r.method = "GET";
			r.timeout = 30;
			r.SetRequestHeader("Authorization", API_KEY);
			
			r.SendWebRequest();

			while (!r.isDone) { }

			if (r.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError(r.result);
				return null;
			}

			JObject obj = JObject.Parse(r.downloadHandler.text);

			List<string> res = new List<string>();

			for (int i = 0; i < obj["tags"].Count(); i++)
			{
				if (IGNORE_TAGS.Contains(obj["tags"][i]["name"].ToString().ToLower()))
				{
					continue;
				}
				res.Add(obj["tags"][i]["name"].ToString());
			}

			return res;
		}

		public static bool CreateTag(string name)
		{
			name = name.Replace("\"", "\\\"");
			Color randCol = UnityEngine.Random.ColorHSV();
			Debug.Log(randCol.ToHexString());
			string body = $"{{\"tag\":{{\"name\": \"{name}\",\"tag_fg\": \"#{randCol.ToHexString().Substring(0,6)}\",\"tag_bg\": \"#{randCol.ToHexString().Substring(0,6)}\"}}}}";
			UnityWebRequest r = UnityWebRequest.Post(CLICKUP_TAGS_URL, body, "application/json");

			r.method = "POST";
			r.timeout = 30;
			r.SetRequestHeader("Authorization", API_KEY);

			r.SendWebRequest();
			while (!r.isDone) { }

			if (r.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError($"{r.result}: {r.downloadHandler.text}");
				return false;
			}

			return r.result == UnityWebRequest.Result.Success;
		}

		public static void GetBugTickets(Action<Dictionary<string, List<BugTicket>>> callback)
		{
			List<string> listIds = GetAllLists();
			for (int i = 0; i < listIds.Count; i++)
			{
				string id = listIds[i];
				UnityWebRequest r = UnityWebRequest.Get($"{CLICKUP_TASKS_URL_PRE}{id}{CLICKUP_TASKS_URL_SUF}?tags[]=bug&include_closed=true");
				r.timeout = 30;
				r.SetRequestHeader("Authorization", API_KEY);

				UnityWebRequestAsyncOperation op = r.SendWebRequest();

				op.completed += (asyncOp) =>
				{
					UnityWebRequestAsyncOperation operation = (UnityWebRequestAsyncOperation)asyncOp;
					UnityWebRequest r = operation.webRequest;
					if (r.result == UnityWebRequest.Result.ProtocolError)
					{
						Debug.LogError(r.result);
						return;
					}

					JObject obj = JObject.Parse(r.downloadHandler.text);

					Dictionary<string, List<BugTicket>> bugDict = new();

					for (int j = 0; j < obj["tasks"].Count(); j++)
					{
						string assignee = obj["tasks"][j]["assignees"].Count() > 0 ? obj["tasks"][j]["assignees"][0]["username"].ToString() : "";
						BugTicket ticket = new BugTicket(obj["tasks"][j]["name"].ToString(), obj["tasks"][j]["status"]["status"].ToString(), obj["tasks"][j]["status"]["color"].ToString(), obj["tasks"][j]["due_date"].ToString(), assignee, obj["tasks"][j]["url"].ToString());
						if (!bugDict.ContainsKey(ticket.Status))
						{
							bugDict.Add(ticket.Status, new());
						}
						bugDict[ticket.Status].Add(ticket);
					}

					callback(bugDict);
				};
			}
		}



		/// <summary>
		/// Gets all list IDs in the Botography space on ClickUp
		/// </summary>
		/// <returns></returns>
		private static List<string> GetAllLists()
		{
			List<string> listIds = new();

			// Getting lists from folders in the space
			UnityWebRequest r = UnityWebRequest.Get($"{CLICKUP_SPACE_URL_PRE}{BOTOGRAPHY_SPACE_ID}{CLICKUP_FOLDERS_URL_SUF}");
			r.method = "GET";
			r.timeout = 30;
			r.SetRequestHeader("Authorization", API_KEY);

			r.SendWebRequest();

			while (!r.isDone) { }

			if (r.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError(r.result);
				return null;
			}

			JObject obj = JObject.Parse(r.downloadHandler.text);

			for (int i = 0; i < obj["folders"].Count(); i++)
			{
				for (int j = 0; j < obj["folders"][i]["lists"].Count(); j++)
				{
					listIds.Add(obj["folders"][i]["lists"][j]["id"].ToString());
				}
			}

			// Getting lists from the space directly (not in folders)
			r = UnityWebRequest.Get($"{CLICKUP_SPACE_URL_PRE}{BOTOGRAPHY_SPACE_ID}{CLICKUP_LIST_URL_SUF}");
			r.method = "GET";
			r.timeout = 30;
			r.SetRequestHeader("Authorization", API_KEY);

			r.SendWebRequest();

			while (!r.isDone) { }

			if (r.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError(r.result);
				return null;
			}

			obj = JObject.Parse(r.downloadHandler.text);

			for (int i = 0; i < obj["lists"].Count(); i++)
			{
				listIds.Add(obj["lists"][i]["id"].ToString());
			}

			return listIds;
		}
	}
}