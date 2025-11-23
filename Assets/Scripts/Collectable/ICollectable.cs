public interface ICollectable
{
    event System.Action<ICollectable> Collected;
    
    public void Collect();
}