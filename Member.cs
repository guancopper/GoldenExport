using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoldenAPI.Model.Base;
using GoldenAPI.Model.Data;

namespace GoldenExport
{
    public class Member: INotifyPropertyChanged
    {
        //选择
        public bool choice { get; set; }
        public bool Choice
        {
            get
            {
                return choice;
            }
            set
            {
                choice = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Choice"));
                }
            }
        }
        //名称
        public string Name { get; set; }
        //标签点ID
        public string ID { get; set; }
        //标签点描述
        public string describe { get; set; }
        //类型
        public DataType Entity { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
