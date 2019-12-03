using System;
using System.Collections.Generic;
using System.Text;

namespace Sid.Log
{
	public interface IHasLog
	{
		ILog Log
		{
			get;
			set;
		}
	}
}
