module YogurtTheBot.Game.Logic.NodeVisitor

open Microsoft.VisualBasic
open Microsoft.VisualBasic
open Microsoft.VisualBasic
open System
open System.Runtime.InteropServices.WindowsRuntime
open YogurtTheBot.Game.Core.Controllers.Language.Expressions
open YogurtTheBot.Game.Core.Controllers.Language.Nodes

type Visit =
    | None
    | Value of string
    | Named of string * Visit
    | Collection of Visit list

let isValue v =
    match v with
    | None -> false
    | Value v -> not (String.IsNullOrEmpty v)
    | _ -> true
    
    
let createCollection values =
    let rec reduceValues values =
        values
        |> List.filter isValue
        |> List.map (fun v ->
            match v with
            | Collection c -> reduceValues c
            | _ -> [v]
        )
        |> List.concat
    
    match values with
    | [] -> None
    | _ -> Collection (reduceValues values)

let createNamed name term =
    let rec allStrings t =
        match t with
        | Value _ -> true
        | Collection c -> Seq.forall allStrings c
        | _ -> false
        
    let rec toString t =
        match t with
        | Value v -> v
        | Collection c -> c |> Seq.map toString |> String.concat ""
        | _ -> ""
        
    if allStrings term then
        Named(name, Value (toString term))
    else
        Named(name, term)
            
            

let rec visit (node: INode) =
    match node.Expression with
    | :? NonTerminal as nonTerm -> createNamed nonTerm.Name (visitTerminal node)
    | _ -> visitTerminal node
and visitTerminal (node: INode) =
    match node with
    | :? SingleNode -> Value node.Value
    | :? ListNode as nodesList ->
        let values =
            nodesList.Nodes
            |> Seq.map visit
            |> Seq.toList
            
        createCollection values
    | _ -> None
