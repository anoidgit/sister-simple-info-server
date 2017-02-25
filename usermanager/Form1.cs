using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace usermanager
{
    public partial class Form1 : Form
    {
        private Dictionary<string, uiposize> sizinfo = new Dictionary<string, uiposize>();
        private bool needsave = false;
        private string dbfile = "userdb.txt";
        private Dictionary<string, string> udb = new Dictionary<string, string>();

        private string GetSHA(string str_sha1_in)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "");
            return str_sha1_out.ToLower();
        }

        private void lduiset()
        {
            foreach (var ctrl in Controls)
            {
                if (ctrl is Button)
                {
                    Button btn = (Button)ctrl;
                    sizinfo[btn.Name] = new uiposize(btn.Width, btn.Height, btn.Top, btn.Left);
                }
                if (ctrl is TextBox)
                {
                    TextBox txb = (TextBox)ctrl;
                    sizinfo[txb.Name] = new uiposize(txb.Width, txb.Height, txb.Top, txb.Left);
                }
                if (ctrl is Label)
                {
                    Label lbl = (Label)ctrl;
                    sizinfo[lbl.Name] = new uiposize(lbl.Width, lbl.Height, lbl.Top, lbl.Left);
                }
                if (ctrl is ListBox)
                {
                    ListBox lbl = (ListBox)ctrl;
                    sizinfo[lbl.Name] = new uiposize(lbl.Width, lbl.Height, lbl.Top, lbl.Left);
                }
                if (ctrl is CheckBox)
                {
                    CheckBox lbl = (CheckBox)ctrl;
                    sizinfo[lbl.Name] = new uiposize(lbl.Width, lbl.Height, lbl.Top, lbl.Left);
                }
            }
            sizinfo["Form1"] = new uiposize(Width, Height, Top, Left);
        }

        private Dictionary<string, string> ldudb(string fname)
        {
            Dictionary<string, string> rt = new Dictionary<string, string>();
            if (File.Exists(fname))
            {
                var sr = new StreamReader(fname);
                rt = new Dictionary<string, string>();
                string lind = null;
                while ((lind = sr.ReadLine()) != null)
                {
                    lind = lind.Trim();
                    if (lind.Length != 0)
                    {
                        var ind = lind.IndexOf("|||");
                        if (ind > 0)
                        {
                            var uname = lind.Substring(0, ind);
                            var pwd = lind.Substring(ind + 3);
                            if (rt.ContainsKey(uname))
                            {
                                rt[uname] = pwd;
                            }
                            else
                            {
                                rt.Add(uname, pwd);
                            }
                        }
                    }
                }
            }
            return rt;
        }

        private Dictionary<string, string> ldudbfiles(string[] fnames)
        {
            Dictionary<string, string> rt = new Dictionary<string, string>();
            foreach (var fname in fnames)
            {
                if (File.Exists(fname))
                {
                    var sr = new StreamReader(fname);
                    rt = new Dictionary<string, string>();
                    string lind = null;
                    while ((lind = sr.ReadLine()) != null)
                    {
                        lind = lind.Trim();
                        if (lind.Length != 0)
                        {
                            var ind = lind.IndexOf("|||");
                            if (ind > 0)
                            {
                                var uname = lind.Substring(0, ind);
                                var pwd = lind.Substring(ind + 3);
                                if (rt.ContainsKey(uname))
                                {
                                    rt[uname] = pwd;
                                }
                                else
                                {
                                    rt.Add(uname, pwd);
                                }
                            }
                        }
                    }
                }
            }
            return rt;
        }

        private void writefile(string fname, string dtwrt)
        {
            StreamWriter sw = null;
            try
            {
                var fs = new FileStream(fname, FileMode.Create, FileAccess.Write, FileShare.None);
                sw = new StreamWriter(fs);
            }
            catch (Exception) { }
            if (sw != null)
            {
                sw.Write(dtwrt);
                sw.Flush();
                sw.Close();
            }
        }

        private void saveudb(string fname, Dictionary<string, string> db)
        {
            if (db != null)
            {
                var dt = new List<string>();
                foreach (var kvp in db)
                {
                    dt.Add(kvp.Key + "|||" + kvp.Value);
                }
                writefile(fname, string.Join("\n", dt.ToArray()));
            }
            needsave = false;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lduiset();
            if (File.Exists(dbfile))
            {
                udb = ldudb(dbfile);
            }
            show();
            label3.Text = "初始化完成";
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            float xr = (float)Width / sizinfo["Form1"].width;
            float yr = (float)Height / sizinfo["Form1"].height;
            foreach (var ctrl in Controls)
            {
                if (ctrl is Button)
                {
                    Button btn = (Button)ctrl;
                    btn.Width = (int)(sizinfo[btn.Name].width * xr);
                    btn.Height = (int)(sizinfo[btn.Name].height * yr);
                    btn.Left = (int)(sizinfo[btn.Name].left * xr);
                    btn.Top = (int)(sizinfo[btn.Name].top * yr);
                }
                if (ctrl is TextBox)
                {
                    TextBox txb = (TextBox)ctrl;
                    txb.Width = (int)(sizinfo[txb.Name].width * xr);
                    txb.Height = (int)(sizinfo[txb.Name].height * yr);
                    txb.Left = (int)(sizinfo[txb.Name].left * xr);
                    txb.Top = (int)(sizinfo[txb.Name].top * yr);
                }
                if (ctrl is Label)
                {
                    Label lbl = (Label)ctrl;
                    lbl.Width = (int)(sizinfo[lbl.Name].width * xr);
                    lbl.Height = (int)(sizinfo[lbl.Name].height * yr);
                    lbl.Left = (int)(sizinfo[lbl.Name].left * xr);
                    lbl.Top = (int)(sizinfo[lbl.Name].top * yr);
                }
                if (ctrl is ListBox)
                {
                    ListBox lbl = (ListBox)ctrl;
                    lbl.Width = (int)(sizinfo[lbl.Name].width * xr);
                    lbl.Height = (int)(sizinfo[lbl.Name].height * yr);
                    lbl.Left = (int)(sizinfo[lbl.Name].left * xr);
                    lbl.Top = (int)(sizinfo[lbl.Name].top * yr);
                }
                if (ctrl is CheckBox)
                {
                    CheckBox lbl = (CheckBox)ctrl;
                    lbl.Width = (int)(sizinfo[lbl.Name].width * xr);
                    lbl.Height = (int)(sizinfo[lbl.Name].height * yr);
                    lbl.Left = (int)(sizinfo[lbl.Name].left * xr);
                    lbl.Top = (int)(sizinfo[lbl.Name].top * yr);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var file = openFileDialog1;
            if (file.ShowDialog() == DialogResult.OK)
            {
                if ((udb.Count > 0) && (needsave))
                {
                    saveudb(dbfile + ".bak", udb);
                }
                if (checkBox1.Checked)
                {
                    var nudb = ldudbfiles(file.FileNames);
                    foreach (var kvp in nudb)
                    {
                        var uname = kvp.Key;
                        if (udb.ContainsKey(uname))
                        {
                            udb[uname] = kvp.Value;
                        }
                        else
                        {
                            udb.Add(uname, kvp.Value);
                        }
                    }
                    show();
                    label3.Text = "读入成功，增加" + nudb.Count.ToString() + "名用户";
                }
                else
                {
                    if (file.FileNames.Length == 1)
                    {
                        dbfile = file.FileName;
                        udb = ldudb(dbfile);
                        show();
                        label3.Text = "读入成功，读入" + udb.Count.ToString() + "名用户";
                    }
                    else
                    {
                        label3.Text = "非合并读入不允许读入多个文件";
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveudb(dbfile, udb);
            label3.Text = "保存成功";
        }

        private void show()
        {
            listBox1.Items.Clear();
            foreach (var uname in udb.Keys)
            {
                listBox1.Items.Add(uname);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var uname = textBox1.Text;
            var pwd = textBox2.Text;
            if ((uname.Length > 0) && (pwd.Length > 0))
            {
                if (uname.IndexOf("|||") == -1)
                {
                    if (udb.ContainsKey(uname))
                    {
                        udb[uname] = GetSHA(pwd + textBox3.Text);
                    }
                    else
                    {
                        udb.Add(uname, GetSHA(pwd + textBox3.Text));
                    }
                    needsave = true;
                    show();
                    label3.Text = "添加成功";
                }
                else
                {
                    label3.Text = "用户名中不允许包含|||";
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((udb.Count > 0) && (needsave))
            {
                saveudb(dbfile + ".bak", udb);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var uname = textBox1.Text;
            bool del = false;
            if ((uname.Length > 0) && (udb.ContainsKey(uname)))
            {
                udb.Remove(uname);
                del = true;
            }
            if (listBox1.SelectedItems.Count > 0)
            {
                foreach (var itm in listBox1.SelectedItems)
                {
                    uname = itm.ToString();
                    if ((uname.Length > 0) && (udb.ContainsKey(uname)))
                    {
                        udb.Remove(uname);
                        del = true;
                    }
                }
            }
            if (del)
            {
                needsave = true;
                show();
                label3.Text = "删除成功";
            }
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            var ind = listBox1.SelectedIndex;
            if ((ind >= 0) && (ind < listBox1.Items.Count))
            {
                var uname = listBox1.SelectedItems[0].ToString();
                if (uname.Length > 0)
                {
                    textBox1.Text = uname;
                }
            }
        }
    }
}
