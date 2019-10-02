namespace HeroService.Core.Tests

open NUnit.Framework
open HeroService.Core.ValidatorUtils
open HeroService.Core.CustomBasicValueTypes
open HeroService.Core.DomainTypes

type DomainTypesCreatorTests() =

    [<TestCase("", "Evans", 12, "Error parsing Name")>]
    [<TestCase("Chris", "", 20, "Error parsing Surname")>]
    [<TestCase("Chris", "Evans", -10, "Error parsing Age")>]
    member this.``PersonInfo.createif fails should return proper error message`` (name, surname, age, expectedErrorMsg) =
        PersonInfo.create name surname age
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual(expectedErrorMsg, errorMsg)
    
    [<Test>]
    member this.``PersonInfo.create happy-path`` () =
        PersonInfo.create "Chris" "Evans" 30
        |> function
            | Ok personInfo ->
                Assert.True((String50.value personInfo.Name |> (=) "Chris"))
                Assert.True((String50.value personInfo.Surname |> (=) "Evans"))
                Assert.True((Positive.value personInfo.Age |> (=) 30))
            | Error _ -> Assert.True(false)
    
    [<TestCase(-1, 10, 10, 10, 10, 10, "Error parsing Strength")>]
    [<TestCase(20, 101, 10, 10, 10, 10, "Error parsing Dexterity")>]
    [<TestCase(50, 0, -10, 10, 10, 10, "Error parsing Constitution")>]
    [<TestCase(20, 10, 10, 200, 10, 10, "Error parsing Intelligence")>]
    [<TestCase(5, 1, 99, 100, 300, 10, "Error parsing Wisdom")>]
    [<TestCase(3, 10, 10, 10, 10, -100, "Error parsing Charisma")>]
    member this.``PersonStats.create if fails should return proper error message`` (str, dex, cons, int, wis, cha, expectedErrorMsg) =
        PersonStats.create str dex cons int wis cha
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual(expectedErrorMsg, errorMsg)
    
    [<Test>]
    member this.``PersonStats.create happy-path`` () =
        PersonStats.create 0 10 20 50 99 100
        |> function
            | Ok personStats ->
                Assert.True((Stat.value personStats.Strength |> (=) 0))
                Assert.True((Stat.value personStats.Dexterity |> (=) 10))
                Assert.True((Stat.value personStats.Constitution |> (=) 20))
                Assert.True((Stat.value personStats.Intelligence |> (=) 50))
                Assert.True((Stat.value personStats.Wisdom |> (=) 99))
                Assert.True((Stat.value personStats.Charisma |> (=) 100))
            | Error _ -> Assert.True(false)

    [<Test>]
    member this.``HumanInfo.create if fails creating PersonInfo should return proper error message`` () =
        let koCreatePersonInfo = fun () -> PersonInfo.create "Chris" "Evans" -10
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        HumanInfo.create koCreatePersonInfo okCreatePersonStats
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual("Failed to create PersonInfo: Error parsing Age", errorMsg)
            
    [<Test>]
    member this.``HumanInfo.create if fails creating PersonStats should return proper error message`` () =
        let koCreatePersonInfo = fun () -> PersonInfo.create "Chris" "Evans" 20
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 -20 50 99 100
        HumanInfo.create koCreatePersonInfo okCreatePersonStats
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual("Failed to create PersonStats: Error parsing Constitution", errorMsg)
            
    [<Test>]
    member this.``HumanInfo.create happy-path`` () =
        let okCreatePersonInfo = fun () -> PersonInfo.create "Chris" "Evans" 35
        let okCreatePersonStats = fun () -> PersonStats.create 10 10 20 50 99 100
        HumanInfo.create okCreatePersonInfo okCreatePersonStats
        |> function
            | Ok { PersonInfo = personInfo; PersonStats = personStats } ->
                Assert.True((String50.value personInfo.Name |> (=) "Chris"))
                Assert.True((String50.value personInfo.Surname |> (=) "Evans"))
                Assert.True((Positive.value personInfo.Age |> (=) 35))
                Assert.True((Stat.value personStats.Strength |> (=) 10))
                Assert.True((Stat.value personStats.Dexterity |> (=) 10))
                Assert.True((Stat.value personStats.Constitution |> (=) 20))
                Assert.True((Stat.value personStats.Intelligence |> (=) 50))
                Assert.True((Stat.value personStats.Wisdom |> (=) 99))
                Assert.True((Stat.value personStats.Charisma |> (=) 100))
            | Error _ -> Assert.True(false)

    [<TestCase("", "This power gives super strength", "Error parsing Name")>]
    [<TestCase("SuperStrength", "", "Error parsing Description")>]
    member this.``SuperPower.create if fails should return proper error message`` (name, description, expectedErrorMsg) =
        SuperPower.create name description
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual(expectedErrorMsg, errorMsg)
    
    [<Test>]
    member this.``SuperPower.create happy-path`` () =
        SuperPower.create "SuperPower" "This power gives super strength"
        |> function
            | Ok personInfo ->
                Assert.True((String50.value personInfo.Name |> (=) "SuperPower"))
                Assert.True((String512.value personInfo.Description |> (=) "This power gives super strength"))
            | Error _ -> Assert.True(false)

    [<Test>]
    member this.``Hero.create if fails parsing HeroName should return proper error message`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let okCreatePersonInfo = fun () -> PersonInfo.create "Chris" "Evans" 35
        let okCreateHumanInfo = fun() -> HumanInfo.create okCreatePersonInfo okCreatePersonStats
        let okCreateSuperEmptyPowerList = fun () -> Ok []
        Hero.create "" okCreateHumanInfo okCreateSuperEmptyPowerList
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual("Error parsing HeroName", errorMsg)
            
    [<Test>]
    member this.``Hero.create if fails creating HumanInfo should return proper error message`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let koCreatePersonInfo = fun () -> PersonInfo.create "Chris" "Evans" -1
        let koCreateHumanInfo = fun() -> HumanInfo.create koCreatePersonInfo okCreatePersonStats
        let okCreateSuperEmptyPowerList = fun () -> Ok []
        Hero.create "Captain America" koCreateHumanInfo okCreateSuperEmptyPowerList
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual("Failed to create HumanInfo: Failed to create PersonInfo: Error parsing Age", errorMsg)
            
    [<Test>]
    member this.``Hero.create if fails creating SuperPowers should return proper error message`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let okCreatePersonInfo = fun () -> PersonInfo.create "Chris" "Evans" 35
        let okCreateHumanInfo = fun() -> HumanInfo.create okCreatePersonInfo okCreatePersonStats
        let koCreateSuperPowerList = fun () -> Error "Error generating SuperPower list"
        Hero.create "Captain America" okCreateHumanInfo koCreateSuperPowerList
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual("Failed to create SuperPower list: Error generating SuperPower list", errorMsg)

    [<Test>]
    member this.``Hero.create happy-path with empty SuperPower list should return Hero.Human`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let okCreatePersonInfo = fun () -> PersonInfo.create "Chris" "Evans" 35
        let okCreateHumanInfo = fun() -> HumanInfo.create okCreatePersonInfo okCreatePersonStats
        let okCreateSuperEmptyPowerList = fun () -> Ok []
        Hero.create "Captain America" okCreateHumanInfo okCreateSuperEmptyPowerList
        |> function
            | Ok hero -> 
                match hero with
                | Hero.Human (heroName, humanInfo) -> 
                    Assert.True((String50.value heroName |> (=) "Captain America"))
                    Assert.True((String50.value humanInfo.PersonInfo.Name |> (=) "Chris"))
                    Assert.True((String50.value humanInfo.PersonInfo.Surname |> (=) "Evans"))
                    Assert.True((Positive.value humanInfo.PersonInfo.Age |> (=) 35))
                    Assert.True((Stat.value humanInfo.PersonStats.Strength |> (=) 0))
                    Assert.True((Stat.value humanInfo.PersonStats.Dexterity |> (=) 10))
                    Assert.True((Stat.value humanInfo.PersonStats.Constitution |> (=) 20))
                    Assert.True((Stat.value humanInfo.PersonStats.Intelligence |> (=) 50))
                    Assert.True((Stat.value humanInfo.PersonStats.Wisdom |> (=) 99))
                    Assert.True((Stat.value humanInfo.PersonStats.Charisma |> (=) 100))
                | Hero.SuperHuman(_) -> Assert.True(false)
            | Error _ -> Assert.True(false)

    [<Test>]
    member this.``Hero.create happy-path with SuperPower list should return Human.SuperHero`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let okCreatePersonInfo = fun () -> PersonInfo.create "Chris" "Evans" 35
        let okCreateHumanInfo = fun() -> HumanInfo.create okCreatePersonInfo okCreatePersonStats
        let okCreateSuperPowerList = fun () -> maybeSeq {
            yield SuperPower.create "SuperPower" "This power gives super strength"
            yield SuperPower.create "SuperDexterity" "This power gives super dexterity"
        }
        Hero.create "Captain America" okCreateHumanInfo okCreateSuperPowerList
        |> function
            | Ok hero -> 
                match hero with
                | Hero.Human (_) -> Assert.True(false)
                | Hero.SuperHuman(heroName, humanInfo, superPowers) -> 
                    Assert.True((String50.value heroName |> (=) "Captain America"))
                    Assert.True((String50.value humanInfo.PersonInfo.Name |> (=) "Chris"))
                    Assert.True((String50.value humanInfo.PersonInfo.Surname |> (=) "Evans"))
                    Assert.True((Positive.value humanInfo.PersonInfo.Age |> (=) 35))
                    Assert.True((Stat.value humanInfo.PersonStats.Strength |> (=) 0))
                    Assert.True((Stat.value humanInfo.PersonStats.Dexterity |> (=) 10))
                    Assert.True((Stat.value humanInfo.PersonStats.Constitution |> (=) 20))
                    Assert.True((Stat.value humanInfo.PersonStats.Intelligence |> (=) 50))
                    Assert.True((Stat.value humanInfo.PersonStats.Wisdom |> (=) 99))
                    Assert.True((Stat.value humanInfo.PersonStats.Charisma |> (=) 100))
                    Assert.AreEqual(2, superPowers.Length)
                    Assert.True((String50.value (superPowers.Item 0).Name |> (=) "SuperPower"))
                    Assert.True((String512.value (superPowers.Item 0).Description |> (=) "This power gives super strength"))
                    Assert.True((String50.value (superPowers.Item 1).Name |> (=) "SuperDexterity"))
                    Assert.True((String512.value (superPowers.Item 1).Description |> (=) "This power gives super dexterity"))
            | Error _ -> Assert.True(false)

    [<Test>]
    member this.``Villain.create if fails parsing VillainName should return proper error message`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let okCreatePersonInfo = fun () -> PersonInfo.create "Otto" "Octavius" 50
        let okCreateHumanInfo = fun() -> HumanInfo.create okCreatePersonInfo okCreatePersonStats
        let okCreateSuperEmptyPowerList = fun () -> Ok []
        Villain.create "" okCreateHumanInfo okCreateSuperEmptyPowerList
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual("Error parsing VillainName", errorMsg)
            
    [<Test>]
    member this.``Villain.create if fails creating HumanInfo should return proper error message`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let koCreatePersonInfo = fun () -> PersonInfo.create "Otto" "Octavius" -1
        let koCreateHumanInfo = fun() -> HumanInfo.create koCreatePersonInfo okCreatePersonStats
        let okCreateSuperEmptyPowerList = fun () -> Ok []
        Villain.create "Dr. Octopus" koCreateHumanInfo okCreateSuperEmptyPowerList
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual("Failed to create HumanInfo: Failed to create PersonInfo: Error parsing Age", errorMsg)
            
    [<Test>]
    member this.``Villain.create if fails creating SuperPowers should return proper error message`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let okCreatePersonInfo = fun () -> PersonInfo.create "Otto" "Octavius" 50
        let okCreateHumanInfo = fun() -> HumanInfo.create okCreatePersonInfo okCreatePersonStats
        let koCreateSuperPowerList = fun () -> Error "Error generating SuperPower list"
        Villain.create "Dr. Octopus" okCreateHumanInfo koCreateSuperPowerList
        |> function
            | Ok _ -> Assert.True(false)
            | Error errorMsg -> Assert.AreEqual("Failed to create SuperPower list: Error generating SuperPower list", errorMsg)

    [<Test>]
    member this.``Villain.create happy-path with empty SuperPower list should return Villain.Human`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let okCreatePersonInfo = fun () -> PersonInfo.create "Otto" "Octavius" 50
        let okCreateHumanInfo = fun() -> HumanInfo.create okCreatePersonInfo okCreatePersonStats
        let okCreateSuperEmptyPowerList = fun () -> Ok []
        Villain.create "Dr. Octopus" okCreateHumanInfo okCreateSuperEmptyPowerList
        |> function
            | Ok villain -> 
                match villain with
                | Villain.Human (villainName, humanInfo) -> 
                    Assert.True((String50.value villainName |> (=) "Dr. Octopus"))
                    Assert.True((String50.value humanInfo.PersonInfo.Name |> (=) "Otto"))
                    Assert.True((String50.value humanInfo.PersonInfo.Surname |> (=) "Octavius"))
                    Assert.True((Positive.value humanInfo.PersonInfo.Age |> (=) 50))
                    Assert.True((Stat.value humanInfo.PersonStats.Strength |> (=) 0))
                    Assert.True((Stat.value humanInfo.PersonStats.Dexterity |> (=) 10))
                    Assert.True((Stat.value humanInfo.PersonStats.Constitution |> (=) 20))
                    Assert.True((Stat.value humanInfo.PersonStats.Intelligence |> (=) 50))
                    Assert.True((Stat.value humanInfo.PersonStats.Wisdom |> (=) 99))
                    Assert.True((Stat.value humanInfo.PersonStats.Charisma |> (=) 100))
                | Villain.SuperHuman(_) -> Assert.True(false)
            | Error _ -> Assert.True(false)

    [<Test>]
    member this.``Villain.create happy-path with SuperPower list should return Human.SuperHuman`` () =
        let okCreatePersonStats = fun () -> PersonStats.create 0 10 20 50 99 100
        let okCreatePersonInfo = fun () -> PersonInfo.create "Otto" "Octavius" 50
        let okCreateHumanInfo = fun() -> HumanInfo.create okCreatePersonInfo okCreatePersonStats
        let okCreateSuperPowerList = fun () -> maybeSeq {
            yield SuperPower.create "SuperPower" "This power gives super strength"
            yield SuperPower.create "SuperDexterity" "This power gives super dexterity"
        }
        Villain.create "Dr. Octopus" okCreateHumanInfo okCreateSuperPowerList
        |> function
            | Ok villain -> 
                match villain with
                | Villain.Human (_) -> Assert.True(false)
                | Villain.SuperHuman(villainName, humanInfo, superPowers) -> 
                    Assert.True((String50.value villainName |> (=) "Dr. Octopus"))
                    Assert.True((String50.value humanInfo.PersonInfo.Name |> (=) "Otto"))
                    Assert.True((String50.value humanInfo.PersonInfo.Surname |> (=) "Octavius"))
                    Assert.True((Positive.value humanInfo.PersonInfo.Age |> (=) 50))
                    Assert.True((Stat.value humanInfo.PersonStats.Strength |> (=) 0))
                    Assert.True((Stat.value humanInfo.PersonStats.Dexterity |> (=) 10))
                    Assert.True((Stat.value humanInfo.PersonStats.Constitution |> (=) 20))
                    Assert.True((Stat.value humanInfo.PersonStats.Intelligence |> (=) 50))
                    Assert.True((Stat.value humanInfo.PersonStats.Wisdom |> (=) 99))
                    Assert.True((Stat.value humanInfo.PersonStats.Charisma |> (=) 100))
                    Assert.AreEqual(2, superPowers.Length)
                    Assert.True((String50.value (superPowers.Item 0).Name |> (=) "SuperPower"))
                    Assert.True((String512.value (superPowers.Item 0).Description |> (=) "This power gives super strength"))
                    Assert.True((String50.value (superPowers.Item 1).Name |> (=) "SuperDexterity"))
                    Assert.True((String512.value (superPowers.Item 1).Description |> (=) "This power gives super dexterity"))
            | Error _ -> Assert.True(false)