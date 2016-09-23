using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace iOSSlidingMenu
{
    internal class DeletedTableSorce : UITableViewSource
    {
        private UINavigationController navigationController;
        private UIStoryboard storyboard;
        private Trash_Controler trash_Controler;
        private List<DeletedTask> _deletedDataTasks;


        UILongPressGestureRecognizer _recognizer;

        string cellIndentifier = "DeleteTableCell";
       
        public DeletedTableSorce(List<DeletedTask> _deletedDataTasks, UINavigationController navigationController, UIStoryboard storyboard, Trash_Controler trash_Controler)
        {
            this._deletedDataTasks = _deletedDataTasks;
            this.navigationController = navigationController;
            this.storyboard = storyboard;
            this.trash_Controler = trash_Controler;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellIndentifier);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIndentifier);


            cell.TextLabel.Text = _deletedDataTasks[indexPath.Row].Title;
            cell.DetailTextLabel.Text = _deletedDataTasks[indexPath.Row].Description;

            switch (_deletedDataTasks[indexPath.Row].Color)
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
            }

            nId = indexPath;
            tempTableView = tableView;

            _recognizer = new UILongPressGestureRecognizer(LongPress);
            _recognizer.MinimumPressDuration = 0.5f;
            cell.Tag = indexPath.Row;
            cell.AddGestureRecognizer(_recognizer);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _deletedDataTasks.Count;
        }


       

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:

                    int i =Datas.RestoreDeletedTask(_deletedDataTasks[indexPath.Row].Id);

                    if (i != -999)
                    {
                        _deletedDataTasks.RemoveAt(indexPath.Row);                          // From Local Datas
                        tempTableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);        // Table row Deleting
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
            return "Restore ";
        }

        NSIndexPath nId;
        private UITableView tempTableView;
        private void LongPress(UILongPressGestureRecognizer rc)
        {
            var c = rc.View;

            if (rc.State == UIGestureRecognizerState.Ended)
            {

                nint buttonClicked = -1;
                UIAlertView alert = new UIAlertView()
                {
                    Title = "Delete",
                    Message = "Are you sure to delete permanently"
                };
                alert.AddButton("Delete");
                alert.AddButton("Cancel");
                alert.Clicked += (sender, buttonArgs) => { buttonClicked = buttonArgs.ButtonIndex; };
                alert.Show();

                while (buttonClicked == -1)
                {
                    NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow(0.5));
                }

                if (buttonClicked == 0)
                {
                    int i = Datas.DeleteRow_DeletedTaskPermanent(_deletedDataTasks[nId.Row].Id);           // Deleting from database

                    if (i != -999)
                    {
                        _deletedDataTasks.RemoveAt(nId.Row);                          // From Local Datas

                        tempTableView.DeleteRows(new NSIndexPath[] { nId }, UITableViewRowAnimation.Fade);        // Table row Deleting

                    }
                    else
                    {
                        new UIAlertView("Alert", "ITEM NOT DELETED: " + _deletedDataTasks[nId.Row].Title, null, "OK", null).Show();
                    }
                }
               
            }
        }

    }
}