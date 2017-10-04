using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 摄像机删除重复文件
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> dics;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    textBox1.Text = dialog.SelectedPath;
                }
                else
                {
                    MessageBox.Show("请选择一个文件夹", "提示");
                }

            }
        }
        /// <summary>
        /// 目录变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            if (Directory.Exists(path)) //目录存在
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo[] dirs = dir.GetDirectories();
                listBoxDirectory.Items.Clear();
                foreach (DirectoryInfo item in dirs)
                {
                    listBoxDirectory.Items.Add(item.Name);
                }
            }

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text))
            {
                DirectoryInfo dir = new DirectoryInfo(textBox1.Text);

                dics = FindFiles(dir);
                listBoxFiles.Items.Clear();
                foreach (var item in dics.Keys)
                {
                    listBoxFiles.Items.Add(dics[item]);
                }
            }
        }
        /// <summary>
        /// 递归查找文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private Dictionary<string, string> FindFiles(DirectoryInfo dir)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            FileInfo[] files = dir.GetFiles();
            if (files.Count() > 0)
            {
                foreach (var item in files)
                {
                    //判断文件名称是否包含（）;
                    if (item.Name.Contains('(')&&item.Name.Contains(')'))
                    {
                        result.Add(item.FullName, item.Name);
                    }
                    
                }
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (dirs.Count() > 0)
            {
                foreach (var item in dirs)
                {
                    Dictionary<string, string> dicTemp = new Dictionary<string, string>();
                    dicTemp = FindFiles(item); 
                    if (dicTemp!=null&&dicTemp.Count>0)
                    {
                        //dics= dics.Concat(dicTemp) as Dictionary<string, string>;
                        foreach (var key in dicTemp.Keys)
                        {
                            result.Add(key, dicTemp[key]);
                        }
                    }
                 
                }
            }

            return result;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (dics.Count()>0)
            {
                foreach (string key in dics.Keys)
                {
                    try
                    {
                        File.Delete(key);
                        count++;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("删除文件失败!"+key,"提示");
                        throw;
                    }
                    
                }
                MessageBox.Show("成功删除"+count+"个文件！");
            }
        }
    }
}
