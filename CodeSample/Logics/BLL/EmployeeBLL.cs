using CodeSample.Logics.DAL;
using CodeSample.Logics.VM;
using CodeSample.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSample.Logics.BLL
{
    public class EmployeeBLL : BaseBLL
    {
        #region Employee 
        public IPagedList<EmployeeVM> GetList(string country, string q, string orderBy, bool asc, int page, int pageSize, int departmentId)
        { 
            var _db = new CodeSampleContext();
            var obj = (from e in _db.Employees
                       join d in _db.Departments on e.DepartmentId equals d.ID
                       where
                         (departmentId==0 || e.DepartmentId== departmentId) &&
                         (string.IsNullOrEmpty(country) || e.Country== country) &&
                        (string.IsNullOrEmpty(q) || (e.FirstName + " " + e.LastName + " " + e.Email + " " + e.Mobile).Contains(q))
                                && e.IsDeleted==false
                       orderby e.ID descending
                       select new EmployeeVM
                       {
                           ID = e.ID,
                           DepartmentId = e.DepartmentId,
                           FirstName = e.FirstName,
                           LastName = e.LastName,
                           Email = e.Email,
                           Mobile = e.Mobile,
                           DOB = e.DOB,
                           Gender = e.Gender,
                           Country = e.Country,
                           State = e.State,
                           City = e.City,
                           ZipCode = e.ZipCode,
                           Address = e.Address,
                           LastActivityOn = e.LastActivityOn,
                           IsActive = e.IsActive,

                           DepartmentTitle = d.Title,
                       });
            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "firstname":
                        obj = asc ? obj.OrderBy(x => x.FirstName) : obj.OrderByDescending(x => x.FirstName);
                        break;
                    case "email":
                        obj = asc ? obj.OrderBy(x => x.Email) : obj.OrderByDescending(x => x.Email);
                        break;
                    case "mobile":
                        obj = asc ? obj.OrderBy(x => x.Mobile) : obj.OrderByDescending(x => x.Mobile);
                        break;
                    case "lastactivityon":
                        obj = asc ? obj.OrderBy(x => x.LastActivityOn) : obj.OrderByDescending(x => x.LastActivityOn);
                        break;
                }
            }
            return obj.ToPagedList(page, pageSize);
        }

        public EmployeeVM GetById(int id)
        {
            var _db = new CodeSampleContext();
            var data = (
                from e in _db.Employees
                where e.ID == id && e.IsDeleted==false
                select new EmployeeVM
                {
                    ID = e.ID,
                    DepartmentId = e.DepartmentId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Mobile = e.Mobile,
                    DOB = e.DOB,
                    Gender = e.Gender,
                    Country = e.Country,
                    State = e.State,
                    City = e.City,
                    ZipCode = e.ZipCode,
                    Address = e.Address,
                    LastActivityOn = e.LastActivityOn,
                    IsActive = e.IsActive,
                }
                ).FirstOrDefault();

            return data;
        } 
          
        public int SaveData(EmployeeVM model)
        {
            var _db = new CodeSampleContext();

            var obj = _db.Employees.FirstOrDefault(x => x.ID == model.ID && x.IsDeleted==false);
            if (obj == null)
            {
                obj = new Employee(); 
                obj.IsDeleted = false;
            }

            obj.DepartmentId = model.DepartmentId;
            obj.FirstName = model.FirstName;
            obj.LastName = model.LastName;
            obj.Email = model.Email;
            obj.Mobile = model.Mobile;
            obj.DOB = model.DOB;
            obj.Gender = model.Gender;
            obj.Country = model.Country;
            obj.State = model.State;
            obj.City = model.City;
            obj.ZipCode = model.ZipCode;
            obj.Address = model.Address;
            obj.IsActive = model.IsActive;

            obj.LastActivityBy = model.LastActivityBy;
            obj.LastActivityOn = Helper.GetCurrentDateTime();
         

            #region Update Section
            if (obj.ID > 0)
            {
                _db.Employees.Attach(obj);
                _db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                _db.Employees.Add(obj);
            }
            #endregion

            return _db.SaveChanges() > 0 ? obj.ID : 0;
        }

        public int Remove(int id, int? lastActivityBy)
        {
            var _db = new CodeSampleContext();
            var obj = _db.Employees.FirstOrDefault(x => x.ID == id && x.IsDeleted == false);
            if (obj != null)
            {
                obj.LastActivityBy = lastActivityBy;
                obj.LastActivityOn = Helper.GetCurrentDateTime();
                obj.IsActive = false;
                obj.IsDeleted = true;

               _db.Employees.Attach(obj);
               _db.Entry(obj).State = System.Data.Entity.EntityState.Modified;

                return _db.SaveChanges() > 0 ? obj.ID : 0;
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}
