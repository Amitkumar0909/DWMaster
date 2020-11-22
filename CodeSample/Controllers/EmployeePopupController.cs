using CodeSample.Logics.BLL;
using CodeSample.Logics.VM;
using System;
using System.Web.Mvc;

namespace CodeSample.Controllers
{ 
    public class EmployeePopupController : Controller
    {
        public ActionResult Index(string country, string q, string orderBy = null, bool asc = true, int page = 1, int pageSize = 20, int departmentId = 0)
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
            ViewBag.TempPath = DateTime.Now.Ticks;
            MVCHelper.DDLCountry(this);
            MVCHelper.DDLState(this);
            MVCHelper.DDLDepartment(this);
            return PartialView();
        }

        [ValidateInput(false)]
        public ActionResult Edit(int id)
        {
            using (var objBLL = new EmployeeBLL())
            {
                var objVM = objBLL.GetById(id);
                MVCHelper.DDLCountry(this, selected: objVM.Country);
                MVCHelper.DDLState(this, selected: objVM.State, countryName:objVM.Country);
                MVCHelper.DDLDepartment(this, selected: objVM.DepartmentId);
                return PartialView(objVM);
            }
        }

        [HttpPost]
        public ActionResult Add(EmployeeVM objVM, string tempPath)
        {
            var resultVM = new ResultVM();
            try
            {
                using (var objBLL = new EmployeeBLL())
                {
                    objVM.Logo = Helper.FilePaths(Server.MapPath("~/temp/employee/" + tempPath + "/img/"));
                    //objVM.LastActivityBy = ActiveUser.ID;
                    int id = objBLL.SaveData(objVM);
                    if (id > 0)
                    {
                        objVM.Logo = Helper.MoveFiles(Server.MapPath("~/temp/employee/" + tempPath + "/img/"), Server.MapPath("~/content/employee/" + id + "/img/"));
                        resultVM.Data = id;
                        resultVM.IsSuccess = true;
                        resultVM.Message = "Record added successfully!";
                    }
                    else
                    {
                        resultVM.Message = "Failed to add record!";
                    }
                }
            }
            catch (Exception ex)
            {
                resultVM.Message = "Something went wrong!";
                resultVM.Errors = ex.ToString();
                 Helper.LogException(ex);
            }
              
            return Json(resultVM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost] 
        public ActionResult Edit(EmployeeVM objVM)
        {
            var resultVM = new ResultVM();
            try
            {
                using (var objBLL = new EmployeeBLL())
                {
                    objVM.Logo = Helper.FilePaths(Server.MapPath("~/content/employee/" + objVM.ID + "/img/"));
                    //objVM.LastActivityBy = ActiveUser.ID;
                    int id = objBLL.SaveData(objVM);
                    if (id > 0)
                    {
                        resultVM.Data = id;
                        resultVM.IsSuccess = true;
                        resultVM.Message = "Record added successfully!";
                    }
                    else
                    {
                        resultVM.Message = "Failed to add record!";
                    }
                }
            }
            catch (Exception ex)
            {
                resultVM.Message = "Something went wrong!";
                resultVM.Errors = ex.ToString();
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