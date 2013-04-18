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
	/// Represents a Strava user (athlete).
	/// </summary>
	public sealed class User
	{
		#region Properties
		/// <summary>
		/// The user id.
		/// </summary>
		public long Id { get; private set; }

		/// <summary>
		/// The user's full name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Strava username (unique?).
		/// </summary>
		public string Username { get; private set; }
		#endregion


		/// <summary>
		/// Creates a new User from a dynamic.
		/// </summary>
		/// <param name="stravaAthlete"></param>
		internal User(dynamic stravaAthlete)
		{
			Id = stravaAthlete.id;
			Name = stravaAthlete.name;
			Username = stravaAthlete.username;
		}
	}
}
