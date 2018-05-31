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

        internal static void InitializeUserSetting()
        {
            using (var statement = connection.Prepare("INSERT INTO " + USER_SETTING_TABLE + " VALUES(?,?,?,?);"))
            {
                statement.Bind(1, "未命名");
                statement.Bind(2, 1);
                statement.Bind(3, 1);
                statement.Bind(4, "未定义");
                statement.Step();
            }
           
        }

        private static SQLiteConnection connection = new SQLiteConnection(DB_NAME);

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

        public static void DeleteEntry(string id)
        {
            using (var statement = connection.Prepare(SQL_DELETE))
            {
                statement.Bind(1, id);
                statement.Step();
            }
        }

        public static UserSetting GetUserSetting()
        {
            UserSetting userSetting = new UserSetting();
            using (var statement = connection.Prepare(
                "SELECT * FROM " + $"{USER_SETTING_TABLE };"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    var catagories = (string)statement["Catagories"];
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

        public static void UpdateCatagory(string catagorys)
        {
            using (var statement = connection.Prepare(
              "UPDATE " + USER_SETTING_TABLE + " SET Catagories = ?"))
            {
                statement.Bind(1, catagorys);
                statement.Step();
            }
        }

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
                statement.Bind(1, "未命名");
                statement.Bind(2, 1);
                statement.Bind(3, 1);
                statement.Bind(4, "未定义");
                statement.Step();
            }

        }
    }
}
