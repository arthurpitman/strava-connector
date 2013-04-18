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
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace StravaConnector
{
	/// <summary>
	/// Dynamic representation of a JSON object.
	///
	/// <para>
	/// Based on:
	/// "Using C# 4.0 and dynamic to parse JSON" by Shawn Weisfeld
	/// http://www.drowningintechnicaldebt.com/ShawnWeisfeld/archive/2010/08/22/using-c-4.0-and-dynamic-to-parse-json.aspx
	/// </para>
	/// </summary>
	public sealed class DynamicJsonObject : DynamicObject
	{
		private readonly IDictionary<string, object> dictionary;


		/// <summary>
		/// Creates a new DynamicJsonObject based on a dictionary.
		/// </summary>
		/// <param name="dictionary"></param>
		public DynamicJsonObject(IDictionary<string, object> dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary cannot be null");

			this.dictionary = dictionary;
		}


		/// <summary>
		/// Attempts to get a value for a member.
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			// try to get property by name
			if (!this.dictionary.TryGetValue(binder.Name, out result))
			{
				// return null to avoid exception
				result = null;
				return true;
			}

			// try to convert result to a list, either of objects or primatives
			var list = result as ArrayList;
			if (list != null && list.Count > 0)
			{
				if (list[0] is IDictionary<string, object>)
					result = new List<object>(list.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)));
				else
					result = new List<object>(list.Cast<object>());
				return true;
			}

			// try to convert result to an object
			var dictionary = result as IDictionary<string, object>;
			if (dictionary != null)
			{
				result = new DynamicJsonObject(dictionary);
				return true;
			}

			return true;
		}
	}
}