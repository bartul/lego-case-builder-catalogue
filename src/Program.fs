module Program

open FsHttp
open LegoUser

let data = LegoApi.query<User[]> "https://d16m5wbro86fg2.cloudfront.net/api/users" (fun json -> json?Users)
           |> Async.AwaitTask
           |> Async.RunSynchronously
    
printfn "DATA: %A" data

printfn "OK."
