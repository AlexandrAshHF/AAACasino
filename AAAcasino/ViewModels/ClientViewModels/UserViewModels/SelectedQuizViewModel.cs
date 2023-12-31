﻿using AAAcasino.Infrastructure.Commands;
using AAAcasino.Models;
using AAAcasino.Services.Database;
using AAAcasino.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AAAcasino.ViewModels.ClientViewModels.UserViewModels
{
    internal class SelectedQuizViewModel : ViewModel, IPageViewModel
    {
        #region IPage
        public string Title => $"Викторина";
        public MainWindowViewModel MainViewModel { get; set; } = null!;
        public void SetAnyModel(object? model)
        {
            try
            {
                QuizModel = model != null ? (QuizModel)model : throw new ArgumentNullException(nameof(model));
                CurrentQuestion = QuizModel.QuizNodes[0];
                _questionNumber = 0;
            }
            catch
            {
                MainViewModel.SelectedPageViewModel = MainViewModel.ClientPageViewModels[(int)NumberClientPage.USER_PAGE];
            }
        }
        #endregion
        #region Fields
        private QuizModel? _quizModel = null;
        public QuizModel? QuizModel
        {
            get => _quizModel;
            set => Set(ref _quizModel, value);
        }

        private QuizNode? _currentQuestion = null;
        public QuizNode? CurrentQuestion
        {
            get => _currentQuestion;
            set => Set(ref _currentQuestion, value);
        }

        private int _questionNumber = 0;
        #endregion
        #region Commands
        public ICommand SendAnsweCommand { get; set; }
        private void OnSendAnswerCommand(object param)
        {
            double totalReward = 0;
            if (_questionNumber < QuizModel.QuizNodes.Count - 1)
            {
                QuizModel.QuizNodes[_questionNumber] = CurrentQuestion;
                _questionNumber++;
                CurrentQuestion = QuizModel.QuizNodes[_questionNumber];
            }
            else
            {
                bool fullRight = true;
                foreach (var item in QuizModel.QuizNodes)
                {
                    foreach (var answ in item.Answers)
                    {
                        fullRight = true;
                        if (answ.UserAnswer != answ.IsCorrect)
                        {
                            fullRight = false;
                            break;
                        }
                        if (fullRight)
                            totalReward += QuizModel.Reward / QuizModel.QuizNodes.Count;
                    }
                }

                Task.Run(() =>
                {
                    MainViewModel.User.Balance += totalReward;
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        db.userModels.Update(MainViewModel.User);
                        db.SaveChanges();
                    }
                });

                //Сброс всех результатов и очистка модели
                Task.Run(() =>
                {
                    foreach (var qn in QuizModel.QuizNodes)
                    {
                        foreach (var uAnsw in qn.Answers)
                        {
                            uAnsw.UserAnswer = false;
                        }
                    }
                    QuizModel = null;
                    CurrentQuestion = null;
                    _questionNumber = 0;
                });

                MainViewModel.SelectedPageViewModel = MainViewModel.ClientPageViewModels[(int)NumberClientPage.USER_PAGE];
            }
        }
        private bool CanSendAnswerCommand(object param) => true;
        #endregion
        public SelectedQuizViewModel()
        {
            SendAnsweCommand = new LamdaCommand(OnSendAnswerCommand, CanSendAnswerCommand);
        }
    }
}