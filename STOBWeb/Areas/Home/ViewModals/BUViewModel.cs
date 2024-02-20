using STOBDataLayer.Models;
using System.Collections.Generic;

namespace STOBWeb.ViewModals
{
    public class BUViewModelList
    {
        public IEnumerable<BUHeads> BU { get; set; }
        public IEnumerable<BUDepartments> BU_DEPT { get; set; }
    }

}
