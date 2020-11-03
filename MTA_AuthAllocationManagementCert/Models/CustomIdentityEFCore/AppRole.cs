using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AuthAllocationManagementCert.Models.CustomIdentityEFCore
{
    public class AppRole : IdentityRole<string>
    {
        public AppRole() : base()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        private AppRole(string roleName, int limitMember, string description)
        {
            Name = roleName;
            NormalizedName = roleName.ToUpper();
            LimitMember = limitMember;
            CreatedDate = DateTime.Now;
        }
        public string RootId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsOverView { get; set; }
        public string Permissions { get; set; }
        public int LinkTenantId { get; set; }
        public int LimitMember { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? DefaultRole { get; set; }
    }
}
