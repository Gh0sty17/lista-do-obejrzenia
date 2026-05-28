namespace DoObejrzenia
{
    public class Movie
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            return $"{Title} ({Year}) - {Genre}";
        }
    }
}