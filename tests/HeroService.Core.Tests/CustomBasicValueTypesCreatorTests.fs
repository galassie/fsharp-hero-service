﻿namespace HeroService.Core.Tests

open NUnit.Framework
open HeroService.Core.CustomBasicValueTypes

type CustomBasicValueTypesCreatorTests () =

    [<TestCase(-10, false)>]
    [<TestCase(-1, false)>]
    [<TestCase(10, true)>]
    [<TestCase(50, true)>]
    [<TestCase(100, true)>]
    [<TestCase(101, true)>]
    member this.``Positive.create should return Some with positive values`` (testValue, isSome: bool) =
        Positive.create testValue
        |> function
            | Some boxValue -> 
                Assert.IsTrue(isSome)
                Assert.AreEqual(testValue, (Positive.value boxValue))
            | None -> Assert.False(isSome)

    [<TestCase(-1, false)>]
    [<TestCase(0, true)>]
    [<TestCase(10, true)>]
    [<TestCase(50, true)>]
    [<TestCase(100, true)>]
    [<TestCase(101, false)>]
    member this.``Positive100.create should return Some with value between 0 and 100`` (testValue, isSome: bool) =
        Positive100.create testValue
        |> function
            | Some boxValue -> 
                Assert.True(isSome)
                Assert.AreEqual(testValue, (Positive100.value boxValue))
            | None -> Assert.False(isSome)

    [<TestCase(null, false)>]
    [<TestCase("", false)>]
    [<TestCase("          ", false)>]
    [<TestCase("?", true)>]
    [<TestCase("Test", true)>]
    [<TestCase("   T   ", true)>]
    [<TestCase("12345678901234567890123456789012345678901234567890", true)>]
    [<TestCase("123456789012345678901234567890123456789012345678901", false)>]
    member this.``String50.create should return Some with value not null or empty and less than 50`` (testValue, isSome: bool) =
        String50.create testValue
        |> function
            | Some boxValue -> 
                Assert.True(isSome)
                Assert.AreEqual(testValue, (String50.value boxValue))
            | None -> Assert.False(isSome)

    [<TestCase(null, false)>]
    [<TestCase("", false)>]
    [<TestCase("          ", false)>]
    [<TestCase("?", true)>]
    [<TestCase("Test", true)>]
    [<TestCase("   T   ", true)>]
    [<TestCase("12345678901234567890123456789012345678901234567890", true)>]
    member this.``String512.create should return Some with value not null or empty and less than 512`` (testValue, isSome: bool) =
        String50.create testValue
        |> function
            | Some boxValue -> 
                Assert.True(isSome)
                Assert.AreEqual(testValue, (String50.value boxValue))
            | None -> Assert.False(isSome)