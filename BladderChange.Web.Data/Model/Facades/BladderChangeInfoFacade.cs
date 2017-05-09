using System;
using System.Collections.Generic;

using System.Configuration;
using System.Data.SqlClient;
using BladderChange.Web.Data.Model.Entities;

namespace BladderChange.Web.Data.Model.Facades
{
    public class BladderChangeInfoFacade
    {
        /// <summary>
        /// Return the list of active machines with bladder info
        /// </summary>
        /// <returns></returns>
        public List<BladderChangeInfo> GetLastestBladderChangeInfoList()
        {
            var sql = "SELECT [machine_no]" +
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
                "  FROM[dbo].[cur_bladder_change]" +
                "  WHERE status = 1";

            var list = new List<BladderChangeInfo>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BTMVLocalApps"].ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var info = new BladderChangeInfo {
                                    MachineNo = reader.GetString(0),
                                    Size = reader.IsDBNull(1)?null:reader.GetString(1),
                                    BladderNameLeft = reader.IsDBNull(2)?null:reader.GetString(2),
                                    BladderLimitLeft = reader.IsDBNull(3)?0:reader.GetInt32(3),
                                    BladderCountLeft = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                    LastChangeLeft = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                    BladderNameRight = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    BladderLimitRight = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                    BladderCountRight = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                                    LastChangeRight = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),                                    
                                    Status = reader.GetBoolean(10),
                                    InsDate = reader.GetDateTime(11),
                                    UpdDate = reader.GetDateTime(12)
                                };

                                list.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {                
                throw;
            }

            return list;
        }
    }
}
