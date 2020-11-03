using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AuthAllocationManagementCert.Models
{
    public class RoleModel
    {
    }
    public class AddRoleReq
    {
        public Guid? RoleID { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
