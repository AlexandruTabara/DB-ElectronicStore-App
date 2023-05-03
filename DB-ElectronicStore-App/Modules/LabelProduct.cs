public class LabelProduct
{
    public int Id { get;  set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

}
