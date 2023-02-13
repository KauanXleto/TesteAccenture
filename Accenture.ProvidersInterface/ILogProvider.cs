using Accenture.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accenture.ProvidersInterface
{
    public interface ILogProvider
    {
        LogInfo SaveLogInfo(LogInfo entity);
        LogInfosPaginationResponse GetLogInfosPagination(LogInfosPaginationRequest entity);
        List<ResumeLogInfo> GetResumeLogInfo();
        List<LogType> GetLogTypes();

        void ExecuteMultipleInsert(string query);
        void CleanLogInfo();
    }
}
