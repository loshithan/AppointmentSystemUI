using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentUI.Components.Domain
{
  public class Appointment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PatientId { get; set; }  // Foreign Key for Patient
        public string ProfessionalId { get; set; }   // Foreign Key for Doctor
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        // Foreign Key for ProfessionalAvailability
        public Guid? ProfessionalAvailabilityId { get; set; }
        public ProfessionalAvailability? ProfessionalAvailability { get; set; }

    }
     public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Canceled
    }
}