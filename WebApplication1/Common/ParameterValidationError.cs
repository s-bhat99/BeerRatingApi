using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Common
{
    public class ParameterValidationError
    {
        public ParameterValidationError(string parameterName, string reason)
        {
            ParameterName = parameterName;
            Reason = reason;
        }

        /// <summary>
        /// Name of parameter that was unable to validate.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string ParameterName { get; set; }

        /// <summary>
        /// Reason why parameter was unable to validate.
        /// </summary>
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }
    }
}
