namespace Size.Domain.Entities;

public class ItemCarrinhoAntecipacao
{
    /// <summary>
    /// Identificador único do item do carrinho
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identificador do carrinho
    /// </summary>
    public int CarrinhoAntecipacaoId { get; set; }
    
    /// <summary>
    /// Carrinho de antecipação
    /// </summary>
    public virtual CarrinhoAntecipacao CarrinhoAntecipacao { get; set; } = null!;
    
    /// <summary>
    /// Identificador da nota fiscal
    /// </summary>
    public int NotaFiscalId { get; set; }
    
    /// <summary>
    /// Nota fiscal do item
    /// </summary>
    public virtual NotaFiscal NotaFiscal { get; set; } = null!;
    
    /// <summary>
    /// Data de adição do item ao carrinho
    /// </summary>
    public DateTime DataAdicao { get; set; } = DateTime.UtcNow;
}