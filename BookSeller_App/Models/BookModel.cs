namespace BookSeller_App.Models
{
    public class BookModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int GenreID { get; set; }
        public int BookWiseAuthorID { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishDate { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    public class GenreModel
    {
        public int GenreID { get; set; }
        public string GenreName { get; set; }


    }
}
