namespace Core.Persistence.Paging;

public abstract class BasePageableModel
{
    public int Status { get; set; } = 200;
    public string Detail { get; set; } = "Success";
    public int Index { get; set; }
    public int Size { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}
