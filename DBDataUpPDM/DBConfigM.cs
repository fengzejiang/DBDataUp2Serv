using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBDataUpPDM
{
    public class DBConfigM
    {
		private String sid;
		private String name;
		private String tbName;
		private int inter;
		private String bgtime;
		private String timefld;
        private String dbconf;
        private String cornStr;
		private List<DBConfigItem> list;

        public string Sid { get => sid; set => sid = value; }
        public string Name { get => name; set => name = value; }
        public string TbName { get => tbName; set => tbName = value; }
        public int Inter { get => inter; set => inter = value; }
        public string Bgtime { get => bgtime; set => bgtime = value; }
        public string Timefld { get => timefld; set => timefld = value; }
        public string Dbconf { get => dbconf; set => dbconf = value; }
        public List<DBConfigItem> List { get => list; set => list = value; }
        public string CornStr { get => cornStr; set => cornStr = value; }

        public string getTablePkFld() {
            string pkfld = "F_StdNo";
            foreach(DBConfigItem item in list){
                if (item.ApiKey.Equals("bdid")) {
                    pkfld = item.Dbfld;
                    break;
                }
            
            }
            return pkfld;
        }

        public string getDBFlds()
        {
            string flds = "";
            foreach (DBConfigItem item in list)
            {
                flds += "," + item.Dbfld;

            }
            flds = flds.Substring(1);
            return flds;
        }
    }
}
