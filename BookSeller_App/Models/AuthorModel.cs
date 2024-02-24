namespace BookSeller_App.Models
{
    public class AuthorModel
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
