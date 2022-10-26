using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaLog.Models
{
    public class BookLoan : Root
    {
        public int BookId { get; set; }
        [ForeignKey("StudentViewModel")]
        public int StudentId { get; set; }

        public bool IsActive { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BorrowStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BorrowEnd { get; set; }

        //Relacionamentos do Entity FrameWork
        public BookViewModel? Book { get; set; }
        public StudentViewModel? Student { get; set; }
    }
}
