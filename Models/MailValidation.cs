using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace TripLog.Models
{
    public static class MailValidation
    {
        public static string EmailDupplicateCheck(TripLogContext context,string email)
        {
            string errorMessage = "";
            if (!string.IsNullOrEmpty(email))
            {
                var trip = context.Trips.Any(c => c.AccommodationEmail.ToLower() == email.ToLower());
                if (trip.Equals(null)) {
                    errorMessage = $"Email address {email} already in use.";
                }
            }
            return errorMessage;
        }
    }
}
