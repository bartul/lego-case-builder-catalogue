module LegoApi

open System
open FsHttp
open System.Text.Json
open LegoSet
open LegoUser

let deserialize<'a> (json : JsonElement) =
    JsonSerializer.Deserialize<'a>(json, JsonSerializerOptions(JsonSerializerDefaults.Web))
let tryDeserialize<'a> (json : JsonElement) =
    try
        Result.Ok(deserialize<'a> json)
    with
        | ex -> Result.Error($"Failed to deserialize JSON data, %s{ex.Message}\n Data: {json}") 

let query<'a> url parameters (jsonMap : JsonElement -> JsonElement) rootUrl =
    task {
        let! response = 
            http {
                GET $"%s{rootUrl}%s{url}"
                query parameters
            }
            |> Request.sendTAsync
        return response
            |> Response.expectStatusCode 200
            |> Result.mapError (fun error -> $"Got http status code '%A{error.actual}' but was expecting '%A{error.expected |> List.toArray}'")
            |> Result.map Response.toJson
            |> Result.map jsonMap
            |> Result.bind tryDeserialize<'a>
    }

let Users = query<User[]> "/api/users" [] (fun json -> json?Users)
let UserByUserName userName = query<User> $"/api/user/by-username/%s{userName}" [] (fun json -> json)
let User (userId : Guid) = query<User> $"/api/user/by-id/%A{userId}" [] (fun json -> json)
let Sets = query<Set[]> "/api/sets" [] (fun json -> json?Sets)
let SetByName name = query<Set> $"/api/set/by-name/%s{name}" [] (fun json -> json)
let Set (setId : Guid) = query<Set> $"/api/set/by-id/%A{setId}" [] (fun json -> json)

