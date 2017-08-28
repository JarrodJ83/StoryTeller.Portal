using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MediatR;
using StoryTeller.ResultAggregation.Events;

namespace storyteller.portal.dotnetify.view_models
{
    public class RunFeedDataSource : IAsyncNotificationHandler<RunCreated>, INotifyPropertyChanged
    {
        public List<string> Runs { get; set; }
        
        public RunFeedDataSource()
        {
            Runs = new List<string>();
        }
        public async Task Handle(RunCreated notification)
        {
            Runs.Add(notification.RunId.ToString());
            OnPropertyChanged(nameof(Runs));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
