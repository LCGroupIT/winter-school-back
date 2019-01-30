using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Model
{
    public class ApiPassport
    {
        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("secondname")]
        public string Secondname { get; set; }

        [JsonIgnore]
        public string Sex { get; set; }

        [JsonProperty("issuedby")]
        public string IssuedBy { get; set; }

        [JsonProperty("issuedon")]
        public DateTime IssuedOn { get; set; }

        [JsonProperty("issueddepartment")]
        public string IssuedDepartment { get; set; }

        [JsonProperty("series")]
        public string Series { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("birthday")]
        public DateTime Birthday { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
