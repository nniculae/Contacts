﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace Contacts.Contracts.Services;

public interface INavigationService
{
    event NavigatedEventHandler Navigated;

    bool CanGoBack { get; }
    Frame? Frame { get; set; }
    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);
    bool NavigateToWithAnimation(string pageKey, object? parameter = null, bool clearNavigation = false, NavigationTransitionInfo? navigationTransitionInfo = null);
    bool GoBack();

}
