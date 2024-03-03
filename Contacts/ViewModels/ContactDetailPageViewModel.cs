using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Validators;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels;

public partial class ContactDetailPageViewModel(
    IContactService contactService,
    ILabelService labelService,
    INavigationService navigation,
    IDialogService dialogService
    ) : ObservableRecipient, INavigationAware
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


    public async Task OnNavigatedTo(object parameter)
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
        var contactLabels = await labelService.GetLabelsByContactIdAsync(Contact.Id);

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

    [RelayCommand]
    public void GoBack()
    {
        navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, "ListContactsInit");
    }

    [RelayCommand]
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

    //TODO: use ObservableValidator??
    public async Task<string> ValidateLabelName(string labelName)
    {
        if (string.IsNullOrEmpty(labelName))
        {
            return "The label name is required";
        }

        var labelFromDb = await labelService.GetLabelByNameAsync(labelName);

        string labelExists = $"The label '{labelName}' already exists";

        if (labelFromDb != null)
        {
            return labelExists;
        }

        if (ContactLabels.Any(l => l.Name == labelName))
        {
            return labelExists;
        }

        return string.Empty;
    }

    public async Task<Label?> CreateLabelAsync()
    {
        var labelName = await dialogService.InputTextDialogAsync(ValidateLabelName, "Create new label", string.Empty);

        if (string.IsNullOrWhiteSpace(labelName))
        {
            return null;
        }

        var newLabel = new Label
        {
            Name = labelName
        };

        ContactLabels.Add(newLabel);
        AllLabels.Add(newLabel);

        var successMessage = $"The label '{newLabel.Name}' was created successfully";
        Messenger.Send(new ValueChangedMessage<string>(successMessage));

        return newLabel;
    }

    [RelayCommand]
    public async Task UpdateContactLabelsAsync()
    {

        if (IsNewContact)
        {
            await contactService.AddAsync(Contact);
            crud = Crud.Created;
        }
        else
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

            await contactService.SaveAsync();
            crud = Crud.Updated;
        }

        GoBack();
    }
}
