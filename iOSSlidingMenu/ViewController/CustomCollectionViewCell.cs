using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace iOSSlidingMenu
{
    class CustomCollectionViewCell : UICollectionViewCell
    {

        public UILabel mainLabel, subLabel;
        public static NSString CellId = new NSString("customCollectionCell");


        UILongPressGestureRecognizer _recognizer;

        UISwipeGestureRecognizer _swipeRecognizer;
        UISwipeGestureRecognizerDirection _swipeDir;

        private ViewController _viewController;


        [Export("initWithFrame:")]
        public CustomCollectionViewCell(CGRect frame) : base(frame)
        {
            BackgroundView = new UIView { BackgroundColor = UIColor.Orange };

            ContentView.Layer.BorderColor = UIColor.Black.CGColor;
            ContentView.Layer.BorderWidth = 1.0f;
            ContentView.BackgroundColor = UIColor.White;

            mainLabel = new UILabel();
            subLabel = new UILabel();
            mainLabel.Frame = new CGRect(2,2, ContentView.Bounds.Width-4,20);
            subLabel.Frame = new CGRect(2, 20, ContentView.Bounds.Width-4, 45);

            mainLabel.BackgroundColor = UIColor.LightGray;

            subLabel.LineBreakMode = UILineBreakMode.WordWrap;
            subLabel.Lines = 0;

            subLabel.Font = UIFont.FromName("Helvetica", 12f);


            ContentView.AddSubview(mainLabel);
            ContentView.AddSubview(subLabel);
        }

        public void UpdateCell(string text)
        {
            mainLabel.Text = text;
            subLabel.Text = text;
            mainLabel.Frame = new CGRect(5, 5, ContentView.Bounds.Width-5, 30);
            subLabel.Frame = new CGRect(5, 35, ContentView.Bounds.Width-5,50);
        }

        Task t_selected;

        internal void UpdateCell(Task _newDataTasks, ViewController _viewController)
        {
            this._viewController = _viewController;

            Console.WriteLine("FOR INDEX : " + _newDataTasks.Id + "   Color Value is :" + _newDataTasks.Color);

            mainLabel.Text = _newDataTasks.Title;
            subLabel.Text = _newDataTasks.Description;

            switch (_newDataTasks.Color)
            {
                case "R":
                    ContentView.BackgroundColor = UIColor.Clear.FromHexString("#ff4e91", 1.0f);
                    break;
                case "G":
                    ContentView.BackgroundColor = UIColor.Clear.FromHexString("#59f859", 1.0f);
                    break;
                case "B":
                    ContentView.BackgroundColor = UIColor.Clear.FromHexString("#43D9FF", 1.0f);
                    break;
                case "Y":
                    ContentView.BackgroundColor = UIColor.Clear.FromHexString("#fff95c", 1.0f);
                    break;
                case "W":
                    ContentView.BackgroundColor = UIColor.Clear.FromHexString("#ffffff", 1.0f);
                    break;

            }

            this._recognizer = new UILongPressGestureRecognizer();
            this._recognizer.MinimumPressDuration = 0.5f;
            this._recognizer.AddTarget(this, new ObjCRuntime.Selector("OnEditModeActivated"));
            this.AddGestureRecognizer(_recognizer);


            this._swipeRecognizer = new UISwipeGestureRecognizer();
            this._swipeRecognizer.AddTarget(this, new ObjCRuntime.Selector("OnDeleteModeActiavated"));
            this.AddGestureRecognizer(_swipeRecognizer);
            t_selected = new Task();
            t_selected = _newDataTasks;
            this._swipeDir = new UISwipeGestureRecognizerDirection();
           

        }

        [Export("OnEditModeActivated")]
        public void OnEditModeActivated()
        {
            if (_recognizer.State == UIGestureRecognizerState.Ended)
            {
                Console.WriteLine("I am her efor long press "+this.mainLabel.Text);

                nint buttonClicked = -1;
                UIAlertView alert = new UIAlertView()
                {
                    Title = "Share",
                    Message = mainLabel.Text + "\n" + subLabel.Text
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
                    Console.WriteLine("GRID Facebbook");
                    ConstantsClass.sharingText = mainLabel.Text + "\n" + subLabel.Text;
                    _viewController.Facebook();
                }
                if (buttonClicked == 1)
                {
                    Console.WriteLine("GRID Twitter");
                    ConstantsClass.sharingText = mainLabel.Text + "\n" + subLabel.Text;
                    _viewController.Twitter();
                }
            }
        }


        [Export("OnDeleteModeActiavated")]
        public void OnDeleteModeActiavated()
        {
            Console.WriteLine("......................................................................................................");

            Console.WriteLine("CONFIRM TO DELETE");

            nint buttonClicked = -1;

            UIAlertView alert = new UIAlertView()
            {
                Title = "Confirm to Delete",
                Message = ""
            };
            alert.AddButton("Confirm");
            alert.AddButton("Cancel");
            alert.Clicked += (sender, buttonArgs) => { buttonClicked = buttonArgs.ButtonIndex; };
            alert.Show();

            while (buttonClicked == -1)
            {
                NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow(0.5));
            }

            if (buttonClicked == 0)
            {
                Console.WriteLine("SELECTING TO DELETE :::::::::::::::::::::::::::::::::::::::");
                Console.WriteLine("//  " + t_selected.Id + "         " + t_selected.Description + "   " + t_selected.Title);

                int i = Datas.DeleteRow(t_selected.Id);                             // deleting from database

                if (i != -999)
                {
                    Console.WriteLine("GRID  DATA IS DELETED FROM DATABASE");

                    //GetallAdddat again from dta base and populte Grid View
                    bool isAnyData = Datas.GetAllData();

                    Console.WriteLine("IS ANY DATA :: " + isAnyData);
                    Console.WriteLine(" DATA LENGTH " + Datas._newDataTasks.Count);

                    if (isAnyData)
                    {
                        //_viewController.PopulateTableView();
                        _viewController.PopulateGridView();
                        _viewController.ChamngesAfterGridCellDelete();
                    }

                  //  _viewController.CreateDataSourceForTable();
                }
                else
                {
                    Console.WriteLine("GRID  DATA IS NOT DELETED FROM DATABASE");
                }
            }
        }


    }
}
