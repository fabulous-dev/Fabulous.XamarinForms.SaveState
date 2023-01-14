# Fabulous.XamarinForms.SaveState

[![build](https://img.shields.io/github/actions/workflow/status/fabulous-dev/Fabulous.XamarinForms.SaveState/build.yml?branch=main)](https://github.com/fabulous-dev/Fabulous.XamarinForms.SaveState/actions/workflows/build.yml) [![NuGet version](https://img.shields.io/nuget/v/Fabulous.XamarinForms.SaveState)](https://www.nuget.org/packages/Fabulous.XamarinForms.SaveState) [![NuGet downloads](https://img.shields.io/nuget/dt/Fabulous.XamarinForms.SaveState)](https://www.nuget.org/packages/Fabulous.XamarinForms.SaveState) [![Discord](https://img.shields.io/discord/716980335593914419?label=discord&logo=discord)](https://discord.gg/bpTJMbSSYK) [![Twitter Follow](https://img.shields.io/twitter/follow/FabulousAppDev?style=social)](https://twitter.com/FabulousAppDev)

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

## Supporting Fabulous

The simplest way to show us your support is by giving this project and the [Fabulous project](https://github.com/fabulous-dev/Fabulous) a star.

You can also support us by becoming our sponsor on the GitHub Sponsors program.  
This is a fantastic way to support all the efforts going into making Fabulous the best declarative UI framework for dotnet.

If you need support see Commercial Support section below.

## Contributing

Have you found a bug or have a suggestion of how to enhance Fabulous? Open an issue and we will take a look at it as soon as possible.

Do you want to contribute with a PR? PRs are always welcome, just make sure to create it from the correct branch (main) and follow the [Contributor Guide](CONTRIBUTING.md).

For bigger changes, or if in doubt, make sure to talk about your contribution to the team. Either via an issue, GitHub discussion, or reach out to the team either using the [Discord server](https://discord.gg/bpTJMbSSYK).