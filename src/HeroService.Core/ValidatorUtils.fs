namespace HeroService.Core

module ValidatorUtils = 
    let validate errorMsg opt =
        match opt with
        | Some value -> Ok value
        | None -> Error errorMsg

    type ValidationBuilder() = 
        member this.Bind(m, f) =
            match m with
            | Ok value -> f value
            | Error errorMsg -> Error errorMsg

        member this.Return(x) = Ok x
    
    let validation = new ValidationBuilder()