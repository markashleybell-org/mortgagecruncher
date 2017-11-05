namespace core

module Helpers =
    let inline toMap kvps =
        kvps
        |> Seq.map (|KeyValue|)
        |> Map.ofSeq

