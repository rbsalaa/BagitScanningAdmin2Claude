using Microsoft.AspNetCore.Identity;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;

namespace SelfKOrderWeb.Factory
{
    public static class ToEntityExtention
    {
        public static IQueryable<SelfKOrderWebUser> ToUserDTO(this IQueryable<IdentityUser> item)
        {/**/
            return item.Select(e => new SelfKOrderWebUser()
            {

                UserName = e.UserName,
                Email = e.Email,
                Id = e.Id,

            });



        }
        public static IQueryable<AddUserDTO> ToUserDTO(this IQueryable<SelfKOrderWebUser> item)
        {
            return item.Select(e => new AddUserDTO()
            {

                FirstName = e.FirstName,
                LastName = e.LastName,
                NormalizedUserName = e.NormalizedUserName,
                Email = e.Email,
                CompID = e.CompID,
                BrandID = e.BrandID,
                IsRBSUser = e.IsRBSUser,
                IsSuperMainAdmin = e.IsSuperMainAdmin,
                FranchID = e.FranchID,
                Id = e.Id,
                BranchID=e.BranchID


            });



        }
        public static IQueryable<RolesDTO> ToaspROLEDTO(this IQueryable<ApplicationRole> item)
        {
            return item.Select(e => new RolesDTO()
            {

                Id = e.Id,
                Name = e.Name,
                NormalizedName = e.NormalizedName,
                ConcurrencyStamp = e.ConcurrencyStamp,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,


            });



        }
        public static RolesDTO ToOneaspROLEeDTO(this ApplicationRole e)
        {
            return new RolesDTO()
            {
                Id = e.Id,
                Name = e.Name,
                NormalizedName = e.NormalizedName,
                ConcurrencyStamp = e.ConcurrencyStamp,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
            };



        }
        public static IQueryable<InventoryDTO> ToInventoryDTO(this IQueryable<SelfKOrderWeb.Models.InventoryWarehouse> data)
        {
            return data.Select(x => new InventoryDTO
            {
                InvID = x.InvID,
                InvName = x.InvName,
                InvName_AR = x.InvName_AR,
                InvLocation = x.InvLocation,
               // BranchID = x.BranchID,
                CompID = x.CompID,
                BrandID = x.BrandID,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                LastModified = x.LastModified
            });
        }

        public static IQueryable<CompanyDTO> ToCompanyDTO(this IQueryable<Company> item)
        {
            return item.Select(e => new CompanyDTO()
            {

                CompID = e.CompID,
                CompName = e.CompName,
                CompCode = e.CompCode,
                IsActive = e.IsActive,
            });



        }
        public static CompanyDTO ToAssignOneCompanyDTO(this Company e)
        {
            return new CompanyDTO()
            {

                CompID = e.CompID,
                CompName = e.CompName,
                CompCode = e.CompCode,
                IsActive = e.IsActive,

            };



        }

