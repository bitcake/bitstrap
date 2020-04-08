using System.Collections;
using System.Collections.Generic;

namespace BitStrap
{
	/// <summary>
	/// An insert optimized queue that removes the last element if a new one comes when
	/// it reaches its maximum capacity. It does not support removal, though.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class CircularBuffer<T>
	{
		public struct Enumerator
		{
			private readonly CircularBuffer<T> circularBuffer;
			private int currentIndex;

			public T Current
			{
				get { return circularBuffer[currentIndex]; }
			}

			public Enumerator( CircularBuffer<T> circularBuffer )
			{
				this.circularBuffer = circularBuffer;
				this.currentIndex = -1;
			}

			public bool MoveNext()
			{
				currentIndex += 1;
				return currentIndex < circularBuffer.Count;
			}
		}

		private readonly T[] elements;
		private int tail;

		/// <summary>
		/// Number of elements.
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// Directly access any element like an array.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get { return elements[( tail + index ) % Count]; }
			set { elements[( tail + index ) % Count] = value; }
		}

		/// <summary>
		/// Creates a CircularBuffer given its capacity.
		/// </summary>
		/// <param name="capacity"></param>
		public CircularBuffer( int capacity )
		{
			elements = new T[capacity];
			Count = 0;
			tail = 0;
		}

		/// <summary>
		/// Append an element. It may override the last element if this reaches its maximum element capacity.
		/// </summary>
		/// <param name="element"></param>
		public void Add( T element )
		{
			elements[tail] = element;

			int capacity = elements.Length;
			Count = Count < capacity ? Count + 1 : capacity;
			tail = ( tail + 1 ) % capacity;
		}

		/// <summary>
		/// Clears the buffer.
		/// </summary>
		public void Clear()
		{
			Count = 0;
			tail = 0;
		}

		/// <summary>
		/// Returns an enumerator.
		/// </summary>
		/// <returns></returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator( this );
		}
	}
}
