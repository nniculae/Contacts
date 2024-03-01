using Contacts.ViewModels;

namespace Contacts.Tests.ViewModels;
[TestClass]
public class HelpViewModelTest
{

    [TestMethod]
    public void ShouldBeCreated()
    {
        var helpViewModel = new HelpViewModel();

        Assert.IsNotNull(helpViewModel);
    }
}
