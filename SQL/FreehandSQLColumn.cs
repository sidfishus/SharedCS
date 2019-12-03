using System;
using System.Collections.Generic;
using System.Text;

namespace Sid
{
	public class FreehandSQLColumn : SQLColumn
	{
		public FreehandSQLColumn(SQLTable table) : base(table)
		{
		}

		public string CustomSelect
		{
			get;
			set;
		}

		public override string Select()
		{
			return string.Format("({0}) as {1}", CustomSelect,Name);
		}

		public override string ReferenceName()
		{
			return Name;
		}

		public override string FullName()
		{
			return Name;
		}
	}
}
