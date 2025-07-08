using System.Linq.Expressions;

namespace Size.Domain.Interfaces;

/// <summary>
/// Interface base para repositórios
/// </summary>
/// <typeparam name="T">Tipo da entidade</typeparam>
public interface IGeralRepo<T> where T : class
{
    /// <summary>
    /// Obtém uma entidade por ID
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <returns>Entidade encontrada ou null</returns>
    Task<T?> ObterPorIdAsync(int id);
    
    /// <summary>
    /// Obtém todas as entidades
    /// </summary>
    /// <returns>Lista de entidades</returns>
    Task<IEnumerable<T>> ObterTodosAsync();
    
    /// <summary>
    /// Busca entidades baseado em uma expressão
    /// </summary>
    /// <param name="predicate">Expressão de busca</param>
    /// <returns>Lista de entidades que atendem ao critério</returns>
    Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate);
    
    /// <summary>
    /// Adiciona uma nova entidade
    /// </summary>
    /// <param name="entidade">Entidade a ser adicionada</param>
    /// <returns>Entidade adicionada</returns>
    Task<T> AdicionarAsync(T entidade);
    
    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    /// <param name="entidade">Entidade a ser atualizada</param>
    /// <returns>Entidade atualizada</returns>
    Task<T> AtualizarAsync(T entidade);
    
    /// <summary>
    /// Remove uma entidade
    /// </summary>
    /// <param name="entidade">Entidade a ser removida</param>
    /// <returns>Task</returns>
    Task RemoverAsync(T entidade);
    
    /// <summary>
    /// Remove uma entidade por ID
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <returns>Task</returns>
    Task RemoverPorIdAsync(int id);
}