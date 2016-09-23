using CoreGraphics;
using Foundation;
using UIKit;

namespace iOSSlidingMenu
{
    public class TaskCollectionViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("APLCollectionViewCell");

        [Export("initWithFrame:")]
        public TaskCollectionViewCell(CGRect frame) : base (frame)
		{
            BackgroundColor = UIColor.Cyan;
            TitleLabel = new UILabel(Bounds);

            ContentView.AddSubview(TitleLabel);
        }

        public UILabel TitleLabel { get; private set; }
    }
}