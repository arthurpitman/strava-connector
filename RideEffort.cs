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
	/// Light weight class used to tie an effort to a ride (and a segment).
	/// </summary>
	public sealed class RideEffort
	{
		#region Properties
		/// <summary>
		/// The id of the Effort.
		/// </summary>
		public long EffortId { get; private set; }

		/// <summary>
		/// Elapsed time in seconds.
		/// </summary>
		public double ElapsedTime { get; private set; }

		/// <summary>
		/// The id of the Segment.
		/// </summary>
		public long SegmentId { get; private set; }
		#endregion


		/// <summary>
		/// Creates a new RideEffort from a dynamic.
		/// </summary>
		/// <param name="stravaRideEffort"></param>
		internal RideEffort(dynamic stravaRideEffort)
		{
			EffortId = stravaRideEffort.id;
			ElapsedTime = (double)stravaRideEffort.elapsed_time;
			SegmentId = stravaRideEffort.segment.id;
		}
	}
}