namespace BowlingGame
open System.Collections.Generic;

type GameState =
    | GameComplete
    | NewFrame of int 
    | InFrame of int * int

type Game() = 
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

   

    let calculatePointsForFrameAndAdvanceGameState throws frameNumber : int list * int * GameState  =
        let (|Spare|_|) throws = 
            let calculatePoints remainingThrows =
                10 + sumFirst 1 remainingThrows

            match throws with 
                | firstThrow::secondThrow::remaining when firstThrow + secondThrow = 10 -> 
                    Some (remaining, (calculatePoints remaining), NewFrame (frameNumber+1))
                | _ -> None

        let (|Strike|_|) throws = 
            let calculatePoints remainingThrows =
                10 + sumFirst 2 remainingThrows
            match throws with 
                | 10::remaining -> Some (remaining, (calculatePoints remaining), NewFrame (frameNumber + 1))
                | _ -> None

        let (|NormalPoints|) throws = 
            match throws with
            | first::second::remaining -> remaining, first + second, NewFrame (frameNumber + 1)
            | first::[] -> [], first, InFrame (frameNumber,first)
            | [] -> [], 0, NewFrame frameNumber


        match throws with
            | Strike strike -> strike
            | Spare spare -> spare
            | NormalPoints normal -> normal

    let calculatePointsForFrame throws frameNumber =
        let isFinalFrame = frameNumber = 10

        let result = calculatePointsForFrameAndAdvanceGameState throws frameNumber

        if not isFinalFrame then
            result
        else
            let remaining,points,_ = result
            [], points, GameComplete

    let iterFrames tillFrame throws =
        let rec iter frameIterator throws =
            if frameIterator > tillFrame then 0
            else
                let remainingThrows, points,_ = calculatePointsForFrame throws frameIterator
                points + (iter (frameIterator + 1) remainingThrows)
        iter 1 throws

    let scoreForFrame forFrame throws =
        iterFrames forFrame throws

    let currentScore throws = scoreForFrame 10 throws

    let appendThrownPins throws pins =
        throws @ [pins]

    let isGameComplete throws = 
        let score1 = currentScore throws
        let score2 = currentScore (appendThrownPins throws 5)
        score1=score2

    let getGameState throws = 
        if isGameComplete throws then GameComplete
        else
            NewFrame 1
        

    let gooi throws pins : int list = 
        let gameState = getGameState throws

        match gameState with
            | GameComplete -> invalidArg "pins" "game is complete"
            | NewFrame frameNumber -> appendThrownPins throws pins
            | InFrame (frameNumber,previousPins) ->
                if previousPins + pins > 10 then invalidArg "pins" "invalid number of pins"
                else
                    appendThrownPins throws pins
                    
            
    
    let mutable throws = []

    member this.Gooi(pins: int) =
        throws <- gooi throws pins

    member this.CurrentScore  = currentScore throws 

    member this.ScoreVoorFrame frame = scoreForFrame frame throws
    
    
