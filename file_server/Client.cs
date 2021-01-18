using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace file_server
{
    class Client : File
    {
        private Socket socket = null;
        private byte[] buf = null;
        private int seek = 0;
        private string save_dir = @"C:\Users\";

        public Client(Socket _socket)
        {
            this.socket = _socket;
            buf = new byte[1];
            this.socket.BeginReceive(buf, 0, buf.Length, SocketFlags.None, callback, this);
        }

        private void InitBufAndState()
        {
            buf = new byte[1];
            state = state.state;
        }

        private void callback(IAsyncResult ar)
        {
            if (this.socket != null && this.socket.Connected)
            {
                int size = this.socket.EndReceive(ar);
                if (size <= 0)
                {
                    this.socket.Disconnect(false);
                    this.socket.Close();
                    this.socket.Dispose();
                    //GC.Collect();
                    return;
                }

                if (state == state.state)
                {
                    switch (buf[0])
                    {
                        case 0:
                            state = state.file_name_size;
                            buf = new byte[4];
                            break;

                        case 1:
                            state = state.file_name;
                            buf = new byte[file_name.Length];
                            seek = 0;
                            break;

                        case 2:
                            state = state.file_size;
                            buf = new byte[4];
                            break;

                        case 3:
                            state = state.file_download;
                            buf = new byte[binary.Length];
                            seek = 0;
                            break;
                    }
                }
                else if (state == state.file_name_size)
                {
                    file_name = new byte[BitConverter.ToInt32(buf, 0)];
                    InitBufAndState();
                }
                else if (state == state.file_name)
                {
                    Array.Copy(buf, 0, file_name, seek, size);
                    seek = seek + size;

                    if(seek >= file_name.Length)
                    {
                        InitBufAndState();
                    }
                }
                else if (state == state.file_size)
                {
                    binary = new byte[BitConverter.ToInt32(buf, 0)];
                    InitBufAndState();
                }
                else if (state == state.file_download)
                {
                    Array.Copy(buf, 0, binary, seek, size);
                    seek = seek + size;

                    if(seek >= binary.Length)
                    {
                        using (FileStream stream = new FileStream(save_dir + Encoding.UTF8.GetString(file_name),
                            FileMode.Create, FileAccess.Write))
                        {
                            stream.Write(binary, 0, binary.Length);
                        }

                        //this.socket.Disconnect(false);
                        //this.socket.Close();
                        //this.socket.Dispose();
                        //GC.Collect();
                        //return;

                        InitBufAndState();
                    }
                }


                this.socket.BeginReceive(buf, 0, buf.Length, SocketFlags.None, callback, this);
            }            
        }


    }
}
