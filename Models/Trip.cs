using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace TripLog.Models
{
    public class Trip
    {
        public int TripId { get; set; }

        [StringLength(30)]
        [Required(ErrorMessage = "Please enter a destination.")]
        [RegularExpression(@"[a-zA-Z\s]*$")]
        public string Destination { get; set; }

        [DateLessThan("EndDate", ErrorMessage = "Start date must be less than end Date")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the date your trip ends.")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Please enter an Accommodation.")]
        [RegularExpression(@"[a-zA-Z0-9\s]*$")]
        public string Accommodation { get; set; }

        public string AccommodationPhone { get; set; }

        [Remote("IsExisted", "tripomValidation")]
        public string AccommodationEmail { get; set; }

        public string ThingToDo1 { get; set; }
        public string ThingToDo2 { get; set; }
        public string ThingToDo3 { get; set; }
    }

    public class DateLessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            if(!Object.ReferenceEquals(value, null)) {
                var currentValue = (DateTime)value;
                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
                if (property == null)
                    throw new ArgumentException("Property with this name not found");
                var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
                if (currentValue > comparisonValue)
                    return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
            return new ValidationResult("Start Date is required.");
        }
    }

    public class DateMinimumAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateMinimumAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            if(!Object.ReferenceEquals(value, null)) {
                var currentValue = (DateTime)value;
                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
                if (property == null)
                    throw new ArgumentException("Property with this name not found");
                var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
                if (currentValue >= DateTime.Today)
                    return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
            return new ValidationResult("End date is required.");

        }
    }

    public class RequiredContactInfoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object v,ValidationContext c) {
            var trip = (ConatactValidationModel)c.ObjectInstance;
            if (string.IsNullOrEmpty(trip.AccommodationPhone) && string.IsNullOrEmpty(trip.AccommodationEmail))
            {
                string msg = base.ErrorMessage ??
                "Enter phone number or email.";
                return new ValidationResult(msg);
            }
            else {
                return ValidationResult.Success;
            }
        }
    }
}



