using System;
using System.ComponentModel.DataAnnotations;

namespace PatientScreeningSystem.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }  // Auto-generated primary key

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 120)]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string AadhaarNumber { get; set; }

        public DateTime EntryTime { get; set; }

        [Required]
        public string ReasonForVisit { get; set; }
    }
}
