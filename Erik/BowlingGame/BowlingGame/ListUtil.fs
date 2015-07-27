namespace ErikTillema.FSharp

module ListUtil = 
    
    /// skips the first n items of source
    let rec skip n source = 
        match n, source with
        | 0, _ -> source
        | _, [] -> invalidOp "Number of items skipped exceeds number of items"
        | n, h::t -> skip (n-1) t

    /// returns the first n items of source.
    /// Throws an InvalidOperationException if there are no n items.
    let take n source = 
        let rec take' accumulated = function
            | 0, _ -> List.rev accumulated
            | _, [] -> invalidOp "Number of items taken exceeds number of items"
            | n, h::t -> take' (h::accumulated) (n-1, t)
        take' [] (n, source)
