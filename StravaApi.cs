// StravaConnector
//
// Copyright (C) 2012, 2013 Arthur Pitman
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace StravaConnector
{
	/// <summary>
	/// Wrapper around the Strava version 1 API.
	/// </summary>
	public class StravaApi
	{
		private static JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

		/// <summary>
		/// Based URL for the api.
		/// </summary>
		private const string apiBase = "http://strava.com/api/v1/";

		/// <summary>
		/// Number of retries.
		/// </summary>
		private const int retryCount = 10;

		static StravaApi()
		{
			javaScriptSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
		}


		/// <summary>
		/// Calls the Strava API and parses the JSON response.
		/// </summary>
		/// <param name="request">the actual request.</param>
		/// <returns> A dynamic representing the response.</returns>
		public static dynamic Call(string request)
		{
			Exception lastException = null;
			var requestStringBuilder = new StringBuilder(apiBase);
			requestStringBuilder.Append(request);

			// retry request if problems occur
			for (int i = 0; i < retryCount; i++)
			{
				try
				{
					// make request
					var webRequest = WebRequest.Create(requestStringBuilder.ToString());
					var webResponse = (HttpWebResponse)webRequest.GetResponse();

					// try to convert the response to a dynamic
					using (var sr = new StreamReader(webResponse.GetResponseStream()))
					{
						var v = (dynamic)javaScriptSerializer.Deserialize(sr.ReadToEnd().Trim(), typeof(object));
						if (v.error == null)
							return v;
						else
							return null;
					}
				}
				catch (Exception e)
				{
					lastException = e;
				}
			}
			throw new StravaApiException("API call failed.", lastException);
		}
	}
}