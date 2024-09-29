using System;
using System.ComponentModel.DataAnnotations;

namespace PatientScreeningSystem.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(0, 120, ErrorMessage = "Please enter a valid age.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Aadhaar Number is required.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhaar Number must be 12 digits.")]
        public string AadhaarNumber { get; set; }

        [Required(ErrorMessage = "Reason for Visit is required.")]
        public string ReasonForVisit { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Blood Group is required.")]
        public string BloodGroup { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }

        public DateTime EntryTime { get; set; }
    }
}
