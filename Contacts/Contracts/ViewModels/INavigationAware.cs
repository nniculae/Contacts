namespace Contacts.Contracts.ViewModels;

public interface INavigationAware
{
    Task OnNavigatedTo(object parameter);

    Task OnNavigatedFrom();
}
