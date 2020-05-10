using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.ApiModels.Response
{
   public class FavouriteResponseModel
    {
        public int UserId { get; set; }
        public List<MovieResponseModel> FavMovies { get; set; }
    }
}
