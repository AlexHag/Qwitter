namespace libUnits;
public class columnEx
{
    public int test(int a)
    {
        return a + 5;
    }

    public long time()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}
