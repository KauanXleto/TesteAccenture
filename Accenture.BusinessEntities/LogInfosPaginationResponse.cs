using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accenture.BusinessEntities
{
    public class LogInfosPaginationResponse
    {
        public int Page { get; set; }
        public int RowsPerpage { get; set; }
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }

        public List<LogInfo> Data { get; set; }
    }
}
