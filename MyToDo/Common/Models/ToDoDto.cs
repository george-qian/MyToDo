﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    /// 待办实体
    /// </summary>
    public class ToDoDto:BaseDto
    {
        private string title;
        private string content;
        private int status;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }


        /// <summary>
        /// 当前状态
        /// </summary>
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

    }
}
