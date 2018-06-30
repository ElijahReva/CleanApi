// Learn more about F# at http://fsharp.org
namespace CleanApi

open Serilog

[<AutoOpen>]
module CleanApi =

    let buildLogger() = 
             Log.Logger <- 
                    (new LoggerConfiguration())
                        .MinimumLevel.Verbose()
                        .WriteTo.Console()
                        .CreateLogger()
    
    [<EntryPoint>]
    let main argv =
        buildLogger()
        let writer = { new IOutput with member this.WriteLine line = printfn "%s" line }
        argv.[0] |> Parser.execute writer
        0
