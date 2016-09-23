using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using System.Linq;

namespace iOSSlidingMenu 
{

    class TableSource : UITableViewSource
    {

        UILongPressGestureRecognizer _recognizer;


        string cellIndentifier = "TableCell";


        private List<Task> _newDataTasks;
        private UINavigationController navigationController;
        private UIStoryboard storyboard;
        private ViewController viewController;


        private List<Task> _filteredTasks = new List<Task>();


        public TableSource(List<Task> _newDataTasks, UINavigationController navigationController, UIStoryboard storyboard, ViewController viewController)
        {
            if (this._newDataTasks != null)
                this._newDataTasks.Clear();

            this._newDataTasks = _newDataTasks;

            this._filteredTasks = _newDataTasks;

     
            this.navigationController = navigationController;
            this.storyboard = storyboard;
            this.viewController = viewController;
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            //return tableItems.Count;
            // return _newDataTasks.Count;
            return _filteredTasks.Count();
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
          
            this.navigationController.SetNavigationBarHidden(true, false);
            CreateNoteViewController createnoteController = this.storyboard.InstantiateViewController("CreateNoteViewController") as CreateNoteViewController;

            //  createnoteController.createdTask = _newDataTasks[indexPath.Row];
            createnoteController.createdTask = _filteredTasks[indexPath.Row];
            createnoteController.update_new = -100;

            this.navigationController.PushViewController(createnoteController, true);

            tableView.DeselectRow(indexPath, true);

        }



        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellIndentifier);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIndentifier);

            var _t = _filteredTasks[indexPath.Row];

            cell.TextLabel.Text = _t.Title;
            cell.DetailTextLabel.Text = _t.Description;


            switch (_t.Color)
            {
                case "R":
                        cell.BackgroundColor = UIColor.Clear.FromHexString("#ff4e91", 1.0f);
                      break;
                case "G":
                        cell.BackgroundColor = UIColor.Clear.FromHexString("#59f859", 1.0f);
                      break;
                case "B":
                        cell.BackgroundColor = UIColor.Clear.FromHexString("#43D9FF", 1.0f);
                      break;
                case "Y":
                        cell.BackgroundColor = UIColor.Clear.FromHexString("#fff95c", 1.0f);
                      break;
                case "W":
                    cell.BackgroundColor = UIColor.White;
                    break;
            }

            _recognizer = new UILongPressGestureRecognizer(LongPress);
            _recognizer.MinimumPressDuration = 0.5f;
            cell.Tag = indexPath.Row;
            cell.AddGestureRecognizer(_recognizer);

            return cell;
            
        }

        internal void PerformSearch(string searchtext)
        {
            searchtext = searchtext.ToLower();
            this._filteredTasks = _newDataTasks.Where(x => x.Title.ToLower().Contains(searchtext)).ToList();
        }

        private void LongPress(UILongPressGestureRecognizer rc)
        {
            var c = rc.View;
            if (rc.State == UIGestureRecognizerState.Ended)
            {

                nint buttonClicked = -1;
                UIAlertView alert = new UIAlertView()
                {
                    Title = "Share",
                    Message = _filteredTasks[(int)c.Tag].Title + "\n" + _filteredTasks[(int)c.Tag].Description
                };
                alert.AddButton("Facebook");
                alert.AddButton("Twitter");
                alert.AddButton("Cancel");
                alert.Clicked += (sender, buttonArgs) => { buttonClicked = buttonArgs.ButtonIndex; };
                alert.Show();

                while (buttonClicked == -1)
                {
                    NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow(0.5));
                }

                if (buttonClicked == 0)
                {
                    Console.WriteLine("List Facebbook");
                    //  ConstantsClass.sharingText = _newDataTasks[(int)c.Tag].Title + "\n" + _newDataTasks[(int)c.Tag].Description;
                    ConstantsClass.sharingText = _filteredTasks[(int)c.Tag].Title + "\n" + _filteredTasks[(int)c.Tag].Description;
                    viewController.Facebook();
                }
                if (buttonClicked == 1)
                {
                    Console.WriteLine("List Twitter");
                    //ConstantsClass.sharingText = _newDataTasks[(int)c.Tag].Title + "\n" + _newDataTasks[(int)c.Tag].Description;
                    ConstantsClass.sharingText = _filteredTasks[(int)c.Tag].Title + "\n" + _filteredTasks[(int)c.Tag].Description;
                    viewController.Twitter();
                }
            }
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    Console.WriteLine("I am deleting here on buttonclick     ...........   CommitEditingStyle");

                    int i = Datas.DeleteRow(_filteredTasks[indexPath.Row].Id);           // Deleting from database

                    if (i != -999)
                    {
                        _filteredTasks.RemoveAt(indexPath.Row);                          // From Local Datas
                       
                     //   tableView.BeginUpdates();
                        tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.None);        // Table row Deleting

                        Datas.GetAllData();

                       // tableView.ReloadData();

                       viewController.CreateDataSourceForTable();

                    }
                    else
                    {
                        new UIAlertView("Alert", "ITEM NOT DELETED: " + _filteredTasks[indexPath.Row].Title, null, "OK", null).Show();
                    }

                  
                    break;
                    

                case UITableViewCellEditingStyle.None:
                   Console.WriteLine("Commiting Edit Style");
                    break;
            }
        
        }

        


        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {
            Console.WriteLine("I am deleting here on buttonclick");
            return "Delete ";// +indexPath.Row;
        }

        

    }
}
