﻿using AAAcasino.Infrastructure.Commands;
using AAAcasino.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace AAAcasino.ViewModels.ClientViewModels.UserViewModels
{
    internal class ControlUserViewModel : ViewModel, IPageViewModel
    {
        #region IPageVM
        public string Title => _userSelectedPage == null ? "UserPage" : _userSelectedPage.Title;
        public MainWindowViewModel MainViewModel { get; set; }
        public void SetAnyModel(object? model)
        {
            _userSelectedPage = MainViewModel.ClientPageViewModels[(int)NumberClientPage.QUIZZES_PAGE];
            _userSelectedPage.MainViewModel = MainViewModel;
            _userSelectedPage.SetAnyModel(null);
        }
        #endregion
        #region Content component
        private IPageViewModel? _userSelectedPage = null;
        public IPageViewModel? UserSelectedPage
        {
            get => _userSelectedPage;
            set => Set(ref _userSelectedPage, value);
        }
        private Visibility _visableTopPanel;
        public Visibility VisableTopPanel
        {
            get => _visableTopPanel;
            set => Set(ref _visableTopPanel, value);
        }
        #endregion
        #region Commands
        public ICommand SwitchToProfileCommand { get; set; }
        private void OnSwitchToProfileCommand(object parameter)
        {
            UserSelectedPage = MainViewModel.ClientPageViewModels[(int)NumberClientPage.USER_PROFILE];
            UserSelectedPage.MainViewModel = MainViewModel;
            UserSelectedPage.SetAnyModel(null);
        }
        private bool CanSwitchToProfileCommand(object parameter)
            => UserSelectedPage != MainViewModel.ClientPageViewModels[(int)NumberClientPage.USER_PAGE];
        public ICommand SwitchToQuizzesCommand { get; set; }
        private void OnSwitchToQuizzesCommand(object parameter)
        {
            UserSelectedPage = MainViewModel.ClientPageViewModels[(int)NumberClientPage.QUIZZES_PAGE];
            UserSelectedPage.SetAnyModel(null);
            UserSelectedPage.MainViewModel = MainViewModel;
        }
        private bool CanSwitchToQuizzesCommand(object parameter)
           => UserSelectedPage != MainViewModel.ClientPageViewModels[(int)NumberClientPage.QUIZZES_PAGE];
        public ICommand SwitchToSlotsCommand { get; set; }
        private void OnSwitchToSlotsCommand(object parameter)
        {
            UserSelectedPage = MainViewModel.ClientPageViewModels[(int)NumberClientPage.SLOTS_PAGE];
            UserSelectedPage.MainViewModel = MainViewModel;
        }
        private bool CanSwitchToSlotsCommand(object parameter)
            => UserSelectedPage != MainViewModel.ClientPageViewModels[(int)NumberClientPage.SLOTS_PAGE];
        #endregion
        public ControlUserViewModel()
        {
            VisableTopPanel = Visibility.Visible;
            SwitchToProfileCommand = new LamdaCommand(OnSwitchToProfileCommand, CanSwitchToProfileCommand);
            SwitchToQuizzesCommand = new LamdaCommand(OnSwitchToQuizzesCommand, CanSwitchToQuizzesCommand);
            SwitchToSlotsCommand = new LamdaCommand(OnSwitchToSlotsCommand, CanSwitchToSlotsCommand);
        }
    }
}
