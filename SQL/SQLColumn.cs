using System;
using System.Collections.Generic;
using Sid;

namespace Sid
{
	public class SQLColumn : IHasProps
	{
		private const string PROPS_Name = "name";
		private const string PROPS_FullName = "fullName";
		private const string PROPS_ReferenceName = "referenceName";
		private const string PROPS_Select = "select";
		private const string PROPS_Table = "table";
		private string m_Name;

		public SQLColumn ForeignKey
		{
			get;
			set;
		}

		public SQLTable Table
		{
			get;
			private set;
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				if(m_Name!=null)
				{
					throw new Exception("The column name should only be specified once.");
				}
				m_Name = value;
				// When the name is set, add the column to the list
				if(Table!=null)
				{
					Table.Columns.Add(value,this);
				}
			}
		}

		public SQLColumn(SQLTable table)
		{
			Table = table;
		}

		public virtual string ReferenceName()
		{
			string rv;
			if (Table != null)
			{
				rv = string.Format("{0}_{1}", Table.Name, Name);
			}
			else
			{
				rv = Name;
			}
			return rv;
		}

		// Can be overriden.
		public virtual string Select()
		{
			string rv;
			if (Table != null)
			{
				rv=string.Format("{0} as {1}", FullName(),ReferenceName());
			}
			else
			{
				rv = Name;
			}
			return rv;
		}

		public virtual string FullName()
		{
			string rv;
			if(Table!=null)
			{
				rv = string.Format("{0}.{1}",Table.Name,Name);
			}
			else
			{
				rv = Name;
			}
			return rv;
		}
			
		public virtual object FormatProps(
		 string property,
		 int? arrayIndex,
		 bool throwIfNotExist = true,
		 Optional<bool> exists = null)
		{
			object rv=null;
			Optional<bool>.Set(exists, true);
			if (string.Equals(property, PROPS_Name, StringComparison.CurrentCultureIgnoreCase))
			{
				rv = Name;
			}
			else if (string.Equals(property, PROPS_FullName, StringComparison.CurrentCultureIgnoreCase))
			{
				rv = FullName();
			}
			else if (string.Equals(property, PROPS_ReferenceName, StringComparison.CurrentCultureIgnoreCase))
			{
				rv = ReferenceName();
			}
			else if (string.Equals(property, PROPS_Select, StringComparison.CurrentCultureIgnoreCase))
			{
				rv = Select();
			}
			else if (string.Equals(property, PROPS_Table, StringComparison.CurrentCultureIgnoreCase))
			{
				rv = Table;
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
	}
}
