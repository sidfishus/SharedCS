using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Sid
{
	public static class FileLib
	{

		public static void WriteToUTF8File(
		 string filename,
		 string filetext)
		{
			FileStream fs = new FileStream(filename, FileMode.Create);
			byte[] fileBytes = Encoding.UTF8.GetBytes(filetext);
			fs.Write(fileBytes, 0, fileBytes.Length);
		}
	}
}
