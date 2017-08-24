using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddRunRequestHandler : IRequestHandler<Requests.AddRunRequest, Run>
    {
        private ICommandHandler<Commands.AddRunForApplication> addRunForApplication;

        public AddRunRequestHandler(ICommandHandler<AddRunForApplication> addRunForApplication)
        {
            this.addRunForApplication = addRunForApplication;
        }

        public async Task<Run> HandleAsync(Requests.AddRunRequest request, CancellationToken cancellationToken)
        {
            var run = new Run
            {
                Name = request.PostedRun.RunName,
                RunDateTime = request.PostedRun.RunDateTime
            };

            var addRunForApplicationCommand = new AddRunForApplication(request.ApplicationId, run);

            await addRunForApplication.ExecuteAsync(addRunForApplicationCommand, cancellationToken);

            return run;
        }
    }
}
