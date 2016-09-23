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
    [Register ("MenuTableViewCell")]
    partial class MenuTableViewCell
    {
        [Outlet]
        UIKit.UIImageView imgViewMenuIcon { get; set; }


        [Outlet]
        UIKit.UILabel lblMenuText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgViewMenuIcon != null) {
                imgViewMenuIcon.Dispose ();
                imgViewMenuIcon = null;
            }

            if (lblMenuText != null) {
                lblMenuText.Dispose ();
                lblMenuText = null;
            }
        }
    }
}