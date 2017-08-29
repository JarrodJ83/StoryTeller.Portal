using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using StoryTeller.ResultAggregation.Events;

namespace storyteller.portal.dotnetify.view_models
{
    public class RunFeedDataSource : IAsyncNotificationHandler<RunCreated>, INotifyPropertyChanged
    {
        private readonly IQueryHandler<LatestRunSumarries, List<RunSummary>> _runSummariesQueryHandler;
        private readonly IQueryHandler<SummaryOfRun, RunSummary> _summaryOfRunQueryHandler;
        public List<RunSummary> Runs { get; set; } = new List<RunSummary>();
        
        public RunFeedDataSource(IQueryHandler<LatestRunSumarries, List<RunSummary>> runSummariesQueryHandler, IQueryHandler<SummaryOfRun, RunSummary> summaryOfRunQueryHandler)
        {
            _runSummariesQueryHandler = runSummariesQueryHandler;
            _summaryOfRunQueryHandler = summaryOfRunQueryHandler;
            _runSummariesQueryHandler.FetchAsync(new LatestRunSumarries(), CancellationToken.None).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    Runs.AddRange(t.Result);
                    NotifyChanges();
                }
            });
        }
        
        public async Task Handle(RunCreated notification)
        {
            RunSummary runSummary = await _summaryOfRunQueryHandler.FetchAsync(new SummaryOfRun(notification.RunId), CancellationToken.None);

            Add(runSummary);
        }

        private void Add(RunSummary runSummary)
        {
            Runs.Add(runSummary);

            NotifyChanges();
        }

        private void NotifyChanges()
        {
            Runs = Runs.OrderByDescending(r => r.RunDateTime).ToList();

            OnPropertyChanged(nameof(Runs));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
