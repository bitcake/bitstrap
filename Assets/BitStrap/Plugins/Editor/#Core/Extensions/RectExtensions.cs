using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Provides a set of methods to aux non-layout editor code.
	/// It contains methods that smartly positions Rects in the Inspector.
	/// </summary>
	public static class RectExtensions
	{
		/// <summary>
		/// Given a source big Rect, it returns a sub Rect with same width
		/// but with the height of EditorGUIUtility.singleLineHeight.
		/// Also, its Y position is calculated as if it's the "rownNumber'th"
		/// Rect from top to bottom.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="rowNumber"></param>
		/// <returns></returns>
		public static Rect Row( this Rect position, int rowNumber )
		{
			position.y += EditorGUIUtility.singleLineHeight * rowNumber;
			position.height = EditorGUIUtility.singleLineHeight;

			return position;
		}

		/// <summary>
		/// Given a Rect, it returns a center anchored copy with a width of "width".
		/// If a negative number is passed, it behaves as if there is a space on the sides with a total width of "width".
		/// </summary>
		/// <param name="position"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public static Rect CenterHorizontally( this Rect position, float width )
		{
			if( width > 0.0f )
			{
				float diff = position.width - width;
				position.x += diff * 0.5f;
				position.width = width;
			}
			else
			{
				position.x -= width * 0.5f;
				position.width += width;
			}

			return position;
		}

		public static Rect CenterVertically( this Rect position, float height )
		{
			if( height > 0.0f )
			{
				float diff = position.height - height;
				position.y += diff * 0.5f;
				position.height = height;
			}
			else
			{
				position.y -= height * 0.5f;
				position.height += height;
			}

			return position;
		}

		/// <summary>
		/// Given a Rect, it returns a left anchored copy with a width of "width".
		/// If a negative number is passed, it behaves as if there is a space on the right with a width of "width".
		/// </summary>
		/// <param name="position"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public static Rect Left( this Rect position, float width )
		{
			if( width > 0.0f )
				position.width = width;
			else
				position.width += width;

			return position;
		}

		/// <summary>
		/// Given a Rect, it returns a right anchored copy with a width of "width".
		/// If a negative number is passed, it behaves as if there is a space on the left with a width of "width".
		/// </summary>
		/// <param name="position"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public static Rect Right( this Rect position, float width )
		{
			if( width > 0.0f )
				position.xMin = position.xMax - width;
			else
				position.xMin -= width;

			return position;
		}

		public static Rect Up( this Rect position, float height )
		{
			if( height > 0.0f )
				position.height = height;
			else
				position.height += height;

			return position;
		}

		public static Rect Down( this Rect position, float height )
		{
			if( height > 0.0f )
				position.yMin = position.yMax - height;
			else
				position.yMin -= height;

			return position;
		}

		public static Rect Left( this Rect position, float width, out Rect target )
		{
			target = position.Left( width );
			return position.Right( -width );
		}

		public static Rect Right( this Rect position, float width, out Rect target )
		{
			target = position.Right( width );
			return position.Left( -width );
		}

		public static Rect Up( this Rect position, float width, out Rect target )
		{
			target = position.Up( width );
			return position.Down( -width );
		}

		public static Rect Down( this Rect position, float width, out Rect target )
		{
			target = position.Down( width );
			return position.Up( -width );
		}

		public static void Expand( this Rect position, out Rect target )
		{
			target = position;
		}
	}
}