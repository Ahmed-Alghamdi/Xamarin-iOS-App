using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using System.Collections.Generic;
using MessageUI;
using Social;

namespace iOSSlidingMenu
{
	public partial class ViewController : UIViewController
	{
		MenuTableSourceClass objMenuTableSource;
		nfloat flViewShiftUpY;
		nfloat flViewBringDownY;


        public ViewController ( IntPtr handle ) : base ( handle )
		{
            
        }


        public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            Console.WriteLine(" public override void ViewDidLoad ()");

            FnInitializeView();
			tableViewLeftMenu.Hidden = true;
			FnBindMenu ();


            bool isDatabase = Datas.CreateDatabase();
            if (isDatabase)
            {
                bool isAnyData = Datas.GetAllData();
                if (isAnyData)
                {
                   
                    // checked what to show :: TABLE VIEW / GRID VIEW
                    PopulateTableView();
                    PopulateGridView();

                    collectionView.Hidden = true;
                    _table.Hidden = true;

                   
                }


            }


            SearchFunction();

        }

        UISearchBar collectionSearchBar;


        UISearchBar sampleSearchBar;
        private void SearchFunction()
        {
            sampleSearchBar = new UISearchBar(new CoreGraphics.CGRect(2, 60, this.View.Frame.Width - 2, 40));
            sampleSearchBar.SearchBarStyle = UISearchBarStyle.Prominent;
            sampleSearchBar.ShowsCancelButton = false;
            sampleSearchBar.Placeholder = "Enter text to search";
            sampleSearchBar.TextChanged += (sender, e) =>
            {
                Search();
            };
            if(_table != null)
                    _table.TableHeaderView = sampleSearchBar;

            //Deleagte class source
            //sampleSearchBar.Delegate = new SearchDelegate();
            //this.View.AddSubview(sampleSearchBar);

            if (collectionView != null)
            {
                collectionSearchBar = new UISearchBar(new CoreGraphics.CGRect(2, 0, this.View.Frame.Width - 2, 40));
                collectionSearchBar.SearchBarStyle = UISearchBarStyle.Prominent;
                collectionSearchBar.ShowsCancelButton = false;
                collectionView.AddSubview(collectionSearchBar);
                collectionSearchBar.Placeholder = "Enter text to search";
               // collectionSearchBar.Delegate = new CollectionSearchDelegate(this);
             
               
                collectionSearchBar.TextChanged += (sender, e) =>
                {
                    collectionSearchBar.ResignFirstResponder();
                    SearchInGrid();
                };
            }
        }

        public  void SearchInGrid()
        {
            collectionViewSource.PerformSearch(collectionSearchBar.Text);
            collectionView.ReloadData();
        }

        private void Search()
        {
            _tableSource.PerformSearch(sampleSearchBar.Text);
            _table.ReloadData();
        }

