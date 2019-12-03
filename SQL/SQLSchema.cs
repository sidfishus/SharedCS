using System;
using System.Collections.Generic;
using System.Text;

namespace Sid
{
	public class SQLSchema
	{
		// Table name mapped to the table instance
		Dictionary<string,SQLTable> m_Tables;

		public SQLSchema()
		{
			m_Tables = new Dictionary<string, SQLTable>();
			BuildSchema();
		}

		// In the future we could automate this
		void BuildSchema()
		{
			BuildHRSchema();
		}

		public SQLTable FindTable(string tableName)
		{
			SQLTable rv = m_Tables[tableName];
			return rv;
		}

		SQLTable CreateTable(string tableName,string primaryKey)
		{
			SQLTable table;
			table = new SQLTable();
			table.Name = tableName;
			m_Tables.Add(tableName, table);
			CreatePrimaryKey(table, primaryKey);
			return table;
		}

		SQLColumn CreateColumn(SQLTable table, string colName)
		{
			SQLColumn col = new SQLColumn(table);
			col.Name = colName;
			return col;
		}

		SQLColumn CreatePrimaryKey(SQLTable table,string colName)
		{
			table.PrimaryKey = CreateColumn(table, colName);
			return table.PrimaryKey;
		}

		SQLColumn CreateParentKey(SQLTable table, string colName,SQLTable parentTable)
		{
			SQLColumn col=CreateColumn(table, colName);
			table.ParentKey=col;
			col.ForeignKey= parentTable.PrimaryKey;
			return col;
		}

		SQLColumn CreateForeignKey(SQLTable table, string colName,SQLTable fkTable)
		{
			SQLColumn col=CreateColumn(table, colName);
			col.ForeignKey= fkTable.PrimaryKey;
			return col;
		}

		void BuildHRSchema()
		{
			SQLTable table;

			//tDevDepts
			SQLTable deptTable= CreateTable("tDevDepts", "DeptId");
			CreateColumn(deptTable, "DeptDesc");

			//hr_tEmployees
			SQLTable empTable = CreateTable("hr_tEmployees", "Emp_Id");
			CreateForeignKey(empTable, "DeptId", deptTable);

			{
				//hr_tCustomerPlannerHed
				SQLTable hedTable = CreateTable("hr_tCustomerPlannerHed", "hedid");
				CreateColumn(hedTable, "descr");

				//hr_tCustomerPlannerDet
				table = CreateTable("hr_tCustomerPlannerDet", "detid");
				CreateParentKey(table, "hedid", hedTable);
				CreateForeignKey(table, "Emp_Id", empTable);
			}
		}
	}
}
