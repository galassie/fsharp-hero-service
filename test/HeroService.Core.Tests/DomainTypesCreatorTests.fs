namespace HeroService.Core.Tests

open NUnit.Framework
open HeroService.Core.CustomBasicValueTypes
open HeroService.Core.DomainTypes

type DomainTypesCreatorTests() =

    [<TestCase("", "Evans", 12, "Error parsing name")>]
    [<TestCase("Chris", "", 20, "Error parsing surname")>]
    [<TestCase("Chris", "Evans", -10, "Error parsing age")>]
    member this.``PersonInfo.create shouold return proper error message`` (name, surname, age, expectedErrorMsg) =
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