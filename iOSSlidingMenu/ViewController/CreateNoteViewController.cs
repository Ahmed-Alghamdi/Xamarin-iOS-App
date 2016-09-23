using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace iOSSlidingMenu
{
    public partial class CreateNoteViewController : UIViewController
    {

        MenuTableSourceClass objMenuTableSource;

        public Task createdTask { get; internal set; }
        public int update_new;


        public CreateNoteViewController (IntPtr handle) : base (handle)
        {
          
        }


        public CreateNoteViewController Delegate { get; set; }
       


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationController.NavigationItem.HidesBackButton = true;
            this.NavigationController.SetNavigationBarHidden(true, false);
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = false;

           

            FnInitializeView();
            tableViewLeftMenu1.Hidden = true;
            FnBindMenu();

            //Is Data passed For editing
            if (update_new == -99)
            {
                createdTask = new Task();
            }
            Custom_DescLayout();


            ThemeButtonEvents();

            SaveNewTask();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (update_new != -99)
            {
                SetPrevData_edit();
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

            btnIcon1.SetBackgroundImage(UIImage.FromBundle("Images/menu_icon"), UIControlState.Normal);

         
            btnIcon1.TouchUpInside += delegate (object sender, EventArgs e)
            {
                FnPerformTableTransition();
            };

        }

        void FnSwipeLeftToRight()
        {
            if (tableViewLeftMenu1.Hidden)
                FnPerformTableTransition();

        }

        void FnSwipeRightToLeft()
        {
            if (!tableViewLeftMenu1.Hidden)
                FnPerformTableTransition();
        }
        void FnPerformTableTransition()
        {
            tableViewLeftMenu1.Hidden = !tableViewLeftMenu1.Hidden;
            var transition = new CATransition();
            transition.Duration = 0.25f;
            transition.Type = CAAnimation.TransitionPush;
            if (tableViewLeftMenu1.Hidden)
            {
                transition.TimingFunction = CAMediaTimingFunction.FromName(new Foundation.NSString("easeOut"));
                transition.Subtype = CAAnimation.TransitionFromRight;
            }
            else
            {
                transition.TimingFunction = CAMediaTimingFunction.FromName(new Foundation.NSString("easeIn"));
                transition.Subtype = CAAnimation.TransitionFromLeft;
            }
            tableViewLeftMenu1.Layer.AddAnimation(transition, null);
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
            tableViewLeftMenu1.Source = objMenuTableSource;
        }
        void FnMenuSelected(string strMenuSeleted)
        {
            switch (strMenuSeleted)
            {
                case "Home":
                   // txtActionBarText1.Text = "Note";
                    this.NavigationController.PopViewController(false);
                    break;
                case "Note":
                 
                    break;
                case "Trash":
                    //  txtActionBarText1.Text = "Deleted Note";
                    this.NavigationController.SetNavigationBarHidden(true, false);
                    Trash_Controler trashController = this.Storyboard.InstantiateViewController("Trash_Controler") as Trash_Controler;
                    this.NavigationController.PushViewController(trashController, true);

                    break;
                case "Theme":
                   // txtActionBarText1.Text = "Change Theme";
                    break;
            }
          
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

        void Custom_DescLayout()
        {
            createnote_desc.Layer.CornerRadius = 5;
            createnote_desc.Layer.BorderWidth = 1f;
            createnote_desc.Layer.BorderColor = UIColor.Black.CGColor;
           

            createnote_title.Layer.CornerRadius = 5;
            createnote_title.Layer.BorderWidth = 1f;
            createnote_title.Layer.BorderColor = UIColor.Black.CGColor;
        }

        void ThemeButtonEvents()
        {
            theme_btn_blue.TouchUpInside += delegate (object sender, EventArgs e)
            {
                createdTask.Color = "B";
                CustomColor(createdTask.Color);
            };

            theme_btn_red.TouchUpInside += delegate (object sender, EventArgs e)
            {
                createdTask.Color = "R";
                CustomColor(createdTask.Color);
            };

            theme_btn_yellow.TouchUpInside += delegate (object sender, EventArgs e)
            {
                createdTask.Color = "Y";
                CustomColor(createdTask.Color);
            };

            theme_btn_green.TouchUpInside += delegate (object sender, EventArgs e)
            {
                createdTask.Color = "G";
                CustomColor(createdTask.Color);
            };

            themeChangeBtn.TouchUpInside += delegate (object sender, EventArgs e)
            {
                themePanel.Hidden = !themePanel.Hidden;
            };
        }

        void CustomColor(string c)
        {

            switch (c)
            {
                case "B":
                    createnote_title.BackgroundColor = UIColor.Clear.FromHexString("#43D9FF", 1.0f);
                    createnote_desc.BackgroundColor = UIColor.Clear.FromHexString("#43D9FF", 1.0f);
                    baseLayout1.BackgroundColor = UIColor.Clear.FromHexString("#43D9FF", 1.0f);

                    break;
                case "R":
                    createnote_title.BackgroundColor = UIColor.Clear.FromHexString("#ff4e91", 1.0f);
                    createnote_desc.BackgroundColor = UIColor.Clear.FromHexString("#ff4e91", 1.0f);
                    baseLayout1.BackgroundColor = UIColor.Clear.FromHexString("#ff4e91", 1.0f);
                    break;
                case "Y":
                    createnote_title.BackgroundColor = UIColor.Clear.FromHexString("#fff95c", 1.0f);
                    createnote_desc.BackgroundColor = UIColor.Clear.FromHexString("#fff95c", 1.0f);
                    baseLayout1.BackgroundColor = UIColor.Clear.FromHexString("#fff95c", 1.0f);
                    break;
                case "G":
                    createnote_title.BackgroundColor = UIColor.Clear.FromHexString("#59f859", 1.0f);
                    createnote_desc.BackgroundColor = UIColor.Clear.FromHexString("#59f859", 1.0f);
                    baseLayout1.BackgroundColor = UIColor.Clear.FromHexString("#59f859", 1.0f);
                    break;
            }
        }


        void SetPrevData_edit()
        {
           if(createdTask != null)
            {

                createnote_title.Text = createdTask.Title;
                createnote_desc.Text = createdTask.Description;
                lastedited_date.Text = createdTask.Date_Time.ToString();

                CustomColor(createdTask.Color);


            }
        }


        void SaveNewTask()
        {
            save_bew_btn.TouchUpInside += delegate (object sender, EventArgs e)
            {

                if (update_new != -99)
                {
                    createdTask.Title = createnote_title.Text;
                    createdTask.Description = createnote_desc.Text;
                    if (createdTask.Color == null)
                    {
                        createdTask.Color = "W";
                    }
                    createdTask.Date_Time = DateTime.Now;


                    int res = Datas.UpdateExistingTask(createdTask, DateTime.Now);

                 
                    if (res != -999)
                    {
                        this.NavigationController.SetNavigationBarHidden(true, false);
                        ViewController trashController = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
                        this.NavigationController.PushViewController(trashController, true);
                    }


                }
                else
                {
                    // INSERTING DATA INTO DATABASE
                    createdTask.Title = createnote_title.Text;
                    createdTask.Description = createnote_desc.Text;
                    if(createdTask.Color == null)
                    {
                        createdTask.Color = "W";
                    }
                    createdTask.Date_Time = DateTime.Now;
                   
                    int i_stats = Datas.InsertNewData(createdTask, DateTime.Now);

                    if (i_stats == 1)
                    {
                        this.NavigationController.SetNavigationBarHidden(true, false);
                        ViewController trashController = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
                        this.NavigationController.PushViewController(trashController, true);
                    }
                }
              
            };
        }
    }
}