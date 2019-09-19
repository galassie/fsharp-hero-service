namespace HeroService.Core.Tests

open NUnit.Framework
open HeroService.Core.CustomBasicValueTypes
open HeroService.Core.DomainTypes

type DomainTypesCreatorTests() =

    [<TestCase("", "Evans", 12, "Error parsing name")>]
    [<TestCase("Chris", "", 20, "Error parsing surname")>]
    [<TestCase("Chris", "Evans", -10, "Error parsing age")>]
    member this.``PersonInfo.create should return proper error message`` (name, surname, age, expectedErrorMsg) =
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
    member this.``PersonStats.create should return proper error message`` (str, dex, cons, int, wis, cha, expectedErrorMsg) =
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