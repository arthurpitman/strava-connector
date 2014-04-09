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
	/// Light weight class for a ride search results.
	/// </summary>
	public sealed class RideSearchResult
	{
		#region Properties
		/// <summary>
		/// The id of the Ride.
		/// </summary>
		public long Id { get; private set; }

		/// <summary>
		/// The name of the Ride.
		/// </summary>
		public string Name { get; private set; }
		#endregion


		/// <summary>
		/// Creates a new RideSearchResult from a dynamic.
		/// </summary>
		/// <param name="rideSearchResult"></param>
		internal RideSearchResult(dynamic rideSearchResult)
		{
			Id = rideSearchResult.id;
			Name = rideSearchResult.name;
		}
	}
}