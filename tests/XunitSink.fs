namespace CleanApi.Tests

open System
open System.IO
open Serilog.Configuration
open Serilog.Core
open Serilog.Formatting
open Serilog.Events
open Xunit.Abstractions

type XunitSink(testOutputHelper: ITestOutputHelper) =    
    let output = testOutputHelper    
    interface ILogEventSink with 
        member this.Emit logEvent = 
            use renderSpace = new StringWriter();
            logEvent.RenderMessage(renderSpace)
            output.WriteLine(renderSpace.ToString());   
            

[<AutoOpen>]            
module SerilogExt =        
    type LoggerSinkConfiguration with
        member this.Xunit(xunitOutput: ITestOutputHelper) =   
            this.Sink(new XunitSink(xunitOutput));
