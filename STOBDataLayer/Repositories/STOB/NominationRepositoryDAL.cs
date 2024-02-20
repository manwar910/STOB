using STOBDataLayer.Contexts;
using STOBDataLayer.Interfaces;
using STOBDataLayer.Models;
using STOBDataLayer.ViewModels;
using STOBEntities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace STOBDataLayer.Repositories.STOB
{
    public class NominationRepositoryDAL : INominationRepositoryDAL
    {
        private STOBContext _dbcontext = null;
        public string UserName { get; set; }
        public string NominatorFullName { get; set; }

        public string LoginUserEmail { get; set; }

        public NominationRepositoryDAL()
        {
            _dbcontext = new STOBContext();
        }


        //public int AddNomination(NominationVM entity, int btnStatus)
        //{
        //    int result = 0;
        //    using (var transaction = _dbcontext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            //Nominees
        //            foreach (var nomineeId in entity.NomineeIds)
        //            {
        //                var emp = _dbcontext.Employees.Find(nomineeId);
        //                var nominee = new Nominees
        //                {
        //                    NOMN_ID = nomId,
        //                    EMP_ID = emp.ID,
        //                    FIRST_NM = emp.FIRST_NAME,
        //                    LAST_NM = emp.LAST_NAME,
        //                    TITLE_TXT = emp.TITLE,
        //                    BU_Dept = emp.DEPARTMENT,
        //                    EMAIL = emp.EMAIL,
        //                    EXEMPT_CD = emp.EXMSTATUS
        //                };
        //                _dbcontext.Nominees.Add(nominee);
        //            }
        //            //NominationStatuses
        //            NominationStatuses nomStatus = new NominationStatuses
        //            {
        //                //NOMN_ID = nomId,
        //                STAT_ID = btnStatus,   //Drafted or Submitted
        //                BY = entity.NOMNR_NM,
        //                DATE = DateTime.Now,
        //                COMMENT = btnStatus == 1 ? "Nomination drafted." : "Nomination submitted for further processing.",
        //            };

        //            //Save Nomination
        //            Nominations nomination = new Nominations
        //            {
        //                AWARD_ID = entity.AWARD_ID,
        //                NOMNR_EMPL_ID = entity.NOMNR_EMPL_ID,
        //                NOMNR_NM = entity.NOMNR_NM,
        //                NOMNR_EMAIL = entity.NOMNR_EMAIL,
        //                ACTV_FLG = entity.ACTV_FLG,
        //                TEAM_NM = entity.TEAM_NM,
        //                NOMNR_BU_DEPT = entity.NOMNR_BU_DEPT,
        //                NOMN_RSN = entity.COMMENT,
        //                NOMN_DT = DateTime.Now,
        //                NominationStatuses = new List<NominationStatuses> { nomStatus },
        //                Nominees = entity.Nominees.ToList()
        //            };

        //            _dbcontext.Nominations.Add(nomination);
        //            _dbcontext.SaveChanges();
        //            int nomId = nomination.NOMN_ID;
        //            result = nomId;
        //            transaction.Commit();
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            transaction.Rollback();
        //            Exception raise = ex;
        //            foreach (var validationErrors in ex.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
        //                    raise = new InvalidOperationException(message, raise);
        //                }
        //            }
        //            throw raise;
        //        }
        //    }
        //    return result;
        //}

        //public int AddNomination(NominationVM model, int btnStatus)
        //{
        //    int result = 0;
        //    string cs = ConfigurationManager.ConnectionStrings["stob_Entities"].ConnectionString;

        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();

        //        using (var transaction = con.BeginTransaction())
        //        {
        //            try
        //            {
        //                int nomId;
        //                var sql = "INSERT INTO ASSOC_AWARD_NOMN(" +
        //                    "AWARD_ID,NOMNR_EMPL_ID,NOMNR_NM,NOMNR_EMAIL,ACTV_FLG,TEAM_NM,NOMNR_BU_DEPT,NOMN_RSN,NOMN_DT) " +
        //                    "VALUES(@AWARD_ID,@NOMNR_EMPL_ID,@NOMNR_NM,@NOMNR_EMAIL,@ACTV_FLG,@TEAM_NM,@NOMNR_BU_DEPT,@NOMN_RSN,@NOMN_DT) " +
        //                    "SELECT CAST(scope_identity() AS int)";
        //                using (var cmd = new SqlCommand(sql, con, transaction))
        //                {
        //                    cmd.Parameters.AddWithValue("@AWARD_ID", model.AWARD_ID);
        //                    cmd.Parameters.AddWithValue("@NOMNR_EMPL_ID", model.NOMNR_EMPL_ID);
        //                    cmd.Parameters.AddWithValue("@NOMNR_NM", model.NOMNR_NM);
        //                    cmd.Parameters.AddWithValue("@NOMNR_EMAIL", model.NOMNR_EMAIL);
        //                    cmd.Parameters.AddWithValue("@ACTV_FLG", true);
        //                    cmd.Parameters.AddWithValue("@TEAM_NM", (object)model.TEAM_NM ?? DBNull.Value);
        //                    cmd.Parameters.AddWithValue("@NOMNR_BU_DEPT", model.NOMNR_BU_DEPT);
        //                    cmd.Parameters.AddWithValue("@NOMN_RSN", model.COMMENT);
        //                    cmd.Parameters.AddWithValue("@NOMN_DT", DateTime.Now);
        //                    nomId = (Int32)cmd.ExecuteScalar();
        //                }

        //                sql = "INSERT INTO NOMN_STATUSES(NOMN_ID, STAT_ID, [BY], DATE, COMMENT) " +
        //                    "VALUES(@NOMN_ID, @STAT_ID, @BY, @DATE, @COMMENT)";
        //                using (var cmd = new SqlCommand(sql, con, transaction))
        //                {
        //                    cmd.Parameters.AddWithValue("@NOMN_ID", nomId);
        //                    cmd.Parameters.AddWithValue("@STAT_ID", btnStatus);
        //                    cmd.Parameters.AddWithValue("@BY", model.NOMNR_NM);
        //                    cmd.Parameters.AddWithValue("@DATE", DateTime.Now);
        //                    cmd.Parameters.AddWithValue("@COMMENT", btnStatus == 1 ? "Nomination drafted." : "Nomination submitted for further processing.");
        //                    cmd.ExecuteNonQuery();
        //                }
        //                var query = String.Format("Select * from EMPLOYEES where ID in ({0})", string.Join(",", model.NomineeIds));
        //                SqlCommand cmdEmp = new SqlCommand(query, con, transaction);
        //                SqlDataReader empReader = cmdEmp.ExecuteReader();
        //                while (empReader.Read())
        //                {
        //                    sql = "INSERT INTO NOMINEES(NOMN_ID, EMP_ID, FIRST_NM, LAST_NM, TITLE_TXT, BU_Dept, EMAIL, EXEMPT_CD) " +
        //                                                                "VALUES(@NOMN_ID, @EMP_ID, @FIRST_NM, @LAST_NM, @TITLE_TXT, @BU_Dept, @EMAIL, @EXEMPT_CD)";

        //                    using (var cmd = new SqlCommand(sql, con, transaction))
        //                    {
        //                        cmd.Parameters.AddWithValue("@NOMN_ID", nomId);
        //                        cmd.Parameters.AddWithValue("@EMP_ID", empReader["ID"]);
        //                        cmd.Parameters.AddWithValue("@FIRST_NM", empReader["FIRST_NAME"]);
        //                        cmd.Parameters.AddWithValue("@LAST_NM", empReader["LAST_NAME"]);
        //                        cmd.Parameters.AddWithValue("@TITLE_TXT", empReader["TITLE"]);
        //                        cmd.Parameters.AddWithValue("@BU_Dept", empReader["DEPARTMENT"]);
        //                        cmd.Parameters.AddWithValue("@EMAIL", empReader["EMAIL"]);
        //                        cmd.Parameters.AddWithValue("@EXEMPT_CD", empReader["EXMSTATUS"]);
        //                        result = cmd.ExecuteNonQuery();
        //                    }
        //                }

        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //            }
        //        }
        //    }

        //    return result;
        //}

        public async Task<int> AddNomination(NominationVM entity, int btnStatus)
        {
            int result = 0;
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    //Save Nomination
                    Nominations nomination = new Nominations
                    {
                        AWARD_ID = entity.AWARD_ID,
                        NOMNR_EMPL_ID = entity.NOMNR_EMPL_ID,
                        NOMNR_NM = entity.NOMNR_NM,
                        NOMNR_EMAIL = entity.NOMNR_EMAIL,
                        ACTV_FLG = entity.ACTV_FLG,
                        TEAM_NM = entity.TEAM_NM,
                        NOMNR_BU_DEPT = entity.NOMNR_BU_DEPT,
                        NOMN_RSN = entity.COMMENT,
                        NOMN_DT = DateTime.Now
                    };
                    _dbcontext.Nominations.Add(nomination);
                    await _dbcontext.SaveChangesAsync();
                    int nomId = nomination.NOMN_ID;

                    //Save NominationStatus 
                    NominationStatuses nomStatus = new NominationStatuses
                    {
                        NOMN_ID = nomId,
                        STAT_ID = btnStatus,    //Drafted or Submitted
                        BY = entity.NOMNR_NM,
                        DATE = DateTime.Now,
                        COMMENT = btnStatus == 1 ? "Nomination drafted." : "Nomination submitted for further processing.",
                    };
                    _dbcontext.NominationStatuses.Add(nomStatus);

                    //Save Nominees
                    foreach (var n in entity.Nominees)
                        n.NOMN_ID = nomId;
                    _dbcontext.Nominees.AddRange(entity.Nominees);

                    await _dbcontext.SaveChangesAsync();
                    result = nomId;
                    transaction.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    transaction.Rollback();
                    Exception raise = ex;
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            return result;
        }
        public int SubmitById(int NomId, string Nominator)  //Submit from DraftNominationPartial
        {
            int result;
            try
            {
                //Save NominationStatus 
                NominationStatuses nomStatus = new NominationStatuses
                {
                    NOMN_ID = NomId,
                    STAT_ID = 2,    //Submitted
                    DATE = DateTime.Now,
                    BY = Nominator,
                    COMMENT = "Submitted for further processing."
                };
                _dbcontext.NominationStatuses.Add(nomStatus);
                result = _dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public int UpdateAsync(Associate_Awards_Nominations entity)
        {
            throw new NotImplementedException();
        }
        public async Task<int> UpdateNomination(NominationVM entity, string type, string modifier)
        {            
            int result = 0;
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    //Update Nomination table
                    var nomination = _dbcontext.Nominations.FirstOrDefault(n=>n.NOMN_ID == entity.NOMN_ID);
                    nomination.AWARD_ID = entity.AWARD_ID;
                    nomination.ACTV_FLG = true;
                    nomination.TEAM_NM = entity.TEAM_NM;
                    if (modifier == "Nominator" || modifier == "Admin") nomination.NOMN_RSN = entity.REASON;

                    //Save NominationStatus only when it is submitted 
                    if (entity.STAT_ID > 1)
                    {
                        NominationStatuses nomStatus = new NominationStatuses
                        {
                            NOMN_ID = entity.NOMN_ID,
                            DATE = DateTime.Now,
                            COMMENT = entity.COMMENT,
                            STAT_ID = 4,  //Modified
                            BY = entity.NOMNR_NM //Any logged-in user (Nominator, 
                        };
                        _dbcontext.NominationStatuses.Add(nomStatus);
                    }

                    //Update Nominees table
                    if (type == "Individual")
                    {
                        if (entity.Nominees != null)
                        {
                            var odlNominee = _dbcontext.Nominees.Where(n => n.NOMN_ID == entity.NOMN_ID).FirstOrDefault();
                            var newNominee = entity.Nominees.FirstOrDefault();
                            //Check whether new Nominee is selected
                            if (odlNominee.EMP_ID != newNominee.EMP_ID)
                            {
                                //Remove Old Nominee
                                _dbcontext.Nominees.Remove(odlNominee);

                                //Add New Nominee
                                newNominee.NOMN_ID = nomination.NOMN_ID;
                                _dbcontext.Nominees.Add(newNominee);
                            }
                        }
                    }
                    else
                    {
                        //Existing Nominees of Nomination
                        var nominees = _dbcontext.Nominees.Where(n => n.NOMN_ID == entity.NOMN_ID).ToList();

                        List<Nominees> nomineesToRemove = new List<Nominees>();
                        if (entity.Nominees != null)
                        {
                            //Nominees to Remove
                            nomineesToRemove = nominees.Where(n => entity.Nominees.Any(a => a.ID != n.ID)).ToList();
                            _dbcontext.Nominees.RemoveRange(nomineesToRemove);
                            await _dbcontext.SaveChangesAsync();

                            //Nominees to Add
                            var nomineesToAdd = entity.Nominees.Where(x => nomineesToRemove.All(y => y.ID != x.ID));
                            foreach (var n in nomineesToAdd)
                                n.NOMN_ID = entity.NOMN_ID;
                            _dbcontext.Nominees.AddRange(nomineesToAdd);
                        }
                    }
                    await _dbcontext.SaveChangesAsync();
                    transaction.Commit();
                    result = 1;
                }
                catch (DbEntityValidationException ex)
                {
                    transaction.Rollback();
                    Exception raise = ex;
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            return result;
        }
        public IEnumerable<NominationVM> GetAllNominations()
        {
            var noms = (from n in _dbcontext.Nominations
                        let currentStatus = n.NominationStatuses.FirstOrDefault(s=>s.NOMN_STAT_ID == n.NominationStatuses.Max(d=>d.NOMN_STAT_ID))
                        select new NominationVM
                        {
                            NOMN_ID = n.NOMN_ID,
                            NOMNR_NM = n.NOMNR_NM,
                            NOMNR_BU_DEPT = n.NOMNR_BU_DEPT,
                            NOMNR_EMAIL = n.NOMNR_EMAIL,
                            TEAM_NM = n.TEAM_NM,
                            Award_Name = n.Award.AWARD_NM,
                            Award_Type = n.Award.AWARD_TYPE,
                            DATE = n.NOMN_DT,
                            STAT_ID = currentStatus.STAT_ID,
                            STAT_TXT = currentStatus.Status.STAT_TXT,
                            STAT_BY = currentStatus.BY,
                            COMMENT = currentStatus.COMMENT,
                            Nominees = n.Nominees.ToList(),
                        }).ToList();

            return noms;
        }
        
        public List<NomineesExcelVM> GetNomineesForBuHeadByType(string type, string buEmail)
        {
            var buDepts = _dbcontext.BUHeads.Where(h => h.BU_EMAIL == buEmail)?.FirstOrDefault().BUDepartments.ToList();
            var buDeptsList = buDepts?.Select(d => d.BU_DEPT).ToList();

            var nomList = _dbcontext.Nominations.Where(n => n.Award.AWARD_TYPE == type).ToList();

            //If "Team" then match BU departments with Nominator's department
            if (type == "Team")
                nomList = nomList.Where(n => buDeptsList.Any(d => d == n.NOMNR_BU_DEPT)).ToList();

            List<NomineesExcelVM> nomineesList = new List<NomineesExcelVM>();
            nomList.ForEach(nom =>
            {
                var nomNominees = nom.Nominees.ToList();
                nomNominees.ForEach(n =>
                {
                    NomineesExcelVM newNominee = new NomineesExcelVM();
                    newNominee.NomineeFirstName = n.FIRST_NM;
                    newNominee.NomineeLastName = n.LAST_NM;
                    newNominee.Title = n.TITLE_TXT;
                    newNominee.Department = n.BU_Dept;
                    newNominee.Exemption = n.EXEMPT_CD;
                    newNominee.Email = n.EMAIL;
                    newNominee.Award = nom.Award.AWARD_NM;
                    newNominee.TeamName = nom.TEAM_NM;
                    newNominee.Nominator = nom.NOMNR_NM;
                    newNominee.NomDate = nom.NOMN_DT.Date;
                    newNominee.Award = nom.Award.AWARD_NM;
                    newNominee.AwardType = nom.Award.AWARD_TYPE;
                    newNominee.Status = nom.NominationStatuses.LastOrDefault().Status.STAT_TXT;
                    nomineesList.Add(newNominee);
                });
            });

            //If "Individual" then match BU departments with Nominees' departments
            if (type == "Individual")
                nomineesList = nomineesList.Where(n => buDeptsList.Any(d => d == n.Department)).ToList();

            return nomineesList;
        }
        public List<NomineesExcelVM> GetNomineesForAdminByType(string type)
        {
            var nomList = _dbcontext.Nominations.Where(n => n.Award.AWARD_TYPE == type).ToList();
            List<NomineesExcelVM> nomineesList = new List<NomineesExcelVM>();
            nomList.ForEach(nom =>
            {
                var nomNominees = nom.Nominees.ToList();
                nomNominees.ForEach(n =>
                {
                    NomineesExcelVM newNominee = new NomineesExcelVM();
                    newNominee.NomineeFirstName = n.FIRST_NM;
                    newNominee.NomineeLastName = n.LAST_NM;
                    newNominee.Title = n.TITLE_TXT;
                    newNominee.Department = n.BU_Dept;
                    newNominee.Exemption = n.EXEMPT_CD;
                    newNominee.Email = n.EMAIL;
                    newNominee.Award = nom.Award.AWARD_NM;
                    newNominee.TeamName = nom.TEAM_NM;
                    newNominee.Nominator = nom.NOMNR_NM;
                    newNominee.NomDate = nom.NOMN_DT.Date;
                    newNominee.Award = nom.Award.AWARD_NM;
                    newNominee.AwardType = nom.Award.AWARD_TYPE;
                    newNominee.Status = nom.NominationStatuses.LastOrDefault().Status.STAT_TXT;
                    nomineesList.Add(newNominee);
                });
            });

            return nomineesList;
        }

        public IEnumerable<NominationVM> GetNominationsByUserName(string UserFullName)
        {
            var noms = (from n in _dbcontext.Nominations
                        where n.NOMNR_NM == UserFullName
                        let nomStatus = n.NominationStatuses.OrderByDescending(d => d.NOMN_STAT_ID).FirstOrDefault()
                        select new NominationVM
                        {
                           NOMN_ID = n.NOMN_ID,
                           NOMNR_NM = n.NOMNR_NM,
                           NOMNR_BU_DEPT = n.NOMNR_BU_DEPT,
                           NOMNR_EMAIL = n.NOMNR_EMAIL,
                           TEAM_NM = n.TEAM_NM,
                           Award_Name = n.Award.AWARD_NM,
                           Award_Type = n.Award.AWARD_TYPE,
                           DATE = n.NOMN_DT,
                           STAT_ID = nomStatus.STAT_ID,
                           STAT_BY = nomStatus.BY,
                           STAT_TXT = nomStatus.Status.STAT_TXT,
                           REASON = n.NOMN_RSN,
                           COMMENT = nomStatus.COMMENT,
                           Nominees = n.Nominees.ToList(),
                        }).ToList();

            return noms;
        }
        public IEnumerable<BuHeadNominationsVM> GetAllBuHeadNominations(string email)
        {
            var buHead = _dbcontext.BUHeads.FirstOrDefault(h => h.BU_EMAIL == email);
            var buHeadDepts = buHead.BUDepartments.Select(d => d.BU_DEPT).ToList();

            if (buHead == null) return Enumerable.Empty<BuHeadNominationsVM>();
            List<Nominations> noms = _dbcontext.Nominations.ToList();
            List<BuHeadNominationsVM> buHeadNomList = new List<BuHeadNominationsVM>();
            noms.ForEach(n =>
            {
                if (n.NominationStatuses.LastOrDefault()?.STAT_ID > 1)   //exclude drafted
                {
                    BuHeadNominationsVM buHeadNom = new BuHeadNominationsVM();

                    if (n.Award.AWARD_TYPE == "Individual")
                    {
                        var nomNominee = n.Nominees;
                        if (buHeadDepts.Any(d => d == nomNominee.FirstOrDefault()?.BU_Dept))
                        {
                            buHeadNom.Nomination = n;
                            buHeadNom.Nominees = nomNominee;
                            buHeadNomList.Add(buHeadNom);
                        }
                    }
                    else if (n.Award.AWARD_TYPE == "Team")
                    {
                        if (buHeadDepts.Any(d => d == n.NOMNR_BU_DEPT))
                        {
                            buHeadNom.Nomination = n;
                            buHeadNom.Nominees = n.Nominees.ToList();
                            buHeadNomList.Add(buHeadNom);
                        }
                    }
                }
            });
            return buHeadNomList;
        }
        public int AddApprovalFlowByBuHead(int NominationId, string buHeadName)
        {
            int result;
            try
            {
                //Save NominationStatus 
                NominationStatuses nomStatus = new NominationStatuses
                {
                    NOMN_ID = NominationId,
                    STAT_ID = 5,    //Approved
                    BY = buHeadName + " [BuHead]",     //Any logged-in user (Nominator, 
                    DATE = DateTime.Now,
                    COMMENT = "Approved by Business Unit Head."
                };
                _dbcontext.NominationStatuses.Add(nomStatus);
                _dbcontext.SaveChanges();
                result = 1;
            }
            catch (DbEntityValidationException ex)
            {
                Exception raise = ex;
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return result;
        }
        public int AddRejectedFlowByBuHead(int NominationId, string buHeadName, string reason)
        {
            int result;
            try
            {
                //Save NominationStatus 
                NominationStatuses nomStatus = new NominationStatuses
                {
                    NOMN_ID = NominationId,
                    STAT_ID = 7,    //Rejected
                    BY = buHeadName + " [BuHead]",     //Any logged-in user (Nominator, 
                    DATE = DateTime.Now,
                    COMMENT = reason
                };
                _dbcontext.NominationStatuses.Add(nomStatus);
                _dbcontext.SaveChanges();
                result = 1;
            }
            catch (DbEntityValidationException ex)
            {
                Exception raise = ex;
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return result;
        }
        
        #region Admin Approval
        public int AddApprovalFlowByAdmin(int NominationId, string adminName)
        {
            int result;
            try
            {
                //Save NominationStatus 
                NominationStatuses nomStatus = new NominationStatuses
                {
                    NOMN_ID = NominationId,
                    STAT_ID = 6,        //Approved
                    BY = adminName + " [Admin]",     //Any logged-in user (Nominator, 
                    DATE = DateTime.Now,
                    COMMENT = "I approve this nomination."
                };
                _dbcontext.NominationStatuses.Add(nomStatus);
                _dbcontext.SaveChanges();
                result = 1;
            }
            catch (DbEntityValidationException ex)
            {
                Exception raise = ex;
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return result;
        }
        #endregion

        #region Admin Rejection
        public int RejectedFlowByAdmin(int NominationId, string adminName, string reason)
        {
            int result;
            try
            {
                //Save NominationStatus 
                NominationStatuses nomStatus = new NominationStatuses
                {
                    NOMN_ID = NominationId,
                    STAT_ID = 7,    //Rejected
                    BY = adminName + " [Admin]",     //Any logged-in user (Nominator, buHead, Admin)
                    DATE = DateTime.Now,
                    COMMENT = reason
                };
                _dbcontext.NominationStatuses.Add(nomStatus);
                _dbcontext.SaveChanges();
                result = 1;
            }
            catch (DbEntityValidationException ex)
            {
                Exception raise = ex;
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return result;
        }
        #endregion

        public int UpdateReviewStatus(ReviewVM model)
        {
            int result;
            try
            {
                //Save NominationStatus 
                NominationStatuses nomStatus = new NominationStatuses
                {
                    NOMN_ID = model.NominationId,
                    STAT_ID = 3,    //Review
                    DATE = DateTime.Now,
                    BY = model.ReviewerName,
                    COMMENT = model.ReviewText
                };
                _dbcontext.NominationStatuses.Add(nomStatus);
                result = _dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int AddAsync(Associate_Awards_Nominations entity)
        {
            throw new NotImplementedException();
        }

        public int DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
