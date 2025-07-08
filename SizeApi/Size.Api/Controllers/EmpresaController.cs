using Microsoft.AspNetCore.Mvc;
using Size.Application.DTOs;
using Size.Application.Interfaces;
using Size.Domain.Entities;
using SizeApi.Application.Interfaces;

namespace Size.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de empresas
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmpresaController : ControllerBase
{
    private readonly IServicoEmpresa _servicoEmpresa;

    /// <summary>
    /// Construtor do controller de empresas
    /// </summary>
    /// <param name="servicoEmpresa">Serviço de empresas</param>
    public EmpresaController(IServicoEmpresa servicoEmpresa)
    {
        _servicoEmpresa = servicoEmpresa;
    }

    /// <summary>
    /// Obtém todas as empresas cadastradas
    /// </summary>
    /// <returns>Lista de empresas</returns>
    /// <response code="200">Lista de empresas retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Empresa>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Empresa>>> ObterTodas()
    {
        var empresas = await _servicoEmpresa.ObterTodasEmpresasAsync();
        return Ok(empresas);
    }

    /// <summary>
    /// Obtém uma empresa específica por ID
    /// </summary>
    /// <param name="id">ID da empresa</param>
    /// <returns>Dados da empresa</returns>
    /// <response code="200">Empresa encontrada</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Empresa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Empresa>> ObterPorId(int id)
    {
        var empresa = await _servicoEmpresa.ObterEmpresaPorIdAsync(id);
        
        if (empresa == null)
            return NotFound($"Empresa com ID {id} não encontrada.");

        return Ok(empresa);
    }

    /// <summary>
    /// Obtém uma empresa por CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ da empresa</param>
    /// <returns>Dados da empresa</returns>
    /// <response code="200">Empresa encontrada</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpGet("cnpj/{cnpj}")]
    [ProducesResponseType(typeof(Empresa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Empresa>> ObterPorCnpj(string cnpj)
    {
        var empresa = await _servicoEmpresa.ObterEmpresaPorCnpjAsync(cnpj);
        
        if (empresa == null)
            return NotFound($"Empresa com CNPJ {cnpj} não encontrada.");

        return Ok(empresa);
    }

    /// <summary>
    /// Cria uma nova empresa
    /// </summary>
    /// <param name="criarEmpresaDto">Dados da empresa a ser criada</param>
    /// <returns>Empresa criada</returns>
    /// <response code="201">Empresa criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmpresaDto>> CriarEmpresa([FromBody] CriarEmpresaDto criarEmpresaDto)
    {
        try
        {
            var empresa = await _servicoEmpresa.CriarEmpresaAsync(criarEmpresaDto);
            return CreatedAtAction(nameof(ObterPorId), new { id = empresa.Id }, empresa);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Atualiza uma empresa existente
    /// </summary>
    /// <param name="id">ID da empresa</param>
    /// <param name="atualizarEmpresaDto">Dados atualizados da empresa</param>
    /// <returns>Empresa atualizada</returns>
    /// <response code="200">Empresa atualizada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpresaDto>> AtualizarEmpresa(int id, [FromBody] AtualizarEmpresaDto atualizarEmpresaDto)
    {
        try
        {
            var empresa = await _servicoEmpresa.AtualizarEmpresaAsync(id, atualizarEmpresaDto);
            return Ok(empresa);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}