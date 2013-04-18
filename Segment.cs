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
using System.Text;
using System.Web;

namespace StravaConnector
{
	/// <summary>
	/// Represents a Strava segment.
	/// </summary>
	public sealed class Segment
	{
		/// <summary>
		/// Represents climb category type.
		/// <para>
		/// See https://strava.zendesk.com/entries/20945952-what-are-segments for details.
		/// </para>
		/// </summary>
		public enum ClimbCategoryType
		{
			/// <summary>
			/// No climb category.
			/// </summary>
			NoCategory,

			/// <summary>
			/// The hardest category, beyond categorization.
			/// </summary>
			CategoryHc,

			/// <summary>
			/// Second hardest category.
			/// </summary>
			Category1,

			/// <summary>
			/// Third hardest category.
			/// </summary>
			Category2,

			/// <summary>
			/// Fourth hardest category.
			/// </summary>
			Category3,

			/// <summary>
			/// Easiest category.
			/// </summary>
			Category4
		}

		/// <summary>
		/// Maximum number of search results that can be retrieved in one request.
		/// </summary>
		private const int ResultCount = 50;

		#region Properties
		/// <summary>
		/// The id of the Segment.
		/// </summary>
		public long Id { get; private set;}

		/// <summary>
		/// The name of the Segment.
		/// </summary>
		public string Name { get; private set;}

		/// <summary>
		/// Length of the Segment, in meters.
		/// </summary>
		public double Distance { get; private set;}

		/// <summary>
		/// Elevation gain of the Segment, in meters.
		/// </summary>
		public double ElevationGain { get; private set;}

		/// <summary>
		/// Highest point of the Segment, in meters.
		/// </summary>
		public double ElevationHigh { get; private set;}

		/// <summary>
		/// Lowest point of the Segment, in meters.
		/// </summary>
		public double ElevationLow { get; private set;}

		/// <summary>
		/// Average gradient of the Segment.
		/// TODO: determine unit!
		/// </summary>
		public double AverageGradient { get; private set;}

		/// <summary>
		/// The climb category of the Segment.
		/// </summary>
		public ClimbCategoryType ClimbCategory { get; private set; }
		#endregion


		/// <summary>
		/// Creates a new Segment from a dynamic.
		/// </summary>
		/// <param name="stravaSegment"></param>
		internal Segment(dynamic stravaSegment)
		{
			Id = stravaSegment.id;
			Name = stravaSegment.name;
			Distance = (double)stravaSegment.distance;
			ElevationGain = (double)stravaSegment.elevationGain;
			ElevationHigh = (double)stravaSegment.elevationHigh;
			ElevationLow = (double)stravaSegment.elevationLow;
			AverageGradient = (double)stravaSegment.averageGrade;
			ClimbCategory = ToClimbCategory(stravaSegment.climbCategory);
		}


		/// <summary>
		/// Retrieves a Segment by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>The Segment.</returns>
		public static Segment ById(long id)
		{
			var stravaSegmentResponse = StravaApi.Call(string.Format("segments/{0}", id));
			if (stravaSegmentResponse == null)
				return null;

			return new Segment(stravaSegmentResponse.segment);
		}


		/// <summary>
		/// Retrieves SegmentEfforts based on search criteria.
		/// </summary>
		/// <param name="offset">Offset in search results.</param>
		/// <param name="count">Number of results to retrieve.</param>
		/// <param name="best">Request best efforts per athlete sorted by elapsed time ascending -- may not work as expected due to Strava bugs.</param>
		/// <param name="athleteId">Athlete id restriction, null for no restriction.</param>
		/// <param name="athleteName">Athlete name restriction, null for no restriction.</param>
		/// <param name="startDate">Start date restriction, null for no restriction.</param>
		/// <param name="endDate">End date restriction, null for no restriction.</param>
		/// <param name="clubId">Club id restriction, null for no restriction.</param>
		/// <param name="startId">Request efforts with an Id greater than or equal to the startId.</param>
		/// <returns>A list of SegmentEffort objects.</returns>
		public List<SegmentEffort> GetEfforts(long offset, long count, bool best = false, long? athleteId = null, string athleteName = null, DateTime? startDate = null, DateTime? endDate = null, long? clubId = null, long? startId = null)
		{
			// build the query
			var queryBuilder = new StringBuilder();
			if (best)
				queryBuilder.Append("best=true&");
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

			var results = new List<SegmentEffort>();
			var tempOffset = offset;

			while (count > 0)
			{
				var stravaSegmentEffortSearchResponse = StravaApi.Call(string.Format("segments/{0}/efforts?{1}offset={2}", Id, restriction, tempOffset));
				if (stravaSegmentEffortSearchResponse == null)
					return results;

				foreach (var stravaSegmentEffort in stravaSegmentEffortSearchResponse.efforts)
				{
					var segmentEffort = new SegmentEffort(stravaSegmentEffort, Id);
					results.Add(segmentEffort);
					count--;
					if (count == 0)
						break;
				}

				if (stravaSegmentEffortSearchResponse.efforts.Count < ResultCount)
					break;

				tempOffset += ResultCount;
			}
			return results;
		}


		/// <summary>
		/// Maps a string to a ClimbCategoryType.
		/// </summary>
		/// <param name="climbCategory"></param>
		/// <returns>A ClimbCategoryType.</returns>
		private static ClimbCategoryType ToClimbCategory(string climbCategory)
		{
			switch (climbCategory)
			{
				case "HC":
					return ClimbCategoryType.CategoryHc;
				case "1":
					return ClimbCategoryType.Category1;
				case "2":
					return ClimbCategoryType.Category2;
				case "3":
					return ClimbCategoryType.Category3;
				case "4":
					return ClimbCategoryType.Category4;
				default: // including "NC"
					return ClimbCategoryType.NoCategory;
			}
		}
	}
}