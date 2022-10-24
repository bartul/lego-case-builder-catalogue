module LegoApi

open System
open FsHttp
open System.Text.Json
open LegoSet
open LegoUser

let private deserialize<'a> (json : JsonElement) =
    JsonSerializer.Deserialize<'a>(json, JsonSerializerOptions(JsonSerializerDefaults.Web))
let private tryDeserialize<'a> (json : JsonElement) =
    try
        Result.Ok(deserialize<'a> json)
    with
        | ex -> Result.Error($"Failed to deserialize JSON data, %s{ex.Message}\n Data: {json}") 

let private query<'a> url parameters (jsonMap : JsonElement -> JsonElement) rootUrl =
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
let UserByUserName rootUrl userName = query<User> $"/api/user/by-username/%s{userName}" [] id rootUrl 
let User rootUrl (userId : Guid) = query<User> $"/api/user/by-id/%A{userId}" [] id rootUrl 
let Sets = query<Set[]> "/api/sets" [] (fun json -> json?Sets)
let SetByName rootUrl name  = query<Set> $"/api/set/by-name/%s{name}" [] id rootUrl
let Set rootUrl (setId : Guid) = query<Set> $"/api/set/by-id/%A{setId}" [] id rootUrl

