public abstract class StatsDecorator : Stats
{
    private Stats m_Stats;

    public Stats GetStats()
    { return m_Stats; }

    public void SetStats(Stats stats)
    { m_Stats = stats; }
}
