using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class AddConfigWindow : Window
    {
        public AddConfigWindow()
        {
            InitializeComponent();
            RefreshTb();
        }

        private void BtnAdd(object sender, RoutedEventArgs e)
        {
            string res = CommonConfig.AddToFileHead(tbInputHead.Text, tbInputPerfix.Text);
            RefreshTb();
            MessageBox.Show(res,"添加结果");
        }

        private void BtnRefresh(object sender, RoutedEventArgs e)
        {
            CommonConfig.RefreshAll();
            RefreshTb();
        }

        private void RefreshTb()
        {
            tbOutput.Text = CommonConfig.ShowKV();
        }
    }
}
