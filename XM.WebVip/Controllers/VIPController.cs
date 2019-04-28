﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using XM.Comm;
using XM.Model;
using XM.Web.Controllers;

namespace XM.WebVip.Controllers
{
    /// <summary>
    /// 作者:曾贤鑫
    /// 日期:2019/4/26
    /// 功能:vip端会用到的一些行为
    /// </summary>
    public class VIPController : BaseController
    {
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:返回会员端首页
        /// </summary>
        /// <returns>页面:首页</returns>
        public ActionResult Index()
        {

            return View();
        }

        #region _Login
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:返回会员端登录页面
        /// </summary>
        /// <returns>页面:登录页面</returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:会员端进行登入的方法
        /// </summary>
        /// <returns>json值</returns>
        [HttpPost]
        public ActionResult Login(string AN, string pwd)
        {
            try
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["vip_AN"] = AN;
                paras["vip_pwd"] = pwd;
                //paras["vip_pwd"] = Md5.GetMD5String(pwd);   //md5加密

                var vip = DALUtility.Vip.QryVipToLogin<VipEntity>(paras);
                if (vip != null)
                {
                    if (vip.StatusID == 2)
                    {
                        return OperationReturn(false, "用户已被禁用，请您联系管理员");
                    }
                    ViewData.Model = vip;
                    return OperationReturn(true, "登录成功,vip_id:" + vip.VipID + ";vip_AN:" + AN);
                }
                else
                {
                    return OperationReturn(false, "用户名密码错误，请您检查");
                }
            }
            catch (Exception ex)
            {
                return OperationReturn(false, "登录异常," + ex.Message);
            }
        }
        #endregion

