using System.Collections.Generic;
using STOBDataLayer.Models;
using STOBEntities.Models;
namespace STOBDataLayer.Interfaces
{
    public interface IAwardNameRepositoryDAL: IGenericRepository<AwardName_Admin>
    {
        AwardName_Admin GetAwardById(int id);
        IEnumerable<Awards> GetAllAwardNameAsync();
    }
}
