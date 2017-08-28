using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal
{
    public class TransactionScopeRequestDecorator<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest
    {
        private readonly IRequestHandler<TRequest> _decorated;

        public TransactionScopeRequestDecorator(IRequestHandler<TRequest> decorated)
        {
            _decorated = decorated;
        }
        public async Task HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _decorated.HandleAsync(request, cancellationToken);
            }
        }
    }
}
