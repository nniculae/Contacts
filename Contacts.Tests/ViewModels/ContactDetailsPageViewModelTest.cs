using Contacts.Contracts.Services;
using Contacts.Core.Contracts.Services;
using Contacts.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Moq;

namespace Contacts.Tests.ViewModels;


[TestClass]
public class ContactDetailsPageViewModelTest
{
    [TestMethod]
    public void ShouldBeCreated()
    {
        var contactServiceMock = new Mock<IContactService>().Object;
        var labelServiceMock = new Mock<ILabelService>().Object;
        var navigationServiceMock = new Mock<INavigationService>().Object;
        var dialogServiceMock = new Mock<IDialogService>().Object;

        var vm = new ContactDetailPageViewModel(contactServiceMock, labelServiceMock, navigationServiceMock, dialogServiceMock);

        Assert.IsNotNull(vm);
    }
}
