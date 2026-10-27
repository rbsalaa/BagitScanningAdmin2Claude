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
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryPaymentMethodDTO
{
   
        public class PaymentMethodRepo : IPaymentMethodRepo
        {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public PaymentMethodRepo(SelfKOrderWebContext _DBcontext ,IAccessFilterService accessFilter)
            {
            _accessFilter = accessFilter;
            DBcontext = _DBcontext;
            }


      
        public async Task<IEnumerable<PaymentMethodDTO>> GetPaymentMethodList(ClaimsPrincipal User)
            {
                var GroupsList =  DBcontext.PaymentMethod.AsQueryable();
            IQueryable<PaymentMethodDTO> filtered2 = _accessFilter.PaymentMethodFilter(User, GroupsList);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
            }

            public async Task<PaymentMethodDTO> GetPaymentMethod( int PaymentMethodID)
            {
                var Group = await DBcontext.PaymentMethod.FirstOrDefaultAsync(x => x.PaymentMethodID == PaymentMethodID);
                return Group?.ToOnePaymentMethodDTO();
            }

            public int SavePaymentMethod( PaymentMethodDTO obj)
            {
                try
                {

                    if (DBcontext.PaymentMethod.Any(x => x.PaymentMethodID == obj.PaymentMethodID)) { return (int)SearchStauts.Dublicated; }

                    var DBObj = new PaymentMethod()
                    {


                        PaymentMethodID = obj.PaymentMethodID,
                        PaymentMethodName = obj.PaymentMethodName,
                        IsActive = true,
                        BranchID = obj.BranchID,
                        BrandID = obj.BrandID,
                        CompID = obj.CompID,
                        FranchID = obj.FranchID,
                        PaymentMethodNameAr=obj.PaymentMethodNameAr

                    };

                    DBcontext.PaymentMethod.Add(DBObj);


                    var r = DBcontext.SaveChanges();
                    return r;
                    return 0;
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }


            }
            public int EditPaymentMethod( PaymentMethodDTO obj)
            {
                try
                {

                    var DBObj = DBcontext.PaymentMethod.FirstOrDefault(x => x.PaymentMethodID == obj.PaymentMethodID );
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {


               
                    DBObj.PaymentMethodID = obj.PaymentMethodID;
                    DBObj.PaymentMethodName = obj.PaymentMethodName;
                    DBObj.PaymentMethodNameAr = obj.PaymentMethodNameAr;
                    DBObj.IsActive = obj.IsActive;
                    DBObj.BranchID = obj.BranchID==0?null: obj.BranchID;
                        DBObj.BrandID = obj.BrandID == 0 ? null : obj.BrandID;
                        DBObj.CompID = obj.CompID == 0 ? null :obj.CompID;
                    DBObj.FranchID = obj.FranchID == 0 ? null :obj.FranchID;
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
            public int DeletePaymentMethod( int id)
            {

                var DBObj = DBcontext.PaymentMethod.FirstOrDefault(x => x.PaymentMethodID == id);

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
