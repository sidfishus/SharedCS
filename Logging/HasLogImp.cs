using System;
using System.Collections.Generic;
using System.Text;

namespace Sid.Log
{
	public class HasLogImp : IHasLog
	{
		public ILog Log
		{
			get;
			set;
		}
	}
}
