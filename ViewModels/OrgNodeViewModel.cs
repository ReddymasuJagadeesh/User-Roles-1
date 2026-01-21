   
namespace UserRoles.ViewModels
        {
            public class OrgChartNodeViewModel
            {
                public string Id { get; set; } = string.Empty;
                public string? ParentUserId { get; set; }
                public string Name { get; set; } = string.Empty;
                public string Role { get; set; } = string.Empty;
            }
        }
   