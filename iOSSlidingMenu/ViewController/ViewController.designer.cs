// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iOSSlidingMenu
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton btnBottom { get; set; }


        [Outlet]
        UIKit.UIButton btnIcon { get; set; }


        [Outlet]
        UIKit.UIImageView imgViewActionIcon { get; set; }


        [Outlet]
        UIKit.UITableView tableViewLeftMenu { get; set; }


        [Outlet]
        UIKit.UILabel txtActionBarText { get; set; }


        [Outlet]
        UIKit.UILabel txtDescription { get; set; }


        [Outlet]
        UIKit.UIView viewDecriptionContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView baseLayout { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView collectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton layoutChangeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton newAddNote { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISearchDisplayController searchDisplayController { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISearchDisplayController searchDisplayInput { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView uiview_top { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (baseLayout != null) {
                baseLayout.Dispose ();
                baseLayout = null;
            }

            if (btnIcon != null) {
                btnIcon.Dispose ();
                btnIcon = null;
            }

            if (collectionView != null) {
                collectionView.Dispose ();
                collectionView = null;
            }

            if (layoutChangeButton != null) {
                layoutChangeButton.Dispose ();
                layoutChangeButton = null;
            }

            if (newAddNote != null) {
                newAddNote.Dispose ();
                newAddNote = null;
            }

            if (searchDisplayController != null) {
                searchDisplayController.Dispose ();
                searchDisplayController = null;
            }

            if (searchDisplayInput != null) {
                searchDisplayInput.Dispose ();
                searchDisplayInput = null;
            }

            if (tableViewLeftMenu != null) {
                tableViewLeftMenu.Dispose ();
                tableViewLeftMenu = null;
            }

            if (uiview_top != null) {
                uiview_top.Dispose ();
                uiview_top = null;
            }
        }
    }
}