using SQLitePCL;
using System;
using System.Collections.Generic;

namespace PocketBook
{
    public static class DataBase
    {
        private static String DB_NAME = "DataEntry.db";
        private static String TABLE_NAME = "DataEntryTable.db";

        private static String SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (Id TEXT PRIMARY KEY, Day TEXT, Month TEXT, Year TEXT, Money DOUBLE, Catagory TEXT);";
        private static String SQL_QUERY = "SELECT * FROM " + TABLE_NAME + " WHERE Id = (?);";
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
        }

        public static void InsertTodoItem(DataEntry dataEntry)
        {
            using (var statement = connection.Prepare(
              SQL_INSERT))
            {
                statement.Bind(1, dataEntry.id);
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
                using (var statement = conn.Prepare(
                    "SELECT * FROM TodoItemTable;"))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {

                        DateTime dateTime = DateTime.Parse((string)statement["Date"]);
                        todoItem = new TodoItem(
                            dateTime,
                            null,
                            (string)statement["Title"],
                            (string)statement["Details"],
                            (double)statement["Size"],
                            (string)statement["ImageName"]);
                        todoItem.id = (string)statement["Id"];
                        todoItem.completed = ((Int64)statement["Completed"] == 1 ? true : false);
                        ItemViewModel.AddTodoItemFromDataBase(todoItem);
                    }
                }
            }
        }

        public static void UpdateItems(TodoItem todoItem)
        {
            using (var conn = new SQLiteConnection(DB_NAME))
            {
                using (var statement = conn.Prepare(
                    SQL_UPDATE))
                {
                    statement.Bind(1, todoItem.imageName);
                    statement.Bind(2, todoItem.date.DateTime.ToString());
                    statement.Bind(3, todoItem.title);
                    statement.Bind(4, todoItem.details);
                    statement.Bind(5, todoItem.size);
                    statement.Bind(7, todoItem.id);
                    statement.Bind(6, todoItem.completed == true ? 1 : 0);
                    statement.Step();

                }
            }
        }

        public static void DeleteItems(string id)
        {
            using (var conn = new SQLiteConnection(DB_NAME))
            {
                using (var statement = conn.Prepare(
                    SQL_DELETE))
                {
                    statement.Bind(1, id);
                    statement.Step();

                }
            }
        }

    }

}
