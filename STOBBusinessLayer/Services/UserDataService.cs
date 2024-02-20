using System.Linq;
using STOBBusinessLayer.Utility;
using STOBDataLayer.Contexts;
using STOBDataLayer.Models.BoilerPlateDefaults;
using STOBEntities.Models;


namespace STOBBusinessLayer.Services
{
    public class UserDataService
    {
        private BoilerPlateDefaults db = new BoilerPlateDefaults();
        private const string USER_TYPE = "User";

        public void Dispose()
        {
            db.Dispose();
        }

        public void CheckForUser(string username, string firstName, string lastName, string emailAddress,
            string phoneNumber)
        {
            using (Logger log = new Logger(new object[] {username, firstName, lastName, emailAddress,
            phoneNumber}))
            {
                //Add the users deets into the model for use later in the LeaveAdmin application.
                var userDataModel = new UserDataModel();
                userDataModel.Username = username;
                userDataModel.FirstName = firstName;
                userDataModel.LastName = lastName;
                userDataModel.EmailAddress = emailAddress;
                userDataModel.PhoneNumber = phoneNumber;

                //Check if the user is in the Database already. 
                var userexists = db.USERS.Any(x => x.USER_NM == username);

                //If user exists in the DB already, update their data and move on with life.
                if (userexists)
                {
                    //Get the Party record from the DB
                    var updateUser = db.USERS.SingleOrDefault(x => x.USER_NM == username);
                    //Get and assign the partId to the model. 
                    userDataModel.UserId = updateUser.USER_ID;
                    //Update any fields that are relevant.
                    updateUser.FIRST_NM = userDataModel.FirstName;
                    updateUser.LAST_NM = userDataModel.LastName;
                    updateUser.USER_NM = userDataModel.Username;
                    updateUser.PH_NO = userDataModel.PhoneNumber;
                    updateUser.EMAIL_ADDR_TXT = userDataModel.EmailAddress;
                    //Save the updated Data to the database. 
                    db.SaveChanges();
                }
                //If user does not exist in the DB already, insert the user and move on with life.  
                else
                {
                    //Insert user into the party table. 
                    var insertNewUser = new USERS();
                    insertNewUser.FIRST_NM = userDataModel.FirstName;
                    insertNewUser.LAST_NM = userDataModel.LastName;
                    insertNewUser.USER_NM = userDataModel.Username;
                    insertNewUser.PH_NO = userDataModel.PhoneNumber;
                    insertNewUser.EMAIL_ADDR_TXT = userDataModel.EmailAddress;
                    db.USERS.Add(insertNewUser);
                    db.SaveChanges();
                }
            }
        }

    }
}
