using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Services;
using Contacts.Validators;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels;

public partial class ContactDetailPageViewModel(
    IContactService contactsService,
    INavigationService navigation) :
    ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private Contact _contact = null!;
    [ObservableProperty]
    private bool _isInEdit = false;
    [ObservableProperty]
    private bool _isNewContact = false;

    [ObservableProperty]
    public bool _areSelectedLabelsDifferent = false;
    public ObservableCollection<Label> AllLabels = [];
    public ObservableCollection<Label> ThisContactLabels = [];
    public List<Label> SelectedLabels = [];
    private Crud crud = Crud.Read;
    public ContactValidator ContactValidator { get; set; } = null!;

    [RelayCommand]
    public async Task GetAllLabelsAsync()
    {
        var labels = await contactsService.GetLabelsAsync();
        AllLabels.Clear();
        foreach (var label in labels)
        {
            AllLabels.Add(label);
        }

    }
    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is int id)
        {
            Contact = await contactsService.FindByIdAsync(id);
            IsNewContact = false;
            IsInEdit = false;
        }
        else
        {
            Contact = new Contact()
            {
                FirstName = string.Empty,
                Address = new Address()
            };

            IsNewContact = true;
            IsInEdit = true;
        }
        ThisContactLabels.Clear();
        foreach (var label in Contact.Labels)
        {
            ThisContactLabels.Add(label);
        }
        
        ContactValidator = new ContactValidator(Contact!);

    }

    public void OnNavigatedFrom()
    {
        var message = new ContactChangedMessage(Contact!.Id, CrudStringMessage.FormatMessage(Contact.Name, crud));
        ((WeakReferenceMessenger)Messenger).Send(message);
    }
    public void GoBack()
    {
        navigation.GoBack();
    }
    public void StartEdit()
    {
        IsInEdit = true;
    }
    [RelayCommand]
    public async Task UpsertAsync()
    {

        await contactsService.Upsert(Contact);

        if (IsNewContact)
        {
            crud = Crud.Created;
        }
        else
        {
            crud = Crud.Updated;
        }

        GoBack();
    }
    [RelayCommand]
    public async Task RemoveAsync()
    {
        await contactsService.RemoveAsync(Contact!);
        crud = Crud.Deleted;
        GoBack();
    }

    [RelayCommand]
    public async Task CreateLabelAsync()
    {
        var element = (FrameworkElement)App.MainWindow.Content;
        var labelName = await element.InputStringDialogAsync(
                "Create new label",
                "");
        if (string.IsNullOrEmpty(labelName))
            return;

        Label newLabel = new() { Name = labelName };
        Contact.Labels.Add(newLabel);

        Contact = await contactsService.Upsert(Contact!);
        // maye add direct aan Labels
        await GetAllLabelsAsync();
        ThisContactLabels.Add(newLabel);

        //if (IsNewContact)
        //{
        //    crud = Crud.Created;
        //}
        //else
        //{
        //    crud = Crud.Updated;
        //}
    }
    [RelayCommand]
    public async Task UpdateContactLabelsAsync()
    {
        Contact.Labels.Clear();
        foreach (var label in SelectedLabels)
        {
            Contact.Labels.Add(label);
        }
        await contactsService.UpsertLabels(Contact);
        GoBack();
    }
}
