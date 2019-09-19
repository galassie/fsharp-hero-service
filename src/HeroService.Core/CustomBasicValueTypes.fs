namespace HeroService.Core

open System;

module CustomBasicValueTypes =

    type Positive = private Positive of int
    module Positive =
        let create value =
            match value with
            | x when x >= 0 -> Some (Positive x)
            | _ -> None
        
        let value (Positive x) = x

    type Positive100 = private Positive100 of int
    module Positive100 =
        let create value =
            match value with
            | x when x >= 0 && x <= 100 -> Some (Positive100 x)
            | _ -> None
        
        let value (Positive100 x) = x

    type String50 = private String50 of string
    module String50 =
        let create value = 
            match value with
            | x when not (String.IsNullOrWhiteSpace(x)) && x.Length <= 50 -> Some (String50 x)
            |_ -> None
        
        let value (String50 x) = x

    type String512 = private String512 of string
    module String512 =
        let create value = 
            match value with
            | x when not (String.IsNullOrWhiteSpace(x)) && x.Length <= 512 -> Some (String512 x)
            |_ -> None
        
        let value (String512 x) = x