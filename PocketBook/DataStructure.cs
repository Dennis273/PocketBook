using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketBook
{
    public class DataEntry
    {
        public float Money { get; set; }
        public DateTime SpendDate { get; set; }
        public string Catagory { get; set; }
        public DataEntry(float money, DateTime spendDate, string catagory)
        {
            Money = money;
            SpendDate = spendDate;
            Catagory = catagory;
        }
    }

    public class CatagoryHandler
    {
        public List<string> Catagories;
        private CatagoryHandler()
        {
            Catagories = new List<string>();
        }

        // Check if s exist in Catagories
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
    
    public class DayData
    {
        int Day;
        float Money;
        public DayData(int day, int money)
        {
            Day = day;
            Money = money;
        }
    }
    public class MonthData
    {
        public int Month;
        public float Money;
        public MonthData(int month, int money)
        {
            Month = month;
            Money = money;
        }
    }
}
