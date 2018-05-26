using System;
using System.Collections.Generic;

namespace PocketBook
{
    public enum DataOperation { Add, Remove, Update };

    class DataProvider
    {
        private List<DataEntry> dataEntries;
        private UserSetting userSetting;

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
            try
            {
                DataBase.InitializeDateBase();
                dataEntries = DataBase.GetAllEntries();
                userSetting = DataBase.GetUserSetting();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }

        public List<MonthData> GetMonthDataOfYear(int year)
        {
            // implement here
            var list = new List<MonthData>();
            //dataEntries.
            for (int i = 1; i <= 12; i++)
            {
                list.Add(new MonthData(i, 0));
            }

            foreach (DataEntry entry in dataEntries)
            {
                if (entry.SpendDate.Year == year)
                {
                    list[entry.SpendDate.Month - 1].Money += entry.Money;
                }
            }
            return list;
        }

        public List<DayData> GetDayDataOfMonth(int year, int month)
        {
            // implement here
            int dayNumber = 0;

            dayNumber = (month % 2) == 0 ? 30 : 31;
            dayNumber = (month < 8) ? dayNumber : dayNumber == 31 ? 30 : 31;
            dayNumber = month == 2 ? 28 : dayNumber;

            if (month == 2 && (year % 400 == 0) || (year % 4 == 0 && year % 100 != 0))
                dayNumber = 29;

            var list = new List<DayData>();
            for (int i = 1; i <= dayNumber; i++)
            {
                list.Add(new DayData(i, 0));
            }

            foreach (DataEntry entry in dataEntries)
            {
                if (entry.SpendDate.Year == year && entry.SpendDate.Month == month)
                {
                    list[entry.SpendDate.Day - 1].Money += entry.Money;
                }
            }
            return list;
        }

        public List<DataEntry> GetDayDataEntry(int year, int month, int day)
        {
            // implement here
            var list = new List<DataEntry>();
            foreach (DataEntry entry in dataEntries)
            {
                if (entry.SpendDate.Day == day && entry.SpendDate.Month == month && entry.SpendDate.Year == year)
                {
                    list.Add(entry);
                }
            }
            return list;
        }

        public Dictionary<string, float> GetPercentageAmongMonth(int year, int month)
        {
            // implement here
            var map = new Dictionary<string, float>();
            foreach (string catagory in userSetting.Catagories)
            {
                map.Add(catagory, 0.0f);
            }
            foreach (DataEntry entry in dataEntries)
            {
                if (entry.SpendDate.Month == month && entry.SpendDate.Year == year)
                {
                    map[entry.Catagory] += entry.Money;
                }
            }
            return map;
        }



        internal void AddDataEntry(DataEntry dataEntry)
        {
            try
            {
                DataBase.InsertEntry(dataEntry);
                dataEntries.Add(dataEntry);
                DataChanged(DataOperation.Add, dataEntry);
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.ToString());
            }
        }

        internal void UpdateDataEntry(DataEntry dataEntry)
        {
            try
            {
                DataBase.UpdateEntry(dataEntry);
                foreach (DataEntry entry in dataEntries)
                {
                    if (entry.Id == dataEntry.Id)
                    {
                        entry.Money = dataEntry.Money;
                        entry.Catagory = dataEntry.Catagory;
                        entry.SpendDate = dataEntry.SpendDate;
                        break;
                    }
                }
                DataChanged(DataOperation.Update, dataEntry);
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.ToString());
            }
        }

        internal void DeleteDataEntry(DataEntry dataEntry)
        {
            try
            {
                DataBase.DeleteEntry(dataEntry.Id);
                dataEntries.Remove(dataEntry);
                DataChanged(DataOperation.Remove, dataEntry);
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.ToString());
            }
        }

        internal void SetUserSetting(UserSetting userSetting)
        {
            try
            {
                DataBase.UpdateUserSetting(userSetting);
                this.userSetting.Username = userSetting.Username;
                this.userSetting.RenewDate = userSetting.RenewDate;
                this.userSetting.Budget = userSetting.Budget;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }

        public UserSetting GetUserSetting()
        {
            return userSetting;
        }

        internal List<string> GetCatagories()
        {
            return userSetting.Catagories;
        }

        internal void AddCatagory(string catagory)
        {
            try
            {
                string newCatagories = "";
                foreach (string temp in userSetting.Catagories)
                {
                    newCatagories += temp + ";";
                }
                if (!newCatagories.Contains(catagory))
                {
                    newCatagories += catagory + ";";
                    DataBase.UpdateCatagory(newCatagories);
                    userSetting.Catagories.Add(catagory);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }

        internal void ChangeCatagory(string oldCatagory, string newCatagory = "未定义")
        {
            try
            {
                if (userSetting.Catagories.Contains(oldCatagory))
                {
                    var newCatagories = "";
                    foreach (string temp in userSetting.Catagories)
                    {
                        newCatagories += temp + ";";
                    }
                    newCatagories = newCatagories.Replace(oldCatagory + ";", "");
                    DataBase.UpdateCatagory(newCatagories);
                    userSetting.Catagories.Remove(oldCatagory);
                    DataBase.UpdateEntryCatagory(oldCatagory, newCatagory);
                    AddCatagory(newCatagory);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }

        internal void __DeleteAll()
        {
            try
            {
                DataBase.__DeleteAllData();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }
    }
}
