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
	/// Represents a Strava effort stream.
	///
	/// <para>
	/// Note, some properties may be null if information is missing.
	/// </para>
	/// </summary>
	public sealed class EffortStream
	{
		#region Properties
		/// <summary>
		/// The altitude component of the EffortStream, maybe fixed by Strava?
		/// </summary>
		public double[] Altitude { get; private set; }

		/// <summary>
		/// The original altitude component of the EffortStream, presumably as recorded by the device.
		/// </summary>
		public double[] OriginalAltitude { get; private set; }

		/// <summary>
		/// The distance component of the EffortStream.
		/// </summary>
		public double[] Distance { get; private set; }

		/// <summary>
		/// The smoothed gradient component of the EffortStream.
		/// </summary>
		public double[] SmoothedGradient { get; private set; }

		/// <summary>
		/// The latitude component of the EffortStream.
		/// </summary>
		public double[] Latitude { get; private set; }

		/// <summary>
		/// The longitude component of the EffortStream.
		/// </summary>
		public double[] Longitude { get; private set; }

		/// <summary>
		/// The moving component of the EffortStream. <c>true</c> indicates movement.
		/// </summary>
		public bool[] Moving { get; private set; }

		/// <summary>
		/// The outlier component of the EffortStream. <c>true</c> indicates an outlier point.
		/// </summary>
		public bool[] Outlier { get; private set; }

		/// <summary>
		/// The resting component of the EffortStream. <c>true</c> indicates rest at this point.
		/// </summary>
		public bool[] Resting { get; private set; }

		/// <summary>
		/// The time component of the EffortStream.
		/// </summary>
		public double[] Time { get; private set; }

		/// <summary>
		/// The total elevation component of the EffortStream.
		/// </summary>
		public double[] TotalElevation { get; private set; }

		/// <summary>
		/// The smoothed velocity component of the EffortStream.
		/// </summary>
		public double[] SmoothedVelocity { get; private set; }

		/// <summary>
		/// Indicates presence of extended properties.
		/// </summary>
		public bool[] ExtendedProperties { get; private set; }

		/// <summary>
		/// The calculated watts component of the EffortStream. <c>true</c> indicates movement at this point.
		/// </summary>
		public double[] CalculatedWatts { get; private set; }
		#endregion


		/// <summary>
		/// Creates a new empty EffortStream.
		/// </summary>
		internal EffortStream()
		{
		}


		/// <summary>
		/// Creates a new EffortStream from a dynamic.
		/// </summary>
		/// <param name="stravaEffortStream"></param>
		internal EffortStream(dynamic stravaEffortStream)
		{
			// break "latlng" into separate Latitude and Longitude properties
			if (stravaEffortStream.latlng != null)
			{
				int count = stravaEffortStream.latlng.Count;
				Latitude = new double[count];
				Longitude = new double[count];
				for (int i = 0; i < count; i++)
				{
					Latitude[i] = (double)stravaEffortStream.latlng[i][0];
					Longitude[i] = (double)stravaEffortStream.latlng[i][1];
				}
			}

			// process other properties
			Altitude = ToDoubleArray(stravaEffortStream.altitude);
			OriginalAltitude = ToDoubleArray(stravaEffortStream.altitude_original);
			Distance = ToDoubleArray(stravaEffortStream.distance);
			Time = ToDoubleArray(stravaEffortStream.time);
			SmoothedGradient = ToDoubleArray(stravaEffortStream.grade_smooth);
			SmoothedVelocity = ToDoubleArray(stravaEffortStream.velocity_smooth);
			TotalElevation = ToDoubleArray(stravaEffortStream.total_elevation);
			CalculatedWatts = ToDoubleArray(stravaEffortStream.watts_calc);
			Moving = ToBoolArray( stravaEffortStream.moving);
			Outlier = ToBoolArray(stravaEffortStream.outlier);
			Resting = ToBoolArray(stravaEffortStream.resting);
		}


		/// <summary>
		/// Converts a dynamic to a double array.
		/// </summary>
		/// <param name="d">The dynamic to convert.</param>
		/// <returns>The double array.</returns>
		private double[] ToDoubleArray(dynamic d)
		{
			if (d == null)
				return null;
			var results = new double[d.Count];
			for (int i = 0; i < results.Length; i++)
				results[i] = (double)d[i];
			return results;
		}


		/// <summary>
		/// Converts a dynamic to a bool array.
		/// </summary>
		/// <param name="d">The dynamic to convert.</param>
		/// <returns>The bool array.</returns>
		private bool[] ToBoolArray(dynamic d)
		{
			if (d == null)
				return null;
			var results = new bool[d.Count];
			for (int i = 0; i < results.Length; i++)
				results[i] = (bool)d[i];
			return results;
		}
	}
}