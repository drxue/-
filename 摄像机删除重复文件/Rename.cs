using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 摄像机删除重复文件
{
    public partial class Rename : Form
    {
        private DirectoryInfo dirInfo;
        private Dictionary<string, string> dic;//保存目录名称新旧对照

        public Rename()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            //查找文件夹
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string dir = folderBrowserDialog.SelectedPath;
                if (!string.IsNullOrEmpty(dir))
                {
                    textBox1.Text = dir;
                    showDir(dir);//显示目录列表
                }
            }

        }


        private void Rename_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Owner.Show();
        }

        /// <summary>
        /// //显示目录在List1
        /// </summary>
        /// <param name="dir"></param>
        private void showDir(string dir)
        {
            dirInfo = new DirectoryInfo(dir);
            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            listBox1.Items.Clear();
            foreach (var item in dirInfos)
            {
                listBox1.Items.Add(item.Name);
            }
        }

        /// <summary>
        /// //计算更改,显示在ListBox2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            int count = 0;
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            dic = new Dictionary<string, string>();
            listBox2.Items.Clear();
            foreach (var dir in dirs)
            {
                string dirName = dir.Name;
                string dirReName = RenameDir(dirName);
                if (dirName != dirReName)
                {
                    count++;
                }
                dic.Add(dir.FullName, dir.Parent.FullName + '\\' + dirReName);
                listBox2.Items.Add(dirReName);
            }
            MessageBox.Show("更改目录" + count + "个！");
        }

        /// <summary>
        /// 更改目录名称
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        private string RenameDir(string dirName)
        {
            if (dirName.Length >= 10)
            {
                string strDate = dirName.Substring(0, 10);
                DateTime dt;
                if (DateTime.TryParse(strDate, out dt))
                {
                    strDate = dt.Date.ToString("yyyy-MM-dd");
                    if (dirName.Length > 10)
                    {
                        dirName = strDate + dirName.Substring(10);
                    }
                    else
                    {
                        dirName = strDate;
                    }
                }
            }
            return dirName;
        }

        /// <summary>
        /// 确认修改目录名称，执行修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dic == null || dic.Count == 0)
            {
                return;
            }

            foreach (var key in dic.Keys)
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(key + '\\');
                    dirInfo.MoveTo(dic[key]);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            MessageBox.Show("Done!");
            return;
        }
    }
}
