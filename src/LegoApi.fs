module LegoApi

open FsHttp
open System.Text.Json

let deserialize<'a> (json : JsonElement) =
    JsonSerializer.Deserialize<'a>(json, JsonSerializerOptions(JsonSerializerDefaults.Web))
let tryDeserialize<'a> (json : JsonElement) =
    try
        Result.Ok(deserialize<'a> json)
    with
        | _ -> Result.Error("Failed to deserialize JSON data") 

let query<'a> url (jsonMap : JsonElement -> JsonElement) =
    task {
        let! response = 
            http { GET url }
            |> Request.sendTAsync
        return response
            |> Response.expectStatusCode 200
            |> Result.mapError (fun error -> $"Got http status code %A{error.actual} but was expecting %A{error.expected.Head}")
            |> Result.map Response.toJson
            |> Result.map jsonMap
            |> Result.bind tryDeserialize<'a>
    }


