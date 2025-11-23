public class CoinSpawner : CollectableSpawner<Coin>
{
    protected override void InitializePool()
    {
        _spawnDelay = 2f;
        base.InitializePool();
    }
}