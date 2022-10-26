using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaLog.Models
{
    public class BookViewModel : Root
    {
        [ForeignKey("AuthorViewModel")]
        public int AuthorId { get; set; }
        public int? LoanId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsBorrowed { get; set; }
        //Relacionamentos do Entity FrameWork
        public AuthorViewModel BookAuthor { get; set; }
        public List<BookLoan>? BookLoan { get; set; }
    }
}
