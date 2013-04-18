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

namespace StravaConnector
{
	/// <summary>
	/// Ties an effort to a ride (and a segment).
	/// </summary>
	public sealed class SegmentEffort
	{
		#region Properties
		/// <summary>
		/// The id of the Effort.
		/// </summary>
		public long EffortId { get; private set; }

		/// <summary>
		/// The id of the Ride.
		/// </summary>
		public long RideId { get; private set; }

		/// <summary>
		/// The id of the Segment.
		/// </summary>
		public long SegmentId { get; private set; }

		/// <summary>
		/// Start date and time GMT.
		/// </summary>
		public DateTime StartDate { get; private set; }

		/// <summary>
		/// Start date and time with local time zone
		/// </summary>
		public DateTime StartDateLocal { get; private set; }

		/// <summary>
		/// Offset of the time zone in seconds.
		/// </summary>
		public double TimeZoneOffset { get; private set; }

		/// <summary>
		/// Total elaspsed time in seconds.
		/// </summary>
		public double ElapsedTime { get; private set; }

		/// <summary>
		/// The user.
		/// </summary>
		public User Athlete { get; private set; }
		#endregion


		/// <summary>
		/// Creates a new SegmentEffort from a dynamic with the given segment id.
		/// </summary>
		/// <param name="stravaSegmentEffort"></param>
		/// <param name="segmentId"></param>
		internal SegmentEffort(dynamic stravaSegmentEffort, long segmentId)
		{
			EffortId = stravaSegmentEffort.id;
			RideId = stravaSegmentEffort.activityId;
			StartDate = DateTime.Parse(stravaSegmentEffort.startDate);
			StartDateLocal = DateTime.Parse(stravaSegmentEffort.startDateLocal);
			TimeZoneOffset = (double)stravaSegmentEffort.timeZoneOffset;
			ElapsedTime = (double)stravaSegmentEffort.elapsedTime;
			Athlete = stravaSegmentEffort.athlete == null ? null : new User(stravaSegmentEffort.athlete);
			SegmentId = segmentId;
		}
	}
}