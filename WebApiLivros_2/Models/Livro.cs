namespace WebApiLivros_2.Models
{
    public class Livro
    {
        public int Id {  get; set; }

        public string Titulo {  get; set; } = string.Empty;

        public int Ano { get; set; }

        public int Edicao { get; set; }

        public int? AutorId { get; set; }

        public Autor Autor { get; set; }
    }
}
