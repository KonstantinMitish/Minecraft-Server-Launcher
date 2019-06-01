using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft_Server_Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SystemFunctions func = new SystemFunctions();
        List<string> serverfiles = new List<string>();
        string secretcode = "fgfrsfedxcygrefvy7tb54347tb5hbb6hvgjbuy8n";
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("minecraftservers\\"))
                Directory.CreateDirectory("minecraftservers\\");
            if (File.Exists("minecraftservers\\serverlist.lst"))
            {


                StreamReader myfile;
                myfile = new StreamReader("minecraftservers\\serverlist.lst", Encoding.UTF8);
                string abc;
                abc = myfile.ReadToEnd();
                myfile.Close();
                string[] servers = Crypter.Decrypt(abc, secretcode).Split(':');
             //   string[] servers = abc.Split(':');
                foreach (string s in servers)
                {
                    string[] ss = s.Split('!');
                    servernames.Items.Add(ss[0]);
                    serverfiles.Add(ss[1]);
                }




            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (File.Exists("minecraftservers\\serverlist.lst"))
                    File.Delete("minecraftservers\\serverlist.lst");
                string check = servernames.Items[0].ToString();
                string FtoWrite = "";
                for (int i = 0; i < servernames.Items.Count; i++)
                {
                    FtoWrite += ":" + servernames.Items[i] + "!" + serverfiles[i];
                }
                string FtoWrite2 = FtoWrite.Substring(1);
                FileStream myFile;
                myFile = new FileStream("minecraftservers\\serverlist.lst", FileMode.Append);
                StreamWriter myfile_1;
                myfile_1 = new StreamWriter(myFile, Encoding.UTF8);
               string abc = Crypter.Encrypt(FtoWrite2, secretcode);
              //  string abc = FtoWrite2;
                
                myfile_1.Write(abc);
                myfile_1.Close();
            }
            catch
            {
                if (File.Exists("minecraftservers\\serverlist.lst"))
                    File.Delete("minecraftservers\\serverlist.lst");
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowose_Click(object sender, EventArgs e)
        {
            dialog.ShowDialog();
            string file = dialog.FileName;
                    txtJar.Text = file;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string file = txtJar.Text;
            if (File.Exists(file))
            
            {
                if (Path.GetExtension(file) == ".jar")
                {
                    txtName.Text.Trim();
                    if (txtName.Text == "" || txtName.Text == null || txtName.Text == " ")
                    {
                        MessageBox.Show("Invalid Name!");
                    }
                    else
                    {
                        if (!servernames.Items.Contains(txtName.Text)&&!Directory.Exists("minecraftservers\\"+txtName.Text))
                        {
                            serverfiles.Add(Path.GetFileName(file));
                            servernames.Items.Add(txtName.Text);
                            Directory.CreateDirectory("minecraftservers\\" + txtName.Text);
                            File.Copy(file, "minecraftservers\\" + txtName.Text + "\\" + Path.GetFileName(file));
                        }
                        else
                        {
                        MessageBox.Show("Server already exists!");
                        }
                        
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Jar!");
                }
            }
            else
            {
                MessageBox.Show("Invalid Jar!");
            }
        }

        private void servernames_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(serverfiles[servernames.SelectedIndex]);
        }
        bool is64=false;
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (int.Parse(txtMemory.Text) > 1024 && func.GetOSBit() == "x32")
            {
                MessageBox.Show("Your os is 32-bit you cant use more memory, than 1024!\r\nStarting server with 1024 Mb...");
                txtMemory.Text = "1024";
            }
            bool process = false;
            if (func.IsJava7Installed())
            {
                process = true;
            }
            else
            {
                DialogResult d = MessageBox.Show("You have no Java or Java is incorrect version\r\nProcess?\r\n(May be crashed!)","Java error",MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                    process = true;
                else
                    process = false;
            }
            //MessageBox.Show(func.GetOSBit());
                is64=(func.GetOSBit()=="x64");
            if(process)
            {
                if (File.Exists("minecraftservers\\" + servernames.Text + "\\start.bat"))
                    File.Delete("minecraftservers\\" + servernames.Text + "\\start.bat");
                //
                FileStream myFile;
                myFile = new FileStream("minecraftservers\\" + servernames.Text + "\\start.bat", FileMode.Append);
                StreamWriter myfile_1;
                myfile_1 = new StreamWriter(myFile, Encoding.UTF8);
                string abc = "CD\r\n\"";
                if (is64)
                    abc += "%ProgramW6432%";
                else
                    abc += "%ProgramFiles%";
                abc += "\\Java\\jre7\\bin\\java.exe\" -Xmx" + txtMemory.Text + "M -Xms" + txtMemory.Text + "M -jar "+ serverfiles[servernames.SelectedIndex]+" nogui";
                myfile_1.Write(abc);
                myfile_1.Close();
                {//eula
                    myFile = new FileStream("minecraftservers\\" + servernames.Text + "\\eula.txt", FileMode.Append);
                    myfile_1 = new StreamWriter(myFile, Encoding.UTF8);
                    abc = @"#By changing the setting below to TRUE you are indicating your agreement to our EULA (https://account.mojang.com/documents/minecraft_eula).";
                    abc += "\r\n";
                    abc += @"#Fri Jan 01 00:00:00 MSK 2021";
                    abc += "\r\n";
                    abc += @"eula=true";
                    myfile_1.Write(abc);
                    myfile_1.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (servernames.SelectedIndex != -1)
            {
                if (Directory.Exists("minecraftservers\\" + servernames.Text))
                    Directory.Delete("minecraftservers\\" + servernames.Text,true);
                serverfiles.RemoveAt(servernames.SelectedIndex);
                servernames.Items.RemoveAt(servernames.SelectedIndex);
            }
        }


    }
    public static class Crypter
    {
        #region Шифроване
        public static string Decrypt(string cipherText, string password,
   string salt = "Kosher", string hashAlgorithm = "SHA1",
   int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY",
    int keySize = 256)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes(password, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;

            using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
            {
                using (MemoryStream memStream = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }
        public static string Encrypt(string plainText, string password,
             string salt = "Kosher", string hashAlgorithm = "SHA1",
           int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY",
            int keySize = 256)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes(password, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }
        #endregion
    }
}
