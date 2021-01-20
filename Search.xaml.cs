using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using GoldenAPI;
using GoldenAPI.Common;
using GoldenAPI.Impl;
using GoldenAPI.Inter;
using GoldenAPI.Model.Base;
using GoldenAPI.Model.Data;
using GoldenAPI.Util;


namespace GoldenExport
{
    /// <summary>
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class Search : Window
    {
        public string ip;
        public RTDBConnection conn;
        //public Data data;
        public CheckBox headcheckBox;
        //public List<int> ids;
        public Search()
        {
            InitializeComponent();    
        }
        //下拉框表选择
        private void TableNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Connection = ConnectionPool.TryGetConnection();
            //实例化基本信息服务，传入连接对象
            BaseImpl ibase = new BaseImpl(Connection);
            string SelectName = this.TableNames.SelectedItem.ToString();
            //筛选条件
            SearchConditionTotal allcssonditonLast = new SearchConditionTotal();
            int[] allBookPoissnt = ibase.Search(new SearchConditionTotal() { Table = SelectName }, 5000, DataSort.ID);
            try
            {
                headcheckBox.IsChecked = false;
            }
            catch 
            {

            };
            //获取所有属性
            List<FullPoint> Allresssult = ibase.GetPointsProperty(allBookPoissnt);
            dataGrid.DataContext = null;
            //data = new Data();
            Data.Clear();
            foreach (FullPoint point in Allresssult)
            {
                Data.Add(new Member()
                {
                    choice = false,
                    Name = SelectName+"."+point.BasePoint.Tag,
                    Entity = point.BasePoint.DataType,
                    ID = point.BasePoint.Id.ToString(),
                    describe = point.BasePoint.Desc
                });
            }
            dataGrid.DataContext = Data.memberData;
        }

        //初始化加载
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化连接池
            ConnectionPool.Init("sa", "golden",this.ip, "6327", 10);
            var Connection = ConnectionPool.TryGetConnection();
            //实例化基本信息服务，传入连接对象
            BaseImpl ibase = new BaseImpl(Connection);
            int[] tableid = ibase.GetTablesId();
            for (int i = 0; i < tableid.Length; i++)
            {
                this.TableNames.Items.Add(ibase.GetTablePropertyById(tableid[i]).Name);
            }
        }
        //数据量ip
        public void Setip(string ip) 
        {
            this.ip = ip;
        }
        //清除
        private void eliminateBtn_Click(object sender, RoutedEventArgs e)
        {
            Data.memberDataChoice.Clear();
            if (dataGrid.Items.Count > 0)
            {
                for (int i = 0; i < Data.memberData.Count; i++)
                {
                    Data.memberData[i].Choice = false;
                }
            }
        }
        //确定
        private void defineBtn_Click(object sender, RoutedEventArgs e) 
        {
            Window window = Window.GetWindow(this);//关闭当前窗体
            window.Close();
        }
        //选中事件
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox headercb = (CheckBox)sender;
            string id = headercb.Tag.ToString();
            Member Mb = Data.memberData.ToList<Member>().Find(t => t.ID == id);
            if (headercb.IsChecked.Value)
            {
                
                Data.AddChoice(new MemberChoice()
                {
                    NameChoice = Mb.Name,
                    IDChoice = Mb.ID,
                    Entity = Mb.Entity,
                    describeChoice = Mb.describe
                });
            }
            else 
            {
                Data.RemoveChoice(id);
            }
            
        }
        //全选
        private void AllCheckBox_Click(object sender, RoutedEventArgs e) 
        {
            headcheckBox = (CheckBox)sender;
            if (dataGrid.Items.Count > 0)
            {
                for (int i = 0; i < Data.memberData.Count; i++)
                {
                    Data.memberData[i].Choice = headcheckBox.IsChecked.Value;
                }
                if (headcheckBox.IsChecked.Value)
                {
                    Data.ClearChoice();
                    foreach (Member Mb in Data.memberData)
                    {
                        Data.AddChoice(new MemberChoice()
                        {
                            NameChoice = Mb.Name,
                            IDChoice = Mb.ID,
                            Entity = Mb.Entity,
                            describeChoice = Mb.describe
                        });
                    }
                }
                else 
                {
                    Data.ClearChoice();
                }
            }
        }
    }
}
