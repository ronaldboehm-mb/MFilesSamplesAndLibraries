﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MFaaP.MFWSClient;
using Newtonsoft.Json;

namespace MFWSSearching
{
	/// <summary>
	/// A example of executing a simple search using the M-Files Web Service.
	/// </summary>
	class Program
	{
		/// <summary>
		/// The term to search for.
		/// </summary>
		private const string queryTerm = "mfws";

		static void Main(string[] args)
		{
			// Execute the search using the library.
			System.Console.WriteLine($"Executing a search using the library.");
			UseLibrary();
			System.Console.WriteLine("Complete.  Press enter to continue.");
			System.Console.ReadLine();

			// Execute the search without using the library.
			System.Console.WriteLine($"Executing a search using the API directly.");
			UseAPIDirectly();
			System.Console.WriteLine("Complete.  Press enter to continue.");
			System.Console.ReadLine();
		}

		/// <summary>
		/// Uses the helper library to execute a search.
		/// </summary>
		static void UseLibrary()
		{

			// Connect to the online knowledgebase.
			// Note that this doesn't require authentication.
			var client = new MFWSClient("http://kb.cloudvault.m-files.com");

			// Execute a quick search for the query term.
			var results = client.QuickSearch(Program.queryTerm);

			// Iterate over the results and output them.
			System.Console.WriteLine($"There were {results.Length} results returned.");
			foreach (var objectVersion in results)
			{
				System.Console.WriteLine($"\t{objectVersion.Title}");
				System.Console.WriteLine($"\t\tType: {objectVersion.ObjVer.Type}, ID: {objectVersion.ObjVer.ID}");
			}


		}

		/// <summary>
		/// Executes a search using the endpoint directly.
		/// </summary>
		static void UseAPIDirectly()
		{
			// Build the url to request (note to encode the query term).
			var url =
				new Uri("http://kb.cloudvault.m-files.com/REST/objects?q=" + System.Net.WebUtility.UrlEncode(Program.queryTerm));

			// Build the request.
			var httpClient = new HttpClient();

			// Start the request.
			var requestTask = httpClient.GetStringAsync(url);
			Task.WaitAll(requestTask);

			// Retrieve the body.
			string responseBody = requestTask.Result;

			// Output the body.
			// System.Console.WriteLine($"Raw content returned: {responseBody}.");

			// Attempt to parse it.  For this we will use the Json.NET library, but you could use others.
			var results = JsonConvert.DeserializeObject<Results<ObjectVersion>>(responseBody);

			// Iterate over the results and output them.
			System.Console.WriteLine($"There were {results.Items.Count} results returned.");
			foreach (var objectVersion in results.Items)
			{
				System.Console.WriteLine($"\t{objectVersion.Title}");
				System.Console.WriteLine($"\t\tType: {objectVersion.ObjVer.Type}, ID: {objectVersion.ObjVer.ID}");
			}

		}
		
		#region Classes for deserialisation (only used with the direct API calls)

		/// <summary>
		/// Results of a query which might leave only a partial set of items.
		/// </summary>
		/// <remarks>Copied from MFaaP.MFWSClient project, only used for deserialisation with the direct API calls.</remarks>
		private class Results<T>
		{
			public Results()
			{
			}

			/// <summary>
			/// Contains results of a query
			/// </summary>
			public List<T> Items { get; set; }

			/// <summary>
			/// True if there were more results which were left out.
			/// </summary>
			public bool MoreResults { get; set; }

		}

		/// <summary>
		/// Based on M-Files API.
		/// </summary>
		/// <remarks>Copied from MFaaP.MFWSClient project, only used for deserialisation with the direct API calls.</remarks>
		private class ObjectVersion
		{

			public ObjectVersion()
			{
			}

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public DateTime AccessedByMeUtc { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public DateTime CheckedOutAtUtc { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public int CheckedOutTo { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public string CheckedOutToUserName { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public int Class { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public DateTime CreatedUtc { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool Deleted { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public string DisplayID { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public List<ObjectFile> Files { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool HasAssignments { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool HasRelationshipsFromThis { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool HasRelationshipsToThis { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool IsStub { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public DateTime LastModifiedUtc { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool ObjectCheckedOut { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool ObjectCheckedOutToThisUser { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public MFObjectVersionFlag ObjectVersionFlags { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public ObjVer ObjVer { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool SingleFile { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool ThisVersionLatestToThisUser { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public string Title { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public bool VisibleAfterOperation { get; set; }

		}



		/// <summary>
		/// Based on M-Files API.
		/// </summary>
		/// <remarks>Copied from MFaaP.MFWSClient project, only used for deserialisation with the direct API calls.</remarks>
		private class ObjectFile
		{

			public ObjectFile()
			{
			}

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public DateTime ChangeTimeUtc { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public string Extension { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public int ID { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public int Version { get; set; }

		}



		/// <summary>
		/// Based on M-Files API.
		/// </summary>
		/// <remarks>Copied from MFaaP.MFWSClient project, only used for deserialisation with the direct API calls.</remarks>
		private class ObjVer
		{

			public ObjVer()
			{
			}

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public int ID { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public int Type { get; set; }

			/// <summary>
			/// Based on M-Files API.
			/// </summary>
			public int Version { get; set; }

		}

		#endregion

	}
}