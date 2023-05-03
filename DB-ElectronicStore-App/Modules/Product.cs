

public class Product
{
    public int Id { get;  set; }
    public string Name { get; set; }
    public int Stock { get; set; }

    // Foreign key for Producer
    public int ProducerId { get; set; }
    public Producer Producer { get; set; }

    // Foreign key for Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public override string ToString()
    {
        return $"Product: {Name}, {Stock}";
    }
}
