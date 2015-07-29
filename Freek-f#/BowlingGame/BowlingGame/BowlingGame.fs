namespace BowlingGame
open System.Collections.Generic;

type Framenumber = int
type FirstThrowPins = int
type RemainingThrows = int list

type private CurrentFrameState =
    | GameComplete
    | NewFrame of RemainingThrows * Framenumber 
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
        | NewFrame(throws,frameNumber) -> frames,throws,frameNumber
        | InFrame(throws,frameNumber,_) -> frames,throws,frameNumber

    let advanceState gameState  =
        let frames,throws,frameNumber = frameState gameState

        let isFinalFrame = frameNumber = 10

        let completeTheFrame remainingThrows = 
            if isFinalFrame then 
                GameComplete 
            else 
                NewFrame(remainingThrows,frameNumber+1)

        let advanceFrameState isFrameComplete remainingThrows = 
            if isFrameComplete then
                completeTheFrame remainingThrows
            else
                InFrame(remainingThrows,frameNumber,0)       

        let updateGameState pointsForFrame nextState =
            pointsForFrame::frames,nextState

        let (|Spare|_|) throws = 
            let calculatePoints remainingThrows =
                10 + sumFirst 1 remainingThrows

            let isFrameComplete remainingThrows = (not isFinalFrame) || (Seq.length remainingThrows) = 1

            let nextFrameState remainingThrows =
                advanceFrameState (isFrameComplete remainingThrows) remainingThrows

            match throws with 
                | firstThrow::secondThrow::remaining when firstThrow + secondThrow = 10 -> 
                    Some (updateGameState (calculatePoints remaining) (nextFrameState remaining))                       
                | _ -> None

        let (|Strike|_|) throws = 
            let calculatePoints remainingThrows =
                10 + sumFirst 2 remainingThrows

            let isFrameComplete remainingThrows = (not isFinalFrame || (Seq.length remainingThrows) = 2)

            let nextFrameState remainingThrows =
                advanceFrameState (isFrameComplete remainingThrows) remainingThrows

            match throws with 
                | 10::remaining -> Some (updateGameState (calculatePoints remaining) (nextFrameState remaining))
                | _ -> None

        let (|NormalPoints|) throws = 
            match throws with
            | first::second::remaining -> updateGameState (first + second) (completeTheFrame remaining)
            | first::[] -> updateGameState first (InFrame ([],frameNumber,first))
            | [] -> frames, (NewFrame([],frameNumber))


        match throws with
            | Strike strike -> strike
            | Spare spare -> spare
            | NormalPoints normal -> normal

    let newGame throws = [],NewFrame(throws,1)

    let getGameState throws = 
        let rec iter gameState =
            let nextState = advanceState gameState
            match nextState with
            |_,GameComplete -> nextState
            |_,InFrame (remaining,frame,points) -> if remaining = [] then nextState else iter nextState
            |_,NewFrame (remaining,frame) -> if remaining = [] then nextState else iter nextState
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
            | _,NewFrame (remaining,frameNumber) -> appendThrownPins throws pins
            | _,InFrame (remaining,frameNumber,previousPins) ->
                if previousPins + pins > 10 then invalidArg "pins" "invalid number of pins"
                else
                    appendThrownPins throws pins
    
    let mutable throws = []

    member this.Gooi(pins: int) =
        throws <- throw throws pins

    member this.CurrentScore  = currentScore throws 

    member this.ScoreVoorFrame frame = scoreForFrame frame throws
    
    
