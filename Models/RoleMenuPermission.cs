namespace SelfKOrderWeb.Models
{
    public class RoleMenuPermission
    {


        public string RoleId { get; set; }

        public Guid NavigationMenuId { get; set; }

        public NavigationMenu NavigationMenu { get; set; }
    }
}
