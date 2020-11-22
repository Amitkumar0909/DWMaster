using CodeSample.Logics.BLL;
using CodeSample.Logics.VM;
using System;
using System.Web.Mvc;

namespace CodeSample.Controllers
{
    public class EmployeeController : Controller
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
            ViewBag.TempPath = DateTime.Now.Ticks;
            MVCHelper.DDLDepartment(this);
            MVCHelper.DDLCountry(this);
            MVCHelper.DDLState(this);
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Edit(int id)
        {
            using (var objBLL = new EmployeeBLL())
            {
                var objVM = objBLL.GetById(id);
                MVCHelper.DDLDepartment(this, selected: objVM.DepartmentId);
                MVCHelper.DDLCountry(this, selected: objVM.Country);
                MVCHelper.DDLState(this, selected: objVM.State, countryName:objVM.Country);
                return View(objVM);
            }
        }

        
        [HttpPost]
        public ActionResult Add(EmployeeVM objVM, string tempPath)
        {
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
                        TempData[Toastr.SUCCESS] = "Record added successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData[Toastr.ERROR] = "Failed to add record!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData[Toastr.ERROR] = "Something went wrong!";
                Helper.LogException(ex);
            }

            ViewBag.TempPath = tempPath;
            MVCHelper.DDLDepartment(this, selected: objVM.DepartmentId);
            MVCHelper.DDLCountry(this, selected: objVM.Country);
            MVCHelper.DDLState(this, selected: objVM.State, countryName: objVM.Country);
            return View(objVM);
        }

         
        [HttpPost]
        public ActionResult Edit(EmployeeVM objVM)
        {
            try
            {
                using (var objBLL = new EmployeeBLL())
                {
                    objVM.Logo = Helper.FilePaths(Server.MapPath("~/content/employee/" + objVM.ID + "/img/"));
                    //objVM.LastActivityBy = ActiveUser.ID;
                    int id = objBLL.SaveData(objVM);
                    if (id > 0)
                    {
                        TempData[Toastr.SUCCESS] = "Record updated successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData[Toastr.ERROR] = "Failed to update record!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData[Toastr.ERROR] = "Something went wrong!";
                Helper.LogException(ex);
            }

            MVCHelper.DDLDepartment(this, selected: objVM.DepartmentId);
            MVCHelper.DDLCountry(this, selected: objVM.Country);
            MVCHelper.DDLState(this, selected: objVM.State, countryName: objVM.Country);
            return View(objVM);
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