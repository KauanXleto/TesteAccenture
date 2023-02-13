using Accenture.BusinessEntities;
using Accenture.BusinessRulesInterface;
using Accenture.Commun.ListHelper;
using Accenture.Commun.TextHelper;
using Accenture.Providers;
using Accenture.ProvidersInterface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Accenture.BusinessRules
{
    public class LogBusinessRules : ILogBusinessRules
    {
        private IConfiguration _config;
        private string connString;
        ILogProvider LogProvider { get; set; }

        public LogBusinessRules(IConfiguration _configuuration,
                                ILogProvider _LogProvider)
        {
            _config = _configuuration;
            connString = _config.GetConnectionString("AccentureDb");
            this.LogProvider = _LogProvider;
        }

        public int SaveLogInfoFromFile(string FilePath)
        {
            var result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                //Pegando os logs do arquivo
                var ListLogInfo = this.GetLogInfoFromFile(FilePath);

                result = ListLogInfo.Count();

                if(result > 0)
                {
                    //Limpa tabela de logs para novos registros
                    this.LogProvider.CleanLogInfo();
                }

                //Separando inserts em 1000 linhas para o banco
                var listItens = ListExtensions.ChunkBy<string>(ListLogInfo.Select(x => x.Script).ToList(), 1000);

                foreach(var item in listItens)
                {
                    var queryMultipleInsert = string.Join("; ", item.Select(x => x).ToList());

                    //Inserindo no banco
                    this.LogProvider.ExecuteMultipleInsert(queryMultipleInsert);
                }

                scope.Complete();
            }

            return result;
        }

        public LogInfo SaveLogInfo(LogInfo entity)
        {
            entity.ExecuteValidation();                       

            return this.LogProvider.SaveLogInfo(entity);
        }

        private List<LogInfo> GetLogInfoFromFile(string FilePath)
        {
            List<LogInfo> result = new List<LogInfo>();

            IEnumerable<string> lines = File.ReadLines(FilePath);

            foreach(var LogInfoString in lines)
            {

                if (!string.IsNullOrWhiteSpace(LogInfoString))
                {
                    #region Preenchendo entidade do log
                    var logItem = new LogInfo();

                    if (LogInfoString.Length < 46)
                        continue;

                    var ipStartString = NormalizeString.StandardizeText(LogInfoString).IndexOf("ip");

                    if(ipStartString < 0 || LogInfoString.IndexOf("]:") < 0)
                        continue;

                    var IpIdentificationStart = ipStartString + 17;
                    var DescriptionStart = LogInfoString.IndexOf("]:") + 2;

                    var Date = LogInfoString.Substring(0, ipStartString);
                    var IpLog = LogInfoString.Substring(ipStartString, 16);
                    var IpIdentification = LogInfoString.Substring(IpIdentificationStart, (DescriptionStart - 1) - IpIdentificationStart);
                    var Description = LogInfoString.Substring(DescriptionStart, (LogInfoString.Length) - DescriptionStart);

                    logItem.LogDate = Date;
                    logItem.LogIp = IpLog;
                    logItem.LogIdentification = IpIdentification;
                    logItem.LogDescription = Description.TrimStart(':');

                    //Pega o tipo do log pela descrição
                    logItem.LogTypeId = (int)LogType.GetLogType(logItem.LogDescription);

                    //Preparando inserto no banco
                    logItem.Script += @$"Insert into LogInfo(LogDate, LogIp, LogIdentification, LogDescription, LogTypeId) 
                                                    values(
                                                    '{NormalizeString.PrepareToSqlInsert(logItem.LogDate)}', 
                                                    '{NormalizeString.PrepareToSqlInsert(logItem.LogIp)}', 
                                                    '{NormalizeString.PrepareToSqlInsert(logItem.LogIdentification)}', 
                                                    '{NormalizeString.PrepareToSqlInsert(logItem.LogDescription)}', 
                                                    {(int)logItem.LogTypeId})";

                    #endregion Preenchendo entidade do log

                    result.Add(logItem);
                }
            }

            return result;
        }

        public LogInfosPaginationResponse GetLogInfosPagination(LogInfosPaginationRequest entity)
        {
            return this.LogProvider.GetLogInfosPagination(entity);
        }

        public List<ResumeLogInfo> GetResumeLogInfo()
        {
            return this.LogProvider.GetResumeLogInfo();
        }
        public List<LogType> GetLogTypes()
        {
            return this.LogProvider.GetLogTypes();
        }
    }
}
