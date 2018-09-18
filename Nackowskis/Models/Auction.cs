using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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
        [MaxLength(20,ErrorMessage = "Maxlength for Title is 20 characters")]
        public string Titel { get; set; }

        [DataMember]
        [Required]
        [MaxLength(255,ErrorMessage = "Max length is 255 characters")]
        public string Beskrivning { get; set; }

        [DataMember]
        public DateTime StartDatum { get; set; }

        [DataMember]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Deadline of auction is obviously required!")]
        [DateRange]
        public DateTime SlutDatum { get; set; }

        [DataMember]
        public int Gruppkod { get; set; }

        [DataMember]
        [Required]
        [RegularExpression("[0-9]*$",ErrorMessage = "Estimate price must be of a numerical value")]
        public int Utropspris { get; set; }

        [DataMember]
        public string SkapadAv { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    internal sealed class DateRangeAttribute : ValidationAttribute
    {
        public DateTime Minimum { get; }
        public DateTime Maximum { get; }

        public DateRangeAttribute(string minimum = null, string maximum = null, string format = null)
        {
            format = format ?? @"yyyy-MM-dd'T'HH:mm:ss.FFFK"; //iso8601

            Minimum = DateTime.Today;
            Maximum = DateTime.Today.AddYears(2);

            if (Minimum > Maximum)
                throw new InvalidOperationException($"Specified max-date '{maximum}' is less than the specified min-date '{minimum}'");
        }
        //0 the sole reason for employing this custom validator instead of the mere rangevalidator is that we wanted to apply invariantculture to the parsing instead of
        //  using currentculture like the range attribute does    this is immensely important in order for us to be able to dodge nasty hiccups in production environments

        public override bool IsValid(object value)
        {
            if (value == null) //0 null
                return true;

            var s = value as string;
            if (s != null && string.IsNullOrEmpty(s)) //0 null
                return true;

            var min = (IComparable)Minimum;
            var max = (IComparable)Maximum;
            return min.CompareTo(value) <= 0 && max.CompareTo(value) >= 0;
        }
        //0 null values should be handled with the required attribute

        public override string FormatErrorMessage(string name) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum, Maximum);
    }

}
