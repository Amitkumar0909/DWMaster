using CodeSample.Logics.DAL;
using CodeSample.Logics.VM;
using System.Collections.Generic;
using System.Linq;

namespace CodeSample.Logics.BLL
{
    public class MasterBLL:BaseBLL
    {
        #region Country States
        public List<TitleVM> GetCountries()
        {
            var _db = new CodeSampleContext();
            return (from c in _db.Countries select new TitleVM { ID = c.ID, Title = c.Name }).ToList();
        }

        public List<TitleVM> GetStates(string countryname)
        {
            var _db = new CodeSampleContext();
            var country = _db.Countries.FirstOrDefault(x => x.Name == countryname);
            if (country != null)
            {
                return (from c in _db.CountryStates.Where(x => x.CountryID == country.ID) select new TitleVM { ID = c.ID, Title = c.Name }).ToList();
            }
            else
            {
                return new List<TitleVM>();
            }
        }
         
        public List<TitleVM> GetDepartments()
        {
            var _db = new CodeSampleContext();
            return (from c in _db.Departments select new TitleVM { ID = c.ID, Title = c.Title }).ToList();
        }

        #endregion
    }
}