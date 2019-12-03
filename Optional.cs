using System;
using System.Collections.Generic;
using System.Text;

namespace Sid
{
	public class Optional<T>
	{
		public T Value
		{
			get;
			set;
		}

		public static bool IsTrue(Optional<bool> opt)
		{
			return (opt != null && opt.Value);
		}

		public static bool IsFalse(Optional<bool> opt)
		{
			return (opt != null && !opt.Value);
		}

		public static void Set(Optional<T> opt,T value)
		{
			if(opt!=null)
			{
				opt.Value = value;
			}
		}
	}
}
