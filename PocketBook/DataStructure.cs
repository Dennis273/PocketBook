using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketBook
{   
    // 消费记录
    public class DataEntry
    {
        public float Money { get; set; }
        public DateTime SpendDate { get; set; }
        public string Catagory { get; set; }
        public string Comment { get; set; }

        public string Id;

        public DataEntry(float money, DateTime spendDate, string catagory, string comment = "", string id = "")
        {
            Money = money;
            SpendDate = spendDate;
            Catagory = catagory;
            Comment = comment;
            Id = id == "" ? Guid.NewGuid().ToString() : id;
        }
    }
    
    // 记录日消费
    public class DayData
    {
        public int Day;
        public float Money;
        public float Percentage;
        public DayData(int day, float money)
        {
            Day = day;
            Money = money;
        }
    }

    // 记录月消费
    public class MonthData
    {
        public int Month;
        public float Money;
        public float Percentage;
        public MonthData(int month, float money)
        {
            Month = month;
            Money = money;
        }
    }

    // 用户设置类
    public class UserSetting
    {
        public string Username;
        public int RenewDate;
        public float Budget;
        public List<string> Catagories;
        public UserSetting(string username = "", int renewDate = 1, float budget = 1, List<string> catagories = null)
        {
            Username = username;
            RenewDate = renewDate;
            Budget = budget;
            Catagories = catagories;
        }
    }
}
