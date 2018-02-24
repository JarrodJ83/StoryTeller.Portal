using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using StoryTeller.ResultAggregation.Events;

namespace storyteller.portal.dotnetify
{
    public interface IEventsHub : INotificationHandler<RunCompleted>, INotificationHandler<RunSpecUpdated>, INotificationHandler<RunCreated>
    {
        bool IsSubscribed<TNotification>(object instance) where TNotification : INotification;
        void Subscribe<TNotification>(object instance, Action<INotification> action) where TNotification : INotification;
        void UnSubscribe<TNotification>(object instance) where TNotification : INotification;
    }

    public class EventsHub : IEventsHub
    {
        public List<Tuple<object, Type, Action<INotification>>> Subscriptions = new List<Tuple<object, Type, Action<INotification>>>();

        public EventsHub()
        {
        }

        #region Receivers


        #endregion

        #region IEventsHub

        public bool IsSubscribed<TNotification>(object instance) where TNotification : INotification
        {
            return GetSubscriptionOrDefault<TNotification>(instance) != null;
        }

        public void Subscribe<TNotification>(object instance, Action<INotification> action) where TNotification : INotification
        {
            var currentSubscription = GetSubscriptionOrDefault<TNotification>(instance);

            if (currentSubscription != null)
                Subscriptions.Remove(currentSubscription);

            Subscriptions.Add(new Tuple<object, Type, Action<INotification>>(instance, typeof(TNotification), action));
        }

        public void UnSubscribe<TNotification>(object instance) where TNotification : INotification
        {
            var sub = GetSubscriptionOrDefault<TNotification>(instance);

            if (sub != null)
                Subscriptions.Remove(sub);
        }

        #endregion 

        #region Receivers

        public void Handle(RunCompleted notification)
        {
            InvokeForSubscriptions(notification);
        }

        public void Handle(RunSpecUpdated notification)
        {
            InvokeForSubscriptions(notification);
        }

        public void Handle(RunCreated notification)
        {
            InvokeForSubscriptions(notification);
        }

        #endregion

        #region Private

        private void InvokeForSubscriptions<TNotification>(TNotification notification) where TNotification : INotification
        {
            var subscriptions = GetSubscriptions<TNotification>();

            foreach (var subscription in subscriptions)
            {
                subscription.Item3.Invoke(notification);
            }
        }

        private Tuple<object, Type, Action<INotification>> GetSubscriptionOrDefault<TNotification>(object instance)
        {
            return Subscriptions.SingleOrDefault(s => s.Item1.Equals(instance) && s.Item2 == typeof(TNotification));
        }

        private List<Tuple<object, Type, Action<INotification>>> GetSubscriptions<TNotification>()
        {
            return Subscriptions.Where(s => s.Item2 == typeof(TNotification)).ToList();
        }


        #endregion  
    }
}
