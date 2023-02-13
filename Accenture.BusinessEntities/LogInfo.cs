using Accenture.Commun.TextHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Accenture.BusinessEntities
{
    public class LogInfo
    {
        public LogInfo()
        {
            
        }

        public int Id { get; set; }


        public string LogDate { get; set; }
        public string LogIp { get; set; }
        public string LogIdentification { get; set; }
        public string LogDescription { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }
        [JsonIgnore]
        public string Script { get; set; }

        public int? LogTypeId { get; set; }

        [JsonIgnore]
        public LogType LogType { get; set; }

        public List<string> ExecuteValidation()
        {
            var result = new List<string>();

            //if (this.UnitValue <= 0)
            //    result.Add("Erro!! É necessário informar um valor válido");

            //if (this.Quantity <= 0)
            //    result.Add("Erro!! É necessário informar uma quantidade válida");

            //if (result != null && result.Count > 0)
            //    throw new Exception(String.Join(";", result));

            return result;
        }
    }
}
