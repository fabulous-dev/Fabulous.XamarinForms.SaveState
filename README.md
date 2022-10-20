Fabulous.XamarinForms.SaveState
--

Add the save state feature to your Fabulous.XamarinForms app.  
This package will automatically save the current state of your app when it is suspended and restore it when it is resumed.

### How to use

1. Add the [Fabulous.XamarinForms.SaveState](https://www.nuget.org/packages/Fabulous.XamarinForms.SaveState/) package to your project

2. Open `Fabulous.XamarinForms.SaveState` at the top of the file where you declare your Fabulous program (eg. `Program.stateful`).

```fs
open Fabulous.XamarinForms.SaveState
```

3. Add an additional Msg case to your app to handle the state save failure.

```fs
type Msg =
    | StateSaveFailure of exn // This one will be called if the serialization fails
    | ... // Other messages
```

4. After the declaration of your Fabulous program, add the `withSaveState` function.  
(This example uses Newtonsoft.Json to serialize and deserialize the app state)

```fs
let encodeAppState (model: Model): string = JsonConvert.SerializeObject(model)
let decodeAppState (encodedState: string): Model = JsonConvert.DeserializeObject<Model>(encodedState)

let program =
    Program.stateful init update view
    |> Program.withSaveState encodeAppState decodeAppState StateSaveFailure
```

<img src="docs/save-state.gif" height="500">