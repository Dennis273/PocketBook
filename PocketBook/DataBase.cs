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
        private static String SQL_UPDATE_USER = "UPDATE " + USER_SETTING_TABLE+ " SET Username = ?, Budget = ?, RenewDate = ?, Catagory = ?";
        private static String TABLE_NAME = "DataEntryTable";
        private static String SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (Id TEXT PRIMARY KEY, Day INTEGER, Month INTEGER, Year INTEGER, Money FLOAT, Catagory TEXT);";
        private static String SQL_INSERT = "INSERT INTO " + TABLE_NAME + " VALUES(?,?,?,?,?,?);";
        private static String SQL_UPDATE = "UPDATE " + TABLE_NAME + " SET Day = ?, Month = ?, Year = ?, Money = ?, Catagory = ?  WHERE Id = ?";
        private static String SQL_DELETE = "DELETE FROM " + TABLE_NAME + " WHERE Id = ?";

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
                    entries.Add(new DataEntry(money, spendDate, catagory, id));
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

        public static List<string> GetCatagories()
        {
            List<string> temp = new List<string>();
            using (var statement = connection.Prepare(
                "SELECT Catagories FROM " + $"{USER_SETTING_TABLE };"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    var catagories = (string)statement["Catagories"];
                    var strs = catagories.Split(";", StringSplitOptions.RemoveEmptyEntries);
                    temp = new List<string>(strs);
                }
            }
            
            return temp;
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

        public static void UpdateDataEntryCatagory(string oldCatagory, string newCatagory)
        {
            using (var statement = connection.Prepare(
             "UPDATE " + TABLE_NAME + " SET Catagory = ? WHERE Catagory = ?"))
            {
                statement.Bind(1, newCatagory);
                statement.Bind(2, oldCatagory);
                statement.Step();
            }
        }
    }
}
