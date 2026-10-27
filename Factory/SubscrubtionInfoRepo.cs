using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Linq;
using System.Security.Cryptography.Xml;
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositorySubscrubtionInfoDTO
{
   
        public class SubscrubtionInfoRepo : ISubscrubtionInfoRepo
        {

            SelfKOrderWebContext DBcontext;
            public SubscrubtionInfoRepo(SelfKOrderWebContext _DBcontext)
            {
            DBcontext = _DBcontext;
            }


      
        public async Task<IEnumerable<SubscrubtionInfoDTO>> GetSubscrubtionInfoList( )
            {
                IEnumerable<SubscrubtionInfoDTO> GroupsList = await DBcontext.SubscrubtionInfo.Include(x => x.Customers).Where(x=>x.IsActive==true).ToSubscrubtionInfoDTO().ToListAsync();
            foreach(var item in GroupsList)
            {
                if (item.CompID != null)
                    item.CompanyName = DBcontext.Company.FirstOrDefault(x => x.CompID == item.CompID).CompName;
                if(item.BrandID!=null)
                item.BrandName = DBcontext.Brand.FirstOrDefault(x => x.CompID == item.CompID && x.BrandID == item.BrandID).BrandName;
               
            }
            return GroupsList;
            }

        public async Task<IEnumerable<SubscrubtionInfoDTO>> GetHistorySubscrubtionInfoList()
        {
            IEnumerable<SubscrubtionInfoDTO> GroupsList = await DBcontext.SubscrubtionInfo.Include(x => x.Customers).Where(x => x.IsActive == false).ToSubscrubtionInfoDTO().ToListAsync();
            foreach (var item in GroupsList)
            {
                if (item.CompID != null)
                    item.CompanyName = DBcontext.Company.FirstOrDefault(x => x.CompID == item.CompID).CompName;
                if (item.BrandID != null)
                    item.BrandName = DBcontext.Brand.FirstOrDefault(x => x.CompID == item.CompID && x.BrandID == item.BrandID).BrandName;


            }
            return GroupsList;
        }

        public async Task<SubscrubtionInfoDTO> GetSubscrubtionInfo( string SubscrubtionInfoID)
            {
               // var Group = await DBcontext.SubscrubtionInfo.Include(x => x.Company).Include(x => x.Brands).FirstOrDefaultAsync(x => x.SupscrubtionID == SubscrubtionInfoID);
            var Group = await DBcontext.SubscrubtionInfo.Include(x=>x.Customers).FirstOrDefaultAsync(x => x.SupscrubtionID == SubscrubtionInfoID);
            var group2 = Group.ToOneSubscrubtionInfoDTO();

            group2.CompanyName = DBcontext.Company.FirstOrDefault(x => x.CompID == group2.CompID).CompName;
            group2.BrandName = DBcontext.Brand.FirstOrDefault(x => x.CompID == group2.CompID && x.BrandID==group2.BrandID).BrandName;


            return group2;
            }

        public async Task<List<SubscrubtionInfoDetailsDTO>> GetSubscrubtionDetailInfo(string id)
        {
            // var Group = await DBcontext.SubscrubtionInfo.Include(x => x.Company).Include(x => x.Brands).FirstOrDefaultAsync(x => x.SupscrubtionID == SubscrubtionInfoID);
            var Group = await DBcontext.SubscrubtionInfoDetails.Where(x=>x.SupscrubtionID==id&&x.IsActive==true).ToSubscrubtionInfoDetailsDTO().ToListAsync();
          
          
            return Group;
        }
        public async Task<List<SubscrubtionInfoDetailsDTO>> GetHestorySubscrubtionDetailInfo(string id)
        {
            // var Group = await DBcontext.SubscrubtionInfo.Include(x => x.Company).Include(x => x.Brands).FirstOrDefaultAsync(x => x.SupscrubtionID == SubscrubtionInfoID);
            var Group = await DBcontext.SubscrubtionInfoDetails.Where(x => x.SupscrubtionID == id && x.IsActive == false).ToSubscrubtionInfoDetailsDTO().ToListAsync();


            return Group;
        }

        public int SaveSubscrubtionInfo( SubscrubtionInfoDTO e)
            {
                try
                {

                    if (DBcontext.SubscrubtionInfo.Any(x => x.SupscrubtionID == e.SupscrubtionID)) { return (int)SearchStauts.Dublicated; }

                var DBObj = new SubscrubtionInfo()
                {


                    SupscrubtionID = e.SupscrubtionID,
                  //  SupscrubtionStartDate = e.SupscrubtionStartDate,
               //    SupscrubtionEndDate = e.SupscrubtionEndDate,
                    SubscriptionTitle = e.SubscriptionTitle,
                    SubscriptionDescription = e.SubscriptionDescription,
                    IsCurrentSupscriped = e.IsCurrentSupscriped,
                    KiosikCount = 0,
                    CompID = e.CompID.Value,
                    IsActive = true,
                    BrandID=e.BrandID.Value,
                    CustomerID=e.CustomerID.Value,
                   
                    //BrandID = e.BrandID.Value


                };

                    DBcontext.SubscrubtionInfo.Add(DBObj);


                    var r = DBcontext.SaveChanges();
                    return r;
                    return 0;
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }


            }
            public int EditSubscrubtionInfo( SubscrubtionInfoDTO obj)
            {
                try
                {

                    var DBObj = DBcontext.SubscrubtionInfo.FirstOrDefault(x => x.SupscrubtionID == obj.SupscrubtionID);
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                {


                    DBObj.IsCurrentSupscriped = obj.IsCurrentSupscriped;
                    DBObj.KiosikCount = obj.KiosikCount;
                    DBObj.CompID = obj.CompID.Value;
                    DBObj.BrandID = obj.BrandID.Value;
                    DBObj.SubscriptionTitle = obj.SubscriptionTitle;
                    DBObj.SubscriptionDescription = obj.SubscriptionDescription;
                    DBObj.CustomerID = obj.CustomerID;

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

        public int AddLicensesDetail(SubscrubtionInfoDetailsDTO e)
        {
            try
            {
                // SupscrubtionDetailID is stored as text. The old code took the text maximum and appended "1"
                // ("1" -> "11" -> "111" ...), which overflowed int after about ten licenses and then every new
                // license failed. Take the numeric maximum instead.
                var ID = DBcontext.SubscrubtionInfoDetails
                    .Select(x => x.SupscrubtionDetailID)
                    .AsEnumerable()
                    .Select(s => int.TryParse(s, out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max() + 1;
                var DBObj = new SubscrubtionInfoDetails()
                {

                    SupscrubtionDetailID = ID.ToString(),
                    SupscrubtionID = e.SupscrubtionID,
                    SupscrubtionStartDate = e.SupscrubtionStartDate,
                    SupscrubtionEndDate = e.SupscrubtionEndDate,
                    BranchID = e.BranchID,
                    BrandID = e.BrandID,
                    KiosikID = e.KiosikID,
                   AutoGeneratedLicenseURl=Security.Encrypt("ValidURL", e.KiosikID,e.BrandID,e.BranchID,ID,e.SupscrubtionID),
                    IsActive = true,
                    ActivationStauts=false,
                };
               DBcontext.SubscrubtionInfoDetails.Add(DBObj);
                var DBObj2 = DBcontext.SubscrubtionInfo.FirstOrDefault(x => x.SupscrubtionID == e.SupscrubtionID);
                DBObj2.KiosikCount = DBObj2.KiosikCount + 1;
                var DBObj3 = DBcontext.Kiosik.FirstOrDefault(x => x.KiosikID == e.KiosikID && x.BrandID==DBObj2.BrandID&&x.BranchID==e.BranchID&&x.CompID==DBObj2.CompID);
                DBObj3.IsCurrentSupscriped = true;
                var r = DBcontext.SaveChanges();
                    return r;

               
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }
        public int DeleteSubscrubtionInfo( string id)
            {

                var DBObj = DBcontext.SubscrubtionInfo.FirstOrDefault(x => x.SupscrubtionID == id);

                try
                {
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound; ;
                    }
                    else
                    {
                        if (DBcontext.SubscrubtionInfoDetails.Any(x => x.SupscrubtionID == DBObj.SupscrubtionID && x.SupscrubtionEndDate>=DateTime.Now))
                        {
                            return (int)SearchStauts.IlligalOperation;
                        }
                        else
                        {
                             DBObj.IsActive=false;
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




        public int DeleteSubscrubtionDetailInfo(string id,string subID)
        {

            var e = DBcontext.SubscrubtionInfoDetails.FirstOrDefault(x => x.SupscrubtionDetailID == id);

            try
            {
                if (e == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    
                    {

                        e.IsActive = false;
                        var DBObj2 = DBcontext.SubscrubtionInfo.FirstOrDefault(x => x.SupscrubtionID == subID);
                        DBObj2.KiosikCount = DBObj2.KiosikCount - 1;
                        var DBObj3 = DBcontext.Kiosik.FirstOrDefault(x => x.KiosikID == e.KiosikID && x.BrandID == e.BrandID && x.BranchID == e.BranchID && x.CompID == DBObj2.CompID);
                        DBObj3.IsCurrentSupscriped = false;
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
