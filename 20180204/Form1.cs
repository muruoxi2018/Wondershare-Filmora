using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace _20180204
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.muruoxi.com/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             * 读取安装目录
             */
            RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Wondershare\Wondershare Video Editor");
            /*
             * 根据能否读取到路径来判断是否安装
             */
            try
            {
                var.path = reg.GetValue("InstallPath").ToString()+@"\";
                reg.Close();
            }
            catch
            {

              MessageBox.Show("请先安装Filmora产品后再应用本补丁", "警告：", MessageBoxButtons.OK, MessageBoxIcon.Error);
              Environment.Exit(0);
            }
            finally
            {
                try
                {
                    Directory.Delete(@var.path + @"Languages", true);
                }
                finally
                {
                    Directory.CreateDirectory(@var.path + @"Languages");
                    /*
                     * 删除配置文件，防止语言设置错误的报错
                     */
                    File.Delete(var.path+ "Filmora.ini");
                    System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
                    var.language = asm.GetManifestResourceStream("_20180204.Resources.English.xml");
                    var.category_languages = asm.GetManifestResourceStream("_20180204.Resources.Category_Languages.ini");
                    if (Environment.Is64BitOperatingSystem)
                    {
                        var.filmora = asm.GetManifestResourceStream("_20180204.Resources.Filmora.exe");
                    }
                    else
                    {
                        MessageBox.Show("从v8.0开始，Filmora只支持64位系统，不再支持32位系统", "警告：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }

                    /*
                     * Main：和谐主程序
                     */
                    
                    FileStream fs;
                    BinaryWriter bw;
                    byte[] temp = new byte[var.filmora.Length];
                    var.filmora.Read(temp, 0, temp.Length);
                    var.filmora.Seek(0, SeekOrigin.Begin);
                    fs = new FileStream(var.path + @"Filmora.exe", FileMode.Create);
                    bw = new BinaryWriter(fs);
                    bw.Write(temp);
                    fs.Close();
                    bw.Close();
                    
                    

                    /*
                     * Other：主界面汉化
                     */
                    byte[] english = new byte[var.language.Length];
                    var.language.Read(english, 0, english.Length);
                    var.language.Seek(0,SeekOrigin.Begin);
                    fs = new FileStream(var.path + @"Languages\English.xml", FileMode.Create);
                    bw = new BinaryWriter(fs);
                    bw.Write(english);
                    fs.Close();
                    bw.Close();
                    
                    

                    /*
                     * Other：特效与插件汉化
                    */

                    string path_category = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Wondershare Video Editor\Resources\Category_Languages.ini";
                    byte[] category = new byte[var.category_languages.Length];
                    var.category_languages.Read(category, 0, category.Length);
                    var.category_languages.Seek(0,SeekOrigin.Begin);
                    FileInfo fInfo = new FileInfo(path_category);
                    if (fInfo.IsReadOnly)
                    {
                        fInfo.IsReadOnly = false;
                    }
                    fs = new FileStream(path_category, FileMode.Create);
                    bw = new BinaryWriter(fs);
                    bw.Write(category);
                    fs.Close();
                    bw.Close();
                    fInfo.IsReadOnly = true;
                    

                    //hosts begin
                    /*
                    string hostspath = @"C:\WINDOWS\system32\drivers\etc\hosts";
                    File.SetAttributes(hostspath, FileAttributes.Normal);
                    FileStream hosts = new FileStream(hostspath, FileMode.Append);
                    StreamWriter sw = new StreamWriter(hosts);
                    sw.WriteLine("127.0.0.1     www.tt.com");
                    sw.Close();
                    hosts.Close();
                    */

                    MessageBox.Show("设置成功", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start("https://www.muruoxi.com/");
                    Environment.Exit(0);
                }
            }



        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                this.Close();
                return;
            }
        }
        public class var
        {
            public static string path;
            public static System.IO.Stream filmora;
            public static System.IO.Stream language;
            public static System.IO.Stream category_languages;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
    }
}
