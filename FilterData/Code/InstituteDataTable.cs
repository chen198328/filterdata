using System.Data;
namespace FilterData.Code
{
    public class InstituteDataTable
    {
        /// <summary>
        //创建机构表
        /// </summary>
        /// <returns></returns>
        public static DataTable Create()
        {
            #region 创建表
            DataTable InstituteTable = new DataTable();
            DataColumn University = new DataColumn("一级机构", typeof(string));
            DataColumn School = new DataColumn("二级机构", typeof(string));
            DataColumn Subject = new DataColumn("学科规范", typeof(string));
            DataColumn Standard = new DataColumn("学院规范", typeof(string));
            DataColumn UniversityCh = new DataColumn("学校规范", typeof(string));
            DataColumn HashCode = new DataColumn("HashCode", typeof(int));
            DataColumn id = new DataColumn("id", typeof(int));
            id.AutoIncrement = true;
            id.AutoIncrementSeed = 1;
            id.AutoIncrementStep = 1;

            InstituteTable.Columns.Add(University);
            InstituteTable.Columns.Add(School);
            InstituteTable.Columns.Add(Subject);
            InstituteTable.Columns.Add(Standard);
            InstituteTable.Columns.Add(UniversityCh);
            InstituteTable.Columns.Add(id);
            InstituteTable.Columns.Add(HashCode);
            #endregion
            InstituteTable.PrimaryKey = new DataColumn[] { id };
            return InstituteTable;
        }
    }
}
