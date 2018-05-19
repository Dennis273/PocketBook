using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketBook
{
    class DataEntry
    {
        public float Money { get; set; }
        public DateTime SpendDate { get; set; }
        public string Catagory { get; set; }
    }
    
    class CatagoryHandler
    {
        public List<string> Catagories;
        private CatagoryHandler()
        {
            Catagories = new List<string>();
        }
        public string GetCatagory(string s)
        {
            if (Catagories.Contains(s)) return s;
            return "";
        }
        static public CatagoryHandler GetCatagoryHandler()
        {
            if (m == null)
            {
                m = new CatagoryHandler();
            }
            return m;
        }
        static private CatagoryHandler m;
    }
}
