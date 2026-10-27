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

namespace IdentityCore.Factory.RepositoryCustomerDTO
{
   
        public class CustomerRepo : ICustomerRepo
        {

            SelfKOrderWebContext DBcontext;
            public CustomerRepo(SelfKOrderWebContext _DBcontext)
            {
            DBcontext = _DBcontext;
            }


      
        public async Task<IEnumerable<CustomerDTO>> GetCustomerList( )
            {
                IEnumerable<CustomerDTO> GroupsList = await DBcontext.Customer.ToCustomerDTO().ToListAsync();
                return GroupsList;
            }

            public async Task<CustomerDTO> GetCustomer( int CustomerID)
            {
            var Group = await DBcontext.Customer.FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            // var Group = await DBcontext.Customer.Include(x => x.SubscrubtionInfo).FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            var groupresult = Group.ToOneCustomerDTO();
                 groupresult.KiossiksList=DBcontext.Kiosik.Where(x=>x.CustomerID == CustomerID).ToKiosikDTO().ToList();
                return groupresult;
            }

            public int SaveCustomer( CustomerDTO e)
            {
                try
                {

                    if (DBcontext.Customer.Any(x => x.CustomerID == e.CustomerID)) { return (int)SearchStauts.Dublicated; }

                var DBObj = new Customer()
                {



                    CustomerName = e.CustomerName,
                    CustomerLocation = e.CustomerLocation,
                    IsSupscriped = e.SupscrubtionID==null?false:true,
                    CustomerPhone = e.CustomerPhone,
                    IsActive = true,
                    CustomerMail = e.CustomerMail,
                   
                  //SupscrubtionID= e.SupscrubtionID== e.SupscrubtionID?null: e.SupscrubtionID

                };

                    DBcontext.Customer.Add(DBObj);


                    var r = DBcontext.SaveChanges();
                    return r;
                    return 0;
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }


            }
            public int EditCustomer( CustomerDTO obj)
            {
                try
                {

                    var DBObj = DBcontext.Customer.FirstOrDefault(x => x.CustomerID == obj.CustomerID);
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {

                    DBObj.CustomerName = obj.CustomerName;
                    DBObj.CustomerLocation = obj.CustomerLocation;
                    DBObj.IsSupscriped = obj.SupscrubtionID==null?false:true;
                    DBObj.CustomerPhone = obj.CustomerPhone;
                    DBObj.IsActive = obj.IsActive;
                    DBObj.CustomerMail = obj.CustomerMail;
                  //  DBObj.SupscrubtionID = obj.SupscrubtionID;

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
            public int DeleteCustomer( int id)
            {

                var DBObj = DBcontext.Customer.FirstOrDefault(x => x.CustomerID == id);

                try
                {
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound; ;
                    }
                    else
                    {
                        if (DBcontext.Kiosik.Any(x => x.CustomerID == DBObj.CustomerID && x.IsActive==true))
                        {
                            return (int)SearchStauts.IlligalOperation;
                        }
                        else
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
