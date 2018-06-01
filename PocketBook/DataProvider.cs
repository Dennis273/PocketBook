using System;
using System.Collections.Generic;

namespace PocketBook
{
    public enum DataOperation { Add, Remove, Update };

    class DataProvider
    {
        private List<DataEntry> dataEntries;
        private UserSetting userSetting;
        private MonthData currentMonth;
        private DayData todayData;
        private static DataProvider instance;
        // DataEntry更新的委托
        public delegate void DataChangedHandler(DataOperation dataOperation, DataEntry dataEntry);
        // DataEntry更新事件
        public event DataChangedHandler DataChanged;

        // 单例模式
        public static DataProvider GetDataProvider()
        {
            if (instance == null)
            {
                instance = new DataProvider();
            }
            return instance;
        }

        // 构造函数, 链接数据库获取数据
        private DataProvider()
        {
            try
            {
                DataBase.InitializeDateBase();
                FetchData();
                DataChanged += UpdataTodayAndCurrentMonth;
                GetPercentageAmongMonth(2018, 5);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }


        // 从数据库获取所有消费记录和用户设置
        // 设置当月消费和当天消费
        // 更新磁贴
        private void FetchData()
        {
            try
            {
                dataEntries = DataBase.GetAllEntries();
                userSetting = DataBase.GetUserSetting();
                if (userSetting.Catagories == null)
                {
                    DataBase.InitializeUserSetting();
                    userSetting = DataBase.GetUserSetting();
                }
                currentMonth = new MonthData(DateTime.Now.Month, 0);
                FetchCurrentMonth();
                todayData = GetDayDataOfMonth(DateTime.Now.Year, DateTime.Now.Month)[DateTime.Now.Day - 1];
                UpdateTile();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }

        // 获取当月消费, 结果存放到成员变量[currentMonth]中
        // 当月是根据用户设置的更新日期计算的
        private void FetchCurrentMonth()
        {
            var endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, userSetting.RenewDate);
            if (DateTime.Now.Day >= userSetting.RenewDate) endTime = endTime.AddMonths(1);
            var startTime = endTime.AddMonths(-1);
            currentMonth.Money = 0;
            foreach (DataEntry entry in dataEntries)
            {
                if (entry.SpendDate.CompareTo(endTime) == -1 && entry.SpendDate.CompareTo(startTime) != -1)
                {
                    currentMonth.Money += entry.Money;
                }
            }
        }

        // 获取当月剩余的日均消费
        public float GetAverageLeftOfCurrentMonth()
        {
            // 返回剩余每日
            // （预算 - 已花费）/ 剩余日期
            var endTime = new DateTime(DateTime.Now.Year, currentMonth.Month, userSetting.RenewDate);
            if (DateTime.Now.Day >= userSetting.RenewDate) endTime = endTime.AddMonths(1);
            return (userSetting.Budget - currentMonth.Money) / (endTime.Subtract(DateTime.Now).Days + 1);
            
        }

        // 更新当月消费
        private void UpdataTodayAndCurrentMonth(DataOperation dataOperation, DataEntry dataEntry)
        {
            var endTime = new DateTime(DateTime.Now.Year, currentMonth.Month, userSetting.RenewDate);
            if (DateTime.Now.Day >= userSetting.RenewDate) endTime = endTime.AddMonths(1);
            var startTime = endTime.AddMonths(-1);
            // 判断所作修改是否和当月有关
            if (dataEntry.SpendDate.CompareTo(endTime) == -1 && dataEntry.SpendDate.CompareTo(startTime) != -1)
            {
               if (dataOperation == DataOperation.Add)
                {
                    currentMonth.Money += dataEntry.Money;
                }
                else if (dataOperation == DataOperation.Remove)
                {
                    currentMonth.Money -= dataEntry.Money;
                }
               else
                {   
                    // 如果是修改DataEntry的值,需要重新获取当月消费
                    FetchCurrentMonth();
                }
                UpdateTile();
            }
        }
        
        // 返回当天的消费
        public DayData GetTodaySpent()
        {
            todayData = GetDayDataOfMonth(DateTime.Now.Year, DateTime.Now.Month)[DateTime.Now.Day - 1];
            return todayData;
        }

        // 返回当月消费
        public MonthData GetSpentMoneyOfCurrentMonth()
        {
            return currentMonth;
        }
        
        // 更新磁体
        private void UpdateTile()
        {
            var endTime = new DateTime(DateTime.Now.Year, currentMonth.Month, userSetting.RenewDate);
            if (DateTime.Now.Day >= userSetting.RenewDate) endTime = endTime.AddMonths(1);

            // 如果当月消费超过30%,更新显示当月消费的磁贴
            if (userSetting.Budget * 0.3 < currentMonth.Money)
            {
                Tile.TileNotificate(currentMonth.Money, userSetting.Budget, currentMonth.Money / userSetting.Budget);
            }

            // 如果当日消费超过剩余的月平均消费, 更新显示当日消费的磁贴
            if (todayData.Money > (userSetting.Budget - currentMonth.Money) / (userSetting.Budget - currentMonth.Money) / (endTime.Subtract(DateTime.Now).Days + 1))
            {
                Tile.TileNotificate(todayData.Money, (userSetting.Budget - currentMonth.Money) / (endTime.Subtract(DateTime.Now).Days+1));
            }
        }


        // 根据消费记录的类别或评价查找相关的消费记录
        // 参数: 查找关键字
        // 返回: 相关消费记录
        public List<DataEntry> SearchDataEntriesByKey(string key)
        {
            var list = new List<DataEntry>();
            foreach (DataEntry entry in dataEntries)
            {
                if (entry.Catagory.Contains(key) || entry.Comment.Contains(key))
                {
                    list.Add(entry);
                }
            }
            return list;
        }


        // 获得某年每月的消费金额
        // 参数: 需要查询的年份
        // 返回: 记录每月消费金额的MonthData列表
        public List<MonthData> GetMonthDataOfYear(int year)
        {   
            var list = new List<MonthData>();
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


        // 获得某月每天的消费金额
        // 参数: 要查询的年月
        // 返回: 记录每天消费金额的DayData列表
        public List<DayData> GetDayDataOfMonth(int year, int month)
        {   
            int dayNumber = 0;
            // 判断要查找的月份有多少天
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


        // 获得某天的所有消费记录
        // 参数: 要查询的年月日
        // 返回记录某天消费记录的DataEntry列表
        public List<DataEntry> GetDayDataEntry(int year, int month, int day)
        {
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

        // 获得某类型的消费占一个月总消费的百分比
        // 参数: 要查询的年月
        // 返回: 记录<类别, 所占百分比>的字典
        public Dictionary<string, float> GetPercentageAmongMonth(int year, int month)
        {
            var map = new Dictionary<string, float>();
            float totalMoney = 0.0f;
            foreach (string catagory in userSetting.Catagories)
            {
                map.Add(catagory, 0.0f);
            }
            foreach (DataEntry entry in dataEntries)
            {
                if (entry.SpendDate.Month == month && entry.SpendDate.Year == year)
                {
                    map[entry.Catagory] += entry.Money;
                    totalMoney += entry.Money;
                }
            }
            List<string> keys = new List<string>(map.Keys);
            for (var i = 0; i < map.Count; i++)
            {
                map[keys[i]] = totalMoney == 0 ? 0 : map[keys[i]] / totalMoney;
            }
            return map;
        }

        // 获得某类型的消费占某年总消费的百分比
        // 参数: 要查询的年份
        // 返回: 记录<类别, 所占百分比>的字典
        public Dictionary<string, float> GetPercentageAmongYear(int year)
        {
            var map = new Dictionary<string, float>();
            float totalMoney = 0.0f;
            foreach (string catagory in userSetting.Catagories)
            {
                map.Add(catagory, 0.0f);
            }
            foreach (DataEntry entry in dataEntries)
            {
                if (entry.SpendDate.Year == year)
                {
                    map[entry.Catagory] += entry.Money;
                    totalMoney += entry.Money;
                }
            }
            List<string> keys = new List<string>(map.Keys);
            for (var i = 0; i < map.Count; i++)
            {
                map[keys[i]] = totalMoney == 0 ? 0 : map[keys[i]] / totalMoney;
            }
            return map;
        }

        // 添加消费记录, 添加到数据库和成员变量[dataEntries]中
        // 参数: 要添加的消费记录
        // 返回: 如果成功添加则返回true, 否则返回false
        internal bool AddDataEntry(DataEntry dataEntry)
        {   
            // 金额小于等于0或未来消费返回false
            if (dataEntry.Money <= 0) return false;
            if (dataEntry.SpendDate > DateTime.Now) return false;
            try
            {
                DataBase.InsertEntry(dataEntry);
                dataEntries.Add(dataEntry);
                DataChanged(DataOperation.Add, dataEntry);
                return true;
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.ToString());
            }
            return false;
        }

        // 更新消费记录, 同时更新数据库和成员变量[dataEntries]
        // 参数: 更新后的消费记录, id需与旧的消费记录相同
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

        // 删除一项消费记录, 同时对数据库和成员变量[dataEntries]进行删除操作
        // 参数: 要删除的消费记录
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

        // 设置用户设置,同时修改数据库中的用户设置
        // 参数: 更新后的用户设置
        internal bool SetUserSetting(UserSetting userSetting)
        {
            if (userSetting.Username == null || userSetting.Username == "") return false;
            if (userSetting.RenewDate == 0) return false;
            if (userSetting.Budget <= 0) return false;
            try
            {
                DataBase.UpdateUserSetting(userSetting);
                this.userSetting.Username = userSetting.Username;
                // 如果更改了更新日期,需要重新获取当月消费
                if (this.userSetting.RenewDate != userSetting.RenewDate)
                {
                    this.userSetting.RenewDate = userSetting.RenewDate;
                    FetchCurrentMonth();
                }         
                this.userSetting.Budget = userSetting.Budget;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
            return true;
        }

        // 返回用户设置
        public UserSetting GetUserSetting()
        {
            return userSetting;
        }

        // 返回用户设置的所有类别
        internal List<string> GetCatagories()
        {
            return userSetting.Catagories;
        }


        // 添加类别, 同时添加到数据库中
        internal void AddCatagory(string catagory)
        {
            if (catagory == "") return;
            try
            {
                string newCatagories = "";
                foreach (string temp in userSetting.Catagories)
                {
                    newCatagories += temp + ";";
                }
                // 判断新添加的类别是否已存在
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


        // 更改类别
        // 参数:
        //  oldCatagory: 需要更改的类别
        //  newCatagory: 新的类别, 默认为"其他"
        internal void ChangeCatagory(string oldCatagory, string newCatagory = "其他")
        {
            if (newCatagory == "") return;
            try
            {   
                // 判断要更改的类别是否存在
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


        // 删除所有的数据
        internal void __DeleteAll()
        {
            try
            {
                DataBase.__DeleteAllData();
                FetchData();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }
    }
}
