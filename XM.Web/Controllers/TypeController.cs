﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XM.Model;

namespace XM.Web.Controllers
{
    public class TypeController : BaseController
    {
        // GET: Type
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllTypeInfo()
        {
            string sort = Request["sort"] == null ? "TypeID" : Request["sort"];
            string order = Request["order"] == null ? "asc" : Request["order"];

            //首先获取前台传递过来的参数
            int pageindex = Request["page"] == null ? 1 : Convert.ToInt32(Request["page"]);
            int pagesize = Request["rows"] == null ? 10 : Convert.ToInt32(Request["rows"]);
            string typeName = Request["type_name"] == null ? "" : Request["type_name"];



            int totalCount;   //输出参数
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["pi"] = pageindex;
            paras["pageSize"] = pagesize;
            paras["type_name"] = typeName;
            paras["sort"] = sort;
            paras["order"] = order;
            var type = DALUtility.Type.QryType<GoodsTypeEntity>(paras, out totalCount);
            if(type!= null)
            {
                log(HttpContext.Session["user_AN"].ToString(), "查询所有商品类型", "true", "查询成功");
            }
            else
            {
                log(HttpContext.Session["user_AN"].ToString(), "查询所有商品类型", "false", "查询失败");
            }
            return PagerData(totalCount, type);
        }
        public ActionResult TypeAdd()
        {
            return View();
        }

        /// <summary>
        /// 新增 类型
        /// </summary>
        /// <returns></returns>
        public ActionResult AddType()
        {
            return SaveType();

        }

        public ActionResult TypeEdit()
        {
            return View();
        }
        /// <summary>
        /// 编辑 类型
        /// </summary>
        /// <returns></returns>
        public ActionResult EditType()
        {

            return SaveType();
        }

        private ActionResult SaveType()
        {
            int id = Convert.ToInt32(Request["id"]);
            string typeName = Request["type_name"];
            int num;
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["id"] = id;
            paras["type_name"] = typeName;
            if (id == 0)
            {
                num = DALUtility.Type.Save(paras);
                if (num > 0)
                {
                    log(HttpContext.Session["user_AN"].ToString(), "添加商品类型", "true", "添加成功");
                    return OperationReturn(true, "添加成功！");
                }
                else
                {
                    log(HttpContext.Session["user_AN"].ToString(), "添加商品类型", "false", "添加失败");
                    return OperationReturn(false, "添加失败！");
                }
                
            }
            num = DALUtility.Type.Save(paras);
            if (num > 0)
            {
                log(HttpContext.Session["user_AN"].ToString(), "修改商品类型", "true", "修改成功");
                return OperationReturn(true, "修改成功！");
            }
            else
            {
                log(HttpContext.Session["user_AN"].ToString(), "修改商品类型", "false", "修改失败");
                return OperationReturn(false, "修改失败！");
            }
            

        }

        public ActionResult DelTypeByIDs()
        {
            string Ids = Request["id"] == null ? "" : Request["id"];
            if (!string.IsNullOrEmpty(Ids))
            {
                log(HttpContext.Session["user_AN"].ToString(), "删除商品类型", "true", "删除成功");
                return OperationReturn(DALUtility.Type.DeleteType(Ids), "删除成功！");
            }
            else
            {
                log(HttpContext.Session["user_AN"].ToString(), "删除商品类型", "false", "删除失败");
                return OperationReturn(false, "删除失败！");
            }
        }
    }
}