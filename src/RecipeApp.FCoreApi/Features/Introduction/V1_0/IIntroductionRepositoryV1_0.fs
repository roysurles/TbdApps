namespace RecipeApp.FCoreApi.Features.Introduction.V1_0.Introduction

open Tbd.Shared.Pagination
open System
open System.Collections.Generic
open System.Linq
open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
open Tbd.WebApi.Shared.Repositories
open System.Data
open System.Data.SqlClient
open Dapper

type IIntroductionRepositoryV1_0 =
    //abstract member SearchAsync: introductionSearchRequestDto: IntroductionSearchRequestDto cancellationToken: CancellationToken -> unit //Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)>

    abstract member DeleteAsync: Guid -> CancellationToken -> Task<int>
    //Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken);


type IntroductionRepositoryV1_0(connectionString: string) =
    inherit BaseRepository(connectionString)
    interface IIntroductionRepositoryV1_0 with
        member this.DeleteAsync id cancellationToken =
            task {
                let x : int = 5
                let b : byte = byte x

                use! connection = this.CreateConnectionAsync(cancellationToken)
                let a : SqlConnection = connection :?> SqlConnection
                use! transaction = a.BeginTransactionAsync(cancellationToken)

                let parameters = {| Id = id |}  //TODO use dict[string, obj]
                let commandDefinition = new CommandDefinition(commandText = "IntroductionDelete", parameters = parameters, transaction = transaction, commandType = CommandType.StoredProcedure, cancellationToken = cancellationToken)

                let! result = connection.ExecuteScalarAsync<int>(commandDefinition).ConfigureAwait(false);

                transaction.Commit()

                return result
            }
