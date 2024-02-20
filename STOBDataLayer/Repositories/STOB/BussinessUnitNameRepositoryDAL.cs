using STOBDataLayer.Contexts;
using STOBDataLayer.Interfaces;
using STOBDataLayer.Model_DTO;
using STOBDataLayer.Models;
using STOBEntities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace STOBDataLayer.Repositories.STOB
{
    public class BussinessUnitNameRepositoryDAL : IBussinessUnitNameNameRepositoryDAL
    {
        private STOBContext _dbcontext = null;
        public BussinessUnitNameRepositoryDAL()
        {
            _dbcontext = new STOBContext();
        }

        public int SaveTrans(BUViewModelDTO bu)
        {
            int result;
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var emp = _dbcontext.Employees.First(e => e.EMAIL == bu.BU_EMAIL);
                    bu.BU_NM = emp.LAST_NAME + ", " + emp.FIRST_NAME;

                    BUHeads buHead = new BUHeads
                    {
                        BU_NM = bu.BU_NM,
                        BU_EMAIL = bu.BU_EMAIL
                    };
                    _dbcontext.BUHeads.Add(buHead);
                    result = _dbcontext.SaveChanges();

                    BUDepartments buDept = new BUDepartments
                    {
                        BU_DEPT = bu.BU_DEPT,
                        BU_ID = buHead.BU_ID,
                        IS_BU_DEPT = true
                    };

                    _dbcontext.BUDepartments.Add(buDept);
                    result = _dbcontext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    result = ex.InnerException.HResult;
                    transaction.Rollback();
                }
            }
            return result;
        }

        public int UpdateTrans(BUViewModelDTO model)
        {
            int result = 0;
            try
            {
                BUHeads buHead = _dbcontext.BUHeads.First(b => b.BU_ID == model.BU_ID);

                //Add New Departments
                if (model.DEPTS != null)
                {
                    foreach (var d in model.DEPTS)
                    {
                        var buHeadDept = new BUDepartments
                        {
                            BU_DEPT = d,
                            BU_ID = buHead.BU_ID,
                            IS_BU_DEPT = false
                        };
                        _dbcontext.BUDepartments.Add(buHeadDept);
                        _dbcontext.SaveChanges();
                    }
                    result = 1;
                }
            }
            catch (DbEntityValidationException ex)
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
                var buhead = _dbcontext.BUHeads.Find(id);
                _dbcontext.BUHeads.Remove(buhead);
                _dbcontext.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {
                result = e.InnerException.HResult;
            }
            return result;
        }

        public int DeleteDepart(int BU_DEPT_ID)
        {
            int result;
            try
            {
                var dept = _dbcontext.BUDepartments.Find(BU_DEPT_ID);
                var buHead = dept.BUHead;
                _dbcontext.BUDepartments.Remove(dept);
                result = _dbcontext.SaveChanges();
                if (result > 0)
                { 
                    //Get Departments for BuHead
                    var buDepts = _dbcontext.BUDepartments.Where(d=>d.BU_ID == dept.BU_ID).ToList();
                    //If no department exits then remove BuHead also
                    if (!buDepts.Any())
                    {
                        _dbcontext.BUHeads.Remove(buHead);
                        _dbcontext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                result = e.InnerException.HResult;
            }
            return result;
        }

        public IEnumerable<BUHeads> GetAllBussinessUnitNameAsync()
        {
            var buHeads = _dbcontext.BUHeads.ToList();
            return buHeads;
        }
        public IEnumerable<BUDepartments> GetAllBussinessUnitDepartAsync()
        {
            var buHeadDepts = _dbcontext.BUDepartments.ToList();
            return buHeadDepts;
        }

        public int UpdateAsync(BussinesUnitName_Admin entity)
        {
            throw new NotImplementedException();
        }

        public int AddAsync(BussinesUnitName_Admin entity)
        {
            throw new NotImplementedException();
        }
    }
}
