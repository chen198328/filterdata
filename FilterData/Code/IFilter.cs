using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace FilterData.Code
{
    public interface IFilter
    {
        /// <summary>
        /// 从文档中获取机构字段
        /// </summary>
        /// <param name="filename"></param>
        List<string> GetInstituteFields(string filename);
        /// <summary>
        /// 获取一级机构,数量
        /// </summary>
        /// <param name="fields"></param>
        Dictionary<string, int> GetParentInstitutes(List<string> fields);
        /// <summary>
        /// 获取二级机构
        /// </summary>
        /// <param name="fields"></param>
        DataTable GetInstitutes(List<string> fields);
        DataTable SelectInstitutes(DataTable institutes, List<string> parentInstitues);

    }
}
