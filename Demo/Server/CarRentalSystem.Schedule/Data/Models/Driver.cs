namespace CarRentalSystem.Schedule.Data.Models
{
    using System.Collections.Generic;

    public class Driver
    {
        public Driver()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }

        public string License { get; set; }

        public string YearsOfExperience { get; set; }

        public string UserId { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
