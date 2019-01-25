using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace _20180204
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        bool beginMove = false;//初始化鼠标位置  
        int currentXPosition;
        int currentYPosition;



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.muruoxi.com/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             * 读取安装目录
             */
            RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Wondershare\Wondershare Filmora");
            /*
             * 根据能否读取到路径来判断是否安装
             */
            try
            {
                var.path = reg.GetValue("InstallPath").ToString() + @"\";
                reg.Close();
            }
            catch
            {

                MessageBox.Show("请先安装Filmora产品后再应用本补丁", "警告：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            try
            {
                Directory.Delete(@var.path + @"Lang\en", true);
                
                Directory.CreateDirectory(@var.path + @"Lang\en");
            }
            finally
            {
                
            }
            
            try
            {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            



            var.language = asm.GetManifestResourceStream("_20180204.Resources.English.xml");
            var.category_languages = asm.GetManifestResourceStream("_20180204.Resources.Category_Languages.ini");
            var.category_xml = asm.GetManifestResourceStream("_20180204.Resources.Category.xml");
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
            var.language.Seek(0, SeekOrigin.Begin);
                System.Diagnostics.Debug.WriteLine(var.path + @"Languages\English.xml");
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
            var.category_languages.Seek(0, SeekOrigin.Begin);
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

            string path_category_xml = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Wondershare Video Editor\Resources\Category.xml";
            byte[] category_xml = new byte[var.category_xml.Length];
            var.category_xml.Read(category_xml, 0, category_xml.Length);
            var.category_xml.Seek(0, SeekOrigin.Begin);
            fInfo = new FileInfo(path_category_xml);
            if (fInfo.IsReadOnly)
            {
                fInfo.IsReadOnly = false;
            }
            fs = new FileStream(path_category_xml, FileMode.Create);
            bw = new BinaryWriter(fs);
            bw.Write(category_xml);
            fs.Close();
            bw.Close();
            fInfo.IsReadOnly = true;


            try
            {
                string hostspath = @"C:\WINDOWS\system32\drivers\etc\hosts";
                File.SetAttributes(hostspath, FileAttributes.Normal);
                FileStream hosts = new FileStream(hostspath, FileMode.Append);
                StreamWriter sw = new StreamWriter(hosts);
                sw.WriteLine("127.0.0.1    platform.wondershare.com");
                sw.WriteLine("127.0.0.1    www.wondershare.com");
                sw.WriteLine("127.0.0.1    cbs.wondershare.com");
                sw.WriteLine("127.0.0.1    www.cbs.wondershare.com");
                sw.Close();
                hosts.Close();
            }
            catch
            {
                MessageBox.Show("屏蔽hosts失败！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            MessageBox.Show("设置成功", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start("https://www.muruoxi.com/");
            Environment.Exit(0);



            }
            catch {
                MessageBox.Show("设置失败", "提示：", MessageBoxButtons.OK , MessageBoxIcon.Error );
                System.Diagnostics.Process.Start("https://www.muruoxi.com/");
                Environment.Exit(0);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                System.Diagnostics.Process.Start("https://www.muruoxi.com/");
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
            public static System.IO.Stream category_xml;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.muruoxi.com/");
            this.Close();
            return;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标  
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标  
            }
        }



        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态  
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x  
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标  
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            }

        }
    }
}
