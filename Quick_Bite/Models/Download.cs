using Quick_Bite.Models;

public class Download
{
    public int Id { get; set; }
    public DateTime DownloadDate { get; set; } = DateTime.UtcNow;

    // Only one of these should be set
    public int? RecipeId { get; set; }
    public Recipe Recipe { get; set; }

    public int? DrinkId { get; set; }
    public Drink Drink { get; set; }

    public int? SideDishId { get; set; }
    public SideDish SideDish { get; set; }

    public bool IsValid()
    {
        var itemCount = new[] { RecipeId, DrinkId, SideDishId }.Count(id => id != null);
        return itemCount == 1;
    }
}