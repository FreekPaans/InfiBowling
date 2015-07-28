namespace BowlingGame
open System.Collections.Generic;

type GameState =
    | GameComplete
    | NewFrame of int list * int 
    | InFrame of int list * int * int

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

   

    let calculatePointsForFrameAndAdvanceGameState throws frameNumber : int * GameState  =
        let (|Spare|_|) throws = 
            let calculatePoints remainingThrows =
                10 + sumFirst 1 remainingThrows

            match throws with 
                | firstThrow::secondThrow::remaining when firstThrow + secondThrow = 10 -> 
                    Some ((calculatePoints remaining), NewFrame (remaining, frameNumber+1))
                | _ -> None

        let (|Strike|_|) throws = 
            let calculatePoints remainingThrows =
                10 + sumFirst 2 remainingThrows
            match throws with 
                | 10::remaining -> Some ((calculatePoints remaining), NewFrame (remaining,frameNumber + 1))
                | _ -> None

        let (|NormalPoints|) throws = 
            match throws with
            | first::second::remaining -> first + second, NewFrame (remaining,frameNumber + 1)
            | first::[] -> first, InFrame ([],frameNumber,first)
            | [] -> 0, NewFrame ([], frameNumber)


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
            let points,_ = result
            points, GameComplete

    let iterFrames tillFrame throws =
        let rec iter frameIterator throws =
            if frameIterator > tillFrame then 0
            else
                let points, state = calculatePointsForFrame throws frameIterator
                let remainingThrows = match state with
                    |GameComplete -> []
                    |NewFrame (r,frame)-> r
                    |InFrame (r,frame,point) -> r
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
            let rec iter throws framenumber =
                let points,state = calculatePointsForFrameAndAdvanceGameState throws framenumber
                match state with
                |GameComplete -> GameComplete
                |InFrame (remaining,frame,points) -> if remaining = [] then state else iter remaining frame
                |NewFrame (remaining,frame) -> if remaining = [] then state else iter remaining frame
            iter throws 1 
        

    let gooi throws pins : int list = 
        if pins < 0 || pins > 10 then invalidArg "pins" "invalid number of pins"

        let gameState = getGameState throws

        match gameState with
            | GameComplete -> invalidArg "pins" "game is complete"
            | NewFrame (remaining,frameNumber) -> appendThrownPins throws pins
            | InFrame (remaining,frameNumber,previousPins) ->
                if previousPins + pins > 10 then invalidArg "pins" "invalid number of pins"
                else
                    appendThrownPins throws pins
                    
            
    
    let mutable throws = []

    member this.Gooi(pins: int) =
        throws <- gooi throws pins

    member this.CurrentScore  = currentScore throws 

    member this.ScoreVoorFrame frame = scoreForFrame frame throws
    
    
