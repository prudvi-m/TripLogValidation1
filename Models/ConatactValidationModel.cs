using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TripLog.Models {

    public class ConatactValidationModel {

        // [RequiredContactInfoAttribute(ErrorMessage = "Either email or phone is required.")]
        [RequiredContactInfoAttribute]
        public string AccommodationPhone { get; set; }

        // [RequiredContactInfoAttribute(ErrorMessage = "Either email or phone is required.")]
        [RequiredContactInfoAttribute]
        [Remote("IsExisted", "tripomValidation")]
        public string AccommodationEmail { get; set; }

        public int PageNumber { get; set; }
    }
}