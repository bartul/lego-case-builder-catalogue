module MainBenchmarks

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Engines
open BenchmarkDotNet.Running
open LegoUser
open LegoSet

[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.Monitoring, targetCount = 50, warmupCount = 5)>]
type CanBuildBenchmark() =

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
    [<Benchmark(Baseline=true)>]
    member _.BruteForce() =
        BuilderCatalogue.canBuild collection setPieces
        |> ignore

    [<Benchmark>]
    member _.Matrix() = 
        BuilderCatalogue.canBuildWithMatrix collection setPieces
        |> ignore

   

[<EntryPoint>]
let main _ =
    BenchmarkRunner.Run<CanBuildBenchmark>() |> ignore
    0