using System.ComponentModel.DataAnnotations;

namespace BibliotecaLog.Models
{
    public class AuthorViewModel : Root
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string ? Name { get; set; }
        //Relacionamentos do Entity FrameWork
        public List<BookViewModel> ? AuthorBooks { get; set; }
    }
}
