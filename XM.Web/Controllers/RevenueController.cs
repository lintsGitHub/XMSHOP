﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XM.Model;

namespace XM.Web.Controllers
{
    public class RevenueController : BaseController
    {
        // GET: Revenue
        public ActionResult Index()
        {
            return View(); 
        }
        public ActionResult GetRechargeRevenue()
        {
            string sort = Request["sort"] == null ? "RechargeID" : Request["sort"];
            string order = Request["order"] == null ? "asc" : Request["order"];

            //首先获取前台传递过来的参数
            int pageindex = Request["page"] == null ? 1 : Convert.ToInt32(Request["page"]);
            int pagesize = Request["rows"] == null ? 10 : Convert.ToInt32(Request["rows"]);
            string startTime = Request["startTime"] == null ? "" : Request["startTime"];
            string endTime = Request["endTime"] == null ? "" : Request["endTime"];
            int AgentID = Request["agent_id"] == null ? 1 : Convert.ToInt32(Request["agent_id"]);
            int VipID = Request["vip_id"] == null ? 1 : Convert.ToInt32(Request["vip_id"]);

            int totalCount;   //输出参数
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["pi"] = pageindex;
            paras["pageSize"] = pagesize;
            paras["AgentID"] = AgentID;
            paras["VipID"] = VipID;
            paras["startTime"] = startTime;
            paras["endTime"] = endTime;
            paras["sort"] = sort;
            paras["order"] = order;
            var charge = DALUtility.Recharge.QryRecharge<RechargeEntity>(paras, out totalCount);
            if(charge != null)
            {
                log(HttpContext.Session["user_AN"].ToString(), "查询充值营收表", "true", "查询成功");
            }
            else
            {
                log(HttpContext.Session["user_AN"].ToString(), "查询充值营收表", "false", "查询失败");
            }
            return PagerData(totalCount, charge);
        }
        public ActionResult GetGoodsRevenue()
        {
            string sort = Request["sort"] == null ? "OrderID" : Request["sort"];
            string order = Request["order"] == null ? "asc" : Request["order"];

            //首先获取前台传递过来的参数
            int pageindex = Request["page"] == null ? 1 : Convert.ToInt32(Request["page"]);
            int pagesize = Request["rows"] == null ? 10 : Convert.ToInt32(Request["rows"]);
            string startTime = Request["startTime"] == null ? "" : Request["startTime"];
            string endTime = Request["endTime"] == null ? "" : Request["endTime"];
            string agentAccountName = Request["agent_AN"] == null ? "" : Request["agent_AN"];


            int totalCount;   //输出参数
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["pi"] = pageindex;
            paras["pageSize"] = pagesize;
            paras["agent_AN"] = agentAccountName;
            paras["startTime"] = startTime;
            paras["endTime"] = endTime;
            paras["sort"] = sort;
            paras["order"] = order;
            var goods = DALUtility.Order.QryOrder<OrderEntity>(paras, out totalCount);
            if(goods != null)
            {
                log(HttpContext.Session["user_AN"].ToString(), "查询所有的商品营收", "true", "查询成功");
            }
            else
            {
                log(HttpContext.Session["user_AN"].ToString(), "查询所有的商品营收", "false", "查询失败");
            }
            return PagerData(totalCount, goods);
        }
    }
}