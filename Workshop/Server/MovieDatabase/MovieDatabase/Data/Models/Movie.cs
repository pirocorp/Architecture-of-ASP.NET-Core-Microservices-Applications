namespace MovieDatabase.Data.Models
{
    using System;

    public class Movie
    {
        public Movie()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string MPAA { get; set; }

        public string Storyline { get; set; }

        public string Poster { get; set; }

        public bool IsAnimation { get; set; }

        public string WriterId { get; set; }

        public Person Writer { get; set; }

        public string DirectorId { get; set; }

        public Person Director { get; set; }
    }
}
