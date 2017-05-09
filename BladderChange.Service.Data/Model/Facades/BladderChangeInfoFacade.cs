using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using Oracle.ManagedDataAccess.Client;

using log4net;
using BladderChange.Service.Data.Model.Entities;

namespace BladderChange.Service.Data.Model.Facades
{
    public class BladderChangeInfoFacade
    {
        private readonly static ILog _logger = LogManager.GetLogger(typeof(BladderChangeInfoFacade).Name);

        /// <summary>
        /// Get the list of currently active machines from DB
        /// </summary>
        /// <returns></returns>
        public List<BladderChangeInfo> GetActiveMachineList()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [machine_no]" +
                ",[size]" +
                ",[bladder_name_left]" +
                ",[bladder_limit_left]" +
                ",[bladder_count_left]" +
                ",[last_change_left]" +
                ",[bladder_name_right]" +
                ",[bladder_limit_right]" +
                ",[bladder_count_right]" +
                ",[last_change_right]" +
                ",[status]" +
                ",[ins_date]" +
                ",[upd_date]" +
                "  FROM [dbo].[cur_bladder_change]" +
                "  WHERE status = 1");

            var list = new List<BladderChangeInfo>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BTMVLocalApps"].ConnectionString))
                {
                    using (var cmd = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                var info = new BladderChangeInfo
                                {
                                    MachineNo = reader.GetString(0),
                                    Size = reader.IsDBNull(1) ? null : reader.GetString(1),                                    
                                    BladderNameLeft = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    BladderLimitLeft = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                    BladderCountLeft = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                    LastChangeLeft = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                    BladderNameRight = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    BladderLimitRight = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                    BladderCountRight = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                                    LastChangeRight = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                                    Status = reader.IsDBNull(10) ? false : reader.GetBoolean(10),
                                    InsDate = reader.GetDateTime(11),
                                    UpdDate = reader.GetDateTime(12)
                                };
                                list.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error querrying data:", ex);
            }
            return list;
        }

