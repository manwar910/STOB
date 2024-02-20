using STOBDataLayer.Contexts;
using System.Linq;

namespace STOBBusinessLayer.Utility
{
    public class ReleaseInformation
    {
        private BoilerPlateDefaults db = new BoilerPlateDefaults();

        public string ReturnlastUpdatedDate()
        {
            var lastUpdatedDateObj = db.RELS_NOTES.OrderByDescending(x => x.RELS_DT).FirstOrDefault();
            //Barrett var lastUpdatedDate = lastUpdatedDateObj != null ? lastUpdatedDateObj.RELS_DT.Value.ToString("yyyy/MM/dd") : "";
            var lastUpdatedDate = lastUpdatedDateObj != null ? lastUpdatedDateObj.RELS_DT.ToString("yyyy/MM/dd") : "";

            return lastUpdatedDate;
        }
        public string ReturnCurrentVersion()
        {
            var currentVersionObj = db.RELS_NOTES.OrderByDescending(x => x.RELS_DT).FirstOrDefault();
            string currentVersion = currentVersionObj != null ? currentVersionObj.RELS_VER_NO.ToString() : "";


            return currentVersion;
        }

    }
}
