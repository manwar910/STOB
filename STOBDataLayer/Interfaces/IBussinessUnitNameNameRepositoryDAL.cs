using STOBDataLayer.Model_DTO;
using STOBDataLayer.Models;
using STOBEntities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STOBDataLayer.Interfaces
{
    public interface IBussinessUnitNameNameRepositoryDAL : IGenericRepository<BussinesUnitName_Admin>
    {
        IEnumerable<BUHeads> GetAllBussinessUnitNameAsync();

        int SaveTrans(BUViewModelDTO bu);
        int UpdateTrans(BUViewModelDTO bu);

        IEnumerable<BUDepartments> GetAllBussinessUnitDepartAsync();

        int DeleteDepart(int BU_DEPT_ID);

    }
}
