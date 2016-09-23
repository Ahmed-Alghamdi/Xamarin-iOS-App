using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace iOSSlidingMenu
{
    public class TaskCollectionViewController : UICollectionViewController
    {
        private UICollectionViewLayout layout;

        public const int MAX_COUNT = 20;

        public TaskCollectionViewController(UICollectionViewLayout layout) : base (layout)
		{
            CollectionView.RegisterClassForCell(typeof(TaskCollectionViewCell), TaskCollectionViewCell.Key);
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return MAX_COUNT;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(TaskCollectionViewCell.Key, indexPath) as TaskCollectionViewCell;

            cell.TitleLabel.Text = "GRID VIEW";

            return cell;
        }

     //   public override UICollectionViewTransitionLayout TransitionLayout(UICollectionView collectionView, UICollectionViewLayout fromLayout, UICollectionViewLayout toLayout)
     //   {
     //       return new APLTransitionLayout(fromLayout, toLayout);
     //   }

        public virtual UICollectionViewController NextViewControllerAtPoint(CGPoint p)
        {
            return null;
        }
    }
}