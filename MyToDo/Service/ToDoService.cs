﻿using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        public ToDoService(HttpRestClient client) : base(client, "ToDo")
        {
        }
    }
}
