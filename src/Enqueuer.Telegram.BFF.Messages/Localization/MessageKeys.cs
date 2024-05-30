namespace Enqueuer.Telegram.BFF.Messages.Localization;

public static class MessageKeys
{
    public const string GeneralErrorInternal = "General_Error_Internal";

    public const string QueueMessageGroupDoesNotHaveAnyQueue = "Queue_Message_GroupDoesNotHaveAnyQueue";

    public const string QueueMessageQueueIsEmpty = "Queue_Message_QueueIsEmpty";

    public const string QueueMessageListGroupQueuesHeader = "Queue_Message_ListGroupQueues_Header";

    public const string QueueMessageListGroupQueuesFooter = "Queue_Message_ListGroupQueues_Footer";

    public const string CreateQueueErrorMissingQueueName = "CreateQueue_Error_MissingQueueName";

    public const string CreateQueueErrorQueueAlreadyExists = "CreateQueue_Error_QueueAlreadyExists";

    public const string CreateQueueErrorInvalidQueueName = "CreateQueue_Error_InvalidQueueName";

    public const string CreateQueueErrorPositionMustBePositive = "CreateQueue_Error_PositionMustBePositive";

    public const string RemoveQueueErrorMissingQueueName = "RemoveQueue_Error_MissingQueueName";
    
    public const string EnqueueErrorMissingQueueName = "Enqueue_Error_MissingQueueName";

    public const string EnqueueErrorQueueDoesNotExist = "Enqueue_Error_QueueDoesNotExist";

    public const string EnqueueErrorUserAlreadyParticipates = "Enqueue_Error_UserAlreadyParticipates";

    public const string EnqueueErrorPositionIsReserved = "Enqueue_Error_PositionIsReserved";

    public const string DequeueErrorMissingQueueName = "Dequeue_Error_MissingQueueName";

    public const string DequeueErrorQueueDoesNotExist = "Dequeue_Error_QueueDoesNotExist";

    public const string DequeueErrorParticipantIsNotEnqueued = "Dequeue_Error_ParticipantIsNotEnqueued";
}
