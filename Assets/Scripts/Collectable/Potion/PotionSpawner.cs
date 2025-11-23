public class PotionSpawner : CollectableSpawner<Potion>
{
    protected override void InitializePool()
    {
        _spawnDelay = 60f;
        base.InitializePool();
    }
}