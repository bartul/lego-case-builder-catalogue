module BuilderCatalogue

open System

let private transformUserPieceToCanonical (piece : LegoUser.Piece) =
    piece.Variants
    |> Array.map (fun var -> {| Part = {| DesignId = piece.PieceId; Material = match Int32.TryParse var.Color with | true, value -> value | false, _ -> -1 |}; UserQuantity = var.Count; SetQuantity = 0 |})

let private transformSetPieceToCanonical (piece : LegoSet.Piece) =
    {| Part = {| DesignId = piece.Part.DesignId; Material = piece.Part.Material |}; UserQuantity = 0; SetQuantity = piece.Quantity|}

let canBuild (collection : LegoUser.Piece[]) (set : LegoSet.Piece[]) =
    match collection, set with 
    | [||], [||] -> true 
    | _, [||] -> true 
    | [||], _ -> false
    | userPieces, setPieces ->
        let userPiecesCanonical =
            userPieces
            |> Array.fold (fun canonical item ->
                canonical
                |> Array.append (item |> transformUserPieceToCanonical))
                [||]
        let setPiecesCanonical =
            setPieces
            |> Array.map transformSetPieceToCanonical
        let totalInventory = 
            userPiecesCanonical
            |> Array.append setPiecesCanonical
            |> Array.groupBy (fun item -> item.Part)
            |> Array.map (fun (key, values) ->
                {| DesignId = key.DesignId; Material = key.Material; UserQuantity = values |> Array.sumBy (fun item -> item.UserQuantity); SetQuantity = values |> Array.sumBy (fun item -> item.SetQuantity) |})

        totalInventory
        |> Array.exists (fun item -> item.SetQuantity > item.UserQuantity)
        |> not

let canBuildWithMatrix collection setPieces =
    // TO DO: Implement
    Threading.Thread.Sleep(1)
    canBuild collection setPieces
