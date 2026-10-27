using SelfKOrderWeb.Areas.Identity.Data;

namespace SelfKOrderWeb.DTO
{
    public class CatItemDTO
    {
        public CategoryDTO obj { get; set; }= new CategoryDTO();
        public  IEnumerable< ItemDTO> unAssignedItem { get; set; }=new List<ItemDTO>();
        public IEnumerable<ModifierCategoryDTO> unAssignedItem2 { get; set; } = new List<ModifierCategoryDTO>();
        public IEnumerable<ModifierDTO> unAssignedItem3 { get; set; } = new List<ModifierDTO>();
        public IEnumerable<ItemDTO> AdsList { get; set; } = new List<ItemDTO>();
        public int categoryID { get; set; }
        public int KiosikID { get; set; }
        public int? SortOrder { get; set; }
    }
}
