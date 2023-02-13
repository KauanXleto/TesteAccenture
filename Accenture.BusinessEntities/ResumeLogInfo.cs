using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Accenture.BusinessEntities
{
    public class ResumeLogInfo
    {
        public string LogTypeId { get; set; }
        public string LogType { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public int TotalRows { get; set; }

        public decimal Percent { 
            get 
            {
                decimal result = 0;

                if (Quantity > 0 && TotalRows > 0)
                    result = Math.Round(((decimal)(Quantity * 100) / (decimal)TotalRows), 2);
                else
                    result = 0;

                return result;
            } 
        }
    }
}
