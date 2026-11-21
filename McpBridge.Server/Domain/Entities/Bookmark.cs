namespace McpBridge.Server.Domain.Entities;

public sealed class Bookmark
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Url { get; private set; }

    public string Description { get; private set; }

    public string Category { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? LastVisitedAt { get; private set; }

    public bool IsFavorite { get; private set; }

    public Bookmark(
        string title,
        string url,
        string description,
        string category,
        bool isFavorite = false)
    {
        Id = Guid.NewGuid();
        Title = title;
        Url = url;
        Description = description;
        Category = category;
        IsFavorite = isFavorite;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string title,
        string url,
        string description,
        string category)
    {
        Title = title;
        Url = url;
        Description = description;
        Category = category;
    }

    public void ToggleFavorite()
    {
        IsFavorite = !IsFavorite;
    }

    public void MarkAsVisited()
    {
        LastVisitedAt = DateTime.UtcNow;
    }
}