using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBird.Application.Model.PagingModel;

namespace EBird.Application.Model.Bird
{
    public class BirdParameters : QueryStringParameters
    {
       public string SortElo { get; set; } = "none";
    }
}
