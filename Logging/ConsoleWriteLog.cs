using System;
using System.Collections.Generic;
using System.Text;

namespace Sid.Log
{
	public class ConsoleWriteLog : ILog
	{
		bool m_Enabled;
		int m_Level;

		public ConsoleWriteLog()
		{
			m_Enabled = true;
			m_Level = 0;
		}

		public void LogLine(
		 string text)
		{
			if (m_Enabled)
			{
				Console.WriteLine(text);
			}
		}

		public void Log(
		 string text)
		{
			if (m_Enabled)
			{
				Console.Write(text);
			}
		}

		public void Enable(bool enable)
		{
			m_Enabled = enable;
		}

		public void SetLevel(int level)
		{
			m_Level = level;
		}

		public int GetLevel()
		{
			return m_Level;
		}
	}
}
