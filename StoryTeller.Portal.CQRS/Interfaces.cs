using System;

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
}
