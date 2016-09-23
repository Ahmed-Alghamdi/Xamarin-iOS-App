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
    [Register ("Trash_Controler")]
    partial class Trash_Controler
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tableViewLeftMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel txtActionBarText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnIcon != null) {
                btnIcon.Dispose ();
                btnIcon = null;
            }

            if (tableViewLeftMenu != null) {
                tableViewLeftMenu.Dispose ();
                tableViewLeftMenu = null;
            }

            if (txtActionBarText != null) {
                txtActionBarText.Dispose ();
                txtActionBarText = null;
            }
        }
    }
}