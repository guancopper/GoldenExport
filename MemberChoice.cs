using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoldenAPI.Model.Base;
using GoldenAPI.Model.Data;

namespace GoldenExport
{
    public class MemberChoice
    {
        //名称
        public string NameChoice { get; set; }
        //标签点ID
        public string IDChoice { get; set; }
        //标签点描述
        public string describeChoice { get; set; }
        //类型
        public DataType Entity { get; set; }
    }
}
