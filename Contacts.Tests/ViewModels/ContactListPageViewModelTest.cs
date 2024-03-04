using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.Messaging;
using Contacts.Contracts.Services;
using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Contacts.Extensions;
using Contacts.Tests.Helpers;
using Contacts.ViewModels;
using Moq;
using System.Reflection;


namespace Contacts.Tests.ViewModels;

[TestClass]
public class ContactListPageViewModelTest
{
    private readonly Mock<IContactService> contactServiceMock = new();
    private readonly Mock<INavigationService> navigationServiceMock = new();

    [TestMethod]
    public async Task OnNavigatedToShouldAddContactsToDataSourceBasedOnLabelId()
    {
        List<Contact> contacts =
        [
            new() { Id = 1, FirstName = "Bob"},
            new() { Id = 2, FirstName = "Mimi"},
        ];

        contactServiceMock.Setup(cs => cs.GetContactsByLabelIdAsync(1)).ReturnsAsync(contacts);
        var vm = CreateViewModel();

        await vm.OnNavigatedTo(1);
        var ogcAsEnumerable = (IEnumerable<ObservableGroup<string, Contact>>)vm.ContactsDataSource;

        CollectionAssert.AreEquivalent(contacts, ogcAsEnumerable.SelectMany(g => g).ToList());

        contactServiceMock.Verify(cs => cs.GetContactsByLabelIdAsync(1), Times.Once);
    }

    [TestMethod]
    public async Task OnNavigatedToShouldRegisterMessengers()
    {
        List<Contact> contacts =
        [
            new() { Id = 1, FirstName = "Bob"},

        ];

        contactServiceMock.Setup(cs => cs.GetContactsByLabelIdAsync(1)).ReturnsAsync(contacts);
        var vm = CreateViewModel();

        await vm.OnNavigatedTo(1);

        Assert.IsTrue(vm.IsActive);
    }

    [TestMethod]
    public async Task OnNavigatedToShouldAddAllContactsToDataSource()
    {
        List<Contact> contacts =
        [
            new() { Id = 1, FirstName = "Bob"},
            new() { Id = 2, FirstName = "Mimi"},
        ];

        contactServiceMock.Setup(cs => cs.GetContactsAsync()).ReturnsAsync(contacts);
        var vm = CreateViewModel();

        await vm.OnNavigatedTo(string.Empty);
        var ogcAsEnumerable = (IEnumerable<ObservableGroup<string, Contact>>)vm.ContactsDataSource;

        CollectionAssert.AreEquivalent(contacts, ogcAsEnumerable.SelectMany(g => g).ToList());

        contactServiceMock.Verify(cs => cs.GetContactsAsync(), Times.Once);
    }

    [TestMethod]
    public void OnNavigatedToShouldsShouldDisposeContactServiceAndUnregisterAllMessages()
    {
        var vm = CreateViewModel();

        vm.OnNavigatedFrom();

        contactServiceMock.Verify(cs => cs.Dispose(), Times.Once);
        Assert.IsFalse(vm.IsActive);
    }

    [TestMethod]
    public void NavigateToCreateShouldGoToDetailsPagePassingNullAsParameter()
    {
        var vm = CreateViewModel();

        vm.NavigateToCreate();

        var detailsViewModel = typeof(ContactDetailPageViewModel).FullName!;
        navigationServiceMock.Verify(ns => ns.NavigateTo(detailsViewModel, null, false), Times.Once);
    }

    [TestMethod]
    public void OnSelectedLabelChangedShoulNavigateToContactListPageBasedOnLabelId()
    {
        Label label = new() { Id = 1, Name = "Friend" };
        var vm = CreateViewModel();

        vm.Call("OnSelectedLabelChanged", label);

        var listViewModel = typeof(ContactListPageViewModel).FullName!;
        navigationServiceMock.Verify(ns => ns.NavigateTo(listViewModel, 1, false), Times.Once);
    }

    [TestMethod]
    public void OnSearchTextChangedShouldFilterContacts()
    {
        List<Contact> contacts =
       [
            new() { Id = 1, FirstName = "Bob"},
            new() { Id = 2, FirstName = "Boris"},
            new() { Id = 2, FirstName = "Jack"},
        ];

        var vm = CreateViewModel();
        // TODO: write some extension methods to work with private fields
        var prop = vm.GetType().GetField("_contacts", BindingFlags.NonPublic | BindingFlags.Instance);
        prop!.SetValue(vm, contacts);

        vm.Call("OnSearchTextChanged", "Bo");
        Assert.AreEqual(2, vm.ContactsDataSource.CountItems());
    }

    [TestMethod]
    public void ReceiveShouldReceiveAMessage()
    {
        Contact jack = new() { Id = 3, FirstName = "Jack" };

        List<Contact> contacts =
        [
            new() { Id = 1, FirstName = "Bob"},
            new() { Id = 2, FirstName = "Boris"},
            jack
        ];

        var vm = CreateViewModel();

        var groupings = contacts.GroupBy(c => c.Name[0].ToString().ToUpperInvariant()).OrderBy(g => g.Key);
        foreach (var item in groupings)
        {
            vm.ContactsDataSource.AddGroup(item);
        }

        vm.IsActive = true;

        WeakReferenceMessenger.Default.Send(new ContactChangedMessage(3, "Contact created"));

        Assert.AreEqual("Contact created", vm.InfoBarMessage);
        Assert.AreEqual(jack, vm.SelectedItem);
        Assert.IsTrue(vm.IsBackFromDetails);

    }

    private ContactListPageViewModel CreateViewModel()
    {
        return new ContactListPageViewModel(contactServiceMock.Object, navigationServiceMock.Object);
    }
}
