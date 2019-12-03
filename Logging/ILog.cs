using System;
using System.Collections.Generic;
using System.Text;

namespace Sid.Log
{
	public interface ILog
	{
		void Log(string text);
		void LogLine(string text);

		void Enable(bool enable);

		void SetLevel(int level);

		int GetLevel();
	}
}
