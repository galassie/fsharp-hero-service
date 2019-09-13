namespace HeroService.Core

open CustomBasicValueTypes

module DomainTypes =

    type Stat = Stat of Positive100

    type PersonInfo = { Name: String50 option; Surname: String50 option; Age: Positive option }
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