#r "node-modules/fable-metadata/lib/Fable.Core.dll"
#load "node_modules/fable-publish-utils/PublishUtils.fs"

open PublishUtils

match args with
| IgnoreCase "publish"::_ ->
    pushNuget "src/Fable.Electron.fsproj"
| _ -> ()
