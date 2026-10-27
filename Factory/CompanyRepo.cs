using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using static SelfKOrderWeb.Factory.Enums;

namespace SelfKOrderWeb.Factory
{
    public class CompanyRepo : ICompanyRepo
    {

        SelfKOrderWebContext DbContext;
        IAccessFilterService _accessFilter;
       // ClaimsPrincipal User;
        public CompanyRepo(SelfKOrderWebContext _DbContextcontext, IAccessFilterService accessFilter)
        {
            DbContext = _DbContextcontext;
             _accessFilter=accessFilter;
         //   User = _User;
        }



        public async Task<IEnumerable<CompanyDTO>> GetCompanyList(ClaimsPrincipal User)
        {
            //IEnumerable<CompanyDTO> GroupsList = await DbContext.Company.ToCompanyDTO().ToListAsync();
            //IEnumerable<CompanyDTO> filtered = _accessFilter.CompFilter(User, GroupsList);
             
            IQueryable<CompanyDTO> filtered2 =  _accessFilter.CompFilter(User, DbContext.Company.AsQueryable());
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<CompanyDTO> GetCompany(  int CompID)
        {
            var Group = await DbContext.Company.FirstOrDefaultAsync(x => x.CompID == CompID);
            return Group.ToAssignOneCompanyDTO();
        }

        public int SaveCompany(  CompanyDTO e)
        {
            try
            {

                if (DbContext.Company.Any(x => x.CompID == e.CompID)) { return (int)SearchStauts.Dublicated; }

                var DbContextObj = new Company()
                {
                      CompName = e.CompName,
                    CompCode = e.CompCode,
                    IsActive = true,

                };

                DbContext.Company.Add(DbContextObj);


                var r = DbContext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditCompany(  CompanyDTO obj)
        {
            try
            {

                var DbContextObj = DbContext.Company.FirstOrDefault(x => x.CompID == obj.CompID);
                if (DbContextObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {




                    DbContextObj.CompName = obj.CompName;
                    DbContextObj.CompCode = obj.CompCode;

                    // DbContext.AppLogs.Add(new AppLog() { Uid = "", ActionDesc = "editing  User with ID " + obj.Uid, ActionDateTime = DateTime.Now, UidNavigation = null });
                    var r = DbContext.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }
        public int DeleteCompany(  int id)
        {

            var DbContextObj = DbContext.Company.FirstOrDefault(x => x.CompID == id);

            try
            {
                if (DbContextObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DbContext.Brand.Any(x => x.CompID == DbContextObj.CompID))
                    {
                        return (int)SearchStauts.IlligalOperation;
                    }
                    else
                    {
                        var x = DbContext.Remove(DbContextObj);
                        //DbContext.AppLogs.Add(new AppLog() { Uid = "", ActionDesc = "UnActive/Delete New User with ID " + id, ActionDateTime = DateTime.Now, UidNavigation = null });

                        return DbContext.SaveChanges();
                    }

                    //return -1;
                }

            }
            catch (Exception ex)
            {
                return (int)SearchStauts.NonDeletedUser;
            }
        }
    }
}