        public void ChamngesAfterGridCellDelete()
        {
            if (ConstantsClass.toShowGrid == true)
            {
                collectionView.Hidden = false;
                _table.Hidden = true;
                layoutChangeButton = UIButton.FromType(UIButtonType.System);
                layoutChangeButton.SetBackgroundImage(UIImage.FromBundle("Images/Grid.png"), UIControlState.Normal);
            }
            else
            {
                collectionView.Hidden = true;
                _table.Hidden = false;
                layoutChangeButton = UIButton.FromType(UIButtonType.System);
                layoutChangeButton.SetBackgroundImage(UIImage.FromBundle("Images/List.png"), UIControlState.Normal);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            Console.WriteLine("public override void ViewDidAppear(bool animated)");
            //collectionView.Hidden = false;
            //_table.Hidden = false;

            if (_table != null && collectionView != null)
            { 
                if (ConstantsClass.toShowGrid == false)
                {
                    collectionView.Hidden = true;
                    _table.Hidden = false;
                    layoutChangeButton = UIButton.FromType(UIButtonType.System);
                    layoutChangeButton.SetBackgroundImage(UIImage.FromFile("Images/Grid.png"), UIControlState.Normal);
                }
                if (ConstantsClass.toShowGrid == true)
                {
                    collectionView.Hidden = false;
                    _table.Hidden = true;

                    layoutChangeButton = UIButton.FromType(UIButtonType.System);
                    layoutChangeButton.SetBackgroundImage(UIImage.FromFile("Images/List.png"), UIControlState.Normal);
                }
            }
         
        }

        void FnInitializeView()
		{

			CGRect rectBounds= UIScreen.MainScreen.Bounds;

			flViewBringDownY = rectBounds.Height - 30f; //complete collapse viewContainer
			flViewShiftUpY = rectBounds.Height - 120f;//200 is the heght of viewContainer

			var recognizerRight = new UISwipeGestureRecognizer (FnSwipeLeftToRight);
			recognizerRight.Direction = UISwipeGestureRecognizerDirection.Right;
			View.AddGestureRecognizer ( recognizerRight );

			var recognizerLeft = new UISwipeGestureRecognizer (FnSwipeRightToLeft);
			recognizerLeft.Direction = UISwipeGestureRecognizerDirection.Left;
			View.AddGestureRecognizer ( recognizerLeft );

            collectionView.BackgroundColor = UIColor.White;


            btnIcon.SetBackgroundImage ( UIImage.FromBundle ( "Images/menu_icon" ) , UIControlState.Normal );

			btnIcon.TouchUpInside += delegate(object sender , EventArgs e )
			{
				FnPerformTableTransition();
			};

            newAddNote.TouchUpInside += delegate (object sender, EventArgs e)
            {
                OpenCreatenoteScreen();
            };

            layoutChangeButton.TouchUpInside += delegate (object sender, EventArgs e)
            {
                LayoutChangeButton();
            };


        }

		void FnSwipeLeftToRight()
		{
			if ( tableViewLeftMenu.Hidden )
				FnPerformTableTransition ();
			 
		}

		void FnSwipeRightToLeft()
		{
			if ( !tableViewLeftMenu.Hidden )
				FnPerformTableTransition ();
		}
		void FnPerformTableTransition()
		{
			tableViewLeftMenu.Hidden = !tableViewLeftMenu.Hidden;
			var transition = new CATransition ();
			transition.Duration = 0.25f;
			transition.Type = CAAnimation.TransitionPush;
			if ( tableViewLeftMenu.Hidden )
			{
				transition.TimingFunction = CAMediaTimingFunction.FromName ( new Foundation.NSString ("easeOut") );
				transition.Subtype = CAAnimation.TransitionFromRight;
			}
			else
			{
				transition.TimingFunction = CAMediaTimingFunction.FromName ( new Foundation.NSString ("easeIn") );
				transition.Subtype = CAAnimation.TransitionFromLeft;
			}
			tableViewLeftMenu.Layer.AddAnimation ( transition , null );
		}
		void FnBindMenu()
		{
			if(objMenuTableSource!=null)
			{
				objMenuTableSource.MenuSelected -= FnMenuSelected;
				objMenuTableSource = null;
			}
			objMenuTableSource = new MenuTableSourceClass ();
			objMenuTableSource.MenuSelected += FnMenuSelected;
			tableViewLeftMenu.Source = objMenuTableSource; 
		}
		void FnMenuSelected(string strMenuSeleted)
		{
            switch (strMenuSeleted)
            {
                case "Home":
                   // txtActionBarText.Text = "Note";
                   
                    break;
                case "Note":
                    // txtActionBarText.Text = "New Note";

                    this.NavigationController.SetNavigationBarHidden(true, false);
                    CreateNoteViewController createnoteController = this.Storyboard.InstantiateViewController("CreateNoteViewController") as CreateNoteViewController;

                    createnoteController.update_new = -99;

                    this.NavigationController.PushViewController(createnoteController, true);


                    break;
                case "Trash":
                   // txtActionBarText.Text = "Deleted Note";

                    this.NavigationController.SetNavigationBarHidden(true, false);
                    Trash_Controler trashController = this.Storyboard.InstantiateViewController("Trash_Controler") as Trash_Controler;
                    this.NavigationController.PushViewController(trashController, true);

                    break;
                case "Theme":
                   // txtActionBarText.Text = "Change Theme";
                    break;
            }
         
			FnSwipeRightToLeft ();
		}
		void FnAnimateView(nfloat frameY,UIView view)
		{
			UIView.Animate ( 0.2f , 0.1f , UIViewAnimationOptions.CurveEaseIn , delegate
			{
				var frame = View.Frame; 
				frame.Y = frameY;
				view.Frame = frame;
			} , null );
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}


        void OpenCreatenoteScreen()
        {
            this.NavigationController.SetNavigationBarHidden(true, false);
            CreateNoteViewController createnoteController = this.Storyboard.InstantiateViewController("CreateNoteViewController") as CreateNoteViewController;

            createnoteController.update_new = -99;

            this.NavigationController.PushViewController(createnoteController, true);

        }


        /* ***********************    DATAS SHOW OF NEW TASKS :  LIST & GRID VIEW ********************************** */

       TableSource _tableSource;
       public  UITableView _table;

        public void CreateDataSourceForTable()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(
                   new Action(() =>
                   {
                      
                       Console.WriteLine("....................   INVIKING ON MAIN THREAD...................................");
                       _tableSource = new TableSource(Datas._newDataTasks, this.NavigationController, this.Storyboard, this);
                       _table.Source = _tableSource;
                       _table.ReloadData();
                   }));

            
        }

        public void PopulateTableView()
        {
            Console.WriteLine("**************************    AFTER DELETING FROM GRID : POPULATING THE TABLE VIEW");

            Console.WriteLine(" POPULATING TABLE VIEW :: " + Datas._newDataTasks.Count);

            if (Datas._newDataTasks.Count >= 0)
            {
                if(_table != null)
                {
                    _table.Dispose();
                    _table.Source = null;
                    _table = null;
                }

                _table = new UITableView();
                _table.Frame = new CoreGraphics.CGRect(0, 40, View.Bounds.Width, View.Bounds.Height - 40);
                CreateDataSourceForTable();
                
             
            }




            View.AddSubview(_table);
            View.SendSubviewToBack(_table);
            _table.Hidden = true;

        }

