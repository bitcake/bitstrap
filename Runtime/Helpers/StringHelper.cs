using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	///     Bunch of helper methods to work with the string class.
	/// </summary>
	public static class StringHelper
	{
		private static readonly Dictionary<Index, string> stringTable =
			new Dictionary<Index, string>(new IndexComparer());

		public static string AbsoluteToRelativePath(string absolutePath)
		{
			var alteredPath = absolutePath.Replace("\\", "/");
			if (alteredPath.StartsWith(Application.dataPath))
				return "Assets" + absolutePath.Substring(Application.dataPath.Length);
			throw new ArgumentException("Full path does not contain the current project's Assets folder",
				"absolutePath");
		}

		public static string RemoveSuffixFromString(string stringToClean, char separator = '_')
		{
			var splitString = stringToClean.Split(separator);
			var pureString = string.Join("_", splitString.Take(splitString.Length - 1).ToArray());

			return pureString;
		}

		/// <summary>
		///     Given a string format and a number, returns its string representation.
		///     It's better to use this method than just number.ToString() since,
		///     in the long term, it does not generate string garbage.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string Format(string format, int number)
		{
			var ui = new Index(format, number);

			if (!stringTable.ContainsKey(ui))
				stringTable[ui] = ui.GetString();

			return stringTable[ui];
		}

		/// <summary>
		///     The same as StringHelper.Format() but as an int extension method.
		/// </summary>
		/// <param name="n"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string ToStringSmart(this int n, string format = "{0}")
		{
			return Format(format, n);
		}

		private class IndexComparer : IEqualityComparer<Index>
		{
			public bool Equals(Index x, Index y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(Index x)
			{
				return x.GetHashCode();
			}
		}

		private struct Index
		{
			public readonly int number;
			public readonly string format;
			private readonly int hashCode;

			public Index(string format, int number)
			{
				this.number = number;
				this.format = format;
				hashCode = number * format.GetHashCode();
			}

			public bool Equals(Index other)
			{
				return number == other.number && format == other.format;
			}

			public override int GetHashCode()
			{
				return hashCode;
			}

			public string GetString()
			{
				return string.Format(format, number);
			}
		}
	}
}