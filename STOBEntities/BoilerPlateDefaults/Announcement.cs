using System;

namespace STOBEntities.BoilerPlateDefaults
{
    [Serializable()]
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementText { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
