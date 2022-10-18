namespace Fabulous.XamarinForms.SaveState

open Xamarin.Forms
open Fabulous
open Fabulous.XamarinForms

open type Fabulous.XamarinForms.View
    
module Program =
    let [<Literal>] AppStateKey = "Fabulous.XF.SaveState"
    
    type SaveStateMsg<'msg, 'model> =
        | AppMsg of 'msg
        | SaveCurrentModel
        | LoadPreviousModel
        | ModelSavingError of exn
        | PreviousModelLoaded of 'model
    
    let private saveModelCmd (encode: 'model -> string) (model: 'model): Cmd<SaveStateMsg<'msg, 'model>> =
        Cmd.ofSub (fun dispatch ->
            try
                let json = encode model
                Application.Current.Properties.[AppStateKey] <- json
            with
            | ex -> dispatch (ModelSavingError ex)
        )
            
    let private loadModelCmd<'msg, 'model> (decode: string -> 'model): Cmd<SaveStateMsg<'msg, 'model>> =
        Cmd.ofMsgOption(
            if Application.Current.Properties.ContainsKey(AppStateKey) then
                let json = Application.Current.Properties.[AppStateKey] :?> string
                let model = decode json
                Some (PreviousModelLoaded model)
            else
                None
        )
        
    let withStateSave
        (encode: 'model -> string)
        (decode: string -> 'model)
        (failure: exn -> 'msg)
        (program: Program<'arg, 'model, 'msg, IApplication>) =
    
        let init args =
            let m, c = program.Init(args)
            m, Cmd.batch [ Cmd.ofMsg LoadPreviousModel; Cmd.map AppMsg c ]
            
        let update (msg, model) =
            match msg with
            | AppMsg appMsg ->
                let m, c = program.Update(appMsg, model)
                m, Cmd.map AppMsg c
            
            | SaveCurrentModel ->
                model, saveModelCmd encode model
            
            | LoadPreviousModel ->
                model, loadModelCmd decode
            
            | ModelSavingError ex ->
                model, Cmd.ofMsg (AppMsg (failure ex))
                
            | PreviousModelLoaded prevModel ->
                prevModel, Cmd.none
                
        let view (model: 'model) =
            (View.map AppMsg (program.View(model)))
                .onStart(LoadPreviousModel)
                .onResume(LoadPreviousModel)
                .onSleep(SaveCurrentModel)
            
        let subscribe (model: 'model) =
            let c = program.Subscribe(model)
            Cmd.map AppMsg c
                
        { Init = init
          Update = update
          Subscribe = subscribe
          View = view
          CanReuseView = program.CanReuseView
          SyncAction = program.SyncAction
          Logger = program.Logger }