namespace CleanApi


open Microsoft.CodeAnalysis
open System.Linq
open System.IO
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax
open Serilog
open System.Collections.Generic
open System.Text
open Microsoft.CodeAnalysis

module GitHubClient =
    
    let execute writer uri =
        ()