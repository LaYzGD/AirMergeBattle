public interface ICell
{
    public bool HasItem { get; }
    public virtual void PlaceItem(CellItem item) { }
    public virtual bool CanPlaceItem(CellItem item) { return false; }
    public virtual void RemoveItem() { }
    public virtual void SetItemFlag(bool flag) { }
}
