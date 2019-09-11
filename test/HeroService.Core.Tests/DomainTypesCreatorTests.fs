namespace HeroService.Core.Tests

open NUnit.Framework
open HeroService.Core.DomainTypes

type DomainTypesCreatorTests () =

    [<TestCase(-1, false)>]
    [<TestCase(0, true)>]
    [<TestCase(10, true)>]
    [<TestCase(50, true)>]
    [<TestCase(100, true)>]
    [<TestCase(101, false)>]
    member this.``Stat create should return Some with value between 0 and 100`` (value: int, isSome: bool) =
        Stat.create value
        |> function
            | Some stat -> 
                Assert.True(isSome)
                let extractedValue = Stat.value stat
                Assert.AreEqual(value, extractedValue)
            | None -> Assert.False(isSome)

    [<Test>]
    member this.``PersonStats create should return stats with default 0 value if out of range`` () =
        let personStats = PersonStats.create 10 -1 50 110 60 100
        Assert.AreEqual(10, Stat.value personStats.Strength)
        Assert.AreEqual(0, Stat.value personStats.Dexterity)
        Assert.AreEqual(50, Stat.value personStats.Constitution)
        Assert.AreEqual(0, Stat.value personStats.Intelligence)
        Assert.AreEqual(60, Stat.value personStats.Wisdom)
        Assert.AreEqual(100, Stat.value personStats.Charisma)
