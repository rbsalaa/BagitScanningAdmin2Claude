using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryBrandDTO
{

	public class ItemRepo : IItemRepo
	{

        SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public ItemRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
        {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }



        public async Task<IEnumerable<ItemDTO>> GetItems(ClaimsPrincipal User)
		{
			//IEnumerable<ItemDTO> GroupsList = await DBcontext.Items.Include(x => x.Groups).ToItemsDTO().ToListAsync();
            IQueryable<ItemDTO> filtered2 = _accessFilter.ItemFilter(User, DBcontext.Items.Include(x => x.Groups));
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
		}

		public async Task<ItemDTO> GetItem(int ItemID)
		{
			var Group = await DBcontext.Items.Include(i => i.ItemModifiers)
        .ThenInclude(im => im.ModifierCat)
            .ThenInclude(mc => mc.ModifierModifierCategories)
                .ThenInclude(mmc => mmc.Modifier)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ItemID == ItemID);

            return Group?.ToOneItemDTO2(DBcontext);
		}

		public int SaveItem(ItemDTO obj)
		{
			try
			{

				if (DBcontext.Items.Any(x => x.ItemID == obj.ItemID)) { return (int)SearchStauts.Dublicated; }

				var DBObj = new Items()
				{


					ItemName = obj.ItemName,
					ItemName_AR = obj.ItemName_AR,
					ItemID = obj.ItemID,
					ItemPrice = obj.ItemPrice,
					GroupID = obj.GroupID,
					IsActive = true,
					IsEnabled=obj.IsEnabled,
					ItemPic = obj.ItemPic,
					ItemDescription = obj.ItemDescription,
					ItemDescription_AR = obj.ItemDescription_AR,
					IsGroupItem = obj.IsGroupItem,
                    BranchID = obj.BranchID,
                    BrandID = obj.BrandID,
                    CompID = obj.CompID,
                    FranchID = obj.FranchID,
					Calories = obj.Calories,
                    Steps = obj.Steps,
                    Alergies = obj.Alergies,
                   AlergiesAr = obj.AlergiesAr,
                    AlohaProductID = obj.AlohaProductID,
					LastModified=DateTime.Now


                };

				DBcontext.Items.Add(DBObj);


				var r = DBcontext.SaveChanges();
				return r;
				return 0;
			}
			catch (Exception ex)
			{
				return (int)SearchStauts.Error;
			}


		}
		public int EditItem(ItemDTO obj)
		{
			try
			{

				var DBObj = DBcontext.Items.FirstOrDefault(x => x.ItemID == obj.ItemID);
				if (DBObj == null)
				{
					return (int)SearchStauts.NotFound;
				}
				else
				{


					DBObj.IsEnabled = obj.IsEnabled;
					DBObj.ItemName = obj.ItemName;
					DBObj.ItemName_AR = obj.ItemName_AR;
					DBObj.ItemDescription_AR = obj.ItemDescription_AR;
					DBObj.ItemDescription = obj.ItemDescription;
					DBObj.ItemID = obj.ItemID;
					DBObj.ItemPrice = obj.ItemPrice;
					DBObj.GroupID = obj.GroupID;
					DBObj.IsActive = true;
                    DBObj.BranchID = obj.BranchID;
                    DBObj.BrandID = obj.BrandID;
                    DBObj.CompID = obj.CompID;
                    DBObj.FranchID = obj.FranchID;
					DBObj.Calories = obj.Calories;
					DBObj.Steps = obj.Steps;
					DBObj.Alergies = obj.Alergies;
                    DBObj.AlergiesAr = obj.AlergiesAr;
                    DBObj.AlohaProductID = obj.AlohaProductID;
                    if (obj.ItemPic != null)
						DBObj.ItemPic = obj.ItemPic;
					DBObj.IsGroupItem = obj.IsGroupItem;

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
		public int DeleteItem(int id)
		{

			var DBObj = DBcontext.Items.FirstOrDefault(x => x.ItemID == id);

			try
			{
				if (DBObj == null)
				{
					return (int)SearchStauts.NotFound; ;
				}
				else
				{
					if (DBcontext.MenuItems.Any(x => x.ItemID == DBObj.ItemID) || DBcontext.ItemsCategory.Any(x => x.ItemID == DBObj.ItemID))
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

