namespace MovieDatabase.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Person
    {
        public Person()
        {
            this.Id = Guid.NewGuid().ToString();

            this.DirectedMovies = new HashSet<Movie>();
            this.WrittenMovies = new HashSet<Movie>();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Picture { get; set; }

        public ICollection<Movie> DirectedMovies { get; set; }

        public ICollection<Movie> WrittenMovies { get; set; }
    }
}
