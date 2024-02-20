using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STOBEntities.Models
{
  public  class NominationGraphAdmin
    {
        public string Dimention { get; set; }
        public int statusvalue { get; set; }
    }
    public class StackedViewModel
    {
        public string StackedDimensionOne { get; set; }
        public List<NominationGraphAdmin> LstData { get; set; }
    }
}
