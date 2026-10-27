using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryTAXGroupDTO
{
   
        public class TAXGroupRepo : ITAXGroupRepo
        {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public TAXGroupRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }


      
        public async Task<IEnumerable<TAXGroupDTO>> GetTAXGroupList(ClaimsPrincipal User)
            {
               
            IQueryable<TAXGroupDTO> filtered2 = _accessFilter.TAXFilter(User, DBcontext.TAXGroup);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
            }

            public async Task<TAXGroupDTO> GetTAXGroup( int TAXID)
            {
                var Group = await DBcontext.TAXGroup.FirstOrDefaultAsync(x => x.TAXID == TAXID);
                return Group?.ToOneTAXGroupDTO();
            }

            public int SaveTAXGroup( TAXGroupDTO e)
            {
                try
                {

                    if (DBcontext.TAXGroup.Any(x => x.TAXID == e.TAXID)) { return (int)SearchStauts.Dublicated; }

                    var DBObj = new TAXGroup()
                    {

                        TAXID = e.TAXID,
                        TAXName = e.TAXName,
                        TAXPercentage = e.TAXPercentage,
                        IsDefault = e.IsDefault,
                        IsActive = true,
                        BranchID = e.BranchID,
                        BrandID = e.BrandID,
                        CompID = e.CompID,
                        FranchID = e.FranchID


                    };

                    DBcontext.TAXGroup.Add(DBObj);


                    var r = DBcontext.SaveChanges();
                    return r;
                    return 0;
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }


            }
            public int EditTAXGroup( TAXGroupDTO e)
            {
                try
                {

                    var DBObj = DBcontext.TAXGroup.FirstOrDefault(x => x.TAXID == e.TAXID );
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                else
                {

                    DBObj.TAXID = e.TAXID;
                    DBObj.TAXName = e.TAXName;
                    DBObj.TAXPercentage = e.TAXPercentage;
                    DBObj.IsDefault = e.IsDefault;
                    DBObj.IsActive = e.IsActive;
                    DBObj.BranchID = e.BranchID;
                    DBObj.BrandID = e.BrandID;
                    DBObj.CompID = e.CompID;
                    DBObj.FranchID = e.FranchID;



                    // db.AppLogs.Add(new AppLog() { Uid = "", ActionDesc = "editing  User with ID " + obj.Uid, ActionDateTime = DateTime.Now, UidNavigation = null });
                    var r = DBcontext.SaveChanges();
                        return r;

                    }
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }
            }
            public int DeleteTAXGroup( int id)
            {

                var DBObj = DBcontext.TAXGroup.FirstOrDefault(x => x.TAXID == id);

                try
                {
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound; ;
                    }
                    else
                    {
                       
                        {
                            var x = DBcontext.Remove(DBObj);
                            //db.AppLogs.Add(new AppLog() { Uid = "", ActionDesc = "UnActive/Delete New User with ID " + id, ActionDateTime = DateTime.Now, UidNavigation = null });

                            return DBcontext.SaveChanges();
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
