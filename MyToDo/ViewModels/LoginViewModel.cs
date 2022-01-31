using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        
        private readonly ILoginService service;
        public LoginViewModel(ILoginService service)
        {
            
            this.service = service;
            CurrentUserDto = new RegisterUserDto();
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login":Login();break;
                case "LoginOut":LoginOut();break;
                case "GoRegister": SelectedIndex = 1;break;
                case "GoLogin": SelectedIndex = 0;break;
                case "Register":Register();break;
            }
        }

        private async void Register()
        {
            if (string.IsNullOrWhiteSpace(CurrentUserDto.Account) ||
                string.IsNullOrWhiteSpace(CurrentUserDto.Password) ||
                    string.IsNullOrWhiteSpace(CurrentUserDto.NewPassword) ||
                    string.IsNullOrWhiteSpace(CurrentUserDto.UserName))
                return;
            if (CurrentUserDto.Password != CurrentUserDto.NewPassword)
                return;
            var registerResult = await service.RegisterAsync(new UserDto()
            {
                UserName = CurrentUserDto.UserName,
                Account = CurrentUserDto.Account,
                Password = StringExtensions.GetMD5(CurrentUserDto.Password)
            });
            if (registerResult.Status)
            {
                SelectedIndex = 0;
            }
        }

        async void Login()
        {
            if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(Password))
                return;
            var loginResult = await service.LoginAsync(new Shared.Dtos.UserDto()
            {
                UserName = String.Empty,
                Account = Account,
                Password = StringExtensions.GetMD5(Password)
            });
            if (loginResult.Status)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
        }

        void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }
        public string Title { get; set; } = "ToDo";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            LoginOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
           
        }
        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; RaisePropertyChanged(); }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(); }
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        private RegisterUserDto currentUserDto;

        public RegisterUserDto CurrentUserDto
        {
            get { return currentUserDto; }
            set { currentUserDto = value; RaisePropertyChanged(); }
        }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
    }
}
