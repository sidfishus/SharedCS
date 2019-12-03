using DateTime = System.DateTime;

namespace Sid
{
	public static class DateFunctions
	{
		public static bool IsFuture(
		 int year,
		 int month,
		 int day)
		{
			DateTime now = DateTime.Now;
			bool future;
			if (year > now.Year)
			{
				future = true;
			}
			else if (month > now.Month && year == now.Year)
			{
				future = true;
			}
			else if (day > now.Day && month == now.Month && year == now.Year)
			{
				future = true;
			}
			else
			{
				future = false;
			}

			return future;
		}

		public static bool IsToday(
		 int year,
		 int month,
		 int day)
		{
			DateTime now = DateTime.Now;
			bool isToday = (year == now.Year && month == now.Month && day == now.Day);
			return isToday;
		}
	}

}