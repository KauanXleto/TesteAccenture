using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accenture.BusinessEntities
{
    public class LogInfosPaginationRequest
    {
        public string LogDate { get; set; }
        public string LogIdentification { get; set; }
        public int? LogTypeId { get; set; }
        public string LogIp { get; set; }
        public string Description { get; set; }

        public int Page { get; set; }
        public int RowsPerpage { get; set; }
    }
}
