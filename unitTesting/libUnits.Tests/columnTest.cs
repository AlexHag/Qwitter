using FluentAssertions;

namespace libUnits.Tests;

public class columnTest
{
    [Fact]
    public void Test1()
    {
        var sut = new columnEx();

        var res = sut.test(5);

        res.Should().Be(10);
    }

    [Fact]
    public void timeTest()
    {
        var sut = new columnEx();

        var res = sut.time();

        res.Should().Be(DateTimeOffset.Now.ToUnixTimeSeconds());
    }
}