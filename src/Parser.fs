namespace CleanApi


open Microsoft.CodeAnalysis
open System.Linq
open System.IO
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax
open Serilog

type IOutput =
    abstract member WriteLine: string -> unit

module Parser =
    open System.Collections.Generic
    open System.Text
    open Microsoft.CodeAnalysis
    
    let private writeArrayTypes (writer: IOutput) x =
        writer.WriteLine <| x.GetType().Name
        
    
    let log event object = 
        Log.Information("{Event} - {@Data}", event, object) 
        object

    let readFile = log "Source File" >> System.IO.File.ReadAllText >> CSharpSyntaxTree.ParseText

    let sourceFiles (path : string) =
        if  path.Contains "bin" ||
            path.Contains "obj" then None else Some path

    let nspaceToString (nspace: NamespaceDeclarationSyntax) =
        let kv = nspace.NamespaceKeyword.ToString() 
        let name = nspace.Name.ToString()     
        sprintf "%s %s" kv name     
        
    
    let clsToString (cls: ClassDeclarationSyntax) =
        let name = cls.Identifier.ToString()
        let word = cls.Keyword.ToString()
        let mods = cls.Modifiers.ToString()        
        sprintf "  %s %s %s" mods word name         
    
    let propToString (prop: PropertyDeclarationSyntax) =
        let mods = prop.Modifiers |> string
        let tp = prop.Type |> string
        let name = prop.Identifier |> string
        prop
            .AccessorList
            .Accessors
        |> Seq.filter(fun a -> 
            a.Modifiers |> Seq.isEmpty || 
            a.Modifiers |> Seq.map (fun a -> a.Text) |> Seq.contains "public")
        |> Seq.map (fun a -> a.Keyword.Text + ";")        
        |> String.concat " "
        |> sprintf "    %s %s %s { %s }" mods tp name  
            
            
    let infToString (prop: InterfaceDeclarationSyntax) =
        let mods = prop.Modifiers |> string
        let key = prop.Keyword.Text
        let name = prop.Identifier |> string
        let tp = prop.BaseList |> string
        sprintf "  %s %s %s %s " mods key  name  tp
            
        
    
    let methodToString (method: MethodDeclarationSyntax) = 
        let name = method.Identifier |> string
        let returnType = method.ReturnType |> string
        let mods = 
            let ``mod`` = method.Modifiers |> string
            if ``mod`` |> System.String.IsNullOrWhiteSpace &&
               method.Parent.GetType() = typeof<InterfaceDeclarationSyntax> then 
               "public"
            else 
                ``mod``
                
        method
            .ParameterList
            .Parameters 
        |> Seq.map (fun p -> p.Type.ToString())
        |> String.concat ", "
        |> sprintf "    %s %s %s(%s)" mods returnType name
        
    
    let typeToString (node: SyntaxNode) =
        let string =
            match node with 
            | :? NamespaceDeclarationSyntax as nspace -> nspace |> nspaceToString
            | :? ClassDeclarationSyntax as cls -> cls |> clsToString
            | :? MethodDeclarationSyntax as method -> method |> methodToString 
            | :? PropertyDeclarationSyntax as prop -> prop |> propToString 
            | :? InterfaceDeclarationSyntax as inf -> inf |> infToString 
            | _ -> ""
            
        if System.String.IsNullOrWhiteSpace string then 
            None
        else 
            Some string
        
    type private JohnyWalker(writer: IOutput) =
        inherit CSharpSyntaxWalker()    
        
        let writer = writer        
        let items = new HashSet<string>()
        let writeIfNew =
            fun (i: string) ->
                let small = i.Trim()
                match items.Contains small with 
                | true -> ()
                | false -> small |> items.Add |> ignore; i |> writer.WriteLine
        
        override this.Visit (node: SyntaxNode) = 
            let a = node
            match node |> typeToString with 
            | Some x -> writeIfNew x                    
            | None -> ()
            base.Visit(node)
        
    let execute (writer: IOutput) folder =
        Directory.EnumerateFiles(folder, "*.cs", SearchOption.AllDirectories)
        |> Seq.choose sourceFiles
        |> Seq.map readFile    
        |> Seq.iter (fun root ->
                        let walker = JohnyWalker(writer) 
                        let node = root.GetRoot()
                        walker.Visit node)