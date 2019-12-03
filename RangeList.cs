using System;
using System.Collections.Generic;
using System.Text;

namespace Sid
{
	public class RangeList<TYPE>
		where TYPE : struct, IComparable
	{
		List<Range<TYPE>> m_RangeList=new List<Range<TYPE>>();
		//~V

		public void AddRange(
		 Range<TYPE> range)
		{
			m_RangeList.Add(range);
		}
	}

	//TODO this would be better as a struct.
	public class Range<TYPE> : IComparable
		where TYPE : struct,IComparable
	{
		public Range()
		{
		}

		public Range(
		 TYPE? min,
		 TYPE? max)
		{
			Min = min;
			Max = max;
		}

		public TYPE? Min
		{
			get;
			set;
		}

		public TYPE? Max
		{
			get;
			set;
		}

		int IComparable.CompareTo(object rhs)
		{
			Range<TYPE> rhsAsRange = (Range <TYPE>)rhs;

			if (!Min.HasValue)
			{
				// If they are both null then they match
				return ((rhsAsRange.Min.HasValue) ? -1 : 0);
			}
			else if (!rhsAsRange.Min.HasValue)
			{
				return 1;
			}
			return Min.Value.CompareTo(rhsAsRange.Min.Value);
		}

		public bool IsInRange(
		 TYPE value)
		{
			//UNTESTED.
			bool rv;
			if (Max.HasValue)
			{
				if (Min.HasValue)
				{
					rv = (Min.Value.CompareTo(value) <= 0 && Max.Value.CompareTo(value) >= 0);
				}
				else
				{
					rv = (Max.Value.CompareTo(value) >= 0);
				}
			}
			else if (Min.HasValue)
			{
				rv = (Min.Value.CompareTo(value) <= 0);
			}
			else
			{
				rv = true;
			}
			return rv;
		}
	}
}
