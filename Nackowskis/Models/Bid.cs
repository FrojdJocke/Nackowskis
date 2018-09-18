using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Nackowskis.Models
{
    [DataContract]
    public class Bid
    {
        [DataMember]
        public int BudID { get; set; }
        [DataMember]
        [RegularExpression("^[0-9]*$",ErrorMessage = "Bid must be a numerical value")]
        public int Summa { get; set; }
        [DataMember]
        public int AuktionID { get; set; }
        [DataMember]
        public string Budgivare { get; set; }
    }
}
