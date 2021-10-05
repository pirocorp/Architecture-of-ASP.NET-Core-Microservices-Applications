﻿namespace CarRentalSystem.Dealers.Models.Dealers
{
    using Common.Model;
    using Data.Models;

    public class DealerOutputModel : IMapFrom<Dealer>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }
}
