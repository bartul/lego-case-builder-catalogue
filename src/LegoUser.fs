module LegoUser

open System

type User = 
  { Id: Guid; 
    UserName: string;
    Location: string;
    BrickCount: int;
    Collection: Piece[] option }
and Piece = 
  { PieceId: int;
    Variants: PieceVariant[] }
and PieceVariant = 
  { Color: string; 
    Count: int }

