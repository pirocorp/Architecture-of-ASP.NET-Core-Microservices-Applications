﻿namespace CarRentalSystem.Dealers.Models.Dealers
{
    using System.ComponentModel.DataAnnotations;

    using static CarRentalSystem.Common.Data.DataConstants.Common;
    using static Data.DataConstants.Dealers;

    public class EditDealerInputModel
    {
        [Required]
        [MinLength(MinNameLength)]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(MinPhoneNumberLength)]
        [MaxLength(MaxPhoneNumberLength)]
        [RegularExpression(PhoneNumberRegularExpression)]
        public string PhoneNumber { get; set; }
    }
}
