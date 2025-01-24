namespace Blog.ViewModels.Posts;

public class ListPostsViewModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public DateTimeOffset LastUpdate { get; set; }
    public string? Category { get; set; }
    public string Author { get; set; }
}