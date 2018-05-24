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
        public string Comment { get; set; }
        public DataEntry(float money, DateTime spendDate, string catagory)
        {
            Money = money;
            SpendDate = spendDate;
            Catagory = catagory;
        }
    }
    
    public class DayData
    {
        string id;
        int Day;
        float Money;
        public DayData(int day, int money)
        {
            Day = day;
            Money = money;
            id = DateTime.Now.ToLongTimeString();
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
