module BuilderCatalogueMain

open FsHttp
open FsToolkit.ErrorHandling

let baseUrl = "https://d16m5wbro86fg2.cloudfront.net"

let getUserByName = LegoApi.UserByUserName baseUrl
let getUser = LegoApi.User baseUrl
let getSet = LegoApi.Set baseUrl
let getSets = LegoApi.Sets baseUrl

let data =
   getUserByName "brickfan35"
   |> TaskResult.bind (fun basicUser -> getUser basicUser.Id)
   |> TaskResult.bind (fun user -> getSets |> TaskResult.map (fun basicSets -> user, basicSets))
   |> TaskResult.bind (fun (user, basicSets) ->
      basicSets
      |> Array.map (fun basicSet -> getSet basicSet.Id)
      |> Array.toList
      |> List.sequenceTaskResultM
      |> TaskResult.map (fun sets -> user, sets))
   |> TaskResult.map (fun (user, sets) ->
      user.UserName,
      sets
      |> List.map (fun set -> set.Name, (BuilderCatalogue.canBuild (Option.defaultValue [||] user.Collection) (Option.defaultValue [||] set.Pieces)))
      |> List.filter snd 
      |> List.map fst)
   |> Async.AwaitTask
   |> Async.RunSynchronously

printfn "DATA: %A" data
printfn "OK."
