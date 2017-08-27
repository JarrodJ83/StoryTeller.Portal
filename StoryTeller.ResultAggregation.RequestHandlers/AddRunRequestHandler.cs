using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Events;
using StoryTeller.ResultAggregation.Models;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddRunRequestHandler : Portal.CQRS.IRequestHandler<Requests.AddRunRequest, Run>
    {
        private readonly IMediator _mediator;
        private ICommandHandler<Commands.AddRunForApplication> addRunForApplication;

        public AddRunRequestHandler(IMediator mediator, ICommandHandler<AddRunForApplication> addRunForApplication)
        {
            _mediator = mediator;
            this.addRunForApplication = addRunForApplication;
        }

        public async Task<Run> HandleAsync(Requests.AddRunRequest request, CancellationToken cancellationToken)
        {
            var run = new Run
            {
                Name = request.PostedRun.RunName,
                RunDateTime = request.PostedRun.RunDateTime
            };

            var addRunForApplicationCommand = new AddRunForApplication(request.AppId, run);

            await addRunForApplication.ExecuteAsync(addRunForApplicationCommand, cancellationToken);

            await _mediator.Publish(new RunCreated(run.Id), cancellationToken);

            return run;
        }
    }
}
