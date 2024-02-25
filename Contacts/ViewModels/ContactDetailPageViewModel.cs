using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Validators;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels;

public partial class ContactDetailPageViewModel(
    IContactService contactService,
    ILabelService labelService,
    INavigationService navigation) : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private Contact _contact = null!;
    [ObservableProperty]
    private bool _isInEdit = false;
    [ObservableProperty]
    private bool _isNewContact = false;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCreateLabelButtonVisible))]
    public bool _isApplyChangesButtonVisible = false;
    public bool IsCreateLabelButtonVisible => !IsApplyChangesButtonVisible;

    // The labels in the flyout
    public ObservableCollection<Label> AllLabels = [];
    // The labels associated temporarily with current Contact
    public ObservableCollection<Label> ContactLabels = [];
    private Crud crud = Crud.Read;
    public ContactValidator ContactValidator { get; set; } = null!;

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is int id)
        {
            Contact = await contactService.FindByIdAsync(id);
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

        await SetContactLabels();
        await SetAllLabelsAsync();

        ContactValidator = new ContactValidator(Contact!);
    }

    private async Task SetContactLabels()
    {
        ContactLabels.Clear();
        var contactLabels = await labelService.GetLabelsByContactId(Contact.Id);

        foreach (var label in contactLabels)
        {
            ContactLabels.Add(label);
        }
    }

    [RelayCommand]
    public async Task SetAllLabelsAsync()
    {
        var labels = await labelService.GetAllLabelsAsync();
        AllLabels.Clear();
        foreach (var label in labels)
        {
            AllLabels.Add(label);
        }
    }

    public void OnNavigatedFrom()
    {
        contactService.Dispose();
        var message = new ContactChangedMessage(Contact!.Id, CrudStringMessage.FormatMessage(Contact.Name, crud));
        Messenger.Send(message);
    }

    public void GoBack()
    {
        navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, "ListContactsInit");
    }

    public void StartEdit()
    {
        IsInEdit = true;
    }

    [RelayCommand]
    public async Task RemoveAsync()
    {
        await contactService.RemoveAsync(Contact!);
        crud = Crud.Deleted;
        GoBack();
    }

    public Label CreateLabelInMemory(string labelName)
    {
        Label newLabel = new() { Name = labelName };
        ContactLabels.Add(newLabel);
        AllLabels.Add(newLabel);

        return newLabel;
    }
    [RelayCommand]
    public async Task UpdateContactLabelsAsync()
    {
        var labelsToRemove = Contact.Labels.Where(label => !ContactLabels.Contains(label, EqualityComparer<Label>.Create(
        (left, right) => left!.Id.CompareTo(right!.Id) == 0))).ToList();

        var labelsToAdd = ContactLabels.Where(label => !Contact.Labels.Contains(label, EqualityComparer<Label>.Create(
            (left, right) => left!.Id.CompareTo(right!.Id) == 0))).ToList();


        foreach (var label in labelsToRemove)
        {
            Contact.Labels.Remove(label);
        }

        Contact.Labels.AddRange(labelsToAdd);

        if (IsNewContact)
        {
            await contactService.AddAsync(Contact);
            crud = Crud.Created;
        }
        else
        {
            await contactService.SaveAsync();
            crud = Crud.Updated;
        }

        GoBack();
    }
}
