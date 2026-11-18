using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiLivros_2.Database;
using WebApiLivros_2.DTOs.LivroDto;
using WebApiLivros_2.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiLivros_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly AppDbContext _banco;

        public LivroController(AppDbContext banco)
        {
            _banco = banco;
        }
        
        //=======================================================================================================

        [HttpPost("Cadastrar")]
        public async Task<ActionResult<LivroRespostaDto>> Cadastrar(CriarLivroDto criarLivro)
        {
            Autor? autor = null;

            if (criarLivro.AutorId.HasValue)
            {
                autor = await _banco.Autores.FirstOrDefaultAsync(a => a.Id == criarLivro.AutorId.Value);

                if(autor == null)
                {
                    return BadRequest("O autor informado não existe!");
                }
            }

            Livro livro = new Livro
            {
                Titulo = criarLivro.Titulo,
                Ano = criarLivro.Ano,
                Edicao = criarLivro.Edicao,
                AutorId = criarLivro.AutorId
            };

            _banco.Livros.Add(livro);
            await _banco.SaveChangesAsync();

            LivroRespostaDto resposta = new LivroRespostaDto
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Ano = livro.Ano,
                Edicao = livro.Edicao,
                AutorId = livro.AutorId,
                NomeAutor = autor != null ? $"{autor.Nome} {autor.Sobrenome}" : null
            };

            return CreatedAtAction(nameof(ListarPorId), new { id = livro.Id }, resposta);

            // nameof(ListarPorId): o nome do método que sabe buscar um autor.
            // new { id = autor.Id }: o id que esse método precisa para achar o autor recém - criado.
            // resposta: o corpo, igual a antes.
        }

        //=======================================================================================================

        [HttpGet("Listar")]
        public async Task<ActionResult<List<LivroRespostaDto>>> Listar()
        {
            var livros = await _banco.Livros.Select(l => new LivroRespostaDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Ano = l.Ano,
                Edicao = l.Edicao,
                AutorId = l.AutorId,
                NomeAutor = l.Autor != null ? $"{l.Autor.Nome} {l.Autor.Sobrenome}" : null

            }).ToListAsync();

            return Ok(livros);
        }

        //=======================================================================================================

        [HttpGet("ListarPorId/{id}")]
        public async Task<ActionResult<LivroRespostaDto>> ListarPorId(int id)
        {
            var livro = await _banco.Livros.Include(l => l.Autor).FirstOrDefaultAsync(l => l.Id == id);

            if(livro == null)
            {
                return NotFound("Livro não encontrado!");
            }

            LivroRespostaDto resposta = new LivroRespostaDto
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Ano = livro.Ano,
                Edicao = livro.Edicao,
                AutorId = livro.AutorId,
                NomeAutor = livro.Autor != null ? $"{livro.Autor.Nome} {livro.Autor.Sobrenome}" : null
            };

            return Ok(resposta);
        }

        //=======================================================================================================

        [HttpPut("Atualizar/{id}")]
        public async Task<ActionResult<LivroRespostaDto>> Atualizar(int id, AttLivroDto attLivro)
        {
            var livro = await _banco.Livros.FirstOrDefaultAsync(l => l.Id == id);

            if(livro == null)
            {
                return NotFound("Livro não encontrado!");
            }

            Autor? autor = null;

            if (attLivro.AutorId.HasValue)
            {
                autor = await _banco.Autores.FirstOrDefaultAsync(a => a.Id == attLivro.AutorId.Value);

                if(autor == null)
                {
                    return BadRequest("O autor informado não existe!");
                }
            }

            livro.Ano = attLivro.Ano;
            livro.Edicao = attLivro.Edicao;
            livro.AutorId = attLivro.AutorId;

            await _banco.SaveChangesAsync();

            LivroRespostaDto resposta = new LivroRespostaDto
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Ano = livro.Ano,
                Edicao = livro.Edicao,
                AutorId = livro.AutorId,
                NomeAutor = autor != null ? $"{autor.Nome} {autor.Sobrenome}" : null
            };

            return Ok(resposta);
        }

        //=======================================================================================================

        [HttpDelete("Remover/{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            var livro = await _banco.Livros.FirstOrDefaultAsync(a => a.Id == id);

            if(livro == null)
            {
                return NotFound("Livro não encontrado!");
            }

            _banco.Livros.Remove(livro);
            await _banco.SaveChangesAsync();

            return NoContent();
        }
    }
}