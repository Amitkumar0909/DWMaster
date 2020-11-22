using CodeSample.Logics.BLL;
using CodeSample.Logics.VM;
using System;
using System.Web.Mvc;

namespace CodeSample.Controllers
{
    public class AEmployeeController : Controller 
    {
        public ActionResult Index(string country, string q, string orderBy = null, bool asc = true, int page = 1, int pageSize = 20, int departmentId=0)
        {
            using (var objBLL = new EmployeeBLL())
            {
                ViewBag.pageSize = pageSize;
                var objVM = objBLL.GetList(country, q, orderBy, asc, page, pageSize, departmentId);
                MVCHelper.DDLDepartment(this, selected: departmentId);
                MVCHelper.DDLCountry(this, selected: country);
                return View(objVM);
            }
        }

        public ActionResult Add()
        {
            MVCHelper.DDLDepartment(this);
            MVCHelper.DDLCountry(this);
            //MVCHelper.DDLState(this);
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Edit(int id)
        {
            ViewBag.ID = id;
            MVCHelper.DDLDepartment(this);
            MVCHelper.DDLCountry(this);
            return View();
        }


        [HttpGet] 
        public JsonResult GetById(int id)
        {
            ResultVM result = new ResultVM();
            try
            {
                using (var objBLL = new EmployeeBLL())
                {
                    result.Data = objBLL.GetById(id);
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.Errors = ex;
                result.Message = ex.Message;
                Helper.LogException(ex);
            }
            return new JsonNetResult() { Data= result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult SaveData(EmployeeVM objVM)
        {
            var resultVM = new ResultVM();
            try
            { 
                using (var objBLL = new EmployeeBLL())
                {
                    //objVM.LastActivityBy = ActiveUser.ID;
                    int id = objBLL.SaveData(objVM);
                    if (id > 0)
                    {
                        resultVM.Message = "Record added successfully!";
                        resultVM.Data = id;
                        resultVM.IsSuccess = true;
                    }
                    else
                    {
                        resultVM.Message = "Failed to add record!";
                    }
                }
            }
            catch (Exception ex)
            {
                resultVM.Errors = ex.ToString();
                resultVM.Message = "Something went wrong!";
                Helper.LogException(ex);
            }

            return Json(resultVM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(int id)
        {
            try
            {
                using (var objBLL = new EmployeeBLL())
                {
                    if (objBLL.Remove(id, null) > 0)
                    {
                        TempData[Toastr.SUCCESS] = "Record deleted successfully!";
                    }
                    else
                    {
                        TempData[Toastr.ERROR] = "Failed to delete record!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData[Toastr.ERROR] = "Something went wrong!";
                Helper.LogException(ex);
            }
            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }
    }
}