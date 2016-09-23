using Foundation;
using System;
using UIKit;
using CoreAnimation;
using CoreGraphics;

namespace iOSSlidingMenu
{
    public partial class Trash_Controler : UIViewController
    {

        MenuTableSourceClass objMenuTableSource;

        UITableView _deletedListTableView;



        public Trash_Controler (IntPtr handle) : base (handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationController.NavigationItem.HidesBackButton = true;
            this.NavigationController.SetNavigationBarHidden(true, false);
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = false;


            FnInitializeView();
            tableViewLeftMenu.Hidden = true;
            FnBindMenu();



            bool b_anyDeletedTask = Datas.GetallDeletedTasks();
            if (b_anyDeletedTask)
            {
                _deletedListTableView = new UITableView();

                Populated_DeletedListView();
            }

        }

       

        void FnInitializeView()
        {

            CGRect rectBounds = UIScreen.MainScreen.Bounds;

            //flViewBringDownY = rectBounds.Height - 30f; //complete collapse viewContainer
            //flViewShiftUpY = rectBounds.Height - 120f;//200 is the heght of viewContainer

            var recognizerRight = new UISwipeGestureRecognizer(FnSwipeLeftToRight);
            recognizerRight.Direction = UISwipeGestureRecognizerDirection.Right;
            View.AddGestureRecognizer(recognizerRight);

            var recognizerLeft = new UISwipeGestureRecognizer(FnSwipeRightToLeft);
            recognizerLeft.Direction = UISwipeGestureRecognizerDirection.Left;
            View.AddGestureRecognizer(recognizerLeft);

            //			viewDecriptionContainer.Hidden = true;

            btnIcon.SetBackgroundImage(UIImage.FromBundle("Images/menu_icon"), UIControlState.Normal);


            btnIcon.TouchUpInside += delegate (object sender, EventArgs e)
            {
                FnPerformTableTransition();
            };

        }



        void FnSwipeLeftToRight()
        {
            if (tableViewLeftMenu.Hidden)
                FnPerformTableTransition();

        }

        void FnSwipeRightToLeft()
        {
            if (!tableViewLeftMenu.Hidden)
                FnPerformTableTransition();
        }
        void FnPerformTableTransition()
        {
            tableViewLeftMenu.Hidden = !tableViewLeftMenu.Hidden;
            var transition = new CATransition();
            transition.Duration = 0.25f;
            transition.Type = CAAnimation.TransitionPush;
            if (tableViewLeftMenu.Hidden)
            {
                transition.TimingFunction = CAMediaTimingFunction.FromName(new Foundation.NSString("easeOut"));
                transition.Subtype = CAAnimation.TransitionFromRight;
            }
            else
            {
                transition.TimingFunction = CAMediaTimingFunction.FromName(new Foundation.NSString("easeIn"));
                transition.Subtype = CAAnimation.TransitionFromLeft;
            }
            tableViewLeftMenu.Layer.AddAnimation(transition, null);
        }
        void FnBindMenu()
        {
            if (objMenuTableSource != null)
            {
                objMenuTableSource.MenuSelected -= FnMenuSelected;
                objMenuTableSource = null;
            }
            objMenuTableSource = new MenuTableSourceClass();
            objMenuTableSource.MenuSelected += FnMenuSelected;
            tableViewLeftMenu.Source = objMenuTableSource;
        }
        void FnMenuSelected(string strMenuSeleted)
        {
            switch (strMenuSeleted)
            {
                case "Home":
                    // txtActionBarText1.Text = "Note";
                    ViewController noteController = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
                    this.NavigationController.PushViewController(noteController, false);
                    break;
                case "Note":
                    // txtActionBarText1.Text = "New Note";
                   
                    CreateNoteViewController createnoteController = this.Storyboard.InstantiateViewController("CreateNoteViewController") as CreateNoteViewController;
                    this.NavigationController.PushViewController(createnoteController, false);
                    break;
                case "Trash":
                    //  txtActionBarText1.Text = "Deleted Note";

                   // this.NavigationController.PopViewController(false);

                    break;
                case "Theme":
                    // txtActionBarText1.Text = "Change Theme";
                    break;
            }
            // txtActionBarText.Text = "I always";// strMenuSeleted;
            FnSwipeRightToLeft();
        }
        void FnAnimateView(nfloat frameY, UIView view)
        {
            UIView.Animate(0.2f, 0.1f, UIViewAnimationOptions.CurveEaseIn, delegate
            {
                var frame = View.Frame;
                frame.Y = frameY;
                view.Frame = frame;
            }, null);
        }




        // POPLATEDLIST VIEW METHOD
        private void Populated_DeletedListView()
        {
            _deletedListTableView.Frame = new CoreGraphics.CGRect(0, 40, View.Bounds.Width, View.Bounds.Height - 40);
            _deletedListTableView.Source = new DeletedTableSorce(Datas._deletedDataTasks, this.NavigationController, this.Storyboard, this);

            _deletedListTableView.SeparatorColor = UIColor.Black;
            _deletedListTableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
         

            _deletedListTableView.ReloadData();

            View.AddSubview(_deletedListTableView);
            View.SendSubviewToBack(_deletedListTableView);

        }


















    }
}