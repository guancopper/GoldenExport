using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Data data;
        public ObservableCollection<Member> memberData;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DatePckerStart.Value = DateTime.Now.AddSeconds(-10);
            this.DatePickerEnd.Value = DateTime.Now;
            dataGrid.DataContext = Data.memberDataChoice;

        }

        private void choice_Click(object sender, RoutedEventArgs e)
        {
            Search search = new Search();
            search.Setip(this.iptext.Text);
            search.Show();
        }
        private void Export_Click(object sender, RoutedEventArgs e) 
        {
            var Connection = ConnectionPool.TryGetConnection();
            //实例化基本信息服务，传入连接对象
            HistorianImpl historian = new HistorianImpl(Connection);

            DataTable dt = new DataTable();
            dt.Columns.Add("时间", typeof(String));
            //列
            foreach (MemberChoice member in Data.memberDataChoice)
            {
                dt.Columns.Add(member.NameChoice, typeof(String));
            }
            //行
            if (Data.memberDataChoice[0].Entity.ToString() == "REAL32")
            {
                try 
                {
                    Entity<FloatData> entitylist = historian.GetFloatArchivedValues(Convert.ToInt32(Data.memberDataChoice[0].IDChoice),
                    10000,
                    Convert.ToDateTime(this.DatePckerStart.Value),
                    Convert.ToDateTime(this.DatePickerEnd.Value));
                    foreach (FloatData floatData in entitylist.Data)
                    {
                        dt.Rows.Add("["+floatData.Time.ToString("yyyy-MM-dd HH:mm:ss") + "." + floatData.Ms.ToString().PadLeft(3, '0')+"]", floatData.Value);
                    }
                } catch { }
                
            }
            if (Data.memberDataChoice[0].Entity.ToString() == "BOOL")
            {
                try 
                {
                    Entity<IntData> entitylist = historian.GetIntArchivedValues(Convert.ToInt32(Data.memberDataChoice[0].IDChoice),
                    10000,
                    Convert.ToDateTime(this.DatePckerStart.Value),
                    Convert.ToDateTime(this.DatePickerEnd.Value));
                    foreach (IntData intData in entitylist.Data)
                    {
                        dt.Rows.Add("[" + intData.Time.ToString("yyyy-MM-dd/HH:mm:ss") + "." + intData.Ms.ToString().PadLeft(3, '0') + "]", intData.Value);
                    }
                } catch { }
                
            }
            //查出数据
            for (int i = 1;i< Data.memberDataChoice.Count;i++) 
            {

                if (Data.memberDataChoice[i].Entity.ToString() == "REAL32") 
                {
                    try 
                    {
                        Entity<FloatData> entitylist = historian.GetFloatArchivedValues(Convert.ToInt32(Data.memberDataChoice[i].IDChoice),
                        10000,
                        Convert.ToDateTime(this.DatePckerStart.Value),
                        Convert.ToDateTime(this.DatePickerEnd.Value));
                        for (int j = 0; j < entitylist.Count; j++)
                        {
                            dt.Rows[j][Data.memberDataChoice[i].NameChoice] = entitylist.Data[j].Value;
                        }
                    } catch { }
                    
                }
                if (Data.memberDataChoice[i].Entity.ToString() == "BOOL")
                {
                    try 
                    {
                        Entity<IntData> entitylist = historian.GetIntArchivedValues(Convert.ToInt32(Data.memberDataChoice[i].IDChoice),
                        10000,
                        Convert.ToDateTime(this.DatePckerStart.Value),
                        Convert.ToDateTime(this.DatePickerEnd.Value));
                        for (int j = 0; j < entitylist.Count; j++)
                        {
                            dt.Rows[j][Data.memberDataChoice[i].NameChoice] = entitylist.Data[j].Value;
                        }
                    } catch { }
                    
                }
                
                
            }
            //导出csv
            string FilePath = System.Environment.CurrentDirectory + "\\data"+"\\"+DateTime.Now.ToString("yyyyMMddHHmmss")+".csv";
            this.CreateCSVWithCommaByDataTableAndFilePath(dt,FilePath);
        }
        public void CreateCSVWithCommaByDataTableAndFilePath(DataTable dt, string FilePath)
        {
            try
            {

                FileStream fileStream = new FileStream(FilePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fileStream, System.Text.UTF8Encoding.Default);

                string colHeaders = "", ls_item = "";
                DataRow[] myRow = dt.Select();
                int i = 0;
                int cl = dt.Columns.Count;
                for (i = 0; i < cl; i++)
                {
                    if (i == (cl - 1))
                    {
                        colHeaders += dt.Columns[i].Caption.ToString() + "\r\n";
                    }
                    else
                    {
                        colHeaders += dt.Columns[i].Caption.ToString() + ",";
                    }
                }
                sw.Write(colHeaders);
                foreach (DataRow row in myRow)
                {
                    for (i = 0; i < cl; i++)
                    {
                        if (i == (cl - 1))
                        {
                            ls_item += row[i].ToString() + "\r\n";
                        }
                        else
                        {
                            ls_item += row[i].ToString() + ",";
                        }
                    }
                    sw.Write(ls_item);
                    ls_item = "";
                }
                sw.Close();
                fileStream.Close();
                System.Diagnostics.Process.Start(FilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
