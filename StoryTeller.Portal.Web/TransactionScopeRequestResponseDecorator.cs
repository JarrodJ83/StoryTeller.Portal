using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using StoryTeller.Portal.CQRS;

namespace StoryTeller.Portal
{
    public class TransactionScopeRequestResponseDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _decorated;

        public TransactionScopeRequestResponseDecorator(IRequestHandler<TRequest, TResponse> decorated)
        {
            _decorated = decorated;
        }
        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                return await _decorated.HandleAsync(request, cancellationToken);
            }
        }
    }
}
