using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentUI.Components.Domain
{
    public class DoctorWithAvailabilitiesDto
    {
        public ApplicationUser Doctor { get; set; }
    public List<ProfessionalAvailability> Availabilities { get; set; }
    }
}