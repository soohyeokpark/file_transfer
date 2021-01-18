using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_server
{
    enum state
    {
        state,
        file_name_size,
        file_name,
        file_size,
        file_download
    }

    class File
    {
        protected state state = state.state;
        public byte[] file_name { get; set; }
        public byte[] binary { get; set; }
    }
}
