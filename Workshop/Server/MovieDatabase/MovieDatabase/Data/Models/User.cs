namespace MovieDatabase.Data.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.Comments = new HashSet<Comment>();
        }

        public ICollection<Comment> Comments { get; set; }
    }
}
