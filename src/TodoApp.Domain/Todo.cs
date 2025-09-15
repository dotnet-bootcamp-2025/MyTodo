namespace TodoApp.Domain.Entities;

public record Todo
{
    // Deja el constructor privado para que solo el método de fábrica lo use
    private Todo() { }

    // Elimina este constructor obsoleto que usa int
    // public Todo(int v1, string title, DateOnly? dueDate, bool v2)
    // {
    //    this.v1 = v1;
    //    Title = title;
    //    DueDate = dueDate;
    //    this.v2 = v2;
    // }

    public Guid Id { get; init; } // Usa 'init'
    public string Title { get; init; } // Usa 'init'
    public string Description { get; init; } // Usa 'init'
    public bool IsCompleted { get; init; } // Usa 'init'
    public DateTime CreatedAt { get; init; } // Usa 'init'
    public DateTime? CompletedAt { get; init; } // Usa 'init'
    public DateOnly? DueDate { get; init; } // Usa 'init'

    public static Todo Create(string title, string description = "", DateOnly? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        return new Todo
        {
            Id = Guid.NewGuid(), // Esto genera el identificador único
            Title = title,
            Description = description ?? string.Empty,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            DueDate = dueDate
        };
    }



    public Todo MarkAsCompleted()
    {
        if (IsCompleted) return this;
        return this with
        {
            IsCompleted = true,
            CompletedAt = DateTime.UtcNow
        };
    }

    public Todo MarkAsIncomplete()
    {
        if (!IsCompleted) return this;
        return this with
        {
            IsCompleted = false,
            CompletedAt = null
        };
    }

    public Todo UpdateDetails(string title, string description, DateOnly? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        return this with
        {
            Title = title,
            Description = description ?? string.Empty,
            DueDate = dueDate
        };
    }
}