public class MenuItemResponseDTO
{
    public string Id { get; set; }= string.Empty;

    public string Name { get; set; }= string.Empty;

    public string CategoryName { get; set; }= string.Empty;

    public string Type { get; set; }= string.Empty;

    public bool IsAvailable { get; set; }

    public decimal BasePrice { get; set; }
    public decimal FinalPrice { get; set; }

    public bool HasOffer { get; set; }
    public decimal? DiscountPercentage { get; set; }

}