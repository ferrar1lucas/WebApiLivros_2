using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebApiLivros_2.Database;
using WebApiLivros_2.DTOs.AutorDto;
using WebApiLivros_2.Models;

namespace WebApiLivros_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutorController : ControllerBase
    {
        private readonly AppDbContext _banco;

        public AutorController(AppDbContext banco)
        {
            _banco = banco;
        }

        //=======================================================================================================

        [HttpPost("Cadastrar")]
        public async Task<ActionResult<AutorRespostaDto>> Cadastrar(CriarAutorDto criarAutor)
        {
            Autor autor = new Autor
            {
                Nome = criarAutor.Nome,
                Sobrenome = criarAutor.Sobrenome
            };

            _banco.Autores.Add(autor);
            await _banco.SaveChangesAsync();

            AutorRespostaDto resposta = new AutorRespostaDto
            {
                Id = autor.Id,
                NomeAutor = $"{autor.Nome} {autor.Sobrenome}"
            };

            return CreatedAtAction(nameof(ListarPorId), new { id = autor.Id }, resposta);

            // nameof(ListarPorId): o nome do método que sabe buscar um autor.
            // new { id = autor.Id }: o id que esse método precisa para achar o autor recém - criado.
            // resposta: o corpo, igual a antes.
        }

        //=======================================================================================================

        [HttpGet("Listar")]
        public async Task<ActionResult<List<AutorRespostaDto>>> Listar()
        {
            var autores = await _banco.Autores.Select(a => new AutorRespostaDto
            {
                Id = a.Id,
                NomeAutor = $"{a.Nome} {a.Sobrenome}"

            }).ToListAsync();

            return Ok(autores);
        }

        //=======================================================================================================

        [HttpGet("ListarPorId/{id}")]
        public async Task<ActionResult<AutorRespostaDto>> ListarPorId(int id)
        {
            var autor = await _banco.Autores.FirstOrDefaultAsync(a => a.Id == id);

            if(autor == null)
            {
                return NotFound("Autor não encontrado!");
            }

            AutorRespostaDto resposta = new AutorRespostaDto
            {
                Id = autor.Id,
                NomeAutor = $"{autor.Nome} {autor.Sobrenome}"
            };

            return Ok(resposta);
        }

        //=======================================================================================================

        [HttpPut("Atualizar/{id}")]
        public async Task<ActionResult<AutorRespostaDto>> Atualizar(int id, AttAutorDto attAutor)
        {
            var autor = await _banco.Autores.FirstOrDefaultAsync(a => a.Id == id);

            if(autor == null)
            {
                return NotFound("Autor não encontrado!");
            }

            autor.Nome = attAutor.Nome;
            autor.Sobrenome = attAutor.Sobrenome;

            await _banco.SaveChangesAsync();

            AutorRespostaDto resposta = new AutorRespostaDto
            {
                Id = autor.Id,
                NomeAutor = $"{autor.Nome} {autor.Sobrenome}"
            };

            return Ok(resposta);
        }

        //=======================================================================================================

        [HttpDelete("Remover/{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            var autor = await _banco.Autores.FirstOrDefaultAsync(a => a.Id == id);

            if(autor == null)
            {
                return NotFound("Autor não encontrado!");
            }

            bool possuiLivros = await _banco.Livros.AnyAsync(a => a.AutorId == id);

            if (possuiLivros)
            {
                return Conflict("O autor possui livros cadastrados!");
            }

            _banco.Autores.Remove(autor);
            await _banco.SaveChangesAsync();

            return NoContent();
        }
    }
}
