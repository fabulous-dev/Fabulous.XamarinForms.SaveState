namespace StateSaveSample

open Newtonsoft.Json
open Xamarin.Forms
open Fabulous.XamarinForms
open Fabulous.XamarinForms.SaveState

open type View

module App =
    type Model = { Count: int; Failure: string option }

    type Msg =
        | StateSaveFailure of exn
        | Increment
        | Decrement

    let init () = { Count = 0; Failure = None }

    let update msg model =
        match msg with
        | StateSaveFailure ex -> { model with Failure = Some ex.Message }
        | Increment -> { model with Count = model.Count + 1 }
        | Decrement -> { model with Count = model.Count - 1 }

    let view model =
        Application(
            ContentPage(
                "StateSaveSample",
                VStack() {
                    Label("Hello from Fabulous v2!")
                        .font(namedSize = NamedSize.Title)
                        .centerTextHorizontal ()

                    match model.Failure with
                    | None -> ()
                    | Some failure -> Label(failure)

                    (VStack() {
                        Label($"Count is {model.Count}").centerTextHorizontal ()

                        Button("Increment", Increment)
                        Button("Decrement", Decrement)
                    })
                        .centerVertical (expand = true)
                }
            )
        )

    let program =
        Program.stateful init update view
        |> Program.withStateSave JsonConvert.SerializeObject JsonConvert.DeserializeObject<Model> StateSaveFailure
