namespace UserRoles.ViewModels
{
    public class OrgNodeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public List<OrgNodeViewModel> Children { get; set; }
            = new List<OrgNodeViewModel>();
    }
}
