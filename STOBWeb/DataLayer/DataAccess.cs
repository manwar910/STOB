using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using Oracle.ManagedDataAccess.Client;

namespace STOBWeb
{
    [Serializable]
    public class DataAccess_Oracle

    {
        static string sql = "SELECT * From EMPLOYEES";
        static string sqldept = "SELECT * From Dept";
        //static string sql = "SELECT per.emplid AS emplid,per.last_name AS last_name, per.first_name AS first_name,per.email_address as email, pos.descr AS title,job.flsa_status As ExmStatus, job.sal_admin_plan,dep.descr as department FROM   adp.ps_personal_data per,adp.ps_job job,adp.ps_dept_tbl dep,adp.ps_employment emp, adp.ps_position_data pos, adp.ps_location_tbl loc WHERE per.emplid = job.emplid AND per.emplid = emp.emplid AND job.effdt = (SELECT MAX(effdt) FROM adp.ps_job WHERE emplid = per.emplid AND empl_rcd_nbr = 0 AND effdt <= TRUNC(SYSDATE)) AND job.EffSeq = (SELECT MAX(EffSeq) FROM adp.PS_Job WHERE EmplID = job.EmplID AND EffDt = job.EffDt) AND job.empl_rcd_nbr = 0 AND job.sal_admin_plan in ('GEN','SRO','S50','FLD','DOC') AND job.empl_status = 'A' AND emp.empl_rcd_nbr = '0' AND emp.hire_dt <= TRUNC(SYSDATE) AND job.deptid = dep.deptid  AND dep.effdt = (SELECT MAX(effdt) FROM adp.ps_dept_tbl WHERE deptid = job.deptid AND eff_status = 'A' AND effdt <= TRUNC(SYSDATE)) AND job.location = loc.location AND loc.effdt = (SELECT MAX(effdt) FROM adp.ps_location_tbl WHERE location = job.location AND eff_status = 'A' AND effdt <= TRUNC(SYSDATE)) AND loc.bldg = 'LOCAL' AND job.position_nbr = pos.position_nbr AND pos.effdt = (SELECT MAX(effdt) FROM adp.ps_position_data WHERE effdt <= TRUNC(SYSDATE) AND position_nbr = job.position_nbr) AND pos.effseq = (SELECT MAX(effseq) FROM adp.ps_position_data WHERE effdt = pos.effdt AND effdt <= TRUNC(SYSDATE) AND position_nbr = pos.position_nbr)";
        static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["oracleDb"].ConnectionString;
        //static string sqldept = "SELECT distinct dep.descr as department FROM  adp.ps_job job,adp.ps_dept_tbl dep,adp.ps_location_tbl loc  WHERE job.effdt = (SELECT MAX(effdt) FROM adp.ps_job WHERE emplid = job.emplid AND empl_rcd_nbr = 0 AND effdt <= TRUNC(SYSDATE))  AND job.EffSeq = (SELECT MAX(EffSeq) FROM adp.PS_Job WHERE EmplID = job.EmplID AND EffDt = job.EffDt)  AND job.empl_rcd_nbr = 0  AND job.sal_admin_plan in ('GEN','SRO','S50','FLD','DOC') AND job.empl_status = 'A' AND job.deptid = dep.deptid AND dep.effdt = (SELECT MAX(effdt) FROM adp.ps_dept_tbl WHERE deptid = job.deptid AND eff_status = 'A' AND effdt <= TRUNC(SYSDATE)) AND job.location = loc.location AND loc.effdt = (SELECT MAX(effdt) FROM adp.ps_location_tbl WHERE location = job.location AND eff_status = 'A' AND effdt <= TRUNC(SYSDATE)) AND loc.bldg = 'LOCAL'";
       
        public static DataTable GetDataSet()
        {

            DataSet ds = new DataSet();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sql, connection))
                {
                    using (var dataAdapter = new SqlDataAdapter(command))
                    {
                        command.CommandType = CommandType.Text;
                        dataAdapter.Fill(ds);
                    }
                }
            }

            return ds.Tables[0];
        }
        public static DataTable GetDataSetDept()
        {
            DataSet ds = new DataSet();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sqldept, connection))
                {
                    using (var dataAdapter = new SqlDataAdapter(command))
                    {
                        command.CommandType = CommandType.Text;
                        dataAdapter.Fill(ds);
                    }
                }
            }

            return ds.Tables[0];
        }

        public static DataTable GetDataSetSqlServer()
        {

            DataSet ds = new DataSet();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sql, connection))
                {
                    using (var dataAdapter = new SqlDataAdapter(command))
                    {
                        command.CommandType = CommandType.Text;
                        dataAdapter.Fill(ds);
                    }
                }
            }

            return ds.Tables[0];
        }

    }
}






