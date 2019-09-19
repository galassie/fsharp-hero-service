namespace HeroService.Core

open CustomBasicValueTypes
open ValidatorUtils

module DomainTypes =
    type Stat = Stat of Positive100

    type PersonInfo = { Name: String50; Surname: String50; Age: Positive }
    module PersonInfo =
        let create inputName inputSurname inputAge = 
            validation {
                let! name = String50.create inputName |> validate "Error parsing name"
                let! surname = String50.create inputSurname |> validate "Error parsing surname"
                let! age = Positive.create inputAge |> validate "Error parsing age"
                return { Name = name; Surname = surname; Age = age }
            }

    type PersonStats = {
        Strength: Stat;
        Dexterity: Stat;
        Constitution: Stat;
        Intelligence: Stat;
        Wisdom: Stat;
        Charisma: Stat
    }
    type HumanInfo = { PersonInfo: PersonInfo; PersonStats: PersonStats }

    type SuperPower = { Name: String50; Description: String512 }

    type Hero =
        | Human of HeroName: String50 * HumanInfo: HumanInfo
        | SuperHuman of HeroName: String50 * HumanInfo: HumanInfo * SuperPowers: SuperPower list

    type Villain =
        | Human of HeroName: String50 * HumanInfo: HumanInfo
        | SuperHuman of HeroName: String50 * HumanInfo: HumanInfo * SuperPowers: SuperPower list