#r "nuget: FsHttp"
#r "nuget: FSharp.SystemTextJson"
#load "LegoSet.fs"
#load "LegoUser.fs"

open FsHttp
open LegoUser
open System.Text.Json

let data : User[] = 
// let data = 
    http { 
        GET "https://d16m5wbro86fg2.cloudfront.net/api/users"
    }
    |> Request.send
    |> Response.toJson
    |> fun json -> json?Users
    |> fun json -> JsonSerializer.Deserialize(json) 

printfn "DATA: %A" data

printfn "OK."

