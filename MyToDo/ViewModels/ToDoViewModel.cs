using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        public ToDoViewModel(IToDoService service, IContainerProvider provider):base(provider)
        {
            this.service = service;
            ToDoDtos = new ObservableCollection<ToDoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
            dialogHost = provider.Resolve<IDialogHostService>();
        }

        private async void Delete(ToDoDto obj)
        {
            try
            {
                var dialogResult =  await dialogHost.Qusetion("温馨提示", $"确认删除待办事项：{obj.Title}?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = ToDoDtos.FirstOrDefault(x => x.Id.Equals(obj.Id));
                    if (model != null)
                    {
                        ToDoDtos.Remove(model);
                    }
                }
            }
            finally { UpdateLoading(false); }

        }

        private void Execute(string obj)
        {
            switch(obj)
            {
                case "新增": Add();break;
                case "查询": GetDataAsync();break;
                case "保存": Save();break;
            }
        }
        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        private bool isRightDrawerOpen;
        private ToDoDto currentDto;
        /// <summary>
        /// 编辑选中对象/新增时对象
        /// </summary>
        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value;RaisePropertyChanged(); }
        }
        private string search;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 添加待办
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void Add()
        {
            CurrentDto = new ToDoDto();
            IsRightDrawerOpen = true;
        }
        private async void Save()
        {
            if(string.IsNullOrWhiteSpace(CurrentDto.Title)||
                    string.IsNullOrWhiteSpace (CurrentDto.Content))
                    return;
            try
            {
                UpdateLoading(true);
                if (CurrentDto.Id > 0)
                {
                    var updateResult = await service.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                            todo.Status = CurrentDto.Status;
                            IsRightDrawerOpen = false;
                        }
                    }
                }
                else
                {
                    var addResult = await service.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }
        private async void Selected(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var todoResult = await service.GetSingleAsync(obj.Id);

                if (todoResult.Status)
                {
                    CurrentDto = todoResult.Result;
                    IsRightDrawerOpen = true;
                }
            }catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }
        public DelegateCommand<string> ExecuteCommand { get;private set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }
        private ObservableCollection<ToDoDto> toDoDtos;
        private readonly IToDoService service;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        async void GetDataAsync()
        {
            UpdateLoading(true);

            int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;

            var todoResult = await service.GetAllAsync(new Shared.Parameters.ToDoParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = Status,
            });
            if (todoResult.Status)
            {
                ToDoDtos.Clear();
                foreach(var item in ((IPagedList<ToDoDto>)todoResult.Result).Items)
                {
                    ToDoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if(navigationContext.Parameters.ContainsKey("Value"))
                SelectedIndex = navigationContext.Parameters.GetValue<int>("Value");
            else
                SelectedIndex = 0;
            GetDataAsync();
        }
    }
}
