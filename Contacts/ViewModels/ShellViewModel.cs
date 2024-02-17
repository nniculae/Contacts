using CommunityToolkit.Mvvm.ComponentModel;
using Contacts.Contracts.Services;
using Contacts.Core.Contracts.Services;
using Contacts.Core.Services;
using Contacts.Helpers;
using Contacts.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;
    private readonly ILabelService labelService;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    //public ObservableCollection<LabelsWithContactsCountDto> Labels { get; set; } = [];
    public ObservableCollection<NavigationViewItemBase> MenuItems { get; set; } = [];
    public ShellViewModel(INavigationService navigationService,
        INavigationViewService navigationViewService, ILabelService _labelService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        labelService = _labelService;
    }

    private async void OnNavigated(object sender, NavigationEventArgs e)
    {
        var labelsFromDb = await labelService.GetLabelsWithContactsCountAsync();
        SetMenuItems(labelsFromDb);

        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType, e.Parameter);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }

    private void SetMenuItems(List<LabelsWithContactsCountDto> labelsFromDb)
    {
        MenuItems.Clear();
        var contactsMenuItem = new NavigationViewItem()
        {
            Content = "Contacts",
            Tag = "ListContactsInit",
            Icon = new SymbolIcon((Symbol)0xE779)
        };
        NavigationHelper.SetNavigateTo(contactsMenuItem, typeof(ContactListPageViewModel).FullName!);

        MenuItems.Add(contactsMenuItem);
        MenuItems.Add(new NavigationViewItemHeader() { Content = "Labels" });

        foreach (var labelWithContactCount in labelsFromDb)
        {
            if (string.IsNullOrEmpty(labelWithContactCount.Label.Name)) continue;
            var item = new NavigationViewItem
            {
                Content = labelWithContactCount.Label.Name,
                Tag = labelWithContactCount.Label.Id,
                Icon = new SymbolIcon((Symbol)0xE8EC), // Do not extract local variable beacause an expetion will be raised
                InfoBadge = new InfoBadge() { Value = labelWithContactCount.ContactsCount }
            };
            NavigationHelper.SetNavigateTo(item, typeof(ContactListPageViewModel).FullName!);

            MenuItems.Add(item);
        }
    }
}
