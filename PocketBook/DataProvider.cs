using System;
using System.Collections.Generic;

namespace PocketBook
{
    public enum DataOperation { Add, Remove, Update };

    class DataProvider
    {

        private List<DataEntry> dataEntries;

        public delegate void DataChangedHandler(DataOperation dataOpration, DataEntry dataEntry);

        public event DataChangedHandler DataChanged;


        public static DataProvider GetDataProvider()
        {
            if (instance == null) instance = new DataProvider();
            return instance;
        }
        private static DataProvider instance;

        private DataProvider()
        {
            dataEntries = DataBase.GetAllEntries();
        }
        public List<MonthData> GetMonthDataOfYear(int year)
        {
            // implement here
            var list = new List<MonthData>();
            for (int i = 1; i <=12; i++)
            {
                list.Add(new MonthData(i, i * 100));
            }
            return list;
        }
        public List<DayData> GetDayDataOfMonth(int year, int month)
        {
            // implement here
            var list = new List<DayData>();
            for (int i = 1; i <= 12; i++)
            {
                list.Add(new DayData(i, i * 10));
            }
            return list;
        }
        public List<DataEntry> GetDayDataEntry(int year, int month, int day)
        {
            // implement here
            var list = new List<DataEntry>();
            for (int i = 1; i <= 12; i++)
            {
                list.Add(new DataEntry(10, DateTime.Now, "food"));
            }
            return list;
        }

        internal void AddDataEntry()
        {
            DataChanged(DataOperation.Add, new DataEntry(10, DateTime.Now, "food"));
        }
    }
}
