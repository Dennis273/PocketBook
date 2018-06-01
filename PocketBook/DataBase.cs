using SQLitePCL;
using System;
using System.Collections.Generic;

namespace PocketBook
{
    public static class DataBase
    {
        private static String DB_NAME = "DataEntry.db";
        private static String USER_SETTING_TABLE = "UserSetting";
        private static String SQL_CREATE_USER_TABLE = "CREATE TABLE IF NOT EXISTS " + USER_SETTING_TABLE + " (Username TEXT PRIMARY KEY, Budget FLOAT, RenewDate INTEGER, Catagories TEXT);";
        private static String TABLE_NAME = "DataEntryTable";
        private static String SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (Id TEXT PRIMARY KEY, Day INTEGER, Month INTEGER, Year INTEGER, Money FLOAT, Catagory TEXT, Comment TEXT);";
        private static String SQL_INSERT = "INSERT INTO " + TABLE_NAME + " VALUES(?,?,?,?,?,?,?);";
        private static String SQL_UPDATE = "UPDATE " + TABLE_NAME + " SET Day = ?, Month = ?, Year = ?, Money = ?, Catagory = ?, Comment = ?  WHERE Id = ?";
        private static String SQL_DELETE = "DELETE FROM " + TABLE_NAME + " WHERE Id = ?";

        // 链接数据库
        private static SQLiteConnection connection = new SQLiteConnection(DB_NAME);

        // 初始化用户设置,在第一次使用时调用
        internal static void InitializeUserSetting()
        {
            using (var statement = connection.Prepare("INSERT INTO " + USER_SETTING_TABLE + " VALUES(?,?,?,?);"))
            {
                statement.Bind(1, "用户");
                statement.Bind(2, 1);
                statement.Bind(3, 1);
                statement.Bind(4, "其他");
                statement.Step();
            }
           
        }

        // 初始化数据库, 创建DataEntry表和UserSetting表
        public static void InitializeDateBase()
        {
            using (var statement = connection.Prepare(SQL_CREATE_TABLE))
            {
                statement.Step();
            }

            using (var statement = connection.Prepare(SQL_CREATE_USER_TABLE))
            {
                statement.Step();
            }
        }

        // 将数据添加到数据库中
        //
        // 参数: 要添加的数据项
        public static void InsertEntry(DataEntry dataEntry)
        {
            using (var statement = connection.Prepare(SQL_INSERT))
            {
                statement.Bind(1, dataEntry.Id);
                statement.Bind(2, dataEntry.SpendDate.Day);
                statement.Bind(3, dataEntry.SpendDate.Month);
                statement.Bind(4, dataEntry.SpendDate.Year);
                statement.Bind(5, dataEntry.Money);
                statement.Bind(6, dataEntry.Catagory);
                statement.Bind(7, dataEntry.Comment);
                statement.Step();
            }
        }

        // 返回DataEntry的所有元组
        public static List<DataEntry> GetAllEntries()
        {
            List<DataEntry> entries = new List<DataEntry>();
            using (var statement = connection.Prepare(
                "SELECT * FROM " + $"{TABLE_NAME };"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    var id = (string)statement["Id"];
                    var money = (float)(Double)statement["Money"];
                    var catagory = (string)statement["Catagory"];
                    var spendDate = new DateTime((int)(Int64)statement["Year"], (int)(Int64)statement["Month"], (int)(Int64)statement["Day"]);
                    var comment = (string)statement["Comment"];
                    entries.Add(new DataEntry(money, spendDate, catagory, comment, id));
                }
            }
            return entries;
        }

        // 更新数据
        //
        // 参数: 要更新的数据项
        public static void UpdateEntry(DataEntry dataEntry)
        {
            using (var statement = connection.Prepare(SQL_UPDATE))
            {
                statement.Bind(1, dataEntry.SpendDate.Day);
                statement.Bind(2, dataEntry.SpendDate.Month);
                statement.Bind(3, dataEntry.SpendDate.Year);
                statement.Bind(4, dataEntry.Money);
                statement.Bind(5, dataEntry.Catagory);
                statement.Bind(6, dataEntry.Comment);
                statement.Bind(7, dataEntry.Id);
                statement.Step();
            }
        }


        // 删除DataEntry中的元组
        //
        // 参数: 要删除的数据项的id
        public static void DeleteEntry(string id)
        {
            using (var statement = connection.Prepare(SQL_DELETE))
            {
                statement.Bind(1, id);
                statement.Step();
            }
        }

        // 返回用户设置
        public static UserSetting GetUserSetting()
        {
            UserSetting userSetting = new UserSetting();
            using (var statement = connection.Prepare(
                "SELECT * FROM " + $"{USER_SETTING_TABLE };"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    var catagories = (string)statement["Catagories"];
                    // 类别在数据库中存放形式:food;drink;
                    // 因此需要将catagories转换成列表
                    var strs = catagories.Split(";", StringSplitOptions.RemoveEmptyEntries);
                    var temp = new List<string>(strs);
                    var username = (string)statement["Username"];
                    var renewDate = (int)(Int64)statement["RenewDate"];
                    var budget = (float)(Double)statement["Budget"];
                    userSetting = new UserSetting(username, renewDate, budget, temp);
                }
            }
            return userSetting;
        }


        // 更新用户设置的类别
        public static void UpdateCatagory(string catagorys)
        {
            using (var statement = connection.Prepare(
              "UPDATE " + USER_SETTING_TABLE + " SET Catagories = ?"))
            {
                statement.Bind(1, catagorys);
                statement.Step();
            }
        }


        // 更新DataEntry中元组的类别就,在更改类别时调用
        //
        // 参数: 
        //  oldCatagory: 需要更改的类别
        //  newCatagory: 更改后的类别
        public static void UpdateEntryCatagory(string oldCatagory, string newCatagory)
        {
            using (var statement = connection.Prepare(
             "UPDATE " + TABLE_NAME + " SET Catagory = ? WHERE Catagory = ?"))
            {
                statement.Bind(1, newCatagory);
                statement.Bind(2, oldCatagory);
                statement.Step();
            }
        }


        // 更新用户设置
        // 参数: 更新后的用户设置
        public static void UpdateUserSetting(UserSetting userSetting)
        {
            using (var statement = connection.Prepare(
             "UPDATE " + USER_SETTING_TABLE + " SET Username = ?, RenewDate = ?, Budget = ?"))
            {
                statement.Bind(1, userSetting.Username);
                statement.Bind(2, userSetting.RenewDate);
                statement.Bind(3, userSetting.Budget);
                statement.Step();
            }
        }


        // 清空所有数据, 包括所有DataEntry元组和UserSetting
        internal static void __DeleteAllData()
        {
            using (var statement = connection.Prepare(
                   "DELETE FROM " + TABLE_NAME))
            {
                statement.Step();
            }
            using (var statement = connection.Prepare(
             "UPDATE " + USER_SETTING_TABLE + " SET Username = ?, RenewDate = ?, Budget = ?, Catagories = ?"))
            {
                statement.Bind(1, "用户");
                statement.Bind(2, 1);
                statement.Bind(3, 1);
                statement.Bind(4, "其他");
                statement.Step();
            }

        }
    }
}
