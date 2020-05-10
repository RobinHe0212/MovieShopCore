using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.ApiModels.Request
{
   public class FavouriteRequestModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
    }
}
