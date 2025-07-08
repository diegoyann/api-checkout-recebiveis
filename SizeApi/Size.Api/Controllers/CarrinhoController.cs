using Microsoft.AspNetCore.Mvc;
using Size.Application.DTOs;
using Size.Application.Interfaces;

namespace Size.Api.Controllers;

/// <summary>
/// Controller para gerenciamento do carrinho de antecipação
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CarrinhoController : ControllerBase
{
    private readonly IServicoCarrinho _servicoCarrinho;

    /// <summary>
    /// Construtor do controller de carrinho
    /// </summary>
    /// <param name="servicoCarrinho">Serviço de carrinho</param>
    public CarrinhoController(IServicoCarrinho servicoCarrinho)
    {
        _servicoCarrinho = servicoCarrinho;
    }

    /// <summary>
    /// Obtém o carrinho ativo de uma empresa no formato de checkout
    /// </summary>
    /// <param name="empresaId">ID da empresa</param>
    /// <returns>Dados do carrinho da empresa no formato de checkout</returns>
    /// <response code="200">Carrinho encontrado</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpGet("empresa/{empresaId}")]
    [ProducesResponseType(typeof(CheckoutDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckoutDto>> ObterCarrinhoPorEmpresa(int empresaId)
    {
        var carrinho = await _servicoCarrinho.ObterCarrinhoFormatoCheckoutAsync(empresaId);
        
        if (carrinho == null)
            return NotFound($"Empresa com ID {empresaId} não encontrada.");

        return Ok(carrinho);
    }

    /// <summary>
    /// Adiciona uma nota fiscal ao carrinho
    /// </summary>
    /// <param name="dto">Dados para adicionar item ao carrinho</param>
    /// <returns>Resultado da operação</returns>
    /// <response code="200">Item adicionado com sucesso</response>
    /// <response code="400">Erro na validação dos dados</response>
    /// <response code="404">Empresa ou nota fiscal não encontrada</response>
    [HttpPost("adicionar-item")]
    [ProducesResponseType(typeof(CarrinhoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarrinhoDto>> AdicionarItem([FromBody] AdicionarItemCarrinhoDto dto)
    {
        try
        {
            var carrinho = await _servicoCarrinho.AdicionarItemAsync(dto);
            return Ok(carrinho);
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

    /// <summary>
    /// Remove uma nota fiscal do carrinho
    /// </summary>
    /// <param name="dto">Dados para remover item do carrinho</param>
    /// <returns>Resultado da operação</returns>
    /// <response code="200">Item removido com sucesso</response>
    /// <response code="400">Erro na validação dos dados</response>
    /// <response code="404">Carrinho ou nota fiscal não encontrada</response>
    [HttpPost("remover-item")]
    [ProducesResponseType(typeof(CarrinhoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarrinhoDto>> RemoverItem([FromBody] RemoverItemCarrinhoDto dto)
    {
        try
        {
            var carrinho = await _servicoCarrinho.RemoverItemAsync(dto);
            return Ok(carrinho);
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

    /// <summary>
    /// Realiza o checkout do carrinho (simulação de antecipação)
    /// </summary>
    /// <param name="empresaId">ID da empresa</param>
    /// <returns>Resultado do checkout com valores calculados</returns>
    /// <response code="200">Checkout realizado com sucesso</response>
    /// <response code="400">Carrinho vazio ou inválido</response>
    /// <response code="404">Carrinho não encontrado</response>
    [HttpPost("checkout/{empresaId}")]
    [ProducesResponseType(typeof(CheckoutDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckoutDto>> RealizarCheckout(int empresaId)
    {
        try
        {
            var resultado = await _servicoCarrinho.RealizarCheckoutAsync(empresaId);
            return Ok(resultado);
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

    /// <summary>
    /// Limpa o carrinho de uma empresa
    /// </summary>
    /// <param name="empresaId">ID da empresa</param>
    /// <returns>Resultado da operação</returns>
    /// <response code="200">Carrinho limpo com sucesso</response>
    /// <response code="404">Carrinho não encontrado</response>
    [HttpDelete("limpar/{empresaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> LimparCarrinho(int empresaId)
    {
        try
        {
            await _servicoCarrinho.LimparCarrinhoAsync(empresaId);
            return Ok(new { message = "Carrinho limpo com sucesso." });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Atualiza o carrinho substituindo todos os itens pelos especificados
    /// </summary>
    /// <param name="atualizarCarrinhoDto">Dados para atualização do carrinho</param>
    /// <returns>Carrinho atualizado</returns>
    /// <response code="200">Carrinho atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpPut]
    [ProducesResponseType(typeof(CarrinhoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarrinhoDto>> AtualizarCarrinho([FromBody] AtualizarCarrinhoDto atualizarCarrinhoDto)
    {
        try
        {
            var carrinho = await _servicoCarrinho.AtualizarCarrinhoAsync(atualizarCarrinhoDto);
            return Ok(carrinho);
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