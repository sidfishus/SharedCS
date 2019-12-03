using System;
using System.Collections.Generic;
using System.Text;


namespace Sid
{
	public class SQLTable : IHasProps
	{
		private const string Props_PrimaryKey = "primaryKey";
		private const string Props_Name = "name";
		
		public string Name
		{
			get;
			set;
		}

		// Column name mapped to the column.
		public Dictionary<string, SQLColumn> Columns
		{
			get;
			private set;
		}

		public SQLColumn PrimaryKey
		{
			get;
			set;
		}

		public SQLColumn ParentKey
		{
			get;
			set;
		}

		public SQLTable()
		{
			Columns = new Dictionary<string, SQLColumn>();
		}

		public virtual object FormatProps(
		 string property,
		 int? arrayIndex,
		 bool throwIfNotExist = true,
		 Optional<bool> exists = null)
		{
			object rv=null;
			Optional<bool>.Set(exists, true);
			if (string.Equals(property, Props_PrimaryKey, StringComparison.CurrentCultureIgnoreCase))
			{
				rv = PrimaryKey;
			}
			else if (string.Equals(property, Props_Name, StringComparison.CurrentCultureIgnoreCase))
			{
				rv = Name;
			}
			else
			{
				if(throwIfNotExist)
				{
					throw new Exception(string.Format("Invalid props property: {0}",property));
				}
				Optional<bool>.Set(exists, false);
			}

			return rv;
		}

		public virtual int Count(
		 string property)
		{
			throw new Exception(string.Format("Invalid count property: {0}", property));
		}

		public SQLColumn FindColumn(string name)
		{
			SQLColumn rv = Columns[name];
			return rv;
		}

		public SQLColumn GetForeignKeyTo(SQLTable table)
		{
			SQLColumn fk=null;
			foreach(var pair in Columns)
			{
				SQLColumn column = pair.Value;
				if (column.ForeignKey!=null && column.ForeignKey.Table==table)
				{
					fk = column;
					break;
				}
			}
			return fk;
		}
	}
}
