﻿using System;
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
        public string id;

        public DataEntry(float money, DateTime spendDate, string catagory, string id = "")
        {
            Money = money;
            SpendDate = spendDate;
            Catagory = catagory;
            id = id == "" ? Guid.NewGuid().ToString() : id;

        }
    }
    
    public class DayData
    {
        public int Day;
        public float Money;
        public DayData(int day, float money)
        {
            Day = day;
            Money = money;
        }
    }
    public class MonthData
    {
        public int Month;
        public float Money;
        public MonthData(int month, float money)
        {
            Month = month;
            Money = money;
        }
    }
}
