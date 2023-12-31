﻿using AAAcasino.ViewModels.Base;
using AAAcasino.ViewModels;
using System.Collections.Generic;
using System.Windows;
using AAAcasino.ViewModels.SlotViewModels;
using AAAcasino.ViewModels.ClientViewModels;
using AAAcasino.ViewModels.ClientViewModels.AdminViewModels;
using AAAcasino.ViewModels.ClientViewModels.UserViewModels;

namespace AAAcasino
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            new MainWindow()
            {
                DataContext = new MainWindowViewModel()
                {
                    ClientPageViewModels = new List<IPageViewModel>
                    {
                        new LogInViewModel(),
                        new SignUpViewModel(),
                        new ControlUserViewModel(),
                        new ProfileViewModel(),
                        new QuizzesViewModel(),
                        new SlotsViewModel(),
                        new SelectedQuizViewModel(),
                        new AdminViewModel(),
                        new CreationQuizViewModel(),
                        new UserProfileViewModel(),
                    },
                    SlotPageViewModels = new List<IPageViewModel>
                    {
                        new SlotMineViewModel(),
                        new RouletteViewModel(),
                        new OneHandViewModel(),
                    },
                }
            }.Show();
        }
    }
}