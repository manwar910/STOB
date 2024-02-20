using System;

namespace STOBEntities.BoilerPlateDefaults
{
    [Serializable]
    public class ReleaseNotes
    {
        public int ReleaseNotesId { get; set; }
        public decimal ReleaseVersionNumber { get; set; }
        public string ReleaseNotesText { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
