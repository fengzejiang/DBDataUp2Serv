using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBDataUp2LY
{
    public class DBConfigItem
    {
        private int cid;
        private String apiKey;
        private String dbfld;
        private String name;

        public int Cid { get => cid; set => cid = value; }
        public string ApiKey { get => apiKey; set => apiKey = value; }
        public string Dbfld { get => dbfld; set => dbfld = value; }
        public string Name { get => name; set => name = value; }
    }
}
