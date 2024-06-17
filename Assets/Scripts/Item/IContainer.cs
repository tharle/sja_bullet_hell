using System.Collections.Generic;

public interface IContainer
{
    public List<Item> Items { get; set;}

    public void AddItem(Item _item);
    public void RemoveItem(Item _item);
    public void RemoveAllIItem();
    public bool HasItem(Item _item);
}
