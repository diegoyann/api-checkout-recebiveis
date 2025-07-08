using System.ComponentModel.DataAnnotations;
using Size.Application.Validations;

namespace Size.Application.DTOs;

/// <summary>
/// DTO para criação de nota fiscal
/// </summary>
public class CriarNotaFiscalDto
{
    /// <summary>
    /// Número da nota fiscal (obrigatório)
    /// </summary>
    [Required(ErrorMessage = "O número da nota fiscal é obrigatório")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "O número da nota fiscal deve ter entre 1 e 50 caracteres")]
    public string Numero { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da nota fiscal (obrigatório)
    /// </summary>
    [Required(ErrorMessage = "O valor da nota fiscal é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor da nota fiscal deve ser maior que zero")]
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Data de vencimento da nota fiscal (obrigatório)
    /// </summary>
    [Required(ErrorMessage = "A data de vencimento é obrigatória")]
    [DataType(DataType.Date, ErrorMessage = "Formato de data inválido")]
    [DataVencimentoFutura]
    public DateTime DataVencimento { get; set; }
    
    /// <summary>
    /// Identificador da empresa proprietária
    /// </summary>
    [Required(ErrorMessage = "O identificador da empresa é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O identificador da empresa deve ser maior que zero")]
    public int EmpresaId { get; set; }
}

/// <summary>
/// DTO para atualização de nota fiscal
/// </summary>
public class AtualizarNotaFiscalDto
{
    /// <summary>
    /// Número da nota fiscal (obrigatório)
    /// </summary>
    [Required(ErrorMessage = "O número da nota fiscal é obrigatório")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "O número da nota fiscal deve ter entre 1 e 50 caracteres")]
    public string Numero { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da nota fiscal (obrigatório)
    /// </summary>
    [Required(ErrorMessage = "O valor da nota fiscal é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor da nota fiscal deve ser maior que zero")]
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Data de vencimento da nota fiscal (obrigatório)
    /// </summary>
    [Required(ErrorMessage = "A data de vencimento é obrigatória")]
    [DataType(DataType.Date, ErrorMessage = "Formato de data inválido")]
    [DataVencimentoFutura]
    public DateTime DataVencimento { get; set; }
}

/// <summary>
/// DTO para retorno de nota fiscal
/// </summary>
public class NotaFiscalDto
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
    
    /// <summary>
    /// Identificador da empresa proprietária
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Nome da empresa proprietária
    /// </summary>
    public string NomeEmpresa { get; set; } = string.Empty;
    
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; }
    
    /// <summary>
    /// Prazo em dias calculado
    /// </summary>
    public int PrazoEmDias { get; set; }
    
    /// <summary>
    /// Valor do deságio calculado
    /// </summary>
    public decimal Desagio { get; set; }
    
    /// <summary>
    /// Valor líquido calculado
    /// </summary>
    public decimal ValorLiquido { get; set; }
}