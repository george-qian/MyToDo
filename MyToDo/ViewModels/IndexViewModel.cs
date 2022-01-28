﻿using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class IndexViewModel:NavigationViewModel
    {
        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        public IndexViewModel(
            IContainerProvider provider,
            IDialogHostService dialog):base(provider)
        {
            this.toDoService = provider.Resolve<IToDoService>();
            this.memoService = provider.Resolve<IMemoService>();
            TaskBars = new ObservableCollection<TaskBar>();
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            CreateTaskBars();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.dialog = dialog;
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(); break;
                case "新增备忘录": AddMemo(); break;
            }
        }
        /// <summary>
        /// 添加待办事项
        /// </summary>
        async void AddToDo()
        {
            var dialogResult = await dialog.ShowDialog("AddToDoView", null);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                if (todo.Id > 0)
                {

                }
                else
                {
                    var addResult = await toDoService.AddAsync(todo);
                    if (addResult.Status)
                        ToDoDtos.Add(addResult.Result);
                }
            }
        }
        /// <summary>
        /// 添加备忘录
        /// </summary>
        async void AddMemo()
        {
            var dialogResult = await dialog.ShowDialog("AddMemoView", null);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                if (memo.Id > 0)
                {

                }
                else
                {
                    var addResult = await memoService.AddAsync(memo);
                    if (addResult.Status)
                        MemoDtos.Add(addResult.Result);
                }
            }
        }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        #region 属性
        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoDtos;
        private readonly IDialogHostService dialog;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        #endregion
        void CreateTaskBars()
        {
            taskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FF0CA0FF", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "9", Color = "#FF1ECA3A", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成率", Content = "100%", Color = "#FF02C6DC", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "19", Color = "#FFFFA000", Target = "" });
        }
    }
}
