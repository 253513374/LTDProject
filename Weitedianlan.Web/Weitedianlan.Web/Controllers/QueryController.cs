using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weitedianlan.Model.Response;
using Weitedianlan.Service;

namespace Weitedianlan.Web.Controllers
{
    public class QueryController : Controller
    {
        private WLabelService _WLabelService;

        public QueryController(WLabelService wLabelService)
        {
            _WLabelService = wLabelService;
        }

        // GET: Query
        public ActionResult Index()
        {
            return View(new ResponseModel() { Code = 400, Status = "数据为空" });
        }

        [HttpGet]
        public ActionResult GetQuerys(string qrcodeid, string orderid)
        {
            // Expression<Func<W_LabelStorage, bool>> wheres;
            var querys = _WLabelService.GetQuerys(qrcodeid, orderid, out int total);

            JsonResult json = Json(new { total = total, Data = querys.Data });
            return json;
        }

        // GET: Query/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Query/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Query/Create
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

        // GET: Query/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Query/Edit/5
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

        // GET: Query/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Query/Delete/5
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