        CustomCollectionSource collectionViewSource;
        public void PopulateGridView()
        {
          
            collectionView.BackgroundColor = UIColor.White;
            collectionView.RegisterClassForCell(typeof(CustomCollectionViewCell), CustomCollectionViewCell.CellId);

            collectionViewSource = new CustomCollectionSource(Datas._newDataTasks, this.NavigationController, this.Storyboard, this);

            if (Datas._newDataTasks.Count >= 0)
            {
                collectionView.Source = collectionViewSource;
            }

            collectionView.Hidden = true;
        }

        


        /* *****************************   LAYOUT CHANGE LIST VIEW -> GRID VIEW -> LIST VIEW ***************************** */


        void LayoutChangeButton()
        {
            layoutChangeButton.SetBackgroundImage(UIImage.FromBundle("Images/Add.png"), UIControlState.Normal);

            bool isAnyData = Datas.GetAllData();
            if (isAnyData)
            {

                // checked what to show :: TABLE VIEW / GRID VIEW
                //  PopulateTableView();
                CreateDataSourceForTable();
                PopulateGridView();

                collectionView.Hidden = true;
                _table.Hidden = true;
            }

            ConstantsClass.toShowGrid = !ConstantsClass.toShowGrid;


            if (ConstantsClass.toShowGrid == true)
            {
               
                collectionView.Hidden = false;
                _table.Hidden = true;
                layoutChangeButton = UIButton.FromType(UIButtonType.System);
               // layoutChangeButton.SetBackgroundImage(UIImage.FromBundle("Images/Grid.png"), UIControlState.Normal);
            }
            else
            {
               
                collectionView.Hidden = true;
                _table.Hidden = false;
                layoutChangeButton = UIButton.FromType(UIButtonType.System);
               // layoutChangeButton.SetBackgroundImage(UIImage.FromBundle("Images/List.png"), UIControlState.Normal);
            }

        }






        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            var transferData = segue.DestinationViewController as CreateNoteViewController;
            if (transferData != null)
            {
                //transferData._currentColorSelected = "G";
            }
        }





        /* **************************************** SOCIAL SHARE METHODS *************************************** */



        void ShareOnEmail()
        {

            if (MFMailComposeViewController.CanSendMail)
            {
                // do mail operations here
                MFMailComposeViewController mailController;
                mailController = new MFMailComposeViewController();


                mailController.SetToRecipients(new string[] { "john@doe.com" });
                mailController.SetSubject("mail test");
                mailController.SetMessageBody("this is a test", false);

                mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                    Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };
            }
        }

       public void Twitter()
        {
            Console.WriteLine("............. CALLING FROM TABLE SOURCE");

            var slComposer = SLComposeViewController.FromService(SLServiceType.Twitter);

            if (SLComposeViewController.IsAvailable(SLServiceKind.Twitter))
            {
                Console.WriteLine("....................................................");

                // Set initial message
                slComposer.SetInitialText(ConstantsClass.sharingText);
               // slComposer.AddImage(UIImage.FromFile("Icon.png"));
                slComposer.CompletionHandler += (result) => {
                    InvokeOnMainThread(() => {
                        DismissViewController(true, null);
                        Console.WriteLine("Results: {0}", result);
                    });
                };

                // Display controller
                PresentViewController(slComposer, true, null);
            }
        }


       public void Facebook()
        {
             var  _facebookComposer = SLComposeViewController.FromService(SLServiceType.Facebook);

            if (SLComposeViewController.IsAvailable(SLServiceKind.Facebook))
            {
                _facebookComposer.SetInitialText(ConstantsClass.sharingText);
                //  FacebookComposer.AddImage(UIImage.FromFile("Icon.png"));
                _facebookComposer.CompletionHandler += (result) => {
                    InvokeOnMainThread(() => {
                        DismissViewController(true, null);
                        Console.WriteLine("Results: {0}", result);
                    });
                };

                // Display controller
                PresentViewController(_facebookComposer, true, null);
            }
        }


        //void SendSMS()
        //{
        //    var smsTo = NSUrl.FromString("sms:18015551234");

        //    UIApplication.SharedApplication.OpenUrl(smsTo);

        //    var imessageTo = NSUrl.FromString("sms:john@doe.com");
        //    UIApplication.SharedApplication.OpenUrl(imessageTo);

        //    var smsTo = NSUrl.FromString("sms:18015551234");
        //    if (UIApplication.SharedApplication.CanOpenUrl(smsTo))
        //    {
        //        UIApplication.SharedApplication.OpenUrl(smsTo);
        //    }
        //    else
        //    {
        //        // warn the user, or hide the button...
        //    }
        //}

    }


    }

