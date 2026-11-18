namespace WebApiLivros_2.DTOs.LivroDto
{
    public class LivroRespostaDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public int Ano { get; set; }

        public int Edicao { get; set; }

        public int? AutorId { get; set; }

        public string NomeAutor { get; set; }
    }
}
