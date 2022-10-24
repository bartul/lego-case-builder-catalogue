module LegoSet

open System 

type Set =
  { Id: Guid; 
    Name: string; 
    SetNumber: string; 
    Pieces: Piece[] option; 
    TotalPieces: int }
and Piece = 
  { Part: Part; 
    Quantity: int }
and Part =
  { DesignId: string; 
    Material: int; 
    PartType: string }
