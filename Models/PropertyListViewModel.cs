using PropertyApp.Models;

public class PropertyListViewModel
{
    public required List<Property> Properties { get; set; }  public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string? Keyword { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinBedrooms { get; set; }
    public int? MaxBedrooms { get; set; }
    public int? MinBathrooms { get; set; }
    public int? MaxBathrooms { get; set; }
}
