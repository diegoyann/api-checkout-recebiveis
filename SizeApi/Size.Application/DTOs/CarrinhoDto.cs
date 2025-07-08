using System.Text.Json.Serialization;
using Size.Application.Converters;

namespace Size.Application.DTOs;

/// <summary>
/// DTO para adicionar item ao carrinho
/// </summary>
public class AdicionarItemCarrinhoDto
{
    /// <summary>
    /// Identificador da empresa
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Identificador da nota fiscal
    /// </summary>
    public int NotaFiscalId { get; set; }
}

/// <summary>
/// DTO para atualizar carrinho (substituir todos os itens)
/// </summary>
public class AtualizarCarrinhoDto
{
    /// <summary>
    /// Identificador da empresa
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Lista de IDs das notas fiscais que devem estar no carrinho
    /// </summary>
    public List<int> NotasFiscaisIds { get; set; } = new();
}

/// <summary>
/// DTO para remover item do carrinho
/// </summary>
public class RemoverItemCarrinhoDto
{
    /// <summary>
    /// Identificador da empresa
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Identificador da nota fiscal
    /// </summary>
    public int NotaFiscalId { get; set; }
}

/// <summary>
/// DTO para retorno do carrinho
/// </summary>
public class CarrinhoDto
{
    /// <summary>
    /// Identificador do carrinho
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identificador da empresa
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Nome da empresa
    /// </summary>
    public string NomeEmpresa { get; set; } = string.Empty;
    
    /// <summary>
    /// Lista de notas fiscais no carrinho
    /// </summary>
    public List<NotaFiscalCarrinhoDto> NotasFiscais { get; set; } = new();
    
    /// <summary>
    /// Valor total bruto do carrinho
    /// </summary>
    public decimal ValorTotalBruto { get; set; }
    
    /// <summary>
    /// Valor total líquido do carrinho
    /// </summary>
    public decimal ValorTotalLiquido { get; set; }
    
    /// <summary>
    /// Data de criação do carrinho
    /// </summary>
    public DateTime DataCriacao { get; set; }
}

/// <summary>
/// DTO para resultado do checkout
/// </summary>
public class CheckoutDto
{
    /// <summary>
    /// Nome da empresa
    /// </summary>
    public string empresa { get; set; } = string.Empty;
    
    /// <summary>
    /// CNPJ da empresa
    /// </summary>
    public string cnpj { get; set; } = string.Empty;
    
    /// <summary>
    /// Limite de antecipação da empresa
    /// </summary>
    public decimal limite { get; set; }
    
    /// <summary>
    /// Lista de notas fiscais com valores calculados
    /// </summary>
    public List<NotaFiscalCheckoutDto> notas_fiscais { get; set; } = new();
    
    /// <summary>
    /// Valor total líquido
    /// </summary>
    public decimal total_liquido { get; set; }
    
    /// <summary>
    /// Valor total bruto
    /// </summary>
    public decimal total_bruto { get; set; }
}

/// <summary>
/// DTO para nota fiscal no carrinho (versão simplificada)
/// </summary>
public class NotaFiscalCarrinhoDto
{
    /// <summary>
    /// Identificador único da nota fiscal
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Número da nota fiscal
    /// </summary>
    public string Numero { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da nota fiscal
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Data de vencimento da nota fiscal
    /// </summary>
    public DateTime DataVencimento { get; set; }
}

/// <summary>
/// DTO para nota fiscal no checkout
/// </summary>
public class NotaFiscalCheckoutDto
{
    /// <summary>
    /// Número da nota fiscal
    /// </summary>
    public string numero { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor bruto da nota fiscal
    /// </summary>
    public decimal valor_bruto { get; set; }
    
    /// <summary>
    /// Valor líquido da nota fiscal
    /// </summary>
    public decimal valor_liquido { get; set; }
}