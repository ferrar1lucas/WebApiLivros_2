namespace WebApiLivros_2.DTOs.LivroDto
{
    public class CriarLivroDto
    {
        public string Titulo { get; set; } = string.Empty;

        public int Ano { get; set; }

        public int Edicao { get; set; }

        public int? AutorId { get; set; }
    }
}
