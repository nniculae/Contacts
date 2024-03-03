using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Contacts.Contracts.Services;
using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Contacts.ViewModels;
using Moq;

namespace Contacts.Tests.ViewModels;

[TestClass]
public class ContactDetailsPageViewModelTest
{
    private readonly Mock<IContactService> contactServiceMock = new();
    private readonly Mock<ILabelService> labelServiceMock = new();
    private readonly Mock<INavigationService> navigationServiceMock = new();
    private readonly Mock<IDialogService> dialogServiceMock = new();

    [TestMethod]
    public void ShouldBeCreated()
    {
        var vm = new ContactDetailPageViewModel(
            contactServiceMock.Object,
            labelServiceMock.Object,
            navigationServiceMock.Object,
            dialogServiceMock.Object
            );

        Assert.IsNotNull(vm);
    }

    [TestMethod]
    public async Task OnNavigatedToShouldCreateNewContact()
    {
        labelServiceMock.Setup(ls => ls.GetLabelsByContactIdAsync(0).Result).Returns([]);

        var allLabels = new List<Label>()
        {
            new(){Name ="Client"},
            new(){Name ="Friend"},
        };

        labelServiceMock.Setup(ls => ls.GetAllOtherLabelsAsync(0).Result).Returns(allLabels);

        var vm = new ContactDetailPageViewModel(
            contactServiceMock.Object,
            labelServiceMock.Object,
            navigationServiceMock.Object,
            dialogServiceMock.Object
            );

        await vm.OnNavigatedTo("");

        Assert.IsNotNull(vm.Contact);
        Assert.IsTrue(vm.IsInEdit);
        Assert.IsTrue(vm.IsNewContact);
        Assert.AreEqual(0, vm.ContactLabels.Count);
        Assert.AreEqual(2, vm.AllLabels.Count);
        Assert.IsNotNull(vm.ContactValidator);
    }

    [TestMethod]
    public async Task OnNavigatedToShouldRetrieveOneContactWithHisAddress()
    {
        var labels = new List<Label>()
        {
            new(){Name ="Client"},
            new(){Name ="Friend"},
        };

        labelServiceMock.Setup(ls => ls.GetLabelsByContactIdAsync(1).Result).Returns(labels);
        labelServiceMock.Setup(ls => ls.GetAllOtherLabelsAsync(1).Result).Returns(labels);

        var contact = new Contact()
        {
            Id = 1,
            FirstName = "Jimi",
            Address = new Address() { City = "London" }
        };

        contactServiceMock.Setup(cs => cs.FindByIdAsync(1).Result).Returns(contact);

        var vm = new ContactDetailPageViewModel(
            contactServiceMock.Object,
            labelServiceMock.Object,
            navigationServiceMock.Object,
            dialogServiceMock.Object
            );

        await vm.OnNavigatedTo(1);

        Assert.IsNotNull(vm.Contact);
        Assert.IsFalse(vm.IsInEdit);
        Assert.IsFalse(vm.IsNewContact);
        Assert.AreEqual(0, vm.ContactLabels.Count);
        Assert.AreEqual(2, vm.AllLabels.Count);
    }

    [TestMethod]
    public void OnNavigatedFromShouldDisposeContactServiceAndSendAMessage()
    {
        contactServiceMock.Setup(x => x.Dispose());

        var vm = CreateViewModel();
        vm.Contact = new Contact()
        {
            Id = 1,
            FirstName = "Jimi",
        };

        WeakReferenceMessenger.Default.Register<ContactChangedMessage>(
            this,
            (_, m) => Assert.IsNotNull(m));

        vm.OnNavigatedFrom();
    }

    [TestMethod]
    public async Task ValidateLabelName_WhenLabelNameIsEmptyItReturnsRequired()
    {
        var vm = CreateViewModel();

        var result = await vm.ValidateLabelName(string.Empty);

        Assert.AreEqual<string>("The label name is required", result);
    }

    [TestMethod]
    public async Task ValidateLabelName_WhenLabelNameExistsInDatabaseItReturnsExists()
    {
        labelServiceMock.Setup(l => l.GetLabelByNameAsync("Jimi")).ReturnsAsync(new Label());

        var vm = CreateViewModel();

        var result = await vm.ValidateLabelName("Jimi");

        Assert.AreEqual<string>("The label 'Jimi' already exists", result);
    }

    [TestMethod]
    public async Task ValidateLabelName_WhenLabelNameExistsInContactLabelsItReturnsExists()
    {
        const string labelName = "Jimi";
        labelServiceMock.Setup(l => l.GetLabelByNameAsync(labelName)).ReturnsAsync(() => null);

        var vm = CreateViewModel();

        vm.ContactLabels.Add(new Label() { Name = labelName });

        var result = await vm.ValidateLabelName(labelName);

        Assert.AreEqual<string>($"The label '{labelName}' already exists", result);
    }
    [TestMethod]
    public async Task ValidateLabelName_WhenLabelNameDoNotExistsItReturnsEmptyString()
    {
        const string labelName = "Jimi";
        labelServiceMock.Setup(l => l.GetLabelByNameAsync(labelName)).ReturnsAsync(() => null);

        var vm = CreateViewModel();

        var result = await vm.ValidateLabelName(labelName);

        Assert.AreEqual<string>(string.Empty, result);
    }

    [TestMethod]
    public async Task CreateLabelAsyncReturnsNull()
    {
        dialogServiceMock.Setup(d => d.InputTextDialogAsync(
            It.IsAny<Func<string, Task<string>>>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        )).ReturnsAsync(string.Empty);

        var vm = CreateViewModel();

        var result = await vm.CreateLabelAsync();

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task CreateLabelAsyncReturnsLabelAndSendMessage()
    {
        const string labelName = "Friend";

        dialogServiceMock.Setup(d => d.InputTextDialogAsync(
                        It.IsAny<Func<string, Task<string>>>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()
                    )).ReturnsAsync(labelName);

        var vm = CreateViewModel();

        WeakReferenceMessenger.Default.Register<ValueChangedMessage<string>>(
           this,
           (_, m) => Assert.AreEqual("The label 'Friend' was created successfully", m.Value));

        var label = await vm.CreateLabelAsync();

        Assert.AreEqual(labelName, label!.Name);
    }

    [TestMethod]
    public async Task UpsertContactAsyncCallsAddAsyncWithContact()
    {
        var contact = new Contact() { FirstName = "Bob" };
        contactServiceMock.Setup(cs => cs.AddAsync(contact)).ReturnsAsync(contact);

        ContactDetailPageViewModel vm = CreateViewModel();

        vm.Contact = contact;
        vm.IsNewContact = true;

        await vm.UpsertContactAsync();

        contactServiceMock.Verify(cs => cs.AddAsync(contact), Times.Once);
    }

    [TestMethod]
    public async Task UpsertContactAsyncCallsSaveAsync()
    {

        var contact = new Contact() { FirstName = "Bob" };
        contactServiceMock.Setup(cs => cs.SaveAsync()).ReturnsAsync(1);

        ContactDetailPageViewModel vm = CreateViewModel();
        vm.Contact = contact;
        vm.IsNewContact = false;

        await vm.UpsertContactAsync();

        contactServiceMock.Verify(cs => cs.SaveAsync(), Times.Once);
    }

    private ContactDetailPageViewModel CreateViewModel()
    {
        return new ContactDetailPageViewModel(
          contactServiceMock.Object,
          labelServiceMock.Object,
          navigationServiceMock.Object,
          dialogServiceMock.Object
          );
    }
}
