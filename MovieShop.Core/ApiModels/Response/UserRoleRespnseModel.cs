using MovieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.ApiModels.Response
{
   public class UserRoleRespnseModel
    {
        public int UserId { get; set; }
        public List<RoleResponseModel> Roles { get; set; }
    }


    public class RoleResponseModel
    {
        public int RoleId { get; set; }

        public string Name { get; set; }
    }
}
