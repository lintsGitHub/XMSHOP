﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XM.Model;

namespace XM.Web.Controllers
{
    public class GoodsController : BaseController
    {
        // GET: Goods
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllGoodsInfo()
        {
            string sort = Request["sort"] == null ? "GoodsID" : Request["sort"];
            string order = Request["order"] == null ? "asc" : Request["order"];

            //首先获取前台传递过来的参数
            int pageindex = Request["page"] == null ? 1 : Convert.ToInt32(Request["page"]);
            int pagesize = Request["rows"] == null ? 10 : Convert.ToInt32(Request["rows"]);
            string goodsName = Request["goods_name"] == null ? "" : Request["goods_name"];
            string goodsIntro = Request["goods_intro"] == null ? "" : Request["goods_intro"];
            decimal goodsPrice = Request["goods_CP"] == null ? 1 : Convert.ToDecimal(Request["goods_CP"]);
            string createBy = Request["goods_BY"] == null ? "" : Request["goods_BY"];
            string createDateTime = Request["goods_CDT"] == null ? "" : Request["goods_CDT"];
            string goodsPic = Request["goods_pic"] == null ? "" : Request["goods_pic"];
            int typeId = Request["type_id"] == null ? 1 : Convert.ToInt32(Request["type_id"]);



            int totalCount;   //输出参数
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["pi"] = pageindex;
            paras["pageSize"] = pagesize;
            paras["goods_name"] = goodsName;
            paras["sort"] = sort;
            paras["order"] = order;
            var goods = DALUtility.Goods.QryGoods<GoodsEntity>(paras, out totalCount);
            if (goods != null)
            {
                log(HttpContext.Session["user_AN"].ToString(), "获取所有商品信息", "true", "获取成功");
            }
            else
            {
                log(HttpContext.Session["user_AN"].ToString(), "获取所有商品信息", "false", "获取失败");
            }
            return PagerData(totalCount, goods);
        }

        public ActionResult AddGoods()
        {
            return View();
        }
        /// <summary>
        /// 新增 产品
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodsAdd()
        {
            return SaveGoods();

        }

        public ActionResult EditGoods()
        {
            return View();
        }
        /// <summary>
        /// 编辑 产品
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodsEdit()
        {

            return SaveGoods();
        }

        private ActionResult SaveGoods()
        {
            int id = Convert.ToInt32(Request["id"]);
            string goodsName = Request["goods_name"];
            string goodsIntro = Request["goods_intro"];
            decimal goodsPrice = Convert.ToDecimal(Request["goods_CP"]);
            string createBy = Request["goods_BY"];
            string goodsPic = Request["goods_pic"];
            int typeId = Convert.ToInt32(Request["type_id"]);

            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["id"] = id;
            paras["goods_name"] = goodsName;
            paras["goods_intro"] = goodsIntro;
            paras["goods_CP"] = goodsPrice;
            paras["type_id"] = typeId;
            paras["goods_pic"] = goodsPic;
            if (id == 0)
            {
                paras["goods_CBY"] = createBy;
                paras["goods_CDT"] = DateTime.Now;
                log(HttpContext.Session["user_AN"].ToString(), "添加商品", "true", "添加成功");
                return OperationReturn(DALUtility.Goods.Save(paras) > 0, "添加成功！" );
            }
            log(HttpContext.Session["user_AN"].ToString(), "修改商品信息", "true", "修改成功");
            return OperationReturn(DALUtility.Goods.Save(paras) > 0, "修改成功！");

        }

        public ActionResult DelGoodsByIDs()
        {
            string Ids = Request["id"] == null ? "" : Request["id"];
            if (!string.IsNullOrEmpty(Ids))
            {
                log(HttpContext.Session["user_AN"].ToString(), "删除商品信息", "true", "删除成功");
                return OperationReturn(DALUtility.Goods.DeleteGoods(Ids),"删除成功！");
            }
            else
            {
                log(HttpContext.Session["user_AN"].ToString(), "删除商品信息", "false", "删除失败");
                return OperationReturn(false,"删除失败");
            }
        }
    }
}