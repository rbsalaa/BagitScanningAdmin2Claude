using SelfKOrderWeb.Models;

namespace SelfKOrderWeb.DTO
{
    public class AspNavigationMenuDTO
    {
        public List<string> CodesList { get; set; }
        public string FatherRoleID { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }
        public string code { get; set; }

        public Guid? ParentMenuId { get; set; }

        public virtual NavigationMenu ParentNavigationMenu { get; set; }

        public string Area { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool IsExternal { get; set; }

        public string ExternalUrl { get; set; }

        public int DisplayOrder { get; set; }


        public bool Permitted { get; set; }

        public bool Visible { get; set; }
    }
}
