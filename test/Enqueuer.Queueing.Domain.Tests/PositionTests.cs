using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Tests;

public class PositionTests
{
    [Theory]
    [InlineData(new uint[0], 1)]
    [InlineData(new uint[] { 1, 2, 3 }, 4)]
    [InlineData(new uint[] { 1, 2, 4, 5 }, 3)]
    [InlineData(new uint[] { 2, 4, 5 }, 1)]
    [InlineData(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 11)]
    public void Test(uint[] reservedPositions, uint expectedPosition)
    {
        Assert.Equal(expectedPosition, Position.GetFirstAvailablePosition(reservedPositions).Number);
    }
}
