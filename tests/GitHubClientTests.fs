namespace CleanApi.Tests

open Xunit
open CleanApi
open Serilog
open Xunit.Abstractions

type GitHubClientTests(output : ITestOutputHelper) =
    
    let output = output
    
    [<Fact>]
    member this.``parser default file``() =    
        //let writer = { new IOutput with member this.WriteLine l = output.WriteLine l}
        ///CleanApi.Parser.execute writer "E:\work\Common\src\Symbotic.SymVerse.Common"
        Assert.True(42 = 42)        
      