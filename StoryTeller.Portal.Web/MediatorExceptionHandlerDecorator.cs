using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace storyteller.portal.dotnetify
{
    public class MediatorExceptionHandlerDecorator : IMediator
    {
        private readonly IMediator _decorated;

        public MediatorExceptionHandlerDecorator(IMediator decorated)
        {
            _decorated = decorated;
        }
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await _decorated.Send(request, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await new Task<TResponse>(() => default(TResponse));
            }
        }

        public async Task Send(IRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await _decorated.Send(request, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
        {
            try
            {
                await _decorated.Publish(notification, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
