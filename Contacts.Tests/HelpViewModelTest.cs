using Contacts.ViewModels;

namespace Contacts.Tests;
[TestClass]
internal class HelpViewModelTest
{

    [TestMethod]
    public void ShouldBeCreated()
    {
        var helpViewModel = new HelpViewModel();

        Assert.IsNotNull(helpViewModel);
    }
}
