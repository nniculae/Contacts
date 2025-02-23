﻿using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics.CodeAnalysis;

namespace Contacts.Services;

public class NavigationService : INavigationService
{
    private readonly IPageService _pageService;
    private object? _lastParameterUsed;
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;

    public Frame? Frame
    {
        get
        {
            if (_frame == null)
            {
                _frame = App.MainWindow.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }

        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => Frame != null && Frame.CanGoBack;

    public NavigationService(IPageService pageService)
    {
        _pageService = pageService;
    }

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }

    public bool GoBack()
    {
        if (CanGoBack)
        {
            var vmBeforeNavigation = _frame.GetPageViewModel();
            _frame.GoBack();
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }

            return true;
        }

        return false;
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        return NavigateToWithAnimation(pageKey, parameter, clearNavigation);
    }

    public bool NavigateToWithAnimation(string pageKey, object? parameter = null,
                    bool clearNavigation = false, NavigationTransitionInfo? navigationTransitionInfo = null)
    {
        var pageType = _pageService.GetPageType(pageKey);

        if(_frame == null)
        {
            return false;
        }
       
        _frame.Tag = clearNavigation;
        var vmBeforeNavigation = _frame.GetPageViewModel();
        var navigated = false;

        if (navigationTransitionInfo != null)
        {
            navigated = _frame.Navigate(pageType, parameter, navigationTransitionInfo);
        }
        else
        {
            navigated = _frame.Navigate(pageType, parameter);
        }

        if (navigated)
        {
            _lastParameterUsed = parameter;
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }
        }

        return navigated;
    }
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            if (frame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }

            Navigated?.Invoke(sender, e);
        }
    }
}
