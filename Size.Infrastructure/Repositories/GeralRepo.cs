using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Size.Domain.Interfaces;
using Size.Infrastructure.Data;

namespace Size.Infrastructure.Repositories;

/// <summary>
/// Implementação base para repositórios
/// </summary>
/// <typeparam name="T">Tipo da entidade</typeparam>
public abstract class GeralRepo<T> : IGeralRepo<T> where T : class
{
    protected readonly SizeContext _context;
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// Construtor do repositório base
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    protected GeralRepo(SizeContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Obtém uma entidade por ID
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <returns>Entidade encontrada ou null</returns>
    public virtual async Task<T?> ObterPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Obtém todas as entidades
    /// </summary>
    /// <returns>Lista de entidades</returns>
    public virtual async Task<IEnumerable<T>> ObterTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Busca entidades baseado em uma expressão
    /// </summary>
    /// <param name="predicate">Expressão de busca</param>
    /// <returns>Lista de entidades que atendem ao critério</returns>
    public virtual async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Adiciona uma nova entidade
    /// </summary>
    /// <param name="entidade">Entidade a ser adicionada</param>
    /// <returns>Entidade adicionada</returns>
    public virtual async Task<T> AdicionarAsync(T entidade)
    {
        await _dbSet.AddAsync(entidade);
        await _context.SaveChangesAsync();
        return entidade;
    }

    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    /// <param name="entidade">Entidade a ser atualizada</param>
    /// <returns>Entidade atualizada</returns>
    public virtual async Task<T> AtualizarAsync(T entidade)
    {
        _dbSet.Update(entidade);
        await _context.SaveChangesAsync();
        return entidade;
    }

    /// <summary>
    /// Remove uma entidade
    /// </summary>
    /// <param name="entidade">Entidade a ser removida</param>
    /// <returns>Task</returns>
    public virtual async Task RemoverAsync(T entidade)
    {
        _dbSet.Remove(entidade);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove uma entidade por ID
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <returns>Task</returns>
    public virtual async Task RemoverPorIdAsync(int id)
    {
        var entidade = await ObterPorIdAsync(id);
        if (entidade != null)
        {
            await RemoverAsync(entidade);
        }
    }
}