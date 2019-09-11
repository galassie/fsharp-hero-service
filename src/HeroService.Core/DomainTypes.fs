namespace HeroService.Core

module DomainTypes =

    type Stat = private Stat of int
    module Stat =
        let create input = 
            match input with
            | x when x >= 0 && x <= 100 -> Some (Stat x)
            | _ -> None

        let value (Stat output) = output

    type PersonInfo = { Name: string option; Surname: string option; Age: int option }
    type PersonStats = {
        Strength: Stat;
        Dexterity: Stat;
        Constitution: Stat;
        Intelligence: Stat;
        Wisdom: Stat;
        Charisma: Stat
    }
    module PersonStats =
        let create str dex cons int wis cha =
            let evalOpt statOpt = match statOpt with | Some stat -> stat | None -> Stat 0
            {
                Strength = Stat.create str |> evalOpt;
                Dexterity = Stat.create dex |> evalOpt;
                Constitution = Stat.create cons |> evalOpt;
                Intelligence = Stat.create int |> evalOpt;
                Wisdom = Stat.create wis |> evalOpt;
                Charisma = Stat.create cha |> evalOpt;
            }

    type HumanInfo = { PersonInfo: PersonInfo; PersonStats: PersonStats }

    type SuperPower = { Name: string; Description: string }    

    type Hero =
        private
        | Human of HeroName: string * HumanInfo: HumanInfo
        | SuperHuman of HeroName: string * HumanInfo: HumanInfo * SuperPowers: SuperPower list
    module Hero =
        let create heroName personInfo personStats superPowers =
            let humanInfo = { PersonInfo = personInfo; PersonStats = personStats }
            match superPowers with
            | [] -> Human (heroName, humanInfo)
            | _ -> SuperHuman (heroName, humanInfo, superPowers)

    type Villain =
        private
        | Human of HeroName: string * HumanInfo: HumanInfo
        | SuperHuman of HeroName: string * HumanInfo: HumanInfo * SuperPowers: SuperPower list
    module Villain =
        let create villainName personInfo personStats superPowers =
            let humanInfo = { PersonInfo = personInfo; PersonStats = personStats }
            match superPowers with
            | [] -> Human (villainName, humanInfo)
            | _ -> SuperHuman (villainName, humanInfo, superPowers)
    
