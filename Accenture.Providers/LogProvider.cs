using Accenture.BusinessEntities;
using Accenture.Commun.TextHelper;
using Accenture.ProvidersInterface;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Accenture.Providers
{
    public class LogProvider : ILogProvider
    {
        private IConfiguration _config;
        private string connString;
        private SqlConnection conn;

        public LogProvider(IConfiguration _configuuration)
        {
            _config = _configuuration;
            connString = _config.GetConnectionString("AccentureDb");
            conn = new SqlConnection(connString);
        }

        public LogInfo SaveLogInfo(LogInfo entity)
        {
            if(string.IsNullOrWhiteSpace(conn.ConnectionString))
                conn = new SqlConnection(connString);


            using (conn)
            {
                entity.Id = conn.QuerySingle<int>(@"Insert into LogInfo(LogDate, LogIp, LogIdentification, LogDescription) 
                                                OUTPUT INSERTED.Id 
                                                values(@LogDate, @LogIp, @LogIdentification, @LogDescription)",
                                        new
                                        {
                                            LogDate = entity.LogDate,
                                            LogIp = entity.LogIp,
                                            LogIdentification = entity.LogIdentification,
                                            LogDescription = entity.LogDescription
                                        });
                return entity;
            }
        }

        public LogInfosPaginationResponse GetLogInfosPagination(LogInfosPaginationRequest entity)
        {
            var result = new LogInfosPaginationResponse();

            if (string.IsNullOrWhiteSpace(conn.ConnectionString))
                conn = new SqlConnection(connString);


            using (conn)
            {
                var sql = @"Select  *,
                                    COUNT(*) OVER () as TotalRows

                                    from LogInfo

                                    Where 1 = 1
                            ";

                if (!string.IsNullOrWhiteSpace(entity.LogIdentification))
                    sql += " and Replace(Replace(LogIdentification, '[', '_'), ']', '_') like '{0}' ";

                if (!string.IsNullOrWhiteSpace(entity.Description))
                    sql += " and LogDescription like '{1}' ";

                if (!string.IsNullOrWhiteSpace(entity.LogIp))
                    sql += " and LogIp like '{3}' ";

                if (entity.LogTypeId != null)
                    sql += " and LogTypeId = {2} ";

                sql += @$"ORDER BY LogInfo.Id 
                            OFFSET({entity.Page} - 1) * {entity.RowsPerpage} ROWS
                            FETCH NEXT {entity.RowsPerpage} ROWS ONLY";

                var whereLogIdentification = NormalizeString.WhereLikeToSq(entity.LogIdentification);
                var whereLogDescription = NormalizeString.WhereLikeToSq(entity.Description);
                var whereLogIp = NormalizeString.WhereLikeToSq(entity.LogIp);

                var qeury = string.Format(sql,  /* @0 */ whereLogIdentification,
                                                /* @1 */ whereLogDescription,
                                                /* @2 */ entity.LogTypeId,
                                                /* @3 */ whereLogIp);

                var data = conn.Query<dynamic>(qeury).ToList();

                result.Page = entity.Page;
                result.RowsPerpage = entity.RowsPerpage;

                var firstItem = data.FirstOrDefault();

                result.TotalRows = (firstItem != null ? firstItem.TotalRows : 0);
                result.TotalPages =(result.TotalRows > 0 ? (int)(Math.Ceiling((decimal)result.TotalRows / (decimal)result.RowsPerpage)) : 0);

                result.Data = JsonSerializer.Deserialize<List<LogInfo>>(JsonSerializer.Serialize(data));
            }

            return result;
        }

        public List<ResumeLogInfo> GetResumeLogInfo()
        {
            var result = new List<ResumeLogInfo>();

            if (string.IsNullOrWhiteSpace(conn.ConnectionString))
                conn = new SqlConnection(connString);


            using (conn)
            {
                var sql = @"Select  LogInfo.LogTypeId,
                                    LogType.Name as 'LogType',
                                    Count(*) as 'Quantity',
                                    (select count(*) from LogInfo) as TotalRows

                                    from LogInfo

                                    join LogType LogType
                                    on LogType.Id = LogInfo.LogTypeId

                                    group by LogInfo.LogTypeId, LogType.Name

                                    order by Count(*) desc
                            ";

                result = conn.Query<ResumeLogInfo>(sql).ToList();
            }

            return result;
        }

        public List<LogType> GetLogTypes()
        {
            var result = new List<LogType>();

            if (string.IsNullOrWhiteSpace(conn.ConnectionString))
                conn = new SqlConnection(connString);


            using (conn)
            {
                var sql = @"Select * from LogType";

                result = conn.Query<LogType>(sql).ToList();
            }

            return result;
        }

        public void ExecuteMultipleInsert(string query)
        {
            if (string.IsNullOrWhiteSpace(conn.ConnectionString))
                conn = new SqlConnection(connString);

            using (conn)
            {
                conn.Execute(query);
            }
        }

        public void CleanLogInfo()
        {
            if (string.IsNullOrWhiteSpace(conn.ConnectionString))
                conn = new SqlConnection(connString);

            using (conn)
            {
                conn.Execute(@"truncate table LogInfo
                               DBCC CHECKIDENT('LogInfo', RESEED, 1)");
            }
        }

        
    }
}
