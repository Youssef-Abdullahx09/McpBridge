using McpBridge.Server.Domain.Entities;
using McpBridge.Server.Domain.Interfaces;

namespace McpBridge.Server.Infrastructure;

public sealed class InMemoryBookmarkRepository : IBookmarkRepository
{
    private readonly List<Bookmark> _bookmarks = SeedData();

    public Task<IReadOnlyList<Bookmark>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Bookmark>>(
            _bookmarks.ToList());
    }

    public Task<Bookmark?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            _bookmarks.FirstOrDefault(x => x.Id == id));
    }

    public Task<IReadOnlyList<Bookmark>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        var results = _bookmarks
            .Where(x =>
                x.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                x.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                x.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult<IReadOnlyList<Bookmark>>(results);
    }

    public Task<IReadOnlyList<Bookmark>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default)
    {
        var results = _bookmarks
            .Where(x => x.Category.Equals(
                category, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult<IReadOnlyList<Bookmark>>(results);
    }

    public Task<IReadOnlyList<Bookmark>> GetFavoritesAsync(
        CancellationToken cancellationToken = default)
    {
        var results = _bookmarks
            .Where(x => x.IsFavorite)
            .ToList();

        return Task.FromResult<IReadOnlyList<Bookmark>>(results);
    }

    public Task<Bookmark> AddAsync(
        Bookmark bookmark,
        CancellationToken cancellationToken = default)
    {
        _bookmarks.Add(bookmark);

        return Task.FromResult(bookmark);
    }

    public Task<bool> UpdateAsync(
        Bookmark bookmark,
        CancellationToken cancellationToken = default)
    {
        var existing = _bookmarks.FirstOrDefault(
            x => x.Id == bookmark.Id);

        if (existing is null)
            return Task.FromResult(false);

        existing.Update(
            bookmark.Title,
            bookmark.Url,
            bookmark.Description,
            bookmark.Category);

        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var bookmark = _bookmarks.FirstOrDefault(x => x.Id == id);

        if (bookmark is null)
            return Task.FromResult(false);

        return Task.FromResult(_bookmarks.Remove(bookmark));
    }

    public Task<bool> ToggleFavoriteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var bookmark = _bookmarks.FirstOrDefault(x => x.Id == id);

        if (bookmark is null)
            return Task.FromResult(false);

        bookmark.ToggleFavorite();

        return Task.FromResult(true);
    }

    public Task<bool> MarkAsVisitedAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var bookmark = _bookmarks.FirstOrDefault(x => x.Id == id);

        if (bookmark is null)
            return Task.FromResult(false);

        bookmark.MarkAsVisited();

        return Task.FromResult(true);
    }

    private static List<Bookmark> SeedData()
    {
        return
        [
            new Bookmark(
                "ASP.NET Core Documentation",
                "https://learn.microsoft.com/aspnet/core",
                "Official ASP.NET Core documentation and tutorials.",
                "Development",
                true),

            new Bookmark(
                "Model Context Protocol",
                "https://modelcontextprotocol.io",
                "Official documentation for the Model Context Protocol.",
                "AI",
                true),

            new Bookmark(
                "Anthropic Documentation",
                "https://docs.anthropic.com",
                "Documentation for Claude models and APIs.",
                "AI"),

            new Bookmark(
                "Microsoft Learn",
                "https://learn.microsoft.com",
                "Microsoft technical learning resources.",
                "Development"),

            new Bookmark(
                "GitHub",
                "https://github.com",
                "Platform for hosting and collaborating on software projects.",
                "Development",
                true),

            new Bookmark(
                "Docker Documentation",
                "https://docs.docker.com",
                "Official Docker documentation and containerization guides.",
                "DevOps"),

            new Bookmark(
                "Kubernetes Documentation",
                "https://kubernetes.io/docs",
                "Official Kubernetes documentation.",
                "DevOps"),

            new Bookmark(
                "OpenTelemetry",
                "https://opentelemetry.io",
                "Observability framework for traces, metrics, and logs.",
                "Monitoring"),

            new Bookmark(
                "LangSmith",
                "https://www.langchain.com/langsmith",
                "Platform for debugging and monitoring AI applications.",
                "AI"),

            new Bookmark(
                "Postman",
                "https://www.postman.com",
                "API development and testing platform.",
                "Development")
        ];
    }
}