namespace RecipeApp.FCoreApi.Features.Helpers

open System
open System.Data
open System.Data.SqlClient
open System.Threading
open System.Threading.Tasks
open System.Runtime.InteropServices

// https://dev.to/kspeakman/dirt-simple-sql-queries-in-f-a37
module DbHelpers =
    type ConnectionHelper =
        static member CreateSqlConnection connectionString =
            new SqlConnection(connectionString)

        static member CreateOpenSqlConnectionAsync connectionString cancellationToken =
            async {
                let sqlConnection = new SqlConnection(connectionString);
                if sqlConnection.State <> ConnectionState.Open
                then
                    do! sqlConnection.OpenAsync(cancellationToken) |> Async.AwaitTask

                return sqlConnection
            }

        static member CreateSqlUnitOfWorkAsync connectionString cancellationToken =
            async {
                let sqlConnection = new SqlConnection(connectionString);
                if sqlConnection.State <> ConnectionState.Open
                then
                    do! sqlConnection.OpenAsync(cancellationToken) |> Async.AwaitTask

                let! sqlTransaction = sqlConnection.BeginTransactionAsync(cancellationToken).AsTask() |> Async.AwaitTask

                return (sqlConnection, sqlTransaction)
            }

    // https://nozzlegear.com/blog/using-dapper-with-fsharp
    // https://stackoverflow.com/questions/70539260/f-async-to-asyncobj
    // https://www.fssnip.net/7Va/title/Await-Computation-Expression-Task-Async-Interoperability
    // https://theburningmonk.com/2012/10/f-helper-functions-to-convert-between-asyncunit-and-task/
    type SqlUoW(cs: string, ct: CancellationToken) as this  =
        do
            async {
                let! c = ConnectionHelper.CreateOpenSqlConnectionAsync cs ct
                this.Connection <- c
                let! t = c.BeginTransactionAsync(ct).AsTask() |> Async.AwaitTask
                this.Transaction <- t
            } |> Async.RunSynchronously

        member val Connection: SqlConnection = new SqlConnection() with get, set
        member val Transaction: Common.DbTransaction = null with get, set

        interface System.IDisposable with
            member this.Dispose() =
                this.Connection.Dispose()
                this.Transaction.Dispose()
        //new(cs: string, ct: CancellationToken) = new SqlUoW(cs, ct)

        //static member GetSqlConnectionAsync(connectionString: string, cancellationToken: CancellationToken, [<Optional; DefaultParameterValue(true)>] openIt: bool) : Async<SqlConnection> =
        //    async {
        //        let sqlConnection = new SqlConnection(connectionString);
        //        if openIt && sqlConnection.State <> ConnectionState.Open
        //        then
        //            do! sqlConnection.OpenAsync(cancellationToken) |> Async.AwaitTask

        //        return sqlConnection
        //    }


        //static member GetSqlConnectionAsync2(connectionString: string) (cancellationToken: CancellationToken) ([<Optional; DefaultParameterValue(true)>] openIt: bool) : Async<SqlConnection> =
        //    async {
        //        let sqlConnection = new SqlConnection("");
        //        if openIt && sqlConnection.State <> ConnectionState.Open
        //        then
        //            do! sqlConnection.OpenAsync(cancellationToken) |> Async.AwaitTask |> Async.Ignore

        //        return sqlConnection
        //    }

