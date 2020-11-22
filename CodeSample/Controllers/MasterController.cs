using CodeSample.Logics.BLL;
using CodeSample.Logics.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeSample.Controllers
{
    public class MasterController : Controller
    {
        #region Country State
        //[HttpGet]
        //[Route("Master/GetCountry")]
        //public JsonResult GetCountry()
        //{
        //    ResultVM result = new ResultVM();
        //    try 
        //    {
        //        using (var objBLL = new MasterBLL())
        //        {
        //            result.Data = objBLL.GetCountries();
        //            result.IsSuccess = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Errors = ex;
        //        result.Message = ex.Message;
        //        result.IsSuccess = false;
        //        Helper.LogException(ex, "MasterController_GetCountry");
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        [Route("Master/GetState")]
        public JsonResult GetState(string countryname)
        {
            ResultVM result = new ResultVM();
            try
            {
                using (MasterBLL oMasterBLL = new MasterBLL())
                {
                    result.Data = oMasterBLL.GetStates(countryname);
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.Errors = ex;
                result.Message = ex.Message;
                result.IsSuccess = false;
                Helper.LogException(ex, "MasterController_GetState");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}