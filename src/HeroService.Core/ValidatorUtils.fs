namespace HeroService.Core

module ValidatorUtils = 
    let validate errorMsg opt =
        match opt with
        | Some value -> Ok value
        | None -> Error errorMsg

    let errorMessageEnricher prefixErrorMsg input =
        match input with
        | Ok x -> Ok x
        | Error errorMsg -> Error (sprintf "%s: %s" prefixErrorMsg errorMsg)
    
    type ValidationBuilder() = 
        member this.Bind(mx, f) =
            match mx with
            | Ok x -> f x
            | Error errorMsg -> Error errorMsg

        member this.Return(x) = Ok x

    type MaybeSeqBuilder() =
        member this.Yield(x) =
            match x with
            | Ok i -> Ok [i]
            | Error _ -> Ok []

        member this.Combine(mx, my) = 
            match mx, my with
            | Ok x, Ok y -> Ok (x@y)
            | Ok x, Error _ -> Ok x
            | Error _, Ok y -> Ok y
            | Error _, Error _ -> Ok []

        member this.Delay(f) =
            f()
    
    let validation = new ValidationBuilder()
    let maybeSeq = new MaybeSeqBuilder()