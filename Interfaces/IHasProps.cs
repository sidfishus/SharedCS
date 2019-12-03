using System;
using System.Collections.Generic;

namespace Sid
{
	public interface IHasProps
	{
		object FormatProps(
		 string property,
		 int? arrayIndex,
		 bool throwIfNotExist=true,
		 Optional<bool> exists=null);

		int Count(
		 string property);
	}
}
