using STOBDataLayer.Contexts;
using STOBDataLayer.Interfaces;
using STOBDataLayer.Models;
using STOBEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace STOBDataLayer.Repositories.STOB
{
    public class AwardNameRepositoryDAL : IAwardNameRepositoryDAL
    {
        private STOBContext _dbcontext = null;
        public AwardNameRepositoryDAL()
        {
            _dbcontext = new STOBContext();
        }
        public int AddAsync(AwardName_Admin entity)
        {
            int result;
            try
            {
                Awards award = new Awards
                {
                    AWARD_NM = entity.AWARD_NM,
                    AWARD_DESC = entity.AWARD_DESC,
                    AWARD_TYPE = entity.AWARD_TYPE,
                    AWARD_EXEMPTION = entity.AWARD_EXEMPTION
                };

                _dbcontext.Awards.Add(award);
                result = _dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                result = ex.InnerException.HResult;
            }
            return result;
        }

        public int DeleteAsync(int id)
        {
            int result;
            try
            {
                var award = _dbcontext.Awards.Find(id);
                _dbcontext.Awards.Remove(award);
                _dbcontext.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {
                result = e.InnerException.HResult;
            }
            return result;
        }

        public IEnumerable<Awards> GetAllAwardNameAsync()
        {
            var sList = _dbcontext.Awards.ToList();
            return sList;
        }

        public AwardName_Admin GetAwardById(int id)
        {
            var award = _dbcontext.Awards.Find(id);
            AwardName_Admin model = new AwardName_Admin
            {
                AWARD_ID = award.AWARD_ID,
                AWARD_NM = award.AWARD_NM,
                AWARD_DESC = award.AWARD_DESC,
                AWARD_TYPE = award.AWARD_TYPE,
                AWARD_EXEMPTION = award.AWARD_EXEMPTION
            };
            return model;
        }

        public int UpdateAsync(AwardName_Admin entity)
        {
            int result;
            try
            {
                var award = _dbcontext.Awards.Find(entity.AWARD_ID);
                award.AWARD_NM = entity.AWARD_NM;
                award.AWARD_TYPE = entity.AWARD_TYPE;
                award.AWARD_DESC = entity.AWARD_DESC;
                award.AWARD_EXEMPTION = entity.AWARD_EXEMPTION;
                result = _dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                result = ex.InnerException.HResult;
            }
            return result;
        }
    }
}