        public static IQueryable<BrandDTO> ToBrandDTO(this IQueryable<Brand> item)
        {
            return item.Select(e => new BrandDTO()
            {

                BrandID = e.BrandID,
                BrandName = e.BrandName,
                BrandCode = e.BrandCode,
                IsActive = e.IsActive,
                CompID = e.CompID,
                CompName = e.Companys.CompName,
                BrandPic = e.BrandPic
            });



        }
        public static IQueryable<ItemDTO> ToItemsDTO(this IQueryable<Items> item)
        {
            return item.Select(e => new ItemDTO()
            {

                ItemID = e.ItemID,
                GroupName = e.Groups.GroupName,
                ItemName = e.ItemName,
                AlergiesAr = e.AlergiesAr,
                ItemDescription = e.ItemDescription,   
                ItemDescription_AR = e.ItemDescription_AR,   
                ItemName_AR = e.ItemName_AR,   
                IsActive = e.IsActive,
                GroupID = e.GroupID,
                ItemPrice = e.ItemPrice,
                IsGroupItem = e.IsGroupItem,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                ItemPic = e.ItemPic,
                Steps = e.Steps,
                Alergies = e.Alergies,
                Calories = e.Calories,
                AlohaProductID = e.AlohaProductID,
                IsEnabled = e.IsEnabled,

            });



        }
        public static ItemDTO ToOneItemDTO(this Items e)
        {
            return new ItemDTO()
            {


                ItemID = e.ItemID,
                ItemName = e.ItemName,
                IsActive = e.IsActive,
                GroupID = e.GroupID,
                AlergiesAr = e.AlergiesAr,
                ItemPrice = e.ItemPrice,
                ItemDescription = e.ItemDescription,
                ItemDescription_AR = e.ItemDescription_AR,
                ItemName_AR = e.ItemName_AR,
                IsGroupItem = e.IsGroupItem,
                ItemPic = e.ItemPic,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                Steps = e.Steps,
                Alergies = e.Alergies,
                Calories = e.Calories,
                AlohaProductID = e.AlohaProductID,
                IsEnabled = e.IsEnabled,
                
   
            };



        }
        public static ItemDTO ToOneItemDTO2(this Items e, SelfKOrderWebContext db)
        {
            return new ItemDTO()
            {
                ItemID = e.ItemID,
                ItemName = e.ItemName,
                IsActive = e.IsActive,
                GroupID = e.GroupID,
                AlergiesAr = e.AlergiesAr,
                ItemPrice = e.ItemPrice,
                ItemDescription = e.ItemDescription,
                ItemDescription_AR = e.ItemDescription_AR,
                ItemName_AR = e.ItemName_AR,
                IsGroupItem = e.IsGroupItem,
                ItemPic = e.ItemPic,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                Steps = e.Steps,
                Alergies = e.Alergies,
                Calories = e.Calories,
                AlohaProductID = e.AlohaProductID,
                IsEnabled = e.IsEnabled,

                // ✅ Grouping ItemModifiers by ModifierCategory
                ModifierCategories = e.ItemModifiers
                    .GroupBy(m => m.ModifierCat)
                    .Select(g => new ModifierCategoryDTO
                    {
                        ModifierCategoryID = g.Key.ModifierCategoryID,
                        ModifierCategoryName = g.Key.ModifierCategoryName,

                        // ✅ Get Modifiers from the many-to-many relationship
                        Modifiers = g.Key.ModifierModifierCategories
                            .Select(mc => new ModifierDTO
                            {
                                AlohProductID = mc.Modifier.AlohaProductID,
                                ModifierName = mc.Modifier.ModifierName
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }




        




        public static IQueryable<CategoryDTO> ToCategoryDTO(this IQueryable<Category> item)
        {
            return item.Select(e => new CategoryDTO()
            {
                CategoryID = e.CategoryID,
                CategoryName = e.CategoryName,
                CategoryName_AR = e.CategoryName_AR,
                CategoryOrder = e.CategoryOrder,
                CategoryPic=e.CategoryPic,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID
                ,IsSuggestionCat = e.IsSuggestionCat,
            });



        }
        public static CategoryDTO ToOneCategoryDTO(this Category e)
        {
            return new CategoryDTO()
            {
                CategoryID = e.CategoryID,
                CategoryName = e.CategoryName,
                CategoryName_AR = e.CategoryName_AR,
                CategoryOrder = e.CategoryOrder,
                CategoryPic = e.CategoryPic,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                IsSuggestionCat = e.IsSuggestionCat,
            };



        }
        public static IQueryable<ModifierDTO> ToModifierDTO(this IQueryable<Modifier> item)
        {
            return item.Select(e => new ModifierDTO()
            {
                ModifierID = e.ModifierID,
                ModifierName = e.ModifierName,
                ModifierName_AR = e.ModifierName_AR,
                ModifierPrice = e.ModifierPrice,
                ModifierPic=e.ModifierPic,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID


            });



        }
        public static ModifierDTO ToOneModifierDTO(this Modifier e)
        {
            return new ModifierDTO()
            {
                ModifierID = e.ModifierID,
                ModifierName = e.ModifierName,
                ModifierName_AR = e.ModifierName_AR,
                ModifierPrice = e.ModifierPrice,
                ModifierPic = e.ModifierPic,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID

            };



        } public static IQueryable<ModifierCategoryDTO> ToModifierCategoryDTO(this IQueryable<ModifierCategory> item)
        {
            return item.Select(e => new ModifierCategoryDTO()
            {
                ModifierCategoryID = e.ModifierCategoryID,
                ModifierCategoryName = e.ModifierCategoryName,
                ModifierCategoryName_AR = e.ModifierCategoryName_AR,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID


            });



        }
        public static ModifierCategoryDTO ToOneModifierCategoryDTO(this ModifierCategory e)
        {
            return new ModifierCategoryDTO()
            {
                ModifierCategoryID = e.ModifierCategoryID,
                ModifierCategoryName = e.ModifierCategoryName,
                ModifierCategoryName_AR = e.ModifierCategoryName_AR,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID


            };



        }
        public static IQueryable<MenuDTO> ToMenuDTO(this IQueryable<Menu> item)
        {
            return item.Select(e => new MenuDTO()
            {
                MenuID = e.MenuID,
                MenuName = e.MenuName,
                IsCurrentMenu = e.IsCurrentMenu,
                IsActive = e.IsActive,
                IsDefault = e.IsDefault,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID
            });



        }
        public static MenuDTO ToMenuDTO2(this Menu e)
        {
            // Flatten items with their main categories
            var itemsWithCategories = e.MenuItems
                .SelectMany(mi => mi.item.ItemCategories
                    .Select(cat => new
                    {
                        Item = mi.item,
                        CategoryID = cat.CategoryID,
                        CategoryName = cat.Category.CategoryName,
                        IsEnabled = mi.IsCurrentEnabled
                    }));

            // Group by category and build DTO
            var categories = itemsWithCategories
                .GroupBy(ic => ic.CategoryID)
                .Select(g => new CategoryDTO
                {
                    CategoryID = g.Key,
                    CategoryName = g.First().CategoryName,
                    Items = g.Select(ic => new ItemDTO
                    {
                        ItemID = ic.Item.ItemID,
                        ItemName = ic.Item.ItemName,
                        ItemPrice = ic.Item.ItemPrice,
                        IsEnabled = ic.IsEnabled,
                        ItemPic = ic.Item.ItemPic,

                        ModifierCategories = ic.Item.ItemModifiers
                            .Select(imc => new ModifierCategoryDTO
                            {
                                ModifierCategoryID = imc.ModifierCategoryID,
                                ModifierCategoryName = imc.ModifierCat.ModifierCategoryName,
                                Modifiers = imc.ModifierCat.Modifiers
                                    .Select(m => new ModifierDTO
                                    {
                                        ModifierID = m.ModifierID,
                                        ModifierName = m.ModifierName,
                                        ModifierPrice = m.ModifierPrice
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList()
                })
                .OrderBy(c => c.CategoryName)
                .ToList();

            // Return the full MenuDTO
            return new MenuDTO
            {
                MenuID = e.MenuID,
                MenuName = e.MenuName,
                IsCurrentMenu = e.IsCurrentMenu,
                IsActive = e.IsActive,
                IsDefault = e.IsDefault,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                categories = categories
            };
        }


        public static MenuDTO ToOneMenuDTO(this Menu e)
        {
            return new MenuDTO()
            {
                MenuID = e.MenuID,
                MenuName = e.MenuName,
                IsCurrentMenu = e.IsCurrentMenu,
                IsActive = e.IsActive,
                IsDefault = e.IsDefault,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID
            };



        }
        public static IQueryable<GroupDTO> ToGroupDTO(this IQueryable<Groups> item)
        {
            return item.Select(e => new GroupDTO()
            {
                GroupID = e.GroupID,
                GroupName = e.GroupName,
                GroupItemsCount = e.GroupItemsCount,
                ItemPrice = e.ItemPrice,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID
            });



        }
        public static GroupDTO ToOneGroupDTO(this Groups e)
        {
            return new GroupDTO()
            {
                GroupID = e.GroupID,
                GroupName = e.GroupName,
                GroupItemsCount = e.GroupItemsCount,
                ItemPrice = e.ItemPrice,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID
            };



        }

        public static BrandDTO ToOneBrandDTO(this Brand e)
        {
            return new BrandDTO()
            {


                BrandID = e.BrandID,
                BrandName = e.BrandName,
                BrandCode = e.BrandCode,
                IsActive = e.IsActive,
                CompID = e.CompID,
                CompName = e.Companys.CompName,
                BrandPic=e.BrandPic

            };



        }
        public static IQueryable<BranchDTO> ToStoreDTO(this IQueryable<Branch> item)
        {
            return item.Select(e => new BranchDTO()
            {
                BrandCode = e.BrandCode,
                BranchID = e.BranchID,
                BranchLocation = e.BranchLocation,
                BranchName = e.BranchName,
                BrandID = e.BrandID,
                IsActive = e.IsActive,
                BrandName = e.Brands.BrandName,
                CompanyName = e.Brands.Companys.CompName,
                CompID = e.CompID,
                FranchID= e.FranchID,
                FranchName = e.FranchID == null ? null : e.Franchis.FranchName





            });



        }



        public static BranchDTO ToOneStoreDTO(this Branch e)
        {
            return new BranchDTO()
            {

                BrandCode = e.BrandCode,
                BranchID = e.BranchID,
                BranchLocation = e.BranchLocation,
                BranchName = e.BranchName,
                BrandID = e.BrandID,
                IsActive = e.IsActive,
                BrandName = e.Brands.BrandName,
                CompanyName = e.Brands.Companys.CompName,
                CompID = e.CompID,
                OldBrndId = e.BrandID,
                OldCompId = e.CompID,
                FranchID = e.FranchID == null ? null:e.FranchID,
                FranchName=e.FranchID==null?null:e.Franchis.FranchName

            };



        }

        public static IQueryable<FranchisingDTO> ToFranshisDTO(this IQueryable<Franchising> item)
        {
            return item.Select(e => new FranchisingDTO()
            {
                FranchID = e.FranchID,
                FranchCode = e.FranchCode,
                FranchLocation = e.FranchLocation,
                FranchName = e.FranchName,
                BrandID = e.BrandID,
                IsActive = e.IsActive,
                BrandName = e.Brands.BrandName,
                CompanyName = e.Brands.Companys.CompName,
                CompID = e.CompID



            });



        }



        public static FranchisingDTO ToOneFranshisDTO(this Franchising e)
        {
            return new FranchisingDTO()
            {

                FranchID = e.FranchID,
                FranchCode = e.FranchCode,
                FranchLocation = e.FranchLocation,
                FranchName = e.FranchName,
                BrandID = e.BrandID,
                IsActive = e.IsActive,
                BrandName = e.Brands.BrandName,
                CompanyName = e.Brands.Companys.CompName,
                CompID = e.CompID,
                OldBrndId=e.BrandID,
                OldCompId=e.CompID

            };



        }

        public static CustomerDTO ToOneCustomerDTO(this Customer e)
        {
            return new CustomerDTO()
            {

                CustomerID = e.CustomerID,
                CustomerName = e.CustomerName,
                CustomerLocation = e.CustomerLocation,
                IsSupscriped = e.IsSupscriped,
                CustomerPhone = e.CustomerPhone,
                IsActive = e.IsActive,
                CustomerMail = e.CustomerMail,
             //   SupscrubtionID = e.SupscrubtionID == null ? "" : e.SupscrubtionID,
             //   SubIsCurrentSupscriped = e.SupscrubtionID == null ? false : e.SubscrubtionInfo.IsCurrentSupscriped,
            //    SupscrubtionTitle = e.SupscrubtionID == null ? null : e.SubscrubtionInfo.SubscriptionTitle,
             //   SupscrubtionDescription = e.SupscrubtionID == null ? null : e.SubscrubtionInfo.SubscriptionDescription,
            //    SubKiosikCount = e.SupscrubtionID == null ? 0 : e.SubscrubtionInfo.KiosikCount,
               

            };



        }
        public static SubscrubtionInfoDTO ToOneSubscrubtionInfoDTO(this SubscrubtionInfo e)
        {
            return new SubscrubtionInfoDTO()
            {

                SupscrubtionID = e.SupscrubtionID,
               // SupscrubtionStartDate = e.SupscrubtionStartDate,
                //SupscrubtionEndDate = e.SupscrubtionEndDate,
                IsCurrentSupscriped = e.IsCurrentSupscriped,
                KiosikCount = e.KiosikCount,
                CompID = e.CompID,
                IsActive = e.IsActive,
                BrandID = e.BrandID,
                SubscriptionTitle= e.SubscriptionTitle,
                SubscriptionDescription= e.SubscriptionDescription,
              CustomerID = e.CustomerID,
              CustomerName = e.CustomerID==null?null:e.Customers.CustomerName,
                OldCompId = e.CompID



            };



        }

        public static SubscrubtionInfoDetailsDTO ToOneSubscrubtionInfoDetailsDTO(this SubscrubtionInfoDetails e)
        {
            return new SubscrubtionInfoDetailsDTO()
            {

                SupscrubtionID = e.SupscrubtionID,
                SupscrubtionStartDate = e.SupscrubtionStartDate,
                SupscrubtionEndDate = e.SupscrubtionEndDate,
                SupscrubtionDetailID = e.SupscrubtionDetailID,
                Key=e.AutoGeneratedLicenseURl,
                IsActive = e.IsActive,
                BrandID = e.BrandID==null?0:e.BrandID.Value,
                BranchID = e.BranchID == null ? 0 : e.BranchID.Value,
                KiosikID = e.KiosikID == null ? 0 : e.KiosikID.Value,
                BrandName = e.Brand.BrandName,
                BranchName = e.Branch.BranchName,
                KiosikName = e.Kiosik.KiosikName,
                ActivationStauts = e.ActivationStauts,


            };



        }

        public static IQueryable<SubscrubtionInfoDetailsDTO> ToSubscrubtionInfoDetailsDTO(this IQueryable<SubscrubtionInfoDetails> item)
        {
            return item.Select(e => new SubscrubtionInfoDetailsDTO()
            {
                SupscrubtionID = e.SupscrubtionID,
                SupscrubtionStartDate = e.SupscrubtionStartDate,
                SupscrubtionEndDate = e.SupscrubtionEndDate,
                SupscrubtionDetailID = e.SupscrubtionDetailID,
                Key=e.AutoGeneratedLicenseURl,
                ActivationStauts=e.ActivationStauts,
                IsActive = e.IsActive,
                BrandID = e.BrandID == null ? 0 : e.BrandID.Value,
                BranchID = e.BranchID == null ? 0 : e.BranchID.Value,
                KiosikID = e.KiosikID == null ? 0 : e.KiosikID.Value,
                BrandName = e.Brand.BrandName,
                BranchName = e.Branch.BranchName,
                KiosikName = e.Kiosik.KiosikName,
                



            });



        }
        public static IQueryable<SubscrubtionInfoDTO> ToSubscrubtionInfoDTO(this IQueryable<SubscrubtionInfo> item)
        {
            return item.Select(e => new SubscrubtionInfoDTO()
            {


                SupscrubtionID = e.SupscrubtionID,
              //  SupscrubtionStartDate = e.SupscrubtionStartDate,
                SubscriptionTitle = e.SubscriptionTitle,
                SubscriptionDescription = e.SubscriptionDescription,
              //  SupscrubtionEndDate = e.SupscrubtionEndDate,
                IsCurrentSupscriped = e.IsCurrentSupscriped,
                KiosikCount = e.KiosikCount,
                CompID = e.CompID,
                IsActive = e.IsActive,
                carrierString = e.SupscrubtionID + "-" +e.BrandID +"-" + e.CompID,
                CustomerName=e.Customers.CustomerName,
            //  CompanyName = db.Company.FirstOrDefault(x=>x.CompID==e.CompID).CompName,
               BrandID = e.BrandID,
            //  BrandName = db.Brand.FirstOrDefault(x => x.BrandID == e.BrandID && x.CompID == e.CompID).BrandName,
            // OldBrndId = e.BrandID,
            OldCompId = e.CompID
            ,CustomerID=e.CustomerID

            });
        }
        public static IQueryable<CustomerDTO> ToCustomerDTO(this IQueryable<Customer> item)
        {
            return item.Select(e => new CustomerDTO()
            {

                CustomerID = e.CustomerID,
                CustomerName = e.CustomerName,
                CustomerLocation = e.CustomerLocation,
                IsSupscriped = e.IsSupscriped,
                CustomerPhone = e.CustomerPhone,
                IsActive = e.IsActive,
                CustomerMail = e.CustomerMail,
             //   SupscrubtionID = e.SupscrubtionID,


            });
        }
        public static IQueryable<KiosikDTO> ToKiosikDTO(this IQueryable<Kiosik> item)
        {
            return item.Select(e => new KiosikDTO()
            {
                KiosikID = e.KiosikID,
                KiosikName = e.KiosikName,
                KiosikRegion = e.KiosikRegion,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                MenuID = e.MenuID,
                CustomerID = e.CustomerID,
                IsActive = e.IsActive,
                NFC = e.NFC,
                Printer= e.Printer,
                BrandName = e.Brand.BrandName,
                BranchName = e.Branch.BranchName,
                CustomerName = e.Customer.CustomerName,
                CompName = e.Company.CompName,
                OldBranchID = e.BranchID,
                OldBrandID = e.BrandID,
                OldCompID = e.CompID,
                OldCustomerID = e.CustomerID,
                IsCurrentSupscriped = e.IsCurrentSupscriped




            });
        }
        public static KiosikDTO ToOneKiosikDTO(this Kiosik e)
        {
            return new KiosikDTO()
            {

                KiosikID = e.KiosikID,
                KiosikName = e.KiosikName,
                KiosikRegion = e.KiosikRegion,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                MenuID = e.MenuID,
                CustomerID = e.CustomerID,
                IsActive = e.IsActive,
                NFC = e.NFC,
                Printer = e.Printer,
                BrandName = e.Brand.BrandName,
                BranchName = e.Branch.BranchName,
                CustomerName = e.Customer.CustomerName,
                CompName = e.Company.CompName,
                OldBranchID = e.BranchID,
                OldBrandID = e.BrandID,
                OldCompID = e.CompID,
                OldCustomerID = e.CustomerID,
                IsCurrentSupscriped = e.IsCurrentSupscriped
            };



        }

        public static IQueryable<AdvertismentsDTO> ToAdvertismentsDTO(this IQueryable<Advertisments> item)
        {
            return item.Select(e => new AdvertismentsDTO()
            {
                AdsID = e.AdsID,
                AdsTitle = e.AdsTitle,
                AdsStartDate = e.AdsStartDate,
                AdsEndDate = e.AdsEndDate,
                AdsURL = e.AdsURL,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID

            });
        }
        public static AdvertismentsDTO ToOneAdvertismentsDTO(this Advertisments e)
        {
            return new AdvertismentsDTO()
            {

                AdsID = e.AdsID,
                AdsTitle = e.AdsTitle,
                AdsStartDate = e.AdsStartDate,
                AdsEndDate = e.AdsEndDate,
                AdsURL = e.AdsURL,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID

            };



        }

        public static IQueryable<BannersDTO> ToBannersDTO(this IQueryable<Banners> item)
        {
            return item.Select(e => new BannersDTO()
            {
                BannerID = e.BannerID,
                BannerTitle = e.BannerTitle,
                BannerStartDate = e.BannerStartDate,
                BannerEndDate = e.BannerEndDate,
                BannerURL = e.BannerURL,
                BannerClickURL = e.BannerClickURL,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                TargetID = e.TargetID,
                TargetType = e.TargetType,
                TargetUrl = e.TargetUrl


            });
        }
        public static BannersDTO ToOneBannersDTO(this Banners e)
        {
            return new BannersDTO()
            {

                BannerID = e.BannerID,
                BannerTitle = e.BannerTitle,
                BannerStartDate = e.BannerStartDate,
                BannerEndDate = e.BannerEndDate,
                BannerURL = e.BannerURL,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                BannerClickURL = e.BannerClickURL,
                TargetID = e.TargetID,
                TargetType = e.TargetType,
                TargetUrl = e.TargetUrl

            };



        }
        public static IQueryable<KiosikAdsDTO> ToKiosikAdsDTO(this IQueryable<KiosikAds> item)
        {
            return item.Select(e => new KiosikAdsDTO()
            {

                KiosikAdsID = e.KiosikAdsID,
                AdsID = e.AdsID,
                KiosikID = e.KiosikID,
                IsActive = e.IsActive,
                KiosikName = e.Kiosik.KiosikName,
                AdsTitle = e.Advertisments.AdsTitle
            });



        }

        public static IQueryable<PaymentMethodDTO> ToPaymentMethodDTO(this IQueryable<PaymentMethod> item)
        {
            return item.Select(e => new PaymentMethodDTO()
            {
                PaymentMethodID = e.PaymentMethodID,
                PaymentMethodName = e.PaymentMethodName,
                PaymentMethodNameAr=e.PaymentMethodNameAr,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID=e.FranchID

            });
        }
        public static PaymentMethodDTO ToOnePaymentMethodDTO(this PaymentMethod e)
        {
            return new PaymentMethodDTO()
            {
                PaymentMethodID = e.PaymentMethodID,
                PaymentMethodName = e.PaymentMethodName,
                PaymentMethodNameAr = e.PaymentMethodNameAr,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID
            };



        }

        public static IQueryable<TAXGroupDTO> ToTAXGroupDTO(this IQueryable<TAXGroup> item)
        {
            return item.Select(e => new TAXGroupDTO()
            {
                TAXID = e.TAXID,
                TAXName = e.TAXName,
                TAXPercentage = e.TAXPercentage,
                IsDefault = e.IsDefault,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
               

            });
        }
        public static IQueryable<OrderSummaryDTO> ToOrderDTO(this IQueryable<Order> item)
        {
            return item.Select(e => new OrderSummaryDTO()
            {
                OrderId = e.OrderId,
                KiosikID = e.KiosikID,
                AlohaOrderId=e.AlohaOrderID,
                OrderDate = e.OrderDate,
                TotalAmount = e.TotalAmount,
                Status = e.Status=="1"?"Completed":"Canceled",
                KiosikName = e.Kiosik.KiosikName,
                PaymmentTransactionID=e.PaymmentTransactionID,
                TenderID= e.TenderID,
                OrderName=e.OrderName,
                JsonString = e.JsonString
                
                


            });
        }
       
        public static IQueryable<OrderDetailDTO> ToOrderDetailDTO(this IQueryable<OrderDetail> item)
        {
            return item.Select(e => new OrderDetailDTO()
            {
                OrderDetailId = e.OrderDetailId,
                Price = e.Price,
                Quantity = e.Quantity,
                JsonString=e.Order.JsonString,
                ItemName = e.Items.ItemName,

                Modifiers = e.Modifiers
                    .Select(m => new ModifierDTO
                    {
                        ModifierID = m.ModifierID,
                        ModifierName = m.ModifierName,
                        ModifierPrice = m.ModifierPrice
                    })
                    .ToList()


            });
        }
        public static TAXGroupDTO ToOneTAXGroupDTO(this TAXGroup e)
        {
            return new TAXGroupDTO()
            {
                TAXID = e.TAXID,
                TAXName = e.TAXName,
                TAXPercentage = e.TAXPercentage,
                IsDefault = e.IsDefault,
                IsActive = e.IsActive,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
              
            };



        }
       
        public static IQueryable<StoreMenuDiscountsDTO> ToStoreMenuDiscountsDTO(this IQueryable<StoreMenuDiscounts> item)
        {
            return item.Select(e => new StoreMenuDiscountsDTO()
            {
                DiscountMenuID = e.DiscountMenuID,
                DiscountTitle = e.DiscountTitle,
                DiscountPercintage = e.DiscountPercintage,
                DiscountStartDate = e.DiscountStartDate,
                DiscountEndDate = e.DiscountEndDate,
                DiscountCode = e.DiscountCode,
                IsActive = e.IsActive,
                IsAllMenuDiscount = e.IsAllMenuDiscount,
                IsPercintage = e.IsPercintage,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
               

            });
        }
        public static StoreMenuDiscountsDTO ToOneStoreMenuDiscountssDTO(this StoreMenuDiscounts e)
        {
            return new StoreMenuDiscountsDTO()
            {
                DiscountMenuID = e.DiscountMenuID,
                DiscountTitle = e.DiscountTitle,
                DiscountPercintage = e.DiscountPercintage,
                DiscountStartDate = e.DiscountStartDate,
                DiscountEndDate = e.DiscountEndDate,
                DiscountCode = e.DiscountCode,
                IsActive = e.IsActive,
                IsAllMenuDiscount = e.IsAllMenuDiscount,
                IsPercintage = e.IsPercintage,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                

            };



        }
        public static IQueryable<DiscountsDTO> ToDiscountsDTO(this IQueryable<Discounts> item)
        {
            return item.Select(e => new DiscountsDTO()
            {
                DiscountID = e.DiscountID,
                DiscountTitle = e.DiscountTitle,
                DiscountPercintage = e.DiscountPercintage,
                DiscountStartDate = e.DiscountStartDate,
                DiscountEndDate = e.DiscountEndDate,
                DiscountCode = e.DiscountCode,
                IsActive = e.IsActive,
                MAXUserNum=e.MAXUserNum,
                IsPercintage=e.IsPercintage,
                 BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
              
            });
        }
        public static DiscountsDTO ToOneTDiscountsDTO(this Discounts e)
        {
            return new DiscountsDTO()
            {
                DiscountID = e.DiscountID,
                DiscountTitle = e.DiscountTitle,
                DiscountPercintage = e.DiscountPercintage,
                DiscountStartDate = e.DiscountStartDate,
                DiscountEndDate = e.DiscountEndDate,
                DiscountCode = e.DiscountCode,
                IsActive = e.IsActive,
                MAXUserNum = e.MAXUserNum,
                IsPercintage = e.IsPercintage,
                BranchID = e.BranchID,
                BrandID = e.BrandID,
                CompID = e.CompID,
                FranchID = e.FranchID,
                
            };



        }

    }
}
