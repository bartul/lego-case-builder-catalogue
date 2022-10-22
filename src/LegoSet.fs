module LegoSet

open System 

type Set =
  { Id: Guid; 
    Name: string; 
    SetNumber: string; 
    Pieces: Piece[]; 
    TotalPieces: int }
and Piece = 
  { Part: Part; 
    Quantity: int }
and Part =
  { DesignId: int; 
    Material: int; 
    PartType: string }
