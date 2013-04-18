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
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;

namespace StravaConnector
{
	/// <summary>
	/// Converts JSON to a dynamic.
	///
	/// <para>
	/// Based on:
	/// "Using C# 4.0 and dynamic to parse JSON" by Shawn Weisfeld
	/// http://www.drowningintechnicaldebt.com/ShawnWeisfeld/archive/2010/08/22/using-c-4.0-and-dynamic-to-parse-json.aspx
	/// </para>
	/// </summary>
	public sealed class DynamicJsonConverter : JavaScriptConverter
	{
		private ReadOnlyCollection<Type> supportedTypes = new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) }));

		/// <summary>
		/// Serializes an object. Not implemented.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public override IDictionary<string, object> Serialize(object o, JavaScriptSerializer s)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Deserializes an object.
		/// </summary>
		/// <param name="d"></param>
		/// <param name="t"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public override object Deserialize(IDictionary<string, object> d, Type t, JavaScriptSerializer s)
		{
			if (d == null)
				throw new ArgumentNullException("dictionary cannot be null");

			return t == typeof(object) ? new DynamicJsonObject(d) : null;
		}


		/// <summary>
		/// Gets types supported by this converter.
		/// </summary>
		public override IEnumerable<Type> SupportedTypes
		{
			get { return supportedTypes; }
		}
	}
}