namespace WebApiLivros_2.Models
{
    public class Autor
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Sobrenome { get; set; } = string.Empty;

        public List<Livro> Livros { get; set; } = new();
    }
}
