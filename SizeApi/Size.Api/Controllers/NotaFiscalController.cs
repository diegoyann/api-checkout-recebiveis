using Microsoft.AspNetCore.Mvc;
using Size.Application.DTOs;
using Size.Application.Interfaces;

namespace Size.API.Controllers;

/// <summary>
/// Controller responsável pelas operações relacionadas às notas fiscais
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class NotasFiscaisController : ControllerBase
{
    private readonly IServicoNotaFiscal _servicoNotaFiscal;

    /// <summary>
    /// Construtor do controller de notas fiscais
    /// </summary>
    /// <param name="servicoNotaFiscal">Serviço de notas fiscais</param>
    public NotasFiscaisController(IServicoNotaFiscal servicoNotaFiscal)
    {
        _servicoNotaFiscal = servicoNotaFiscal;
    }

    /// <summary>
    /// Cria uma nova nota fiscal
    /// </summary>
    /// <param name="criarNotaFiscalDto">Dados da nota fiscal a ser criada</param>
    /// <returns>Nota fiscal criada</returns>
    /// <response code="201">Nota fiscal criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="409">Número da nota fiscal já existe para a empresa</response>
    [HttpPost]
    [ProducesResponseType(typeof(NotaFiscalDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<NotaFiscalDto>> CriarNotaFiscal([FromBody] CriarNotaFiscalDto criarNotaFiscalDto)
    {
        // Verifica se o modelo é válido
        if (!ModelState.IsValid)
        {
            var erros = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            
            return BadRequest(new { message = "Dados inválidos", errors = erros });
        }

        try
        {
            var notaFiscal = await _servicoNotaFiscal.CriarNotaFiscalAsync(criarNotaFiscalDto);
            return CreatedAtAction(nameof(ObterNotaFiscalPorId), new { id = notaFiscal.Id }, notaFiscal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém uma nota fiscal por ID
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <returns>Nota fiscal encontrada</returns>
    /// <response code="200">Nota fiscal encontrada</response>
    /// <response code="404">Nota fiscal não encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(NotaFiscalDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<NotaFiscalDto>> ObterNotaFiscalPorId(int id)
    {
        var notaFiscal = await _servicoNotaFiscal.ObterNotaFiscalPorIdAsync(id);
        if (notaFiscal == null)
            return NotFound(new { message = "Nota fiscal não encontrada" });

        return Ok(notaFiscal);
    }

    /// <summary>
    /// Obtém todas as notas fiscais de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Lista de notas fiscais da empresa</returns>
    /// <response code="200">Lista de notas fiscais</response>
    [HttpGet("empresa/{empresaId}")]
    [ProducesResponseType(typeof(IEnumerable<NotaFiscalDto>), 200)]
    public async Task<ActionResult<IEnumerable<NotaFiscalDto>>> ObterNotasFiscaisPorEmpresa(int empresaId)
    {
        var notasFiscais = await _servicoNotaFiscal.ObterNotasFiscaisPorEmpresaAsync(empresaId);
        return Ok(notasFiscais);
    }

    /// <summary>
    /// Remove uma nota fiscal
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <returns>Confirmação da remoção</returns>
    /// <response code="204">Nota fiscal removida com sucesso</response>
    /// <response code="404">Nota fiscal não encontrada</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> RemoverNotaFiscal(int id)
    {
        try
        {
            await _servicoNotaFiscal.RemoverNotaFiscalAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza uma nota fiscal existente
    /// </summary>
    /// <param name="id">ID da nota fiscal</param>
    /// <param name="atualizarNotaFiscalDto">Dados atualizados da nota fiscal</param>
    /// <returns>Nota fiscal atualizada</returns>
    /// <response code="200">Nota fiscal atualizada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Nota fiscal não encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(NotaFiscalDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<NotaFiscalDto>> AtualizarNotaFiscal(int id, [FromBody] AtualizarNotaFiscalDto atualizarNotaFiscalDto)
    {
        // Verifica se o modelo é válido
        if (!ModelState.IsValid)
        {
            var erros = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            
            return BadRequest(new { message = "Dados inválidos", errors = erros });
        }

        try
        {
            var notaFiscal = await _servicoNotaFiscal.AtualizarNotaFiscalAsync(id, atualizarNotaFiscalDto);
            return Ok(notaFiscal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}