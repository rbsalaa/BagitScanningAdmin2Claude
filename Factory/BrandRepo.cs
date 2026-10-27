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

namespace IdentityCore.Factory.RepositoryBrandDTO
{

	public class BrandRepo : IBrandRepo
	{

		SelfKOrderWebContext DBcontext;
		IAccessFilterService _accessFilter;

        public BrandRepo(SelfKOrderWebContext _DBcontext  ,IAccessFilterService accessFilter)
		{
			DBcontext = _DBcontext;
			_accessFilter = accessFilter;
		}



		public async Task<IEnumerable<BrandDTO>> GetBrandList(ClaimsPrincipal User)
		{
			var GroupsList =  DBcontext.Brand.Include(x => x.Companys).AsQueryable();

            IQueryable<BrandDTO> filtered2 = _accessFilter.BrandFilter(User, GroupsList);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
		}

		public async Task<BrandDTO> GetBrand(int BrandID)
		{
			var Group = await DBcontext.Brand.Include(x => x.Companys).FirstOrDefaultAsync(x => x.BrandID == BrandID);
			return Group?.ToOneBrandDTO();
		}

		public int SaveBrand(BrandDTO obj)
		{
			try
			{

				if (DBcontext.Brand.Any(x => x.BrandID == obj.BrandID)) { return (int)SearchStauts.Dublicated; }

				var DBObj = new Brand()
				{


					BrandName = obj.BrandName,
					BrandCode = obj.BrandCode,
					BrandPic = obj.BrandPic,
					CompID = obj.CompID,
					IsActive = true

				};

				DBcontext.Brand.Add(DBObj);


				var r = DBcontext.SaveChanges();
				return r;
				return 0;
			}
			catch (Exception ex)
			{
				return (int)SearchStauts.Error;
			}


		}
		public int EditBrand(BrandDTO obj)
		{
			try
			{

				var DBObj = DBcontext.Brand.FirstOrDefault(x => x.BrandID == obj.BrandID && x.CompID == obj.CompID);
				if (DBObj == null)
				{
					return (int)SearchStauts.NotFound;
				}
				else
				{




					DBObj.BrandName = obj.BrandName;
					DBObj.BrandCode = obj.BrandCode;
					if (obj.BrandPic != null)
						DBObj.BrandPic = obj.BrandPic;
					DBObj.CompID = obj.CompID;

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
		public int DeleteBrand(int id)
		{

			var DBObj = DBcontext.Brand.FirstOrDefault(x => x.BrandID == id);

			try
			{
				if (DBObj == null)
				{
					return (int)SearchStauts.NotFound; ;
				}
				else
				{
					if (DBcontext.Branch.Any(x => x.BrandID == DBObj.BrandID))
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
