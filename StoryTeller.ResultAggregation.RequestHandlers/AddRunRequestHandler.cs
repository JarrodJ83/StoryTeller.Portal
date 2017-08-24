using System.Threading;
using System.Threading.Tasks;
using StoryTeller.Portal.CQRS;
using StoryTeller.ResultAggregation.Commands;

namespace StoryTeller.ResultAggregation.RequestHandlers
{
    public class AddRunRequestHandler : IRequestHandler<Requests.AddRunRequest, int>
    {
        private ICommandHandler<Commands.AddRunForApplication, int> addRunForApplication;

        public AddRunRequestHandler(ICommandHandler<AddRunForApplication, int> addRunForApplication)
        {
            this.addRunForApplication = addRunForApplication;
        }

        public async Task<int> HandleAsync(Requests.AddRunRequest request, CancellationToken cancellationToken)
        {
            var addRunForApplicationCommand = new AddRunForApplication(request.ApplicationId, request.RunName, request.RunDateTime);
            await addRunForApplication.ExecuteAsync(addRunForApplicationCommand, cancellationToken);
            return addRunForApplicationCommand.Key;
        }
    }
}
