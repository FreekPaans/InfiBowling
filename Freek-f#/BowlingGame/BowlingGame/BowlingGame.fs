namespace BowlingGame
open System.Collections.Generic;

type Framenumber = int
type FirstThrowPins = int
type RemainingThrows = int list

type private CurrentFrameState =
    | GameComplete
    | InFrame of RemainingThrows * Framenumber * FirstThrowPins

type FramePoints = int

type Frame = FramePoints

type Frames = Frame list

type GameState = Frames * CurrentFrameState

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

    let frameState gameState = 
        let frames,frameState = gameState

        match frameState with
        | GameComplete -> frames,[],10
        | InFrame(throws,frameNumber,_) -> frames,throws,frameNumber

    let advanceState gameState  =
        let frames,throws,frameNumber = frameState gameState

        let updateGameState pointsForFrame nextState =
            pointsForFrame::frames,nextState

        let (|Spare|_|) throws = 
            let calculateSparePoints remainingThrows =
                10 + sumFirst 1 remainingThrows

            let isFrameComplete = 
                (Seq.length throws) >= 3

            match throws with 
            | firstThrow::secondThrow::remaining 
                when firstThrow + secondThrow = 10 -> 
                Some ((calculateSparePoints remaining),remaining, isFrameComplete)                       
            | _ -> None

        let (|Strike|_|) throws = 
            let calculateStrikePoints remainingThrows =
                10 + sumFirst 2 remainingThrows

            let isFrameComplete = 
                    (Seq.length throws) >= 3

            match throws with 
            | 10::remaining -> Some ((calculateStrikePoints remaining),remaining,isFrameComplete)
            | _ -> None

        let matchNormalThrow throws nextFrame =
            match throws with
            | first::second::remaining -> updateGameState (first + second) (nextFrame remaining)
            | first::[] -> updateGameState first (InFrame ([],frameNumber,first))
            | [] -> frames, (InFrame([],frameNumber,0))

        let (|NormalPoints|) throws =
            match throws with
            | first::second::remaining -> (first + second),remaining,true
            | first::[] -> first,[],false
            | [] -> 0,[],false

        let advanceStateFinalFrame gameState =
            match throws with
                | Strike(framePoints,remaining,true) -> updateGameState framePoints GameComplete
                | Strike(framePoints,remaining,false) -> updateGameState framePoints (InFrame(remaining,frameNumber,0))
                | Spare (framePoints,remaining,true) -> updateGameState framePoints GameComplete
                | Spare (framePoints,remaining,false) -> updateGameState framePoints (InFrame(remaining,frameNumber,0))
                | NormalPoints (points,remaining,true) -> updateGameState points GameComplete
                | NormalPoints (points,remaining,false) -> updateGameState points (InFrame(remaining,frameNumber,points))

        let advanceStateNormalFrame gameState =
            let completeTheFrame remainingThrows = 
                InFrame(remainingThrows,frameNumber+1,0)

            match throws with
                | Strike (points,remaining,_) -> updateGameState points (completeTheFrame remaining)
                | Spare (points,remaining,_) -> updateGameState points (completeTheFrame remaining)
                | NormalPoints (points,remaining,true)-> updateGameState points (completeTheFrame remaining)
                | NormalPoints (points,remaining,false)-> updateGameState points (InFrame (remaining,frameNumber,points))

        let isFinalFrame = frameNumber = 10

        if isFinalFrame then
            advanceStateFinalFrame gameState
        else
            advanceStateNormalFrame gameState

    let newGame throws = [],InFrame(throws,1,0)

    let getGameState throws = 
        let rec iter gameState =
            let nextState = advanceState gameState
            match nextState with
            |_,GameComplete -> nextState
            |_,InFrame (remaining,frame,points) -> if remaining = [] then nextState else iter nextState
        iter (newGame throws)

    let scoreForFrame forFrame throws =
        let frames,_=  getGameState throws
        sumFirst forFrame (frames |> List.rev)

    let currentScore throws = scoreForFrame 10 throws

    let appendThrownPins throws pins =
        throws @ [pins]

    let throw throws pins : int list = 
        if pins < 0 || pins > 10 then invalidArg "pins" "invalid number of pins"

        let gameState = getGameState throws

        match gameState with
        | _,GameComplete -> invalidArg "pins" "game is complete"
        | _,InFrame (remaining,frameNumber,previousPins) ->
            if previousPins + pins > 10 then invalidArg "pins" "invalid number of pins"
            else
                appendThrownPins throws pins
    
    let mutable throws = []

    member this.Gooi(pins: int) =
        throws <- throw throws pins

    member this.CurrentScore  = currentScore throws 

    member this.ScoreVoorFrame frame = scoreForFrame frame throws
    
    
