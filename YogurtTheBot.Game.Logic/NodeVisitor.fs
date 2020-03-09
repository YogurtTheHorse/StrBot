module YogurtTheBot.Game.Logic.NodeVisitor

open System
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
            | _ -> [ v ])
        |> List.concat

    match values with
    | [] -> None
    | _ -> Collection(reduceValues values)

let createNamed name term =
    let rec allStrings t =
        match t with
        | Value _ -> true
        | Collection c -> Seq.forall allStrings c
        | _ -> false

    let rec toString t =
        match t with
        | Value v -> v
        | Collection c ->
            c
            |> Seq.map toString
            |> String.concat ""
        | _ -> ""

    if allStrings term then Named(name, Value(toString term))
    else Named(name, term)



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

let isNammed v =
    match v with
    | Named _ -> true
    | _ -> false

let rec clearUnnammed node =
    let rec clearCollection c =
        c
        |> Seq.map clearUnnammed
        |> Seq.filter isNammed
        |> Seq.toList

    match node with
    | Collection c -> Collection(clearCollection c)
    | Named(name, Collection c) -> Named(name, Collection(clearCollection c))
    | Named(_, Value _) -> node
    | _ -> None

let get visit (path: string) =    
    let rec _get visit keys =        
        let k :: tail = keys
        
        let nextVisit = 
            match visit with
            | Collection c ->
                let foundV =
                    Seq.tryFind (fun e ->
                        match e with
                        | Named (n, v) when n = k -> true
                        | _ -> false
                    ) c
                
                match foundV with
                | Some v -> v
                | Option.None -> None
            | Named (name, v) when name = k -> v
            | _ -> None
            
        match tail, nextVisit with
        | [], Named (_, v) -> v
        | [], _ -> None
        | _, Named (_, v) -> _get v tail
        | _ -> None
        
    _get visit (path.Split "." |> Seq.toList)

let value v =
    match v with
    | Value v -> v
    | Named (_ , Value v) -> v
    | _ -> null