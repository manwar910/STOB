using System.Linq;
using STOBDataLayer.Models.BoilerPlateDefaults;
using STOBEntities.BoilerPlateDefaults;

namespace STOBDataLayer.Repositories.BoilerPlateDefaults
{
    public class UserRepository : GenericRepository<Contexts.BoilerPlateDefaults, USERS, Users>
    {

        public string GetFirstLastNameById(int userId)
        {
            var entireUser = FindModelById(userId);
            return entireUser.FirstName + " " + entireUser.LastName;
        }

        public Users GetUserByName(string username)
        {
            //Check if there is an anon user name in the system. If not, Insert one, then move forward. This only exists because we nuke data so much and no one will rememeber to re-enter the anon user. 
            if (username == "anonymous")
            {
                var anon = FindModelsBy(x => x.USER_NM == "anonymous" && x.DELETE_DT == null).SingleOrDefault();

                if (anon == null)
                {
                    var anonUser = new Users()
                    {
                        FirstName = "anonymous",
                        LastName = "anonymous",
                        UserName = "anonymous"
                    };

                    AddModel(ref anonUser);

                    return anonUser;
                }
            }

            return FindModelsBy(x => x.USER_NM == username && x.DELETE_DT == null).SingleOrDefault();
        }

        public override Users ConvertToModel(USERS entity)
        {
            Users user = new Users()
            {
                UserId = entity.USER_ID,
                FirstName = entity.FIRST_NM,
                LastName = entity.LAST_NM,
                UserName = entity.USER_NM,
                PhoneNumber = entity.PH_NO,
                EmailAddress = entity.EMAIL_ADDR_TXT,
                DeleteDate = entity.DELETE_DT
            };
            return user;
        }

        public override USERS ConvertToDbObject(Users entity)
        {
            var us = new USERS()
            {
                USER_ID = entity.UserId,
                FIRST_NM = entity.FirstName,
                LAST_NM = entity.LastName,
                USER_NM = entity.UserName,
                PH_NO = entity.PhoneNumber,
                EMAIL_ADDR_TXT = entity.EmailAddress,
                DELETE_DT = entity.DeleteDate
            };
            return us;
        }
    }
}
