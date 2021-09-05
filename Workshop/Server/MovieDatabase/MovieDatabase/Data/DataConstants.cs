namespace MovieDatabase.Data
{
    public static class DataConstants
    {
        public static class Comment
        {
            public const int TitleMaxLength = 100;

            public const int ContentMaxLength = 5000;

            public const int UserIdLength = 36;
        }

        public static class Genre
        {
            public const int NameMaxLength = 100;
        }

        public static class Movie
        {
            public const int MpaaMaxLength = 50;

            public const int NameMaxLength = 150;

            public const int PosterMaxLength = 2048;

            public const int StorylineMaxLength = 5000;
        }

        public static class Person
        {
            public const int NameMaxLength = 150;

            public const int PictureMaxLength = 2048;
        }

        public static class Series
        {
            public const int MpaaMaxLength = 50;

            public const int NameMaxLength = 150;

            public const int PosterMaxLength = 2048;

            public const int StorylineMaxLength = 5000;
        }
    }
}