        /// <summary>
        /// Update the data back to DB after getting lastest info
        /// </summary>
        /// <param name="infoList"></param>
        public int UpdateBladderChangeInfo(List<BladderChangeInfo> infoList)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE cur_bladder_change " +
                "SET size = @size" +
                ", bladder_name_left = @bladder_name_left" +
                ", bladder_limit_left = @bladder_limit_left" +
                ", bladder_count_left = @bladder_count_left" +
                ", last_change_left = @last_change_left" +
                ", bladder_name_right = @bladder_name_right" +
                ", bladder_limit_right = @bladder_limit_right" +
                ", bladder_count_right = @bladder_count_right" +
                ", last_change_right = @last_change_right" +
                ", upd_date = @upd_date" +
                " WHERE machine_no = @machine_no");

            int updateCount = 0;
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BTMVLocalApps"].ConnectionString))
                {
                    using (var cmd = new SqlCommand(sqlBuilder.ToString(), connection))
                    {
                        connection.Open();
                        //Init the parameters                        
                        cmd.Parameters.Add(new SqlParameter("size", SqlDbType.VarChar,10));
                        cmd.Parameters.Add(new SqlParameter("bladder_name_left", SqlDbType.VarChar,20));
                        cmd.Parameters.Add(new SqlParameter("bladder_limit_left", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("bladder_count_left", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("last_change_left", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("bladder_name_right", SqlDbType.VarChar,20));
                        cmd.Parameters.Add(new SqlParameter("bladder_limit_right", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("bladder_count_right", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("last_change_right", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("upd_date", SqlDbType.DateTime));
                        cmd.Parameters.Add(new SqlParameter("machine_no", SqlDbType.VarChar,10));

                        foreach (var info in infoList)
                        {
                            if (info.IsModified)
                            {                                
                                cmd.Parameters["size"].Value = info.Size;
                                cmd.Parameters["bladder_name_left"].Value = info.BladderNameLeft;
                                cmd.Parameters["bladder_limit_left"].Value = info.BladderLimitLeft;
                                cmd.Parameters["bladder_count_left"].Value = info.BladderCountLeft;
                                cmd.Parameters["last_change_left"].Value = info.LastChangeLeft;
                                cmd.Parameters["bladder_name_right"].Value = info.BladderNameRight;
                                cmd.Parameters["bladder_limit_right"].Value = info.BladderLimitRight;
                                cmd.Parameters["bladder_count_right"].Value = info.BladderCountRight;
                                cmd.Parameters["last_change_right"].Value = info.LastChangeRight;
                                cmd.Parameters["upd_date"].Value = info.UpdDate;
                                cmd.Parameters["machine_no"].Value = info.MachineNo;

                                updateCount += cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating info:", ex);                
            }

            return updateCount;
        }

        /// <summary>
        /// Querying the lastest info from CTMT DB 
        /// </summary>
        /// <param name="infoList"></param>
        public void GetLastestBladderChangeInfo(List<BladderChangeInfo> infoList)
        {
            var machineList = BuildMachineNoString(infoList);

            var bladderCountQuery = new StringBuilder();
            bladderCountQuery.Append("SELECT FR1.MACHINENO, CURERESULT_SPARE003_R BLADDER_COUNT_R,CURERESULT_SPARE004_R BLADDER_LIMIT_R,CURERESULT_SPARE003_L BLADDER_COUNT_L,CURERESULT_SPARE004_L BLADDER_LIMIT_L, FR1.DATARCVTIME " +
                "FROM FILE_RESULT FR1 JOIN " +
                "(SELECT MAX(DATARCVTIME) RCVTIME, TRIM(MACHINENO) MNO " +
                "FROM FILE_RESULT ");
            bladderCountQuery.Append("WHERE TRIM(MACHINENO) IN ");
            bladderCountQuery.Append(machineList);
            bladderCountQuery.Append(" GROUP BY TRIM(MACHINENO) " +
                "ORDER BY TRIM(MACHINENO)) FR2 " +
                "ON FR1.DATARCVTIME = FR2.RCVTIME AND TRIM(FR1.MACHINENO) = FR2.MNO " +
                "WHERE TRIM(FR1.MACHINENO) IN  " );
            bladderCountQuery.Append(machineList);
            bladderCountQuery.Append(" ORDER BY FR1.MACHINENO ");

            var bladderNameQuery = new StringBuilder();
            bladderNameQuery.Append("select FP.MACHINENO, FP.PRODUCTIONCODE_R,FP.IF_BLADDERNAME_R BLADDERNAME_R, FP.PRODUCTIONCODE_L,FP.IF_BLADDERNAME_L BLADDERNAME_L,FP.DATARCVTIME " +
                "from FILE_PROCSEND FP JOIN " +
                "(SELECT MAX(DATARCVTIME), MACHINENO FROM FILE_PROCSEND ");
            bladderNameQuery.Append("WHERE TRIM(MACHINENO) IN ");
            bladderNameQuery.Append(machineList);
            bladderNameQuery.Append(" GROUP BY MACHINENO) FP1 ON FP.MACHINENO = FP1.MACHINENO " +
                "WHERE TRIM(FP.MACHINENO) IN  ");
            bladderNameQuery.Append(machineList);
            bladderNameQuery.Append(" ORDER BY FP.MACHINENO");
            
            try
            {
                using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["CTMT"].ConnectionString))
                {
                    using(var cmd = new OracleCommand(bladderCountQuery.ToString(), connection))
                    {
                        connection.Open();
                        _logger.Debug("Bladder count query:" + bladderCountQuery.ToString());
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string machineNo = reader.GetString(0);
                                var info = infoList.FirstOrDefault(x => x.MachineNo.Equals(machineNo.Trim()));
                                if (info != null)
                                {
                                    int newBladderCountRight = reader.GetInt32(1);
                                    int newBladderLimitRight = reader.GetInt32(2);                                    
                                    int newBladderCountLeft = reader.GetInt32(3);
                                    int newBladderLimitLeft = reader.GetInt32(4);
                                    DateTime lastUpdate = reader.GetDateTime(5);

                                    //if the new bladder count is much different from the current count, 
                                    // bladder change may have occured
                                    int deltaLeft = newBladderCountLeft - info.BladderCountLeft;
                                    if (deltaLeft < 0 || deltaLeft > 2)
                                    {
                                        info.LastChangeLeft = info.BladderCountLeft;
                                    }

                                    int deltaRight = newBladderCountRight - info.BladderCountRight;
                                    if (deltaRight < 0 || deltaRight > 2)
                                    {
                                        info.LastChangeRight = info.BladderCountRight;
                                    }

                                    info.BladderCountRight = newBladderCountRight;
                                    info.BladderLimitRight = newBladderLimitRight;
                                    info.BladderCountLeft = newBladderCountLeft;
                                    info.BladderLimitLeft = newBladderLimitLeft;
                                    info.UpdDate = lastUpdate;

                                    info.IsModified = true;
                                }
                            }
                        }
                    }
                    
                    using (var cmd = new OracleCommand(bladderNameQuery.ToString(), connection))
                    {
                        _logger.Debug("Bladder name query:" + bladderNameQuery.ToString());
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string machineNo = reader.GetString(0);
                                var info = infoList.FirstOrDefault(x => x.MachineNo.Equals(machineNo.Trim()));
                                if (info != null)
                                {
                                    string productionCode = reader.GetString(1);
                                    string bladderName = reader.GetString(2);

                                    info.Size = productionCode;
                                    info.BladderNameLeft = bladderName;
                                    info.BladderNameRight = bladderName;

                                    info.IsModified = true;
                                }
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting lastest bladder info:", ex);                
            }
        }

        /// <summary>
        /// Generate the string contains the list of machines to use in SQL query
        /// </summary>
        /// <param name="infoList"></param>
        /// <returns></returns>
        private string BuildMachineNoString(List<BladderChangeInfo> infoList)
        {
            var builder = new StringBuilder();
            builder.Append("(");

            foreach (var info in infoList)
            {
                builder.Append("'");
                builder.Append(info.MachineNo);
                builder.Append("',");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Utility method to be called one time to setup master data
        /// </summary>
        public int CreateMachineList()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO [dbo].[cur_bladder_change] " +
                "([machine_no]" +
                ",[size]" +
                ",[bladder_name_left]" +
                ",[bladder_limit_left]" +
                ",[bladder_count_left]" +
                ",[last_change_left]" +
                ",[bladder_name_right]" +
                ",[bladder_limit_right]" +
                ",[bladder_count_right]" +
                ",[last_change_right]" +
                ",[status]" +
                ",[ins_date]" +
                ",[upd_date]) " +
                " VALUES" +
                "(@machine_no" +
                ",null" +
                ",null" +
                ",0" +
                ",0" +
                ",0" +
                ",null" +
                ",0" +
                ",0" +
                ",0" +
                ",0" +
                ",GETDATE()" +
                ",GETDATE())");

            int machineCount = 0;
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BTMVLocalApps"].ConnectionString))
                {
                    using (var cmd = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("machine_no", ""));

                        for (int line = 1; line <=7; line++)
                        {
                            for(int machine = 1; machine <=13; machine++)
                            {                                
                                var machineNo = $@"{line:00}-{machine:00}";
                                cmd.Parameters["machine_no"].Value = machineNo;                                
                                int count = cmd.ExecuteNonQuery();
                                machineCount += count;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Data processing error:", ex);                
            }

            return machineCount;            
        }
    }
}
