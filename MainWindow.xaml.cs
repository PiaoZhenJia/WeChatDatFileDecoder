using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommonConfig.RefreshAll();
        }

        /// <summary>
        /// 输入选择按钮
        /// </summary>
        private void Input_Button_Click(object sender, RoutedEventArgs e)
        {
            tbInput.Text = ChoseFile();
        }

        /// <summary>
        /// 输出选择按钮
        /// </summary>
        private void Output_Button_Click(object sender, RoutedEventArgs e)
        {
            tbOutput.Text = ChoseFile();
        }

        /// <summary>
        /// 开始转换按钮
        /// </summary>
        private void Do_Action(object sender, RoutedEventArgs e)
        {
            if (tbInput.Text.Equals("请选择文件夹"))
            {
                MessageBox.Show("请选择dat文件所在位置","准备工作未就绪");
                return;
            }

            if (tbOutput.Text.Equals("请选择文件夹"))
            {
                MessageBox.Show("请选择解密后文件存储位置", "准备工作未就绪");
                return;
            }

            DirectoryInfo root = new DirectoryInfo(tbInput.Text);
            FileInfo[] files = root.GetFiles();

            Dictionary<byte, string> keyValuePairs = new Dictionary<byte, string>();
            keyValuePairs.Add(0xFF, "jpg");
            keyValuePairs.Add(0x89, "png");
            keyValuePairs.Add(0x47, "gif");
            int countDat = 0;
            int countSuccess = 0;
            int countDefault = 0;
            foreach (var f in files)
            {
                if (f.Name.Split('.')[f.Name.Split('.').Length-1].ToLower().Equals("dat"))//仅对dat后缀操作，大小写不敏感
                {
                    countDat++;
                    byte[] fileBytesArr = File2Bytes(f.FullName);
                    String prefix = "";
                    byte[] needHead;
                    byte key = 0;
                    //确定文件格式
                    foreach (var kv in CommonConfig.fileHead)
                    {
                        needHead = String2Hex(kv.Key);
                        if ((fileBytesArr[0] ^ needHead[0]) == (fileBytesArr[1] ^ needHead[1]))
                        {
                            key = (byte)(fileBytesArr[0] ^ needHead[0]);
                            prefix = kv.Value;
                            break;
                        }
                    }
                    //如果没有找到匹配
                    if (prefix.Equals(""))
                    {
                        countDefault++;
                        break;
                    }
                    for (int i = 0; i < fileBytesArr.Length; i++)
                    {
                        fileBytesArr[i] ^= key;
                    }
                    Bytes2File(fileBytesArr,tbOutput.Text + System.IO.Path.DirectorySeparatorChar + f.Name + "." + prefix);
                    countSuccess++;
                }
            }
            MessageBox.Show("转换完成，目录文件数" + files.Length + "dat文件数" + countDat + "已经转换" + countSuccess + "未匹配" + countDefault);
        }

        /// <summary>
        /// 文件夹选择函数
        /// </summary>
        private String ChoseFile()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            dialog.ShowDialog();
            try
            {
                return dialog.FileName;
            }
            catch (Exception)
            {
                return "请选择文件夹";
            }
            
        }

        private byte[] String2Hex(String str)
        {
            byte[] b = new byte[2];
            b[0] = (byte)Int32.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            b[1] = (byte)Int32.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            return b;
        }

        /// <summary>
        /// 感谢，这两个方法从https://www.cnblogs.com/xuxml/p/13946816.html复制
        /// </summary>
        public static byte[] File2Bytes(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }

        public static void Bytes2File(byte[] buff, string savepath)
        {
            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }
            Console.WriteLine(savepath);
            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();

        }

        private void AddConfigBtn(object sender, RoutedEventArgs e)
        {
            AddConfigWindow w1 = new AddConfigWindow();
            w1.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thanks thank= new Thanks();
            thank.Show();
        }
    }
}
