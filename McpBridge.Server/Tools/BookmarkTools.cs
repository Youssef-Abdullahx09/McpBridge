using System.ComponentModel;
using McpBridge.Server.Domain.Entities;
using McpBridge.Server.Domain.Interfaces;
using ModelContextProtocol.Server;

namespace McpBridge.Server.Tools;

[McpServerToolType]
public static class BookmarkTools
{
    [McpServerTool(Name = "get_bookmarks")]
    [Description("Get all bookmarks stored in the system.")]
    public static async Task<IReadOnlyList<Bookmark>> GetBookmarks(
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    [McpServerTool(Name = "get_bookmark")]
    [Description("Get a specific bookmark using its unique ID.")]
    public static async Task<Bookmark?> GetBookmark(
        [Description("The unique ID of the bookmark.")] Guid id,
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(id, cancellationToken);
    }

    [McpServerTool(Name = "search_bookmarks")]
    [Description(
        "Search bookmarks by title, description, or category. " +
        "Use this when looking for bookmarks matching a particular topic.")]
    public static async Task<IReadOnlyList<Bookmark>> SearchBookmarks(
        [Description("The keyword or phrase to search for.")] string searchTerm,
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            throw new ArgumentException("Search term cannot be empty.");

        return await repository.SearchAsync(
            searchTerm.Trim(), cancellationToken);
    }

    [McpServerTool(Name = "get_bookmarks_by_category")]
    [Description("Get all bookmarks belonging to a specific category.")]
    public static async Task<IReadOnlyList<Bookmark>> GetBookmarksByCategory(
        [Description("The category name, such as AI, Development, or DevOps.")] string category,
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty.");

        return await repository.GetByCategoryAsync(
            category.Trim(), cancellationToken);
    }

    [McpServerTool(Name = "get_favorite_bookmarks")]
    [Description("Get all bookmarks marked as favorites.")]
    public static async Task<IReadOnlyList<Bookmark>> GetFavoriteBookmarks(
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.GetFavoritesAsync(cancellationToken);
    }

    [McpServerTool(Name = "add_bookmark")]
    [Description(
        "Create a new bookmark with a title, URL, description, and category.")]
    public static async Task<Bookmark> AddBookmark(
        [Description("The bookmark title.")] string title,
        [Description("The absolute HTTP or HTTPS URL.")] string url,
        [Description("A short description of the bookmarked website.")] string description,
        [Description("The category of the bookmark.")] string category,
        [Description("Whether the bookmark should be marked as a favorite.")] bool isFavorite,
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        ValidateBookmark(title, url, category);

        var bookmark = new Bookmark(
            title.Trim(),
            url.Trim(),
            description?.Trim() ?? string.Empty,
            category.Trim(),
            isFavorite);

        return await repository.AddAsync(bookmark, cancellationToken);
    }

    [McpServerTool(Name = "update_bookmark")]
    [Description("Update an existing bookmark using its unique ID.")]
    public static async Task<Bookmark?> UpdateBookmark(
        [Description("The unique ID of the bookmark.")] Guid id,
        [Description("The updated bookmark title.")] string title,
        [Description("The updated HTTP or HTTPS URL.")] string url,
        [Description("The updated bookmark description.")] string description,
        [Description("The updated bookmark category.")] string category,
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        ValidateBookmark(title, url, category);

        var bookmark = await repository.GetByIdAsync(
            id, cancellationToken);

        if (bookmark is null)
            return null;

        bookmark.Update(
            title.Trim(),
            url.Trim(),
            description?.Trim() ?? string.Empty,
            category.Trim());

        var updated = await repository.UpdateAsync(
            bookmark, cancellationToken);

        return updated ? bookmark : null;
    }

    [McpServerTool(Name = "delete_bookmark")]
    [Description("Delete a bookmark using its unique ID.")]
    public static async Task<bool> DeleteBookmark(
        [Description("The unique ID of the bookmark to delete.")]
        Guid id,
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(id, cancellationToken);
    }

    [McpServerTool(Name = "toggle_favorite")]
    [Description(
        "Toggle the favorite status of a bookmark. " +
        "If it is a favorite, it becomes a non-favorite, and vice versa.")]
    public static async Task<bool> ToggleFavorite(
        [Description("The unique ID of the bookmark.")]
        Guid id,
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.ToggleFavoriteAsync(
            id, cancellationToken);
    }

    [McpServerTool(Name = "mark_as_visited")]
    [Description(
        "Mark a bookmark as visited and update its last visited timestamp.")]
    public static async Task<bool> MarkAsVisited(
        [Description("The unique ID of the bookmark.")]
        Guid id,
        IBookmarkRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.MarkAsVisitedAsync(
            id, cancellationToken);
    }

    private static void ValidateBookmark(
        string title,
        string url,
        string category)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category is required.");

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp &&
             uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                "A valid HTTP or HTTPS URL is required.");
        }
    }
}