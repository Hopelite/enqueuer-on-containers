using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Tests;

public class GroupTests
{
    private readonly IGroupFactory _groupFactory;

    public GroupTests()
    {
        _groupFactory = new GroupFactory();
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("__Queue__")]
    [InlineData("16 Queue 9")]
    [InlineData("Q113113 #@m3")]
    [InlineData("What a name for a queue")]
    [InlineData("QueueWithQuiteLongNameButShorterThan64Characters")]
    public void CreateQueue_SuccessfullyAddsQueue(string queueName)
    {
        const long GroupId = 1;
        var group = _groupFactory.Create(GroupId);

        group.CreateQueue(queueName);

        Assert.Contains(group.Queues, q => q.Name.Equals(queueName));
    }

    [Fact]
    public void CreateQueue_QueueAlreadyExists_ThrowsException()
    {
        const long GroupId = 1;
        const string QueueName = "ExistingQueueName";

        var group = _groupFactory.Create(GroupId);
        group.CreateQueue(QueueName);

        Assert.Throws<QueueAlreadyExistsException>(() => group.CreateQueue(QueueName));
    }

    [Fact]
    public void CreateQueue_QueueNameIsTooLong_ThrowsException()
    {
        const long GroupId = 1;
        const string ExtremelyLongQueueName = "QueueWithQuiteLongNameThatIsDefinitelyLongerThan64CharactersLikeThisOne";

        var group = _groupFactory.Create(GroupId);

        Assert.Throws<InvalidQueueNameException>(() => group.CreateQueue(ExtremelyLongQueueName));
    }

    [Fact]
    public void DeleteQueue_SuccessfullyDeletesQueue()
    {
        const long GroupId = 1;
        const string QueueName = "ExistingQueueName";

        var group = _groupFactory.Create(GroupId);
        group.CreateQueue(QueueName);

        group.DeleteQueue(QueueName);

        Assert.Empty(group.Queues);
    }

    [Fact]
    public void DeleteQueue_QueueDoesNotExist_ThrowsException()
    {
        const long GroupId = 1;
        const string QueueName = "NonExistingQueueName";

        var group = _groupFactory.Create(GroupId);

        Assert.Throws<QueueDoesNotExistException>(() => group.DeleteQueue(QueueName));
    }
}