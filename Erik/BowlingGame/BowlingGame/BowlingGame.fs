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

    
    module BowlingGame =
        let calculatePointsAndAdvanceToNextFrame throws =
            let sumFirstSplitRemaining n items =
                let toTake = (min n (Seq.length items))
                
                let sum = 
                    items
                    |> Seq.take toTake
                    |> Seq.fold (+) 0

                sum,Seq.skip toTake items |> Seq.toList

            let sumFirst n items =
                let sum,items = sumFirstSplitRemaining n items
                sum
                    
            let (|Spare|_|) throws = 
                let calculatePoints remainingThrows =
                    10 + sumFirst 1 remainingThrows

                match throws with 
                    | firstThrow::secondThrow::remaining when firstThrow + secondThrow = 10 -> 
                        Some (remaining, (calculatePoints remaining))
                    | _ -> None

            let (|Strike|_|) throws = 
                let calculatePoints remainingThrows =
                    10 + sumFirst 2 remainingThrows
                match throws with 
                    | 10::remaining -> Some (remaining, (calculatePoints remaining))
                    | _ -> None

            let (|NormalPoints|) throws = 
                let sum, remainingThrows = sumFirstSplitRemaining 2 throws
                remainingThrows,sum


            match throws with
                | Strike strike -> strike
                | Spare spare -> spare
                | NormalPoints normal -> normal

        let calculatePointsForFrame throws frameNumber =
            let isFinalFrame = frameNumber = 10

            let result = calculatePointsAndAdvanceToNextFrame throws

            if not isFinalFrame then
                result
            else
                let remaining,points = result
                [], points

        let iterFrames tillFrame throws =
            let rec iter frameIterator throws =
                if frameIterator > tillFrame then 0
                else
                    let remainingThrows, points = calculatePointsForFrame throws frameIterator
                    points + (iter (frameIterator + 1) remainingThrows)
            iter 1 throws

        let scoreForFrame forFrame throws =
            iterFrames forFrame throws

        let currentScore throws = scoreForFrame 20 throws

        let appendThrownPins throws pins =
            throws @ [pins]

        let isGameComplete throws = 
            let score1 = currentScore throws
            let score2 = currentScore (appendThrownPins throws 5)
            score1=score2

        let gooi throws pins : int list = 
            if isGameComplete throws then invalidArg "pins" "game is complete"
            else
                appendThrownPins throws pins
            

        type Game2() =
            let mutable throws = []

            member this.Gooi(pins: int) =
                throws <- gooi throws pins

            member this.CurrentScore  = currentScore throws // we hebben nog geen logica om het laatste frame te berekenen

            member this.ScoreVoorFrame frame = scoreForFrame frame throws
            
    //            List.nth throws ((frame-1)/2)  + List.nth throws ((frame-1)/2+1) 