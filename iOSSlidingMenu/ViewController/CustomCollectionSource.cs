using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using ObjCRuntime;
using UIKit;
using System.Linq;

namespace iOSSlidingMenu
{

    class CustomCollectionSource : UICollectionViewSource
    {
        public List<string> rows { get; set; }



        private List<Task> _newDataTasks;
        private UINavigationController navigationController;
        private UIStoryboard storyboard;
        private ViewController viewController;


        public List<Task> _filteredTasks = new List<Task>();

        public CustomCollectionSource(List<Task> _newDataTasks, UINavigationController navigationController, UIStoryboard storyboard, ViewController viewController)
        {
            if (this._newDataTasks != null)
                this._newDataTasks.Clear();

            this._newDataTasks = _newDataTasks;

            this._filteredTasks = _newDataTasks;


            this.navigationController = navigationController;
            this.storyboard = storyboard;
            this.viewController = viewController;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {

            //   return _newDataTasks.Count;
            return _filteredTasks.Count();
        }

        public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CustomCollectionViewCell)collectionView.CellForItem(indexPath);
            cell.mainLabel.Alpha = 0.5f;
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CustomCollectionViewCell)collectionView.CellForItem(indexPath);
            cell.mainLabel.Alpha =1.0f;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CustomCollectionViewCell)collectionView.DequeueReusableCell(CustomCollectionViewCell.CellId, indexPath);

            //  cell.UpdateCell(_newDataTasks[indexPath.Row], viewController);

            cell.UpdateCell(_filteredTasks[indexPath.Row], viewController);

            return cell;
        }

       


        public override bool ShouldShowMenu(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override bool CanPerformAction(UICollectionView collectionView, Selector action, NSIndexPath indexPath, NSObject sender)
        {
            return true;
        }

       

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            this.navigationController.SetNavigationBarHidden(true, false);
            CreateNoteViewController createnoteController = this.storyboard.InstantiateViewController("CreateNoteViewController") as CreateNoteViewController;

            createnoteController.update_new = -100;
            createnoteController.createdTask = _filteredTasks[indexPath.Row];

            this.navigationController.PushViewController(createnoteController, true);

            collectionView.DeselectItem(indexPath, true);

        }

        internal void PerformSearch(string searchtext)
        {
            searchtext = searchtext.ToLower();
            this._filteredTasks = _newDataTasks.Where(x => x.Title.ToLower().Contains(searchtext)).ToList();
        }
    }
}
