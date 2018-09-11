using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Nackowskis.Models
{
    [DataContract]
    public class Auction
    {
        [DataMember]
        public int AuktionID { get; set; }
        [DataMember]
        [Required]
        [MinLength(3,ErrorMessage = "Title must be of at least 3 characters")]
        public string Titel { get; set; }
        [DataMember]
        [Required]
        public string Beskrivning { get; set; }
        [DataMember]
        public DateTime StartDatum { get; set; }
        [DataMember]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Deadline of auction is obviously required!")]
        public DateTime SlutDatum { get; set; }
        [DataMember]
        public int Gruppkod { get; set; }
        [DataMember]
        [Required]
        public int Utropspris { get; set; }
        [DataMember]
        public string SkapadAv { get; set; }
    }
}
