namespace HeroService.Core

open CustomBasicValueTypes
open ValidatorUtils

module DomainTypes =
    
    type PersonInfo = { Name: String50; Surname: String50; Age: Positive }
    module PersonInfo =
        let create inputName inputSurname inputAge = 
            validation {
                let! name = String50.create inputName |> validate "Error parsing Name"
                let! surname = String50.create inputSurname |> validate "Error parsing Surname"
                let! age = Positive.create inputAge |> validate "Error parsing Age"
                return { Name = name; Surname = surname; Age = age }
            }
    
    type Stat = Stat of Positive100
    module Stat =
        let value (Stat x) = Positive100.value x

    type PersonStats = { Strength: Stat; Dexterity: Stat; Constitution: Stat; Intelligence: Stat; Wisdom: Stat; Charisma: Stat }
    module PersonStats =
        let create inputStr inputDex inputCons inputInt inputWis inputCha =
            let intoStat opt = match opt with | Some x -> Some (Stat x) | None -> None
            validation {
                let! str = Positive100.create inputStr |> intoStat |> validate "Error parsing Strength"
                let! dex = Positive100.create inputDex |> intoStat |> validate "Error parsing Dexterity"
                let! cons = Positive100.create inputCons |> intoStat |> validate "Error parsing Constitution"
                let! int = Positive100.create inputInt |> intoStat |> validate "Error parsing Intelligence"
                let! wis = Positive100.create inputWis |> intoStat |> validate "Error parsing Wisdom"
                let! cha = Positive100.create inputCha |> intoStat |> validate "Error parsing Charisma"
                return { Strength = str; Dexterity = dex; Constitution = cons; Intelligence = int; Wisdom = wis; Charisma = cha }
            }

    type HumanInfo = { PersonInfo: PersonInfo; PersonStats: PersonStats }
    module HumanInfo =
        let create tryCreatePersonInfo tryCreatePersonStats =
            let errorMessageEnricher prefixErrorMsg input =
                match input with
                | Ok x -> Ok x
                | Error errorMsg -> Error (sprintf "%s: %s" prefixErrorMsg errorMsg)
            validation {
                let! personInfo = tryCreatePersonInfo() |> errorMessageEnricher "Failed to create PersonInfo"
                let! personStats = tryCreatePersonStats() |> errorMessageEnricher "Failed to create PersonStats"
                return { PersonInfo = personInfo; PersonStats = personStats }
            }

    type SuperPower = { Name: String50; Description: String512 }

    type Hero =
        | Human of HeroName: String50 * HumanInfo: HumanInfo
        | SuperHuman of HeroName: String50 * HumanInfo: HumanInfo * SuperPowers: SuperPower list

    type Villain =
        | Human of HeroName: String50 * HumanInfo: HumanInfo
        | SuperHuman of HeroName: String50 * HumanInfo: HumanInfo * SuperPowers: SuperPower list