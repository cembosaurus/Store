using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Business.Data.Tools
{
    public class DbQueryComandInterceptor : DbCommandInterceptor
    {

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            //command.CommandText += "\r\nOFFSET @__p_0 ROWS FETCH NEXT @__p_0 ROWS ONLY";


            Console.WriteLine($"---------------------------------------------- > COMMAND: {command.CommandText}");



            return new(result);
        }




        //// runs before a query is executed
        //public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        //{
        //    Console.WriteLine($"--------------------------------> Before Query execution. Query : {command.CommandText}");
        //    return result;
        //}
        //// runs after a query is excuted
        //public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        //{
        //    Console.WriteLine($"--------------------------------> After Query execution. Query : {command.CommandText}");
        //    return result;
        //}



        //public override InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result)
        //    => result;

        ///// <inheritdoc />
        //public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
        //    => result;

        ///// <inheritdoc />
        //public override DbCommand CommandInitialized(CommandEndEventData eventData, DbCommand result)
        //    => result;

        ///// <inheritdoc />

        ///// <inheritdoc />
        //public override InterceptionResult<object> ScalarExecuting(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<object> result)
        //    => result;

        ///// <inheritdoc />
        //public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        //    => result;

        ///// <inheritdoc />
        //public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<DbDataReader> result,
        //    CancellationToken cancellationToken = default)
        //    => new(result);

        ///// <inheritdoc />
        //public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<object> result,
        //    CancellationToken cancellationToken = default)
        //    => new(result);

        ///// <inheritdoc />
        //public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        //    DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<int> result,
        //    CancellationToken cancellationToken = default)
        //    => new(result);

        ///// <inheritdoc />

        ///// <inheritdoc />
        //public override object? ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object? result)
        //    => result;

        ///// <inheritdoc />
        //public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        //    => result;

        ///// <inheritdoc />
        //public override ValueTask<DbDataReader> ReaderExecutedAsync(
        //    DbCommand command,
        //    CommandExecutedEventData eventData,
        //    DbDataReader result,
        //    CancellationToken cancellationToken = default)
        //    => new(result);

        ///// <inheritdoc />
        //public override ValueTask<object?> ScalarExecutedAsync(
        //    DbCommand command,
        //    CommandExecutedEventData eventData,
        //    object? result,
        //    CancellationToken cancellationToken = default)
        //    => new(result);

        ///// <inheritdoc />
        //public override ValueTask<int> NonQueryExecutedAsync(
        //    DbCommand command,
        //    CommandExecutedEventData eventData,
        //    int result,
        //    CancellationToken cancellationToken = default)
        //    => new(result);

        ///// <inheritdoc />
        //public override void CommandCanceled(DbCommand command, CommandEndEventData eventData)
        //{
        //}

        ///// <inheritdoc />
        //public override Task CommandCanceledAsync(
        //    DbCommand command,
        //    CommandEndEventData eventData,
        //    CancellationToken cancellationToken = default)
        //    => Task.CompletedTask;

        ///// <inheritdoc />
        //public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
        //{
        //}

        ///// <inheritdoc />
        //public override Task CommandFailedAsync(
        //    DbCommand command,
        //    CommandErrorEventData eventData,
        //    CancellationToken cancellationToken = default)
        //    => Task.CompletedTask;

        ///// <inheritdoc />
        //public override InterceptionResult DataReaderClosing(DbCommand command, DataReaderClosingEventData eventData, InterceptionResult result)
        //    => result;

        ///// <inheritdoc />
        //public override ValueTask<InterceptionResult> DataReaderClosingAsync(
        //    DbCommand command,
        //    DataReaderClosingEventData eventData,
        //    InterceptionResult result)
        //    => new(result);

        ///// <inheritdoc />
        //public override InterceptionResult DataReaderDisposing(
        //    DbCommand command,
        //    DataReaderDisposingEventData eventData,
        //    InterceptionResult result)
        //    => result;








    }

}
