using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Contacts.Contracts.Services;
using Contacts.Core.Contracts.Services;
using Contacts.Core.Dto;
using Contacts.Helpers;
using Contacts.Services;
using Contacts.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels;

public partial class ShellViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<string>>, IRecipient<ValueChangedMessage<bool>>
{
    [ObservableProperty]
    private bool isBackEnabled;
    [ObservableProperty]
    private object? selected;
    [ObservableProperty]
    private bool _isInfoBarOpen;
    [ObservableProperty]
    private string? _infoBarTitle;
    [ObservableProperty]
    private string? _infoBarMessage;
    [ObservableProperty]
    private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Success;

    [ObservableProperty]
    private bool _showOverlay;
    [ObservableProperty]
    private double _opacityOverlay = 0.0;

    private readonly ILabelService labelService;

    public INavigationService NavigationService { get; }
    public INavigationViewService NavigationViewService { get; }

    public ObservableCollection<NavigationViewItemBase> MenuItems { get; set; } = [];


    public ShellViewModel(INavigationService navigationService,
        INavigationViewService navigationViewService, ILabelService _labelService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        labelService = _labelService;
        IsActive = true;
    }

    private async void OnNavigated(object sender, NavigationEventArgs e)
    {
        
        var labelsFromDb = await labelService.GetLabelsWithContactsCountAsync();
        List<Label> labelsNotAssociated = await labelService.GetNotAssociatedLabels();
        SetMenuItems(labelsFromDb, labelsNotAssociated);

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

    private void SetMenuItems(List<LabelsWithContactsCountDto> labelsFromDb, List<Label> notAssociatedLabels)
    {
        MenuItems.Clear();
        var contactsMenuItem = new NavigationViewItem()
        {
            Content = "All Contacts",
            Tag = "ListContactsInit",
            Icon = new SymbolIcon((Symbol)0xE779)
        };
        NavigationHelper.SetNavigateTo(contactsMenuItem, typeof(ContactListPageViewModel).FullName!);

        MenuItems.Add(contactsMenuItem);

        // Header labels
        var labelHeader = new NavigationViewItemHeader() { Content = "Labels" };
        MenuItems.Add(labelHeader);


        foreach (var labelWithContactCount in labelsFromDb)
        {

            var item = new NavigationViewItem
            {
                Content = labelWithContactCount.Label.Name,
                Tag = labelWithContactCount.Label.Id,
                Icon = new SymbolIcon((Symbol)0xE8EC), // Do not extract local variable beacause an expetion will be raised
                InfoBadge = new InfoBadge() { Value = labelWithContactCount.ContactsCount }
            };


            NavigationHelper.SetNavigateTo(item, typeof(ContactListPageViewModel).FullName!);

            MenuFlyout mf = new MenuFlyout();
            mf.Items.Add(new MenuFlyoutItem() { Text = "Change label name", Icon = new SymbolIcon((Symbol)0xE70F), Command = UpdateLabelCommand, CommandParameter = labelWithContactCount.Label });
            mf.Items.Add(new MenuFlyoutItem() { Text = "Remove label", Icon = new SymbolIcon((Symbol)0xE74D), Command = RemoveLabelCommand, CommandParameter = labelWithContactCount.Label });
            item.ContextFlyout = mf;

            MenuItems.Add(item);
        }

        // Separator
        MenuItems.Add(new NavigationViewItemSeparator());

        // Empty contact labels
        foreach (var label in notAssociatedLabels)
        {
            var item = new NavigationViewItem
            {
                Content = label.Name,
                Tag = label.Id,
                Icon = new SymbolIcon((Symbol)0xE8EC), // Do not extract local variable beacause an expetion will be raised
                InfoBadge = new InfoBadge() { Value = 0 },
                Opacity = 0.7
            };

            MenuFlyout mf = new MenuFlyout();

            mf.Items.Add(new MenuFlyoutItem() { Text = "Change label name", Icon = new SymbolIcon((Symbol)0xE70F), Command = UpdateLabelCommand, CommandParameter = label });
            mf.Items.Add(new MenuFlyoutItem() { Text = "Remove label", Icon = new SymbolIcon((Symbol)0xE74D), Command = RemoveLabelCommand, CommandParameter = label });
            item.ContextFlyout = mf;

            MenuItems.Add(item);
        }
    }

    [RelayCommand]
    public async Task UpdateLabelAsync(object labelObj)
    {
        var label = (Label)labelObj;
        var element = (FrameworkElement)App.MainWindow.Content;
        var labelName = await element.InputStringDialogAsync(
                "Change label name",
                label.Name);
        if (string.IsNullOrEmpty(labelName))
            return;
        label.Name = labelName;
        await labelService.UpsertAsync(label);

        var message = $"The label '{labelName}' was updated successfully";
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<string>(message));

        NavigationService.NavigateTo(typeof(ContactListPageViewModel).FullName!, "ListContactsInit");
    }

    [RelayCommand]
    public async Task RemoveLabelAsync(object labelObj)
    {
        var element = (FrameworkElement)App.MainWindow.Content;
        bool? ok = await element.ConfirmationDialogAsync("Are you sure?");
        if (ok == null) return;

        var label = (Label)labelObj;
        await labelService.RemoveAsync(label);


        var message = $"The label '{label.Name}' was removed successfully";
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<string>(message));

        NavigationService.NavigateTo(typeof(ContactListPageViewModel).FullName!, "ListContactsInit");
    }

    private async Task DisplayInfoMsgAsync(InfoBarSeverity infoBarSeverity, string? title, string? message,
       int delayMs = 6000)
    {
        if (title is null && message is null)
        {
            IsInfoBarOpen = false;
            return;
        }

        InfoBarSeverity = infoBarSeverity;
        InfoBarTitle = title;
        InfoBarMessage = message;
        IsInfoBarOpen = true;
        await Task.Delay(delayMs);
        IsInfoBarOpen = false;
    }

    public async void Receive(ValueChangedMessage<string> message)
    {
        await DisplayInfoMsgAsync(InfoBarSeverity, "Info", message.Value);
    }

    public void Receive(ValueChangedMessage<bool> message)
    {
        ShowOverlay = message.Value;
    }
}
