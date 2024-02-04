using System;
using System.Collections;
using System.Collections.Generic;

namespace Enqueuer.EventBus.Abstractions
{
    public class SubscriptionList : IEnumerable<KeyValuePair<string, Type>>
    {
        private readonly Dictionary<string, Type> _eventTypes = new Dictionary<string, Type>();

        public void Add(string eventName, Type eventType)
        {
            _eventTypes[eventName] = eventType;
        }

        public bool TryGet(string eventName, out Type eventType)
        {
            return _eventTypes.TryGetValue(eventName, out eventType);
        }

        public IEnumerator<KeyValuePair<string, Type>> GetEnumerator()
        {
            return _eventTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
