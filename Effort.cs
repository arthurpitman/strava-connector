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

namespace StravaConnector
{
	/// <summary>
	/// Represents a Strava effort.
	/// </summary>
	public sealed class Effort : Movement
	{
		#region Properties
		/// <summary>
		/// The Segment id.
		/// </summary>
		public long SegmentId { get; private set; }

		/// <summary>
		/// The Strava user associated with the effort.
		/// </summary>
		public User Athlete { get; private set; }

		/// <summary>
		/// The Ride id.
		/// </summary>
		public long RideId { get; private set; }

		/// <summary>
		/// Lazy acccess to the effort stream.
		/// </summary>
		public EffortStream Stream
		{
			get
			{
				if (stream == null)
				{
					var stravaEffortStreamResponse = StravaApi.Call(string.Format("stream/efforts/{0}", Id));
					if (stravaEffortStreamResponse == null)
						stream = new EffortStream();
					else
						stream = new EffortStream(stravaEffortStreamResponse);
				}
				return stream;
			}
		}
		private EffortStream stream = null;
		#endregion


		/// <summary>
		/// Creates a new Effort from a dynamic.
		/// </summary>
		/// <param name="stravaEffort"></param>
		internal Effort(dynamic stravaEffort) : base((object)stravaEffort)
		{
			SegmentId = stravaEffort.segment.id;
			Athlete = stravaEffort.athlete == null ? null : new User(stravaEffort.athlete);
			RideId = stravaEffort.ride.id;
		}


		/// <summary>
		/// Retrieves an Effort by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>The Effort.</returns>
		public static Effort ById(long id)
		{
			var stravaEffortResponse = StravaApi.Call(string.Format("efforts/{0}", id));
			if (stravaEffortResponse == null)
				return null;

			return new Effort(stravaEffortResponse.effort);
		}
	}
}