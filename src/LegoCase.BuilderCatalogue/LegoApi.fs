module LegoApi

open System
open FsHttp
open System.Text.Json
open FsToolkit.ErrorHandling
open LegoSet
open LegoUser

let private deserialize<'a> (json : JsonElement) =
    JsonSerializer.Deserialize<'a>(json, JsonSerializerOptions(JsonSerializerDefaults.Web))
let private tryDeserialize<'a> (json : JsonElement) =
    try
        Result.Ok(deserialize<'a> json)
    with
        | ex -> Result.Error($"Failed to deserialize JSON data, %s{ex.Message}\n Data: {json}") 
let private trySendTAsync context =
    try
        TaskResult.ofTask (context |> Request.sendTAsync)
    with
        | ex -> TaskResult.error $"Failed to send http request, %s{ex.Message}" 
    
let private query<'a> url parameters (jsonMap : JsonElement -> JsonElement) rootUrl =
    task {
        let apiUrl = $"%s{rootUrl}%s{url}"
        let! response =
            http {
                GET apiUrl
                query parameters
            }
            |> trySendTAsync

        return response
            |> Result.bind (fun response ->
                Response.expectStatusCode 200 response
                |> Result.mapError (fun error -> $"Got http status code '%A{error.actual}' but was expecting '%A{error.expected |> List.toArray}' from %s{apiUrl}")) 
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

