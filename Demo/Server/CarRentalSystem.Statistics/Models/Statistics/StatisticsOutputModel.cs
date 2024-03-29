﻿namespace CarRentalSystem.Statistics.Models.Statistics
{
    using Common.Model;
    using Data.Models;

    public class StatisticsOutputModel : IMapFrom<Statistics>
    {
        public int TotalCarAds { get; set; }

        public int TotalRentedCars { get; set; }
    }
}
