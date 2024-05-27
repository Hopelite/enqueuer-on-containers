namespace Enqueuer.Identity.Contract.V1.Models.Enums
{
    public static class AllowedScope
    {
        public const string QueueAll = "queue";

        public const string QueueCreate = "queue:create";

        public const string QueueDelete = "queue:delete";

        // TODO: add all the other scopes
    }
}
