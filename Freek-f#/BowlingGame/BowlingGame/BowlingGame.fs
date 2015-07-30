namespace BowlingGame
open System.Collections.Generic;

type Framenumber = int
type FirstThrowPins = int
type RemainingThrows = int list

type private CurrentFrameState =
    | GameComplete
    | InFrame of RemainingThrows * Framenumber * FirstThrowPins
    | FinalFrameExtraBalls of RemainingThrows

type FramePoints = int

type Frame = FramePoints

type Frames = Frame list

type GameState = Frames * CurrentFrameState

type private FramePointsStatus =
    | PointsComplete
    | PointsIncomplete

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
        | FinalFrameExtraBalls(throws)->frames,throws,10

    let advanceState gameState  =
        let frames,throws,frameNumber = frameState gameState

        let updateGameState pointsForFrame nextState =
            pointsForFrame::frames,nextState

        let (|Spare|_|) throws = 
            let calculateSparePoints remainingThrows =
                10 + sumFirst 1 remainingThrows

            let isFrameComplete = 
                if (Seq.length throws) >= 3 then PointsComplete else PointsIncomplete

            match throws with 
            | firstThrow::secondThrow::remaining 
                when firstThrow + secondThrow = 10 -> 
                Some ((calculateSparePoints remaining),remaining, isFrameComplete)                       
            | _ -> None

        let (|Strike|_|) throws = 
            let calculateStrikePoints remainingThrows =
                10 + sumFirst 2 remainingThrows

            let isFrameComplete = 
                if (Seq.length throws) >= 3 then PointsComplete else PointsIncomplete

            match throws with 
            | 10::remaining -> Some ((calculateStrikePoints remaining),remaining,isFrameComplete)
            | _ -> None

        let (|NormalPoints|) throws =
            match throws with
            | first::second::remaining -> (first + second),remaining,PointsComplete
            | first::[] -> first,[],PointsIncomplete
            | [] -> 0,[],PointsIncomplete

        let advanceStateFinalFrame gameState =
            match throws with
                | Strike(framePoints,remaining,PointsComplete) -> updateGameState framePoints GameComplete
                | Strike(framePoints,remaining,PointsIncomplete) -> updateGameState framePoints (FinalFrameExtraBalls remaining)
                | Spare (framePoints,remaining,PointsComplete) -> updateGameState framePoints GameComplete
                | Spare (framePoints,remaining,PointsIncomplete) -> updateGameState framePoints (FinalFrameExtraBalls remaining)
                | NormalPoints (points,remaining,PointsComplete) -> updateGameState points GameComplete
                | NormalPoints (points,remaining,PointsIncomplete) -> updateGameState points (InFrame(remaining,frameNumber,points))

        let advanceStateNormalFrame gameState =
            let completeTheFrame remainingThrows = 
                InFrame(remainingThrows,frameNumber+1,0)

            match throws with
                | Strike (points,remaining,_) -> updateGameState points (completeTheFrame remaining)
                | Spare (points,remaining,_) -> updateGameState points (completeTheFrame remaining)
                | NormalPoints (points,remaining,PointsComplete)-> updateGameState points (completeTheFrame remaining)
                | NormalPoints (points,remaining,PointsIncomplete)-> updateGameState points (InFrame (remaining,frameNumber,points))

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
            |_,InFrame ([],frame,points) -> nextState
            |_,InFrame (remaining,frame,points) -> iter nextState
            |_,FinalFrameExtraBalls _  -> iter nextState
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
        | _,FinalFrameExtraBalls _  -> appendThrownPins throws pins
        | _,InFrame (remaining,frameNumber,previousPins) ->
            if previousPins + pins > 10 then invalidArg "pins" "invalid number of pins"
            else
                appendThrownPins throws pins
    
    let mutable throws = []

    member this.Gooi(pins: int) =
        throws <- throw throws pins

    member this.CurrentScore  = currentScore throws 

    member this.ScoreVoorFrame frame = scoreForFrame frame throws
    
    
