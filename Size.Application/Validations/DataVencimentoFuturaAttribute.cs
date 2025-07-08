using System.ComponentModel.DataAnnotations;

namespace Size.Application.Validations;

/// <summary>
/// Atributo de validação customizado para garantir que a data de vencimento seja futura
/// </summary>
public class DataVencimentoFuturaAttribute : ValidationAttribute
{
    /// <summary>
    /// Construtor padrão com mensagem de erro
    /// </summary>
    public DataVencimentoFuturaAttribute() : base("A data de vencimento deve ser maior que a data atual")
    {
    }

    /// <summary>
    /// Valida se a data é futura
    /// </summary>
    /// <param name="value">Valor a ser validado</param>
    /// <param name="validationContext">Contexto de validação</param>
    /// <returns>Resultado da validação</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("A data de vencimento é obrigatória");
        }

        if (value is DateTime dataVencimento)
        {
            // Compara apenas a data, ignorando a hora
            if (dataVencimento.Date <= DateTime.Now.Date)
            {
                return new ValidationResult("A data de vencimento deve ser maior que a data atual");
            }
        }
        else
        {
            return new ValidationResult("Formato de data inválido");
        }

        return ValidationResult.Success;
    }
}