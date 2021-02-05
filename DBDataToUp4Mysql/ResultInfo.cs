
using System.Collections.Generic;


namespace DBDataToUp4Mysql
{
    public class ResultInfo
    {
        private int id;
        private string result;
        private Dictionary<string, string> data;

        public int Id { get => id; set => id = value; }
        public string Result { get => result; set => result = value; }
        public Dictionary<string, string> Data { get => data; set => data = value; }
    }
}
