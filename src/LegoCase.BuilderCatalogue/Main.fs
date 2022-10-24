module Program

open System
open FsHttp

let baseUrl = "https://d16m5wbro86fg2.cloudfront.net"
let getSet = LegoApi.Set baseUrl
    
let data =
           getSet (Guid("11aabef6-4fd1-4464-bc46-15814f01baa6"))
           |> Async.AwaitTask
           |> Async.RunSynchronously

printfn "DATA: %A" data

printfn "OK."
