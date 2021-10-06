namespace CarRentalSystem.Admin.Models.Identity
{
    using Common.Model;

    public class UserInputModel : IMapFrom<LoginFormModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
