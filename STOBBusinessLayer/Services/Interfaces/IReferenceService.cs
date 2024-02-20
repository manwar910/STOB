using STOBEntities.BoilerPlateDefaults;
using STOBDataLayer.Models.BoilerPlateDefaults;
using System.Collections.Generic;

namespace STOBBusinessLayer.Services.Interfaces
{
    public interface IReferenceService : IService
    {
        IEnumerable<FAQ> GetFaqs(bool showDeletes);
        void InsertNewFaq(Faq newFaq);
        Faq GetFaqEdit(int faqId);
        void PostFaqEdit(Faq editedFaq);
        IEnumerable<ReleaseNotes> GetRelsNotes(bool showDeletes);
        void InsertNewRelsNotes(ReleaseNotes newRelsNotes);
        ReleaseNotes GetRelsNotesEdit(int relsNotesId);
        void PostRelsNotesEdit(ReleaseNotes editedRelsNotes);
        IEnumerable<Announcement> GetAnncmnts(bool currentOnly);
        void InsertNewAnncmt(Announcement newAnncmnt);
        Announcement GetAnncmntEdit(int anncmntId);
        void PostAnncmntEdit(Announcement editedAnncmnt);
    }
}
