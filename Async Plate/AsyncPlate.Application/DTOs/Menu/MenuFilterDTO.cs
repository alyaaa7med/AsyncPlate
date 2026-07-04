public class MenuFilterDTO
{
    public string Type { get; set; } = string.Empty; 

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public bool? AvailableOnly { get; set; } = true;
    public string? CategoryId { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}