using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;

namespace iOSSlidingMenu
{



    public static class Datas
    {
        public static List<Task> _newDataTasks = new List<Task>();

        public static List<DeletedTask> _deletedDataTasks = new List<DeletedTask>();

        static string dbPath;

        // CREATING THE NEW DATABASE :  CHECKING IF DATABASE NOT EXISTS CREATE NEW ONE
        public static bool CreateDatabase()
        {
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database_test.db3");

            Console.WriteLine("DB PATH "+dbPath);

            bool exists = File.Exists(dbPath);

            if (!exists)
            {
                try
                {
                    var db = new SQLiteConnection(dbPath);
                    db.CreateTable<Task>();
                    db.CreateTable<DeletedTask>();
                    new UIKit.UIAlertView("Database Alert", "Database created succesfully", null, "OK", null).Show();
                    db.Close();
                }
                catch (SQLiteException e)
                {
                    new UIKit.UIAlertView("Database Alert", "Database creation failed"+e.Message, null, "OK", null).Show();
                }
 
            }

            return exists;

        }


        internal static int InsertNewData(Task createdTask, DateTime now)
        {
           
            var db = new SQLiteConnection(dbPath);
            int i = db.Insert(createdTask);
            if (i == 0)
            {
                new UIKit.UIAlertView("Task alert", "New Task not Saved " + i, null, "OK", null).Show();
            }
            else
            {
                new UIKit.UIAlertView("Task alert", "New Task Saved " + i, null, "OK", null).Show();
                GetAllData();
            }

            db.Close();

            return i;
        }

        public static bool  GetAllData()
        {
            bool b = false;

            var db = new SQLiteConnection(dbPath);

            var task_getData = db.Table<Task>();

            if (task_getData != null)
            {
                Console.WriteLine("ENTRED DATA FOUND");

                _newDataTasks.Clear();

                foreach (var s in task_getData)
                {

                    Task t = new Task();
                    t.Id = s.Id;
                    t.Title = s.Title;
                    t.Description = s.Description;
                    t.Color = s.Color;
                    t.Date_Time = s.Date_Time;

                    _newDataTasks.Add(t);

                    t = null;

                }

                b = true;
            }
            else
            {
                Console.WriteLine("NO DATA FOUND IN DATABASE");
                b = false;
            }

            db.Close();

            return b;

        }

      

        internal static int UpdateExistingTask(Task createdTask, DateTime now)
        {
            int i = -999;

            var db = new SQLiteConnection(dbPath);

            var existingItem = db.Get<Task>(createdTask.Id);

            if (existingItem != null)
            {
                Console.WriteLine("DATA FTECHED : " + existingItem.Id + " , " + existingItem.Title + " , " + existingItem.Description + " , " + existingItem.Color);

                existingItem.Id = createdTask.Id;
                existingItem.Title = createdTask.Title;
                existingItem.Description = createdTask.Description;
                existingItem.Color = createdTask.Color;
                existingItem.Date_Time = createdTask.Date_Time;

                i = db.Update(existingItem);

            }

            db.Close();

            return i;
        }

        internal static int DeleteRow(int v)
        {
            int i = -999;

            var db = new SQLiteConnection(dbPath);

            Task existingItem = db.Get<Task>(v);

            if (existingItem != null)
            {
              i =  db.Delete<Task>(v);

                // HERE DELETING FROM CREATE TABLE AND INSERTING TO TRASH TABLE
               DeletedTask dt = new DeletedTask();

                dt.Id = existingItem.Id;
                dt.Title = existingItem.Title;
                dt.Description = existingItem.Description;
                dt.Color = existingItem.Color;
                dt.Date_Time = existingItem.Date_Time;

                Insert_DeletedData(dt);
             }
            db.Close();
            return i;
        }


        static void Print_NewListOfGetData()
        {
            Console.WriteLine("Total Data in NewLIst Of Task :" + _newDataTasks.Count);
            foreach (Task t in _newDataTasks)
            {
                Console.WriteLine(t.Id + "                        " + t.Title + "                         " + t.Description + "                          " + t.Color + "               " + t.Date_Time.ToString());
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static int Insert_DeletedData(DeletedTask deletedTask)
        {

            var db = new SQLiteConnection(dbPath);
            int i = db.Insert(deletedTask);
            if (i == 0)
            {
                new UIKit.UIAlertView("Trash alert FAILED", "Trash Data insertion failed " + i, null, "OK", null).Show();
            }
            else
            {
                new UIKit.UIAlertView("Trash alert SUCCESS", "Trash Data insertion success " + i, null, "OK", null).Show();
               
            }

            db.Close();

            return i;
        }

        public static bool GetallDeletedTasks()
        {
            bool b = false;

            var db = new SQLiteConnection(dbPath);

            var task_delData = db.Table<DeletedTask>();

            if (task_delData != null)
            {
                Console.WriteLine("DELETED DATA FOUND");

                _deletedDataTasks.Clear();

                foreach (var s in task_delData)
                {

                    DeletedTask t = new DeletedTask();
                    t.Id = s.Id;
                    t.Title = s.Title;
                    t.Description = s.Description;
                    t.Color = s.Color;
                    t.Date_Time = s.Date_Time;

                    _deletedDataTasks.Add(t);

                    t = null;

                }

                b = true;
            }
            else
            {
                Console.WriteLine("NO DELETED DATA FOUND IN DATABASE");
                b = false;
            }

            db.Close();

            return b;

        }

        internal static int DeleteRow_DeletedTaskPermanent(int id)
        {
            int i = -999;

            var db = new SQLiteConnection(dbPath);

            DeletedTask existingItem = db.Get<DeletedTask>(id);

            if (existingItem != null)
            {
                i = db.Delete<DeletedTask>(id);
            }

            Console.WriteLine("..................   DETE TRASH RESULT"+i);

            db.Close();
            return i;
        }

        internal static int RestoreDeletedTask(int id)
        {
            int i = -999;

            var db = new SQLiteConnection(dbPath);

            DeletedTask existingItem = db.Get<DeletedTask>(id);

            if (existingItem != null)
            {
                i = db.Delete<DeletedTask>(id);

                Task restoreTask = new Task();

                // restoreTask.Id = existingItem.Id;
                restoreTask.Title = existingItem.Title;
                restoreTask.Description = existingItem.Description;
                restoreTask.Color = existingItem.Color;
                restoreTask.Date_Time = existingItem.Date_Time;

                 i = Datas.InsertNewData(restoreTask, DateTime.Now);
            }

            return i;
        }

    }


    public class Task
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public DateTime Date_Time { get; set; }

        public override string ToString()
        {
            return string.Format("{0:MMM dd yy}", Date_Time);
        }
    }



    public class DeletedTask
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public DateTime Date_Time { get; set; }

        public override string ToString()
        {
            return string.Format("{0:MMM dd yy}", Date_Time);
        }
    }



}
