namespace BowlingGame
open System.Collections.Generic;

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

    let calculatePointsAndAdvanceToNextFrame throws =
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

    let currentScore throws = scoreForFrame 20 throws // 20 -> we hebben nog geen logica om het laatste frame te berekenen

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
    
    let mutable throws = []

    member this.Gooi(pins: int) =
        throws <- gooi throws pins

    member this.CurrentScore  = currentScore throws 

    member this.ScoreVoorFrame frame = scoreForFrame frame throws
    
    
