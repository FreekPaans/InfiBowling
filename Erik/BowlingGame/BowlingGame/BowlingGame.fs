namespace BowlingGame

    open System.Collections.Generic;
    open ErikTillema.FSharp
    
    type Frame = 
        /// Strike with next 2 throws
        | Strike of Option<int> * Option<int> 
        /// Spare with next throw
        | Spare of Option<int>
        /// Normal frame of 2 throws
        | NormalFrame of int * int 
        /// Unfinished frame of 1 throw
        | UnfinishedFrame of int 

    type Game(frames: Frame list, throws: int list) = 
        
        /// Frames of this game
        let mutable frames = frames

        /// List of int, throws of this game, used to build the list of frames
        let mutable throws = throws
        
        /// Creates a list of Frames based on the list of pins thrown 
        let createGame (throws: int list) = 
            /// Checks that no throw is invalid by itself
            let checkThrows throws = 
                for throw in throws do
                    if (throw < 0 || throw > 10) then invalidArg "throws" "invalid number of pins" 
                
            /// Creates a Frame by looking at the next couple of throws in list of throws
            let createFrame throws = 
                match throws with
                | [] -> invalidArg "throws" "Can't build a frame without pins thrown" 
                | 10::t ->
                    match t with
                    | [] -> (Strike(None, None), t)
                    | n::[] -> (Strike(Some(n), None), t)
                    | 10::m::_ -> (Strike(Some(10), Some(m)), t)
                    | n::m::_ when n + m > 10 -> invalidArg "throws" "invalid number of pins"
                    | n::m::_ -> (Strike(Some(n), Some(m)), t)
                | a::b::t when a + b = 10 ->
                    match t with
                    | [] -> (Spare(None), t)
                    | n::_ -> (Spare(Some(n)), t)
                | a::b::t when a + b > 10 -> invalidArg "throws" "invalid number of pins"
                | a::b::t -> (NormalFrame(a, b), t)
                | a::[] -> (UnfinishedFrame(a), [])

            let rec createGame' nacc acc (throws : int list) =
                match (nacc, acc, throws) with
                | 10, NormalFrame(_,_)::_  , _::t -> invalidArg "throws" "too many throws" 
                | 10, Spare(_)::_          , _::_::t -> invalidArg "throws" "too many throws" 
                | 10, Strike(_,_)::_       , _::_::_::t -> invalidArg "throws" "too many throws" 
                | 10, _, _ -> List.rev acc // don't add more Frames after 10th frame
                | _, _, [] -> List.rev acc
                | _ -> 
                    let frame, rest = createFrame throws
                    createGame' (nacc+1) (frame::acc) rest

            checkThrows throws
            throws |> createGame' 0 []

        /// Returns the score of the given frame
        static let score (frame: Frame) =
            match frame with
            | Spare None -> 10
            | Spare (Some n) -> 10 + n
            | Strike (None, None) -> 10
            | Strike (Some n, None) -> 10 + n
            | Strike (Some n, Some m) -> 10 + n + m
            | Strike (None, Some n) -> failwith "Strike then None, Some" 
            | NormalFrame (n, m) -> n + m
            | UnfinishedFrame n -> n

        /// returns the score of the frame-th frame in game (zero based)
        static let scoreOf frame frames = 
            frames |> ListUtil.skip frame |> List.head |> score

        /// returns the cummulative score of the frame-th frame (zero based)
        static let cummScoreOf frame frames = 
            frames |> ListUtil.take (frame + 1) |> List.sumBy score

        new () = Game([], [])
               
        member this.CurrentScore = cummScoreOf (List.length frames - 1) frames

        /// Returns the cummulative score of the i-th frame (one based)
        member this.ScoreVoorFrame i = cummScoreOf (i-1) frames

        member this.Gooi (p: int) = 
            throws <- throws @ [p]
            frames <- throws |> createGame

    type Game2() =
        let mutable throws = []

        let rec throwsPerFrame throws =
            match throws with
            | [] -> [0]
            | throw1::[] -> [throw1]
            | throw1::throw2::tail -> [throw1+throw2] @ (throwsPerFrame tail)

        let scoreVoorFrame frame =
            throwsPerFrame throws
            |> Seq.nth (frame-1) 
            
              
//            let aantalThrows = (Seq.min [Seq.length throws;(frame * 2)])
//            throws
//            |> Seq.take aantalThrows
//            |> Seq.sum 

        member this.Gooi(pins: int) =
            throws <-  throws @ [pins]

        member this.CurrentScore  = Seq.sum throws

        member this.ScoreVoorFrame frame = scoreVoorFrame frame
            
//            List.nth throws ((frame-1)/2)  + List.nth throws ((frame-1)/2+1) 