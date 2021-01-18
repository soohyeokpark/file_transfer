using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace file_client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<object> sendList = new List<object>();
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\targetDir");
            foreach (FileInfo f in di.GetFiles())
            {
                sendList.Add(f.FullName);
            }




            //FileInfo file = new FileInfo(@"C:\Users\soohyeok.park\Desktop\test.png");
            //byte[] buf = new byte[file.Length];

            //using (FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            //{
            using (Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

                try
                {
                    client.Connect(ip);

                    foreach (var file in sendList)
                    {
                        FileInfo target = new FileInfo(file.ToString());
                        byte[] buf = new byte[target.Length];

                        using (FileStream stream = new FileStream(target.FullName, FileMode.Open, FileAccess.Read))
                        {
                            
                            stream.Read(buf, 0, buf.Length);


                            client.Send(new byte[] { 0 });
                            client.Send(BitConverter.GetBytes(target.Name.Length));


                            client.Send(new byte[] { 1 });
                            client.Send(Encoding.UTF8.GetBytes(target.Name));


                            client.Send(new byte[] { 2 });
                            client.Send(BitConverter.GetBytes(buf.Length));


                            client.Send(new byte[] { 3 });
                            client.Send(buf);
                        }
                    }


                    
                }
                catch (Exception _ex)
                {
                    MessageBox.Show(_ex.Message);
                }
                finally
                {
                    client.Disconnect(false);
                    client.Close();
                    client.Dispose();
                }

            }
            //}
        }
    }
}
