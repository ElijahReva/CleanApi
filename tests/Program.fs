// Learn more about F# at http://fsharp.org

open System;
open System.Collections.Generic;
open System.IO

open System.Linq;
open System.Text;
open System.Threading.Tasks;
open Microsoft.CodeAnalysis;
open Microsoft.CodeAnalysis.CSharp;
open Microsoft.CodeAnalysis.CSharp.Syntax;
[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    
    

    let solutionPath = "E:\work\Common\src";
    Directory.EnumerateFiles(solutionPath, "*.cs", SearchOption.AllDirectories)
    |> Seq.map (fun x -> printfn "File - %s" x; x)
    |> Seq.map System.IO.File.ReadAllText
    |> Seq.map (fun x -> CSharpSyntaxTree.ParseText(x))
    // |> Seq.map (fun tree -> tree.GetCompilationUnitRoot())
    |> Seq.iter (fun root -> 
                    let methods =
                        root.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
        
                    methods
                    |> Seq.iter (fun x -> Printf.printfn "%A" x))
    
    0 // return an integer exit code
