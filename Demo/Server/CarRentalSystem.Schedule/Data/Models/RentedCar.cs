﻿namespace CarRentalSystem.Schedule.Data.Models
{
    using System.Collections.Generic;

    public class RentedCar
    {
        public RentedCar()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }

        public string Information { get; set; }

        public int Kilometers { get; set; }

        public bool HasInsurance { get; set; }

        public int CarAdId { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
