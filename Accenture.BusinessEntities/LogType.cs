using Accenture.Commun.TextHelper;
using System;
using System.Collections.Generic;
using System.IO;

namespace Accenture.BusinessEntities
{
    public class LogType
    {
        public LogType()
        {
            
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public enum ELogType
        {
            unknown = 1,
            Received_disconnect,
            Invalid_user,
            Reverse_mapping_checking,
            Many_authentication_failures,
            Connection_reset,
            Connection_closed,
            Did_not_receive_identification_string,
            Session_closed,
            Session_opened,
            Does_not_map_back_to_the_address,
            Corrupted_MAC_on_input,
            Could_not_write_ident_string,
            Bad_protocol_version_identification
        }

        public static ELogType GetLogType(string Description)
        {
            var result = ELogType.unknown;

            if (!string.IsNullOrWhiteSpace(Description))
            {
                foreach (var name in Enum.GetNames(typeof(ELogType)))
                {
                    if (StringCompare.ContainsString(Description, name.ToString().Replace("_", " ")))
                    {
                        result = (ELogType)Enum.Parse(typeof(ELogType), name);
                        continue;
                    }
                }
            }

            return result;
        }
    }
}
