using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Weitedianlan.Model.Response;
using Weitedianlan.Service;

namespace Weitedianlan.Web.Controllers
{
    public class WlabelController : Controller
    {
        private DateTime DateTime { set; get; } = DateTime.Now.AddDays(-60).Date;

        private WLabelService WLabelService;

        public WlabelController(WLabelService wLabelService)
        {
            this.WLabelService = wLabelService;
        }

        // GET: Wlabel
        [HttpGet]
        public ActionResult Index()
        {
            var response = WLabelService.GetWLabelServiceList(DateTime);
            return View(response);
        }

        public ActionResult UserAdd()
        {
            return View();
        }

        // GET: Wlabel/Details/5
        public ActionResult Details(string id)
        {
            if (id.Length < 0) return Json(new ResponseModel { Code = 0, Status = "单号为空" });
            var reponse = this.WLabelService.GetDetails(id);
            return View(reponse);
        }

        [HttpGet]
        public ActionResult GetQuerys(string datetimestr, string selectId)
        {
            var response = this.WLabelService.GetWLabelServiceList(DateTime, selectId, datetimestr);
            if (response.Code == 200)
            {
                JsonResult json = Json(new { total = response.DataCount, qrcount = response.QrcodeDataCount, data = response.Data });
                return json;
            }
            else
            {
                return Json(new { });
            }

            //if(datetimestr!="")
            //{
            //    DateTime dateTime = DateTime.ParseExact(datetimestr, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            //}
            //else
            //{
            //    return Json(new { });
            //}
        }

        // GET: Wlabel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wlabel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Wlabel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Wlabel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Wlabel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Wlabel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}