        #region _Signin
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:返回会员端注册页面
        /// </summary>
        /// <returns>页面:注册时,返回注册页面</returns>
        public ActionResult Signin()
        {
            return View();
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:会员端进行注册
        /// </summary>
        /// <returns>json值</returns>
        [HttpPost]
        public ActionResult Signin(VipEntity vip)
        {
            return save(0);
        }
        #endregion

        #region _update
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:会员端进行修改信息页面
        /// </summary>
        /// <returns>页面:修改信息页面</returns>
        public ActionResult Update()
        {
            return View();
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:会员端进行修改信息页面
        /// </summary>
        /// <returns>json值</returns>
        [HttpPost]
        public ActionResult Update(VipEntity vip)
        {
            return save(int.Parse(Request["vip_id"]));
        }/// <summary>
         /// 作者:曾贤鑫
         /// 日期:2019/4/28
         /// 功能:返回密码找回页面,输入用户名
         /// </summary>
         /// <returns>页面</returns>
        public ActionResult FoundPwdPage()
        {
            return View();
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/28
        /// 功能:发送邮件
        /// </summary>
        /// <returns>json值</returns>
        [HttpPost]
        public ActionResult FoundPwd()
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("vip_AN", Request["vip_AN"]);
            var vip = DALUtility.Vip.QryVipInfo<VipEntity>(param);

            bool boo = false;
            string strMailContent = "<a href='http://172.16.31.234:6666/VIP/UpdatePwd'>修改密码</a>";

            if (vip != null)
            {
                boo = EmailHelper.send(vip.VipEmail, "修改密码", strMailContent);
            }

            return OperationReturn(boo, "邮件已发送,请登录邮箱进行下一步操作!");
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/28
        /// 功能:修改密码
        /// </summary>
        /// <returns>json值</returns>
        public ActionResult UpdatePwdPage()
        {
            return save(int.Parse(Request["vip_id"]));

        }
        #endregion

        #region _invitation
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:会员端进行邀请注册
        /// </summary>
        /// <param name="vip_AN"></param>
        /// <returns>json值</returns>
        public ActionResult Invitation(string vip_AN)
        {
            return OperationReturn(true, vip_AN);
        }

        public ActionResult RechargePage()
        {
            return View();
        }
        #endregion

        #region _recharge
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 功能:会员端进行充值
        /// </summary>
        /// <param name="vip_AN"></param>
        /// <returns>json值</returns>
        public ActionResult Recharge()
        {
            DateTime date = DateTime.Now;

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("recharge_name", Request["recharge_name"]);
            param.Add("recharge_price", Request["recharge_price"]);
            param.Add("recharge_time", date);
            param.Add("agent_id", Request["agent_id"]);
            param.Add("vip_id", Request["vip_id"]);

            int iCheck = DALUtility.Vip.Recharge(param);

            if(iCheck > 0)
            {
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("remainder", Request["recharge_price"]);
                p.Add("vip_AN", Request["vip_AN"]);
                //p.Add("vip_AN", HttpContext.Session["vip_AN"]);
                int i = DALUtility.Vip.InsertRemainder(p);

                if (i != 2)
                {
                    Url.Action("CheckRecharge", "Agent", new RouteValueDictionary {
                    { "vip_id",Request["vip_id"]},
                    { "recharge_price",Request["recharge_price"]},
                    { "recharge_time", date}
                    });

                    return OperationReturn(true, "充值成功");
                }
                else
                {
                    return OperationReturn(false, "充值失败");
                }
            }
            return OperationReturn(false,"充值失败");
        }
        #endregion

        #region _shopping
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/26
        /// 查看余额
        /// </summary>
        /// <returns></returns>
        public ActionResult buy()
        {
            DateTime date = DateTime.Now;

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("order_date", date);
            param.Add("order_address", Request["order_address"]);
            param.Add("order_mp", Request["order_mp"]);
            param.Add("vip_AN", Request["vip_AN"]);
            param.Add("agent_AN", Request["agent_AN"]);
            param.Add("order_total", Request["order_total"]);


            param.Add("buy_time", date);
            param.Add("buy_count", Request["buy_count"]);
            param.Add("buy_AN", Request["buy_AN"]);
            param.Add("goods_id", Request["goods_id"]);
            param.Add("buy_total", Request["buy_total"]);

            int iCheck = DALUtility.Vip.Buy(param);

            Debug.WriteLine(iCheck);

            if(iCheck > 0)
            {
                return OperationReturn(false, iCheck == 1 ? "用户余额不足,请充值后从试!" :  "购物出错,请重试!");
            }
            return OperationReturn(true,"购物成功");
        }
        #endregion

        #region _vipInfo
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/28
        /// 功能:返回vip个人中心页面
        /// </summary>
        /// <returns>页面</returns>
        public ActionResult VipInfo()
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("vip_AN", Request["vip_AN"]);

            var vip = DALUtility.Vip.QryVipInfo<VipEntity>(param);

            ViewBag.vipInfo = vip;
            return View();
        }
        #endregion

        #region _address
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/28
        /// 功能:返回vip收货地址
        /// </summary>
        /// <returns>页面</returns>
        public ActionResult AddressPage()
        {
            return View();
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/28
        /// 功能:返回vip收货地址
        /// </summary>
        /// <returns>json值</returns>
        [HttpPost]
        public ActionResult Address()
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("id", int.Parse(Request["id"]));
            param.Add("address_name", Request["address_name"]);
            param.Add("vip_id", int.Parse(Request["vip_id"]));

            string result = DALUtility.Vip.QryVipAddress(param, out int iCount);

            return Content(result);
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/28
        /// 功能:vip地址添加
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertAddress()
        {
            return SaveAddress(0);
        }

        public ActionResult UpdateAddress()
        {
            return SaveAddress(int.Parse(Request["vip_id"]));
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/28
        /// 功能:删除收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteAddress()
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("id", int.Parse(Request["address_id"]));
            param.Add("vip_id", int.Parse(Request["vip_id"]));

            int iCheck = DALUtility.Vip.DeleteAddress(param);

            if(iCheck == 0)
            {
                return OperationReturn(false,"删除收货地址失败");
            }
            return OperationReturn(true, "删除收货地址成功");
        }
        #endregion

        #region _自定义
        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/25
        /// 功能:添加/修改vip公用方法
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult save(int ID)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["id"] = ID;
            paras["vip_AN"] = Request["vip_AN"];
            paras["vip_mp"] = Request["vip_mp"];
            paras["vip_Email"] = Request["vip_Email"];

            int iCheck = DALUtility.Vip.checkANandMBandEmail(paras);

            if (iCheck > 0)
            {
                return OperationReturn(false, iCheck == 1 ? "所输入的用户名重复,请重新输入!" : (iCheck == 2 ? "所输入的手机号码重复,请重新输入!" : "所输入的邮箱重复,请重新输入!"));
            }
            else
            {
                paras["vip_pwd"] = Request["vip_pwd"];
                paras["vip_CDT"] = DateTime.Now;
                paras["status_id"] = Request["status_id"];
                paras["agent_id"] = Request["agent_id"];
                int result = DALUtility.Vip.saveVIP(paras);
                if (ID == 0)
                {
                    return OperationReturn(result > 0, "注册成功");
                }
                else
                {
                    return OperationReturn(result > 0, "修改成功");
                }
            }
        }

        /// <summary>
        /// 作者:曾贤鑫
        /// 日期:2019/4/28
        /// 功能:vip地址的添加或者修改公用方法
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveAddress(int ID)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("id", ID);
            param.Add("address_name", Request["address_name"]);
            param.Add("vip_id", int.Parse(Request["vip_id"]));

            int iCheck = DALUtility.Vip.SaveAddress(param);
            ContentResult strResult = null;
            switch (iCheck)
            {
                case 0:
                    strResult = OperationReturn(true, "添加收货地址成功");
                    break;
                case 1:
                    strResult = OperationReturn(true, "修改收货地址成功");
                    break;
                case 2:
                    if (ID == 0)
                    {
                        strResult = OperationReturn(false, "添加收货地址失败");
                    }
                    strResult = OperationReturn(false, "修改收货地址失败");
                    break;
            }
            return strResult;
        }
        #endregion
    }
}