namespace TodoApp.Domain
{
    public record Todo (int Id, string Title, DateOnly? DueDate, bool IsDone = false);
}
