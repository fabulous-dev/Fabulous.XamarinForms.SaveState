Example using Newtonsoft.Json to serialize and deserialize the app state

```fs
open Fabulous
open Fabulous.XamarinForms
open Fabulous.XamarinForms.SaveState

type Msg =
    | StateSaveFailure of exn // This one will be called if the serialization fails
    | ... // Other messages

Program.stateful init update view
|> Program.withSaveState
    JsonConvert.SerializeObject
    JsonConvert.DeserializeObject<Model>
    StateSaveFailure
```