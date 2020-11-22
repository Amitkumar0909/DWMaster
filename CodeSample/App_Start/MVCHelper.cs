using CodeSample.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CodeSample
{
    public static class MVCHelper
    {
        public static void DDLCountry(Controller _controller, string key = "Title", string value = "Title", object selected = null)
        {
            using (var objBLL = new MasterBLL())
            {
                _controller.ViewBag.Countries = new SelectList(objBLL.GetCountries(), key, value, selected);
            }
        }

        public static void DDLState(Controller _controller, string key = "Title", string value = "Title", object selected = null, string countryName = null)
        {
            using (var objBLL = new MasterBLL())
            {
                _controller.ViewBag.States = new SelectList(objBLL.GetStates(countryName), key, value, selected);
            }
        }
         
        public static void DDLDepartment(Controller _controller, string key = "ID", string value = "Title", object selected = null)
        {
            using (var objBLL = new MasterBLL())
            {
                _controller.ViewBag.Departments = new SelectList(objBLL.GetDepartments(), key, value, selected);
            }
        }
    }
}