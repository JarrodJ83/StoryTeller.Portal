using System.Threading;
using System.Threading.Tasks;

namespace StoryTeller.Portal.CQRS
{
    public interface IQuery<TResult>
    {
    }

    public interface IQueryHandler<in TQry, TResult> where TQry : IQuery<TResult>
    {
        Task<TResult> FetchAsynx(TQry qry, CancellationToken cancellationToken);
    }

    public interface ICommand
    {
    }

    public interface ICommandHandler<in TCmd> where TCmd : ICommand
    {
        Task ExecuteAsync(TCmd cmd, CancellationToken cancellationToken);
    }

    public interface ICommand<out TKey> where TKey : struct 
    {
        TKey Key { get; }
    }

    public interface ICommandHandler<in TCmd, TKey> where TCmd : ICommand<TKey> where TKey : struct 
    {
        Task ExecuteAsync(TCmd cmd, CancellationToken cancellationToken);
    }

    public interface IRequest
    {
    }

    public interface IRequest<TResponse>
    {
    }

    public interface IRequestHandler<in TRequest> where TRequest : IRequest
    {
        Task HandleAsync(TRequest request, CancellationToken cancellationToken);
    }

    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
