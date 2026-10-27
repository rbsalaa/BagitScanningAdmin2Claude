using SelfKOrderWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SelfKOrderWeb.DTO
{
    
        public class ItemCombined
        {
        public ItemDTO obj { get; set; } = new ItemDTO();
        public IEnumerable<ItemDTO> list { get; set; } = new List<ItemDTO>();
    }
    }
