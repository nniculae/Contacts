using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using Contacts.Contracts.Services;
using Contacts.Helpers;
using Contacts.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Contacts.Services;

public class NavigationViewService : INavigationViewService
{
    private readonly INavigationService _navigationService;

    private readonly IPageService _pageService;

    private NavigationView? _navigationView;

    public IList<object>? MenuItems => _navigationView?.MenuItems;

    public object? SettingsItem => _navigationView?.SettingsItem;

    public NavigationViewService(INavigationService navigationService, IPageService pageService)
    {
        _navigationService = navigationService;
        _pageService = pageService;
    }

    [MemberNotNull(nameof(_navigationView))]
    public void Initialize(NavigationView navigationView)
    {
        _navigationView = navigationView;
        _navigationView.BackRequested += OnBackRequested;
        _navigationView.ItemInvoked += OnItemInvoked;
    }

    public void UnregisterEvents()
    {
        if (_navigationView != null)
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }
    }

    public NavigationViewItem? GetSelectedItem(Type pageType, object? parameter = null)
    {

        // https://stackoverflow.com/questions/64806837/uwp-navigationview-menuitems-results-empty-if-populated-programmatically
        // https://github.com/microsoft/TemplateStudio/issues/4616

        if (_navigationView != null)
        {
            var items = _navigationView.MenuItemsSource as ObservableCollection<NavigationViewItemBase>;
            if (items == null) return null;
            
            return GetSelectedItem(items, pageType, parameter) ?? GetSelectedItem(_navigationView.FooterMenuItems, pageType);
        }

        return null;
    }

    //public NavigationViewItem? GetSelectedItem(Type pageType)
    //{

    //    if (_navigationView != null)
    //    {
    //        return GetSelectedItem(_navigationView.MenuItems, pageType) ?? GetSelectedItem(_navigationView.FooterMenuItems, pageType);
    //    }

    //    return null;
    //}

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => _navigationService.GoBack();

    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        }
        else
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
            {
                _navigationService.NavigateTo(pageKey, selectedItem.Tag);
            }
        }
    }

    private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType, object? parameter =null)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(item, pageType, parameter))
            {
                return item;
            }

            //var selectedChild = GetSelectedItem(item.MenuItems, pageType);
            //if (selectedChild != null)
            //{
            //    return selectedChild;
            //}
        }

        return null;
    }

    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType, object? parameter = null)
    {
        
        bool result = false;

        if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
        {
            if (parameter is int p && menuItem.Tag is int t)
            {
                result = (_pageService.GetPageType(pageKey) == sourcePageType) && (p == t);
            }
            else if (parameter is string s &&  menuItem.Tag is string)
            {
                result = (_pageService.GetPageType(pageKey) == sourcePageType) && (s == (string)menuItem.Tag);
            }

        }

        return result;
    }

    //private bool IsMenuItemForPageType2(NavigationViewItem menuItem, Type sourcePageType, object? parameter = null)
    //{
    //    // it always picks the first

    //    if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
    //    {
    //        if (parameter is int p)
    //        {
    //            return (_pageService.GetPageType(pageKey) == sourcePageType) && (p == (int)menuItem.Tag);
    //        }
    //        else if (parameter is string s)
    //        {
    //            return (_pageService.GetPageType(pageKey) == sourcePageType) && (s == (string)menuItem.Tag);
    //        }

    //    }

    //    return false;
    //}
}
