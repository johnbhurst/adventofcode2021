#!/usr/bin/env -S dotnet fsi
// Copyright 2023 John Hurst
// John Hurst (john.b.hurst@gmail.com)
// 2023-12-03
// See https://adventofcode.com/2023/day/2

open System.Text.RegularExpressions

type Combo = { Red: int; Green: int; Blue: int}
type Game = { ID: int; Combos: Combo list }

let parseColor s =
    let m = Regex.Match(s, "([0-9]+) ([a-z]+)")
    if m.Success then
        let count = int m.Groups.[1].Value
        let color = m.Groups.[2].Value
        color, count
    else
        failwithf "Cannot parse color from '%s'" s

let parseCombo (s:string) =
    let m = (s.Split(", ")) |> Seq.map parseColor  |> Map.ofSeq
    { Red = (if Map.containsKey "red" m then Map.find "red" m else 0);
        Green = (if Map.containsKey "green" m then Map.find "green" m else 0);
        Blue = (if Map.containsKey "blue" m then Map.find "blue" m else 0) }

let parseCombos (s:string) =
    Seq.map parseCombo (s.Split("; "))
        |> Seq.toList

let parseGame s =
    let m = Regex.Match(s, "^Game ([0-9]+): (.*)$")
    if m.Success then
        let id = int m.Groups.[1].Value
        let combos = parseCombos m.Groups.[2].Value
        { ID = id; Combos = combos }
    else
        failwithf "Cannot parse game from '%s'" s

let minimumCombo game =
    let maxRed = List.maxBy (fun combo -> combo.Red) game.Combos
    let maxGreen = List.maxBy (fun combo -> combo.Green) game.Combos
    let maxBlue = List.maxBy (fun combo -> combo.Blue) game.Combos
    { Red = maxRed.Red; Green = maxGreen.Green; Blue = maxBlue.Blue }

let power combo = combo.Red * combo.Green * combo.Blue

System.IO.File.ReadLines( fsi.CommandLineArgs.[1] )
    |> Seq.map parseGame
    |> Seq.map (minimumCombo >> power)
    |> Seq.sum
    |> printfn "%A"
