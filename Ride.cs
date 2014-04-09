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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace StravaConnector
{
	/// <summary>
	/// Represents a Strava ride.
	/// </summary>
	public sealed class Ride : Movement
	{
		/// <summary>
		/// Maximum number of search results that can be retrieved in one request.
		/// </summary>
		private const int ResultCount = 50;


		#region Properties
		/// <summary>
		/// Location of the Ride.
		/// </summary>
		public string Location { get; private set; }

		/// <summary>
		/// User defined name for the ride.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Bike used for the Ride.
		/// </summary>
		public BikeModel Bike { get; private set; }

		/// <summary>
		/// Description of the Ride.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// If <c>true</c>, the Ride was a commute.
		/// </summary>
		public bool Commute { get; private set; }

		/// <summary>
		/// If <c>true</c>, the Ride was training.
		/// </summary>
		public bool Trainer { get; private set; }

		/// <summary>
		/// The user.
		/// </summary>
		public User Athlete { get; private set; }

		/// <summary>
		/// Lazy access to the Ride's efforts.
		/// </summary>
		public RideEffort[] Efforts
		{
			get
			{
				if (efforts == null)
					GetEfforts();
				return efforts;
			}
		}
		private RideEffort[] efforts = null;
		#endregion


		/// <summary>
		/// Creates a new Ride from a dynamic.
		/// </summary>
		/// <param name="stravaRide"></param>
		internal Ride(dynamic stravaRide) : base((object)stravaRide)
		{
			Location = stravaRide.location;
			Name = stravaRide.name;
			Bike =  stravaRide.bike == null ? null : new BikeModel(stravaRide.bike);
			Description = stravaRide.description;
			Commute = stravaRide.commute == null ? false : stravaRide.commute;
			Trainer = stravaRide.trainer == null ? false : stravaRide.trainer;
			Athlete = stravaRide.athlete == null ? null : new User(stravaRide.athlete);
		}


		/// <summary>
		/// Retrieves a Ride by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>The Ride.</returns>
		public static Ride ById(long id)
		{
			var stravaRideResponse = StravaApi.Call(string.Format("rides/{0}", id));
			if (stravaRideResponse == null)
				return null;

			return new Ride(stravaRideResponse.ride);
		}


		/// <summary>
		/// Searches for Rides based on criteria.
		/// </summary>
		/// <param name="offset">Offset in search results.</param>
		/// <param name="count">Number of results to retrieve.</param>
		/// <param name="athleteId">Athlete id restriction, null for no restriction.</param>
		/// <param name="athleteName">Athlete name restriction, null for no restriction.</param>
		/// <param name="startDate">Start date restriction, null for no restriction.</param>
		/// <param name="endDate">End date restriction, null for no restriction.</param>
		/// <param name="clubId">Club id restriction, null for no restriction.</param>
		/// <param name="startId">Only retrieve rides with an id greater or equal to the start id, null for no restriction.</param>
		/// <returns>A list of RideSearchResult objects.</returns>
		public static List<RideSearchResult> Search(long offset, long count, long? athleteId = null, string athleteName = null, DateTime? startDate = null, DateTime? endDate = null, long? clubId = null, long? startId = null)
		{
			// build the query
			var queryBuilder = new StringBuilder();
			if (athleteId.HasValue)
				queryBuilder.AppendFormat("athleteId={0}&", athleteId.Value);
			if (athleteName != null)
				queryBuilder.AppendFormat("athleteName={0}&", HttpUtility.UrlEncode(athleteName));
			if (startDate.HasValue)
				queryBuilder.AppendFormat("startDate={0}&", startDate.Value.ToString("yyyy'-'MM'-'dd"));
			if (endDate.HasValue)
				queryBuilder.AppendFormat("endDate={0}&", endDate.Value.ToString("yyyy'-'MM'-'dd"));
			if (clubId.HasValue)
				queryBuilder.AppendFormat("clubId={0}&", clubId.Value);
			if (startId.HasValue)
				queryBuilder.AppendFormat("startId={0}&", startId.Value);

			var restriction = queryBuilder.ToString();

			var results = new List<RideSearchResult>();
			var tempOffset = offset;

			// keep searching until enough results have been retrieved
			while (count > 0)
			{

				var stravaRideSearchResponse = StravaApi.Call(string.Format("rides?{0}offset={1}", restriction, tempOffset));
				if (stravaRideSearchResponse == null)
					return results;

				foreach (var stravaRideSearchResult in stravaRideSearchResponse.rides)
				{
					var segmentEffort = new RideSearchResult(stravaRideSearchResult);
					results.Add(segmentEffort);
					count--;
					if (count == 0)
						break;
				}

				if (stravaRideSearchResponse.rides.Count < ResultCount)
					break;

				tempOffset += ResultCount;
			}
			return results;
		}


		/// <summary>
		/// Populate the Ride's efforts.
		/// </summary>
		private void GetEfforts()
		{
			var stravaRideEffortsResponse = StravaApi.Call(string.Format("rides/{0}/efforts", Id));
			if (stravaRideEffortsResponse == null)
				return;

			if ((stravaRideEffortsResponse.efforts != null) && (stravaRideEffortsResponse.efforts.Count > 0))
			{
				efforts = (from x in (IEnumerable<dynamic>)stravaRideEffortsResponse.efforts
						   select new RideEffort(x)).ToArray();
			}
		}

	}
}