namespace MovieDatabase.Data.Models
{
    using System;

    public class Series
    {
        public Series()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int StartYear { get; set; }

        public int? EndYear { get; set; }

        public string MPAA { get; set; }

        public string Poster { get; set; }

        public string Storyline { get; set; }

        public bool IsAnimation { get; set; }
    }
}
