namespace BowlingGame
open System.Collections.Generic;

type Framenumber = int
type FirstThrowPins = int
type RemainingThrows = int list

type private ActiveFrame =
    | InFrame of RemainingThrows * Framenumber * FirstThrowPins
    | FinalFrameExtraBalls of RemainingThrows

type private CurrentFrameState =
    | GameComplete
    | GameActive of ActiveFrame

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


    let advanceState (gameState:ActiveFrame) : FramePoints*CurrentFrameState =

        let throws,frameNumber =
            match gameState with
            | InFrame (throws, frameNumber, _)-> throws,frameNumber
            | FinalFrameExtraBalls throws -> throws,10

        let updateGameState pointsForFrame nextState =
            pointsForFrame,nextState

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
                | Strike(framePoints,remaining,PointsIncomplete) -> updateGameState framePoints (GameActive (FinalFrameExtraBalls remaining))
                | Spare (framePoints,remaining,PointsComplete) -> updateGameState framePoints GameComplete
                | Spare (framePoints,remaining,PointsIncomplete) -> updateGameState framePoints (GameActive (FinalFrameExtraBalls remaining))
                | NormalPoints (points,remaining,PointsComplete) -> updateGameState points GameComplete
                | NormalPoints (points,remaining,PointsIncomplete) -> updateGameState points (GameActive (InFrame(remaining,frameNumber,points)))

        let advanceStateNormalFrame gameState =
            let completeTheFrame remainingThrows = 
                (GameActive (InFrame(remainingThrows,frameNumber+1,0)))

            match throws with
                | Strike (points,remaining,_) -> updateGameState points (completeTheFrame remaining)
                | Spare (points,remaining,_) -> updateGameState points (completeTheFrame remaining)
                | NormalPoints (points,remaining,PointsComplete)-> updateGameState points (completeTheFrame remaining)
                | NormalPoints (points,remaining,PointsIncomplete)-> updateGameState points (GameActive (InFrame (remaining,frameNumber,points)))

        let isFinalFrame = frameNumber = 10

        if isFinalFrame then
            advanceStateFinalFrame gameState
        else
            advanceStateNormalFrame gameState

    let newGame throws = [],(GameActive (InFrame(throws,1,0)))

    let getGameState throws = 
        let rec iter gameState =
            let frames,currentFrameState = gameState

            match currentFrameState with
            | GameComplete -> gameState
            | GameActive(InFrame ([],_,_)) ->gameState
            | GameActive activeFrameState -> 
                let updateGameState points nextFrameState =
                    (points::frames),nextFrameState

                let points,nextFrameState = advanceState activeFrameState

                let nextState = updateGameState points nextFrameState

                match nextFrameState with
                |GameComplete
                |GameActive(_) -> iter nextState

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
        | _,GameActive(InFrame (remaining,frameNumber,previousPins)) 
            when previousPins + pins > 10 -> invalidArg "pins" "invalid number of pins"
        | _,GameActive(_) -> appendThrownPins throws pins
            
    
    let mutable throws = []

    member this.Gooi(pins: int) =
        throws <- throw throws pins

    member this.CurrentScore  = currentScore throws 

    member this.ScoreVoorFrame frame = scoreForFrame frame throws
    
    
