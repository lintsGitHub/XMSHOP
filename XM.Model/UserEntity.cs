﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XM.Model
{
    public class UserEntity
    {
        /// <summary>
        /// 主键 Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 用户登录 UserId
        /// </summary>
        public string UserAccountName { get; set; }

        /// <summary>
        /// 用户登录密码 UserPwd
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// 用户电话
        /// </summary>
        public string UserMobliePhone { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 创建人 
        /// </summary>
        public string UserCreateBy { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime UserCreateDate { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        ///用户状态
        /// </summary>
        public int StatusID { get; set; }
    }
}
