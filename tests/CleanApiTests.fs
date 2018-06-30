// Learn more about F# at http://fsharp.org
namespace CleanApi.Tests

open System.IO
open System.Reflection
open Xunit
open CleanApi
open Serilog
open Xunit.Abstractions

type CleanApiTests(output : ITestOutputHelper) =
    
    let output = output
    let path =
        let getP (current: DirectoryInfo) = current.Parent  
        Assembly.GetExecutingAssembly().Location
        |> DirectoryInfo 
        |> getP
        |> getP
        |> getP
        |> getP
        |> (fun x -> x.FullName)
       
    
    [<Fact>]
    member this.Example() =
        let writer = { new IOutput with member this.WriteLine l = output.WriteLine l}
        CleanApi.Parser.execute writer path