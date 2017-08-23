namespace StoryTeller.Portal.CQRS
{
    public interface IQuery<TResult>
    {
    }

    public interface IQueryHandler<in TQry, out TResult> where TQry : IQuery<TResult>
    {
        TResult Fetch(TQry qry);
    }

    public interface ICommand
    {
    }

    public interface ICommandHandler<in TCmd> where TCmd : ICommand
    {
        void Execute(TCmd cmd);
    }

    public interface IRequest
    {
    }

    public interface IRequest<TResponse>
    {
    }

    public interface IRequestHandler<in TRequest> where TRequest : IRequest
    {
        void Handle(TRequest request);
    }

    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest request);
    }
}
