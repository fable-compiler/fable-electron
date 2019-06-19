# Fable.Electron

Fable bindings for [Electron](https://electronjs.org/).

## Current status

The bindings are based on the Typescript definitions for Electron 5.0 using [ts2fable](https://fable.io/ts2fable/) and have been cleaned and improved by hand. However, a thorough pass through the generated bindings in order to cross-check them with the Electron documentation is still ongoing, hence the beta status. The bindings should be safe to use, but some (likely minor) breaking changes may occur before the final version is released.

Contributions are welcome!
--------------------------

Electron releases frequently, and I can make no promises to keep the bindings updated in a timely manner. Pull requests are more than welcome, whether it’s bindings for new APIs, new helpers, bugfixes, or just improving typos and formatting in the documentation. If you want to create a PR with non-trivial changes, consider opening an issue first so you don’t waste time and effort on something that might not be accepted or might already be underway.

How to use the bindings
----------

Note that since Fable.Electron is mostly just bindings, [the official Electron docs](https://electronjs.org/docs) is the place to go for general Electron usage.

For an example app with complete boilerplate for Electron apps using Fable and Elmish with hot module reloading, time-travel debugging, etc., check out [Fable-Elmish-Electron-Material-UI demo](https://github.com/cmeeren/fable-elmish-electron-material-ui-demo).

To get started with the bindings, simply:

* Install the `Fable.Electron` Nuget package as well as the `electron` npm package. Make sure the `electron` version is compatible with the bindings.
* `open Electron`. In addition to all the the type definitions, you now have access to two entry points for the Electron API:
  * `main` for everything that can be used from the main process
  * `renderer` for everything that can be used from the renderer process

You can also use `electron` to access everything, but consider using only `main` or `renderer`.

Helpers
-------

In addition to the bindings, Fable.Electron also gives you access to some convenience helpers if you `open Electron.Helpers`:

### Accelerators (keyboard shortcuts)

Accelerators in Electron are simply strings with a specific format. If you want, you can use `createAccelerator` with the `Modifier` and `Key` helper types to create them in a strongly typed manner:

```f#
// Returns an accelerator string that can be used to register shortcuts
createAccelerator: Modifier list -> Key -> string
```

Example:

```f#
let accelerator = createAccelerator [Modifier.Ctrl; Modifier.Alt] Key.Tilde
main.globalShortcut.register(accelerator, fun () -> (* do stuff *))

// The above is identical to:
main.globalShortcut.register("Ctrl+Alt+~", fun () -> (* do stuff *))
```

If you need to use a key that is not in the Key type but that you know works, you can use a string with Fable's `!!` operator, or just skip the helper altogether (also, please make a PR to add the key!):

```f#
let accelerator = createAccelerator [Modifier.Ctrl] !!"="

// The above is identical to:
let accelerator = "Ctrl+="
```

## Deployment checklist

1. Make necessary changes to the code
2. Update the changelog
3. Update the version and release notes in the package info, as well as the message stating which Electron version the bindings are created for in the package description.
4. Commit and tag the commit (this is what triggers deployment from  AppVeyor). For consistency, the tag should ideally be identical to the package version number.
5. Push the changes and the tag to the repo. If AppVeyor build succeeds, the package is automatically published to NuGet.
