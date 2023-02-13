using Accenture.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accenture.BusinessRulesInterface
{
    public interface ILogBusinessRules
    {

        LogInfo SaveLogInfo(LogInfo FilePath);
        int SaveLogInfoFromFile(string FilePath);
        LogInfosPaginationResponse GetLogInfosPagination(LogInfosPaginationRequest entity);
        List<ResumeLogInfo> GetResumeLogInfo();
        List<LogType> GetLogTypes();

    }
}
