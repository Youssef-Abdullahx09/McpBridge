using McpBridge.Server.Domain.Entities;

namespace McpBridge.Server.Domain.Interfaces;

public interface IBookmarkRepository
{
    Task<IReadOnlyList<Bookmark>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Bookmark?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Bookmark>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Bookmark>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Bookmark>> GetFavoritesAsync(
        CancellationToken cancellationToken = default);

    Task<Bookmark> AddAsync(
        Bookmark bookmark,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        Bookmark bookmark,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ToggleFavoriteAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> MarkAsVisitedAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}