using System;
using System.Threading.Tasks;
using MvvmQuickCross;
using CloudAuction.Shared.ViewModels;
using CloudAuction.Shared.ViewModels.Design;

namespace CloudAuction.Shared
{
    public sealed class CloudAuctionApplication : ApplicationBase
    {
        private ICloudAuctionNavigator _navigator;

        public CloudAuctionApplication(ICloudAuctionNavigator navigator, object currentNavigationContext = null, TaskScheduler uiTaskScheduler = null)
            : base(currentNavigationContext, uiTaskScheduler)
        {
            // Services that have a platform-specific implementation, such as the navigator,
            // are instantiated in a platform-specific project and passed to this application 
            // as a cross-platform interface.
            _navigator = navigator;

            // TODO: Create instances for all services that have a cross-platform implementation
        }

        new public static CloudAuctionApplication Instance { get { return (CloudAuctionApplication)ApplicationBase.Instance; } }

        public MainViewModel MainViewModel { get; private set; }
        public AuctionViewModel AuctionViewModel { get; private set; }
        public ProductsViewModel ProductsViewModel { get; private set; }
        public ProductViewModel ProductViewModel { get; private set; }
        public OrderViewModel OrderViewModel { get; private set; }
        public OrderResultViewModel OrderResultViewModel { get; private set; }

        /* TODO: For each viewmodel, add a public property with a private setter like this:
        public _VIEWNAME_ViewModel _VIEWNAME_ViewModel { get; private set; }
         * Note that the New-View and New-ViewModel commands add the above code automatically (see http://github.com/MacawNL/MvvmQuickCross#new-viewmodel). */

        public void ContinueToMain(MainViewModel.SubView? subView = null, bool skipNavigation = false)
        {
            if (MainViewModel == null) MainViewModel = new MainViewModel();
            if (subView.HasValue)
            {
                switch (subView.Value)
                {
                    case MainViewModel.SubView.Auction: if (AuctionViewModel == null) AuctionViewModel = new AuctionViewModelDesign(); break;
                    case MainViewModel.SubView.Products: if (ProductsViewModel == null) ProductsViewModel = new ProductsViewModelDesign(); break;
                }
            }
            if (!skipNavigation) RunOnUIThread(() => _navigator.NavigateToMainView(CurrentNavigationContext, subView));
        }

        public void ContinueToProduct(ProductViewModel product, bool skipNavigation = false)
        {
            if (ProductViewModel == null) ProductViewModel = new ProductViewModel();
            ProductViewModel.Initialize(product); // Note that we update the existing ProductViewModel instance instead of replacing it with a new instance, because views may exist that are bound to the existing instance.
            if (!skipNavigation) RunOnUIThread(() => _navigator.NavigateToProductView(CurrentNavigationContext));
        }

        public void ContinueToOrder(Bid bid, bool skipNavigation = false)
        {
            if (OrderViewModel == null) OrderViewModel = new OrderViewModelDesign();
            OrderViewModel.Initialize(bid);
            if (!skipNavigation) RunOnUIThread(() => _navigator.NavigateToOrderView(CurrentNavigationContext));
        }

        public void ContinueToOrderResult(Bid bid, bool skipNavigation = false)
        {
            if (OrderResultViewModel == null) OrderResultViewModel = new OrderResultViewModel();
            OrderResultViewModel.Initialize(bid);
            if (!skipNavigation) RunOnUIThread(() => _navigator.NavigateToOrderResultView(CurrentNavigationContext));
        }

        /* TODO: For each view, add a method (with any parameters needed) to initialize its viewmodel
         * and then navigate to the view using the navigator, like this:

        public void ContinueTo_VIEWNAME_(bool skipNavigation = false)
        {
            if (_VIEWNAME_ViewModel == null) _VIEWNAME_ViewModel = new _VIEWNAME_ViewModelDesign(); // TODO: Once _VIEWNAME_ViewModel has runtime data, instantiate that instead of _VIEWNAME_ViewModelDesign
            // Any actions to update the viewmodel go here
            if (!skipNavigation) RunOnUIThread(() => _navigator.NavigateTo_VIEWNAME_View(CurrentNavigationContext));
        }
         * The skipNavigation parameter is needed in cases where the OS has already navigated to the view for you;
         * in that case you only need to initialize the viewmodel.
         * Note that the New-View command adds the above code automatically (see http://github.com/MacawNL/MvvmQuickCross#new-view). */
    }
}
