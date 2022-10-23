module Program

open System
open FsHttp

    
let data =
           LegoApi.Set (Guid("11aabef6-4fd1-4464-bc46-15814f01baa6")) "https://d16m5wbro86fg2.cloudfront.net"  
           |> Async.AwaitTask
           |> Async.RunSynchronously

printfn "DATA: %A" data

printfn "OK."
