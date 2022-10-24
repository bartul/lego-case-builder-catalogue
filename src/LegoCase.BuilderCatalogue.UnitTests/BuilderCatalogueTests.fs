module Tests

open Expecto
open LegoUser
open LegoSet

[<Tests>]
let tests =
  testList "builder catalog" [
    testCase "can build when both are empty" <| fun _ ->
      let collection = [||]
      let setPieces = [||]
      Expect.isTrue (BuilderCatalogue.canBuild collection setPieces) "User should be able to build this set"

    testCase "can build empty set" <| fun _ ->
      let collection = [| { PieceId = "3023"; Variants = [| { Color = "36"; Count = 1 } |] } |]
      let setPieces = [||]
      Expect.isTrue (BuilderCatalogue.canBuild collection setPieces) "User should be able to build this set"

    testCase "can build single piece set" <| fun _ ->
      let collection = [| { PieceId = "3023"; Variants = [| { Color = "36"; Count = 1 } |] } |]
      let setPieces = [| { Part = { DesignId = "3023"; Material = 36; PartType = "rigid"  }; Quantity = 1 } |]
      Expect.isTrue (BuilderCatalogue.canBuild collection setPieces) "User should be able to build this set"

    testCase "can build because of invalid color code" <| fun _ ->
      let collection = [| { PieceId = "3023"; Variants = [| { Color = "A36"; Count = 1 } |] } |]
      let setPieces = [| { Part = { DesignId = "3023"; Material = 36; PartType = "rigid"  }; Quantity = 1 } |]
      Expect.isFalse (BuilderCatalogue.canBuild collection setPieces) "User should not be able to build this set"

    testCase "can not build single piece set as user doesn't have a correct design id" <| fun _ ->
      let collection = [| { PieceId = "9999"; Variants = [| { Color = "36"; Count = 1 } |] } |]
      let setPieces = [| { Part = { DesignId = "3023"; Material = 36; PartType = "rigid"  }; Quantity = 1 } |]
      Expect.isFalse (BuilderCatalogue.canBuild collection setPieces) "User should not be able to build this set"

    testCase "can not build single piece set as user doesn't have a correct material" <| fun _ ->
      let collection = [| { PieceId = "3023"; Variants = [| { Color = "99"; Count = 1 } |] } |]
      let setPieces = [| { Part = { DesignId = "3023"; Material = 36; PartType = "rigid"  }; Quantity = 1 } |]
      Expect.isFalse (BuilderCatalogue.canBuild collection setPieces) "User should not be able to build this set"

    testCase "can not build single piece set as user doesn't have a correct quantity" <| fun _ ->
      let collection = [| { PieceId = "3023"; Variants = [| { Color = "36"; Count = 1 } |] } |]
      let setPieces = [| { Part = { DesignId = "3023"; Material = 36; PartType = "rigid"  }; Quantity = 99 } |]
      Expect.isFalse (BuilderCatalogue.canBuild collection setPieces) "User should not be able to build this set"

    testCase "can build set" <| fun _ ->
      let collection = [|
        { PieceId = "3023"; Variants = [| { Color = "36"; Count = 50 }; { Color = "2"; Count = 10 }; { Color = "8"; Count = 20 } |] }
        { PieceId = "77232"; Variants = [| { Color = "8"; Count = 10 } |] }
        { PieceId = "3023"; Variants = [| { Color = "7"; Count = 6 } |] }
        { PieceId = "3024"; Variants = [| { Color = "2"; Count = 12 } |] }
        { PieceId = "11477"; Variants = [| { Color = "8"; Count = 66 } |] }
        { PieceId = "2431"; Variants = [| { Color = "7"; Count = 43 } |] }
        { PieceId = "63864"; Variants = [| { Color = "9"; Count = 18 } |] }
        { PieceId = "3710"; Variants = [| { Color = "7"; Count = 43 } |] }
      |]
      let setPieces = [|
        { Part = { DesignId = "3023"; Material = 36; PartType = "rigid"  }; Quantity = 1 }
        { Part = { DesignId = "3023"; Material = 8; PartType = "rigid"  }; Quantity = 10 }
        { Part = { DesignId = "77232"; Material = 8; PartType = "rigid"  }; Quantity = 4 }
        { Part = { DesignId = "3023"; Material = 7; PartType = "rigid"  }; Quantity = 6 }
        { Part = { DesignId = "3024"; Material = 2; PartType = "rigid"  }; Quantity = 10 }
        { Part = { DesignId = "11477"; Material = 8; PartType = "rigid"  }; Quantity = 6 }
        { Part = { DesignId = "2431"; Material = 7; PartType = "rigid"  }; Quantity = 2 }
      |]
      Expect.isTrue (BuilderCatalogue.canBuild collection setPieces) "User should be able to build this set"

    testCase "can't build set" <| fun _ ->
      let collection = [|
        { PieceId = "3023"; Variants = [| { Color = "36"; Count = 50 }; { Color = "2"; Count = 10 }; { Color = "8"; Count = 20 } |] }
        { PieceId = "77232"; Variants = [| { Color = "8"; Count = 1 } |] } // <- count low 
        { PieceId = "3023"; Variants = [| { Color = "7"; Count = 6 } |] }
        { PieceId = "3024"; Variants = [| { Color = "2"; Count = 12 } |] }
        { PieceId = "11477"; Variants = [| { Color = "8"; Count = 66 } |] }
        { PieceId = "2431"; Variants = [| { Color = "7"; Count = 43 } |] }
        { PieceId = "63864"; Variants = [| { Color = "9"; Count = 18 } |] }
        { PieceId = "3710"; Variants = [| { Color = "7"; Count = 43 } |] }
      |]
      let setPieces = [|
        { Part = { DesignId = "3023"; Material = 36; PartType = "rigid"  }; Quantity = 1 }
        { Part = { DesignId = "3023"; Material = 8; PartType = "rigid"  }; Quantity = 10 }
        { Part = { DesignId = "77232"; Material = 8; PartType = "rigid"  }; Quantity = 4 }
        { Part = { DesignId = "3023"; Material = 7; PartType = "rigid"  }; Quantity = 6 }
        { Part = { DesignId = "3024"; Material = 2; PartType = "rigid"  }; Quantity = 10 }
        { Part = { DesignId = "11477"; Material = 8; PartType = "rigid"  }; Quantity = 6 }
        { Part = { DesignId = "2431"; Material = 7; PartType = "rigid"  }; Quantity = 2 }
      |]
      Expect.isFalse (BuilderCatalogue.canBuild collection setPieces) "User should not be able to build this set"
 ]
