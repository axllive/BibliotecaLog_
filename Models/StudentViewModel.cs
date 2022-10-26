using System.ComponentModel.DataAnnotations;

namespace BibliotecaLog.Models
{
    public class StudentViewModel : Root
    { 
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string StudentName{ get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int EnrollmentNumber { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Email { get; set; }
        //Relacionamentos do Entity FrameWork
        public List<BookLoan> Loans { get; set; }
    }
}
