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
	/// Common base class for Strava rides and efforts.
	/// </summary>
	public abstract class Movement
	{
		#region Properties
		/// <summary>
		/// The id of the Movement.
		/// </summary>
		public long Id { get; protected set; }

		/// <summary>
		/// Start date and time GMT.
		/// </summary>
		public DateTime StartDate { get; protected set; }

		/// <summary>
		/// Start date and time with local time zone
		/// </summary>
		public DateTime StartDateLocal { get; protected set; }

		/// <summary>
		/// Offset of time zone in seconds.
		/// </summary>
		public double TimeZoneOffset { get; protected set; }

		/// <summary>
		/// Total elaspsed time in seconds.
		/// </summary>
		public double ElapsedTime { get; protected set; }

		/// <summary>
		/// Total moving time in seconds.
		/// </summary>
		public double MovingTime { get; protected set; }

		/// <summary>
		/// Distance in meters.
		/// </summary>
		public double Distance { get; protected set; }

		/// <summary>
		/// Average speed in kilometers per hour.
		/// </summary>
		public double AverageSpeed { get; protected set; }

		/// <summary>
		/// Average watts.
		/// </summary>
		public double AverageWatts { get; protected set; }

		/// <summary>
		/// Maximum speed in kilometers per hour.
		/// </summary>
		public double MaximumSpeed { get; protected set; }

		/// <summary>
		/// Difference between end and start elevation in meters.
		/// </summary>
		public double ElevationGain { get; protected set; }
		#endregion


		/// <summary>
		/// Creates a new Movement from a dynamic.
		/// </summary>
		/// <param name="stravaMovement"></param>
		internal Movement(dynamic stravaMovement)
		{
			Id = stravaMovement.id;
			StartDate = DateTime.Parse(stravaMovement.startDate);
			StartDateLocal = DateTime.Parse(stravaMovement.startDateLocal);
			TimeZoneOffset = (double)stravaMovement.timeZoneOffset;
			ElapsedTime = (double)stravaMovement.elapsedTime;
			Distance = (double)stravaMovement.distance;
			ElevationGain = (double)stravaMovement.elevationGain;
			MovingTime = stravaMovement.movingTime == null ? ElapsedTime : (double)stravaMovement.movingTime;
			AverageSpeed = stravaMovement.averageSpeed == null ? -1 : (double)stravaMovement.averageSpeed / 1000;
			AverageWatts = stravaMovement.averageWatts == null ? -1 : (double)stravaMovement.averageWatts;
			MaximumSpeed = stravaMovement.maximumSpeed == null ? -1 : (double)stravaMovement.maximumSpeed / 1000;
		}
	}
}