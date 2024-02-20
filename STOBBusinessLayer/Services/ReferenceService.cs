using System.Collections.Generic;
using System.Linq;
using STOBBusinessLayer.Services.Interfaces;
using STOBDataLayer.Repositories.BoilerPlateDefaults;
using STOBEntities.BoilerPlateDefaults;
using STOBDataLayer.Models.BoilerPlateDefaults;

namespace STOBBusinessLayer.Services
{
    public class ReferenceService : IReferenceService
    {
        private FaqRepository _faqRepo;
        private AnnouncementsRepository _announcementsRepo;
        private ReleaseNotesRepository _releaseNotesRepo;

        public ReferenceService(FaqRepository faqRepo, AnnouncementsRepository announcementsRepo, ReleaseNotesRepository releaseNotesRepo)
        {
            _faqRepo = faqRepo;
            _announcementsRepo = announcementsRepo;
            _releaseNotesRepo = releaseNotesRepo;
        }

        public void Dispose()
        {
        }

        #region faqs
        //public IEnumerable<Faq> GetFaqs(bool showDeletes)
        //{
        //    var faqs = _faqRepo.GetAllModels(showDeletes);
        //    return faqs.OrderBy(o => o.SortOrderNumber).ToList();
        //}
        IEnumerable<FAQ> IReferenceService.GetFaqs(bool showDeletes)
        {
            var faqs = _faqRepo.GetAllModels(showDeletes);
            return (IEnumerable<FAQ>)faqs.OrderBy(o => o.SortOrderNumber).ToList();
        }
        public void InsertNewFaq(Faq newFaq)
        {
            _faqRepo.AddModel(ref newFaq);
        }

        public Faq GetFaqEdit(int faqId)
        {
            return _faqRepo.FindModelById(faqId);
        }

        public void PostFaqEdit(Faq editedFaq)
        {
            _faqRepo.EditModel(ref editedFaq, editedFaq.FaqId);
        }
        #endregion

        #region release notes
        public IEnumerable<ReleaseNotes> GetRelsNotes(bool showDeletes)
        {
            var notes = _releaseNotesRepo.GetAllModels(showDeletes);
            return notes.OrderByDescending(n => n.ReleaseDate).ToList();
        }

        public void InsertNewRelsNotes(ReleaseNotes newRelsNotes)
        {
            _releaseNotesRepo.AddModel(ref newRelsNotes);
        }

        public ReleaseNotes GetRelsNotesEdit(int relsNotesId)
        {
            return _releaseNotesRepo.FindModelById(relsNotesId);
        }

        public void PostRelsNotesEdit(ReleaseNotes editedRelsNotes)
        {
            _releaseNotesRepo.EditModel(ref editedRelsNotes, editedRelsNotes.ReleaseNotesId);
        }
        #endregion

        #region announcements
        public IEnumerable<Announcement> GetAnncmnts(bool currentOnly)
        {
            var announcements = _announcementsRepo.GetAllModels(currentOnly);
            return announcements.OrderByDescending(a => a.AnnouncementText).ToList();
        }

        public void InsertNewAnncmt(Announcement newAnncmnt)
        {
            _announcementsRepo.AddModel(ref newAnncmnt);
        }

        public Announcement GetAnncmntEdit(int anncmntId)
        {
            return _announcementsRepo.FindModelById(anncmntId);
        }

        public void PostAnncmntEdit(Announcement editedAnncmnt)
        {
            _announcementsRepo.EditModel(ref editedAnncmnt, editedAnncmnt.AnnouncementId);
        }

        public IEnumerable<FAQ> GetFaqs(bool showDeletes)
        {
            throw new System.NotImplementedException();
        }

      
        Faq IReferenceService.GetFaqEdit(int faqId)
        {
            throw new System.NotImplementedException();
        }

       

        IEnumerable<ReleaseNotes> IReferenceService.GetRelsNotes(bool showDeletes)
        {
            throw new System.NotImplementedException();
        }

      

        ReleaseNotes IReferenceService.GetRelsNotesEdit(int relsNotesId)
        {
            throw new System.NotImplementedException();
        }

       

        IEnumerable<Announcement> IReferenceService.GetAnncmnts(bool currentOnly)
        {
            throw new System.NotImplementedException();
        }

      

        Announcement IReferenceService.GetAnncmntEdit(int anncmntId)
        {
            throw new System.NotImplementedException();
        }

       

        #endregion
    }
}
