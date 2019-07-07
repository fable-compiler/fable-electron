﻿module rec Electron


open System
open Fable.Core
open Fable.Core.JS
open Browser.Types
open Node.Base
open Node.Buffer

// TODO: check if U2, U3 etc. can be replaced with overloads
// TODO: ensure that all onceX, addListenerX, removeListenerX events refer to
// the onX event
// TODO: make all references to null and undefined refer to None instead
// TODO: go through all usages of Event and check that it really is Event at runtime


/// All Electron APIs. Consider using `main` or `renderer` as your entry points;
/// there is nothing here that is not accessible through them.
[<ImportAll("electron")>]
let electron : IExports = jsNative

/// All Electron APIs usable from the main process.
[<ImportAll("electron")>]
let main : MainInterface = jsNative

/// All Electron APIs usable from the renderer process.
[<ImportAll("electron")>]
let renderer : RendererInterface = jsNative


type IExports =
  inherit AllElectron
  abstract TouchBarButton: TouchBarButtonStatic
  abstract TouchBarColorPicker: TouchBarColorPickerStatic
  abstract TouchBarGroup: TouchBarGroupStatic
  abstract TouchBarLabel: TouchBarLabelStatic
  abstract TouchBarPopover: TouchBarPopoverStatic
  abstract TouchBarScrubber: TouchBarScrubberStatic
  abstract TouchBarSegmentedControl: TouchBarSegmentedControlStatic
  abstract TouchBarSlider: TouchBarSliderStatic
  abstract TouchBarSpacer: TouchBarSpacerStatic

type EventEmitter<'self> =
  abstract addListener: event: string * listener: (Event -> unit) -> 'self
  abstract on: event: string * listener: (Event -> unit) -> 'self
  abstract once: event: string * listener: (Event -> unit) -> 'self
  abstract removeListener: event: string * listener: (Event -> unit) -> 'self
  abstract removeAllListeners: ?event: string -> 'self
  abstract setMaxListeners: n: int -> 'self
  abstract getMaxListeners: unit -> int
  abstract listeners: event: string -> (Event -> unit) []
  abstract emit: event: string * [<ParamArray>] args: obj [] -> bool
  abstract listenerCount: event: string -> int
  abstract prependListener: event: string * listener: (Event -> unit) -> 'self
  abstract prependOnceListener: event: string * listener: (Event -> unit) -> 'self
  abstract eventNames: unit -> string []

type ReturnValueEvent =
  inherit Browser.Types.Event
  /// Set this to return a custom value.
  abstract returnValue: obj option with get, set

type IpcMainEvent =
  inherit Browser.Types.Event
  /// Set this to the value to be returned in a synchronous message.
  abstract returnValue: obj option with get, set
  /// The ID of the renderer frame that sent this message.
  abstract frameId: int with get, set
  /// The webContents that sent the message. You can call sender.send to reply
  /// to the asynchronous message. See webContents.send for more information.
  abstract sender: WebContents with get, set
  /// A function that will send an IPC message to the renderer frame that sent
  /// the original message that you are currently handling. You should use this
  /// method to "reply" to the sent message in order to guarantee the reply will
  /// go to the correct process and frame.
  abstract reply: channel: string * [<ParamArray>] args: obj [] -> unit

type IpcRendererEvent =
  inherit Browser.Types.Event
  /// Returns the webContents.id that sent the message, you can call
  /// event.sender.sendTo(event.senderId, ...) to reply to the message, see
  /// ipcRenderer.sendTo for more information. This only applies to messages
  /// sent from a different renderer. Messages sent directly from the main
  /// process set event.senderId to 0.
  abstract senderId: unit -> int

type TrayInputEvent =
  inherit Browser.Types.Event
  abstract altKey: bool with get, set
  abstract shiftKey: bool with get, set
  abstract ctrlKey: bool with get, set
  abstract metaKey: bool with get, set
  /// Whether an accelerator was used to trigger the event as opposed to another user gesture like mouse click
  abstract triggeredByAccelerator: bool with get, set

type CommonInterface =
  /// Perform copy and paste operations on the system clipboard. On Linux, there
  /// is also a selection clipboard. To manipulate it you need to pass
  /// ClipboardType.Selection to relevant methods.
  ///
  /// https://electronjs.org/docs/api/clipboard
  abstract clipboard: Clipboard with get, set
  /// Submit crash reports to a remote server.
  ///
  /// https://electronjs.org/docs/api/crash-reporter
  abstract crashReporter: CrashReporter with get, set
  /// Create tray, dock, and application icons using PNG or JPG files. In
  /// Electron, for the APIs that take images, you can pass either file paths or
  /// NativeImage instances. An empty image will be used when null is passed.
  ///
  /// https://electronjs.org/docs/api/native-image
  abstract NativeImage: NativeImageStatic with get, set
  /// Manage files and URLs using their default applications. Provides functions
  /// related to desktop integration.
  ///
  /// https://electronjs.org/docs/api/shell
  abstract shell: Shell with get, set

type MainInterface =
  inherit CommonInterface
  /// Control your application's event lifecycle.
  ///
  /// https://electronjs.org/docs/api/app
  abstract app: App with get, set
  /// Enable apps to automatically update themselves.
  ///
  /// https://electronjs.org/docs/api/auto-updater
  abstract autoUpdater: AutoUpdater with get, set
  /// Create and control views. A BrowserView can be used to embed additional
  /// web content into a BrowserWindow. It is like a child window, except that
  /// it is positioned relative to its owning window.
  ///
  /// https://electronjs.org/docs/api/browser-view
  abstract BrowserView: BrowserViewStatic with get, set
  /// Create and control browser windows.
  ///
  /// https://electronjs.org/docs/api/browser-window
  abstract BrowserWindow: BrowserWindowStatic with get, set
  /// Make HTTP/HTTPS requests. Also see the `net` module to create
  /// ClientRequest instances.
  ///
  /// https://electronjs.org/docs/api/client-request
  abstract ClientRequest: ClientRequestStatic with get, set
  /// Collect tracing data from Chromium's content module to find performance
  /// bottlenecks and slow operations. This module does not include a web
  /// interface, so you need to open chrome://tracing/ in a Chrome browser and
  /// load the generated file to view the result. Note: You should not use this
  /// module until the `ready` event of the `app` module is emitted.
  ///
  /// https://electronjs.org/docs/api/content-tracing
  abstract contentTracing: ContentTracing with get, set
  /// Display native system dialogs for opening and saving files, alerting, etc.
  ///
  /// https://electronjs.org/docs/api/dialog
  abstract dialog: Dialog with get, set
  /// Detect keyboard events when the application does not have keyboard focus.
  /// The `globalShortcut` module can register/unregister a global keyboard
  /// shortcut with the operating system so that you can customize the
  /// operations for various shortcuts. Note: The shortcut is global; it will
  /// work even if the app does not have the keyboard focus. You should not use
  /// this module until the ready event of the app module is emitted.
  ///
  /// https://electronjs.org/docs/api/global-shortcut
  abstract globalShortcut: GlobalShortcut with get, set
  /// In-app purchases on Mac App Store.
  ///
  /// https://electronjs.org/docs/api/in-app-purchase
  abstract inAppPurchase: InAppPurchase with get, set
  /// Communicate asynchronously from the main process to renderer processes.
  /// This module it handles asynchronous and synchronous messages sent from a
  /// renderer process (web page). Messages sent from a renderer will be emitted
  /// to this module.
  ///
  /// https://electronjs.org/docs/api/ipc-main
  abstract ipcMain: IpcMain with get, set
  /// Create native application menus and context menus.
  ///
  /// https://electronjs.org/docs/api/menu
  abstract Menu: MenuStatic with get, set
  /// Add items to native application menus and context menus.
  ///
  /// https://electronjs.org/docs/api/menu-item
  abstract MenuItem: MenuItemStatic with get, set
  /// Issue HTTP/HTTPS requests using Chromium's native networking library.
  ///
  /// https://electronjs.org/docs/api/net
  abstract net: Net with get, set
  /// Logging network events for a session.
  ///
  /// https://electronjs.org/docs/api/net-log
  abstract netLog: NetLog with get, set
  /// Create OS desktop notifications.
  ///
  /// https://electronjs.org/docs/api/notification
  abstract Notification: NotificationStatic with get, set
  /// Monitor power state changes.
  ///
  /// https://electronjs.org/docs/api/power-monitor
  abstract powerMonitor: PowerMonitor with get, set
  /// Block the system from entering low-power (sleep) mode.
  ///
  /// https://electronjs.org/docs/api/power-save-blocker
  abstract powerSaveBlocker: PowerSaveBlocker with get, set
  /// Register a custom protocol and intercept existing protocol requests.
  ///
  /// https://electronjs.org/docs/api/protocol
  abstract protocol: Protocol with get, set
  /// Retrieve information about screen size, displays, cursor position, etc.
  /// You cannot require or use this module until the `ready` event of the `app`
  /// module is emitted.
  ///
  /// https://electronjs.org/docs/api/screen
  abstract screen: Screen with get, set
  /// Manage browser sessions, cookies, cache, proxy settings, etc.
  ///
  /// https://electronjs.org/docs/api/session
  abstract Session: SessionStatic with get, set
  /// Get system preferences.
  ///
  /// https://electronjs.org/docs/api/system-preferences
  abstract systemPreferences: SystemPreferences with get, set
  /// Create TouchBar layouts for native macOS applications
  ///
  /// https://electronjs.org/docs/api/touch-bar
  abstract TouchBar: TouchBarStatic with get, set
  /// Add icons and context menus to the system's notification area.
  ///
  /// https://electronjs.org/docs/api/tray
  abstract Tray: TrayStatic with get, set
  /// Render and control web pages.
  ///
  /// https://electronjs.org/docs/api/web-contents
  abstract WebContents: WebContentsStatic with get, set

type RendererInterface =
  inherit CommonInterface
  /// Access information about media sources that can be used to capture audio
  /// and video from the desktop using the `navigator.mediaDevices.getUserMedia`
  /// API.
  ///
  /// https://electronjs.org/docs/api/desktop-capturer
  abstract desktopCapturer: DesktopCapturer with get, set
  /// Communicate asynchronously from a renderer process to the main process.
  ///
  /// https://electronjs.org/docs/api/ipc-renderer
  abstract ipcRenderer: IpcRenderer with get, set
  /// Use main process modules from the renderer process. The `remote` module
  /// provides a simple way to do inter-process communication (IPC) between the
  /// renderer process (web page) and the main process.
  abstract remote: Remote with get, set
  /// Customize the rendering of the current web page. This module is an
  /// instance of the WebFrame class representing the top frame of the current
  /// BrowserWindow. Sub-frames can be retrieved by certain properties and
  /// methods (e.g. webFrame.firstChild).
  abstract webFrame: WebFrame with get, set

type AllElectron =
  inherit MainInterface
  inherit RendererInterface

[<StringEnum; RequireQualifiedAccess>]
type GpuInfoType =
  /// Basic GPU info.
  | Basic
  /// Complete GPU info.
  | Complete

[<StringEnum; RequireQualifiedAccess>]
type AppPathName =
  /// The user's home directory.
  | Home
  /// Per-user application data directory, which by default points to: -
  /// %APPDATA% on Windows - $XDG_CONFIG_HOME or ~/.config on Linux -
  /// ~/Library/Application Support on macOS
  | AppData
  /// The directory for storing your app's configuration files, which by default
  /// it is the AppData directory appended with your app's name.
  | UserData
  /// Temporary directory.
  | Temp
  /// The current executable file.
  | Exe
  /// The libchromiumcontent library.
  | Module
  /// The current user's Desktop directory.
  | Desktop
  /// Directory for a user's "My Documents".
  | Documents
  /// Directory for a user's downloads.
  | Downloads
  /// Directory for a user's music.
  | Music
  /// Directory for a user's pictures.
  | Pictures
  /// Directory for a user's videos.
  | Videos
  /// Directory for your app's log folder.
  | Logs
  /// Full path to the system version of the Pepper Flash plugin.
  | PepperFlashSystemPlugin

[<StringEnum; RequireQualifiedAccess>]
type SetJumpListResult =
  /// Nothing went wrong.
  | Ok
  /// One or more errors occurred, enable runtime logging to figure out the
  /// likely cause.
  | Error
  /// An attempt was made to add a separator to a custom category in the Jump
  /// List. Separators are only allowed in the standard Tasks category.
  | InvalidSeparatorError
  /// An attempt was made to add a file link to the Jump List for a file type
  /// the app isn't registered to handle.
  | FileTypeRegistrationError
  /// Custom categories can't be added to the Jump List due to user privacy or
  /// group policy settings.
  | CustomCategoryAccessDeniedError

type App =
  inherit EventEmitter<App>
  /// Emitted when the application has finished basic startup. On Windows and
  /// Linux, the `will-finish-launching` event is the same as the `ready` event;
  /// on macOS, this event represents the `applicationWillFinishLaunching`
  /// notification of `NSApplication`. You would usually set up listeners for
  /// the `open-file` and `open-url` events here, and start the crash reporter
  /// and auto updater. In most cases, you should do everything in the `ready`
  /// event handler.
  [<Emit "$0.on('will-finish-launching',$1)">] abstract onWillFinishLaunching: listener: (Event -> unit) -> App
  /// See onWillFinishLaunching.
  [<Emit "$0.once('will-finish-launching',$1)">] abstract onceWillFinishLaunching: listener: (Event -> unit) -> App
  /// See onWillFinishLaunching.
  [<Emit "$0.addListener('will-finish-launching',$1)">] abstract addListenerWillFinishLaunching: listener: (Event -> unit) -> App
  /// See onWillFinishLaunching.
  [<Emit "$0.removeListener('will-finish-launching',$1)">] abstract removeListenerWillFinishLaunching: listener: (Event -> unit) -> App
  /// Emitted when Electron has finished initializing. You can call
  /// `app.isReady()` to check if this event has already fired.
  ///
  /// Extra parameters:
  ///
  ///   - launchInfo: On macOS, this object holds the `userInfo` of the
  ///     `NSUserNotification` that was used to open the application, if it was
  ///     launched from Notification Center
  [<Emit "$0.on('ready',$1)">] abstract onReady: listener: (Event -> obj -> unit) -> App
  /// See onReady.
  [<Emit "$0.once('ready',$1)">] abstract onceReady: listener: (Event -> obj -> unit) -> App
  /// See onReady.
  [<Emit "$0.addListener('ready',$1)">] abstract addListenerReady: listener: (Event -> obj -> unit) -> App
  /// See onReady.
  [<Emit "$0.removeListener('ready',$1)">] abstract removeListenerReady: listener: (Event -> obj -> unit) -> App
  /// Emitted when all windows have been closed. If you do not subscribe to this
  /// event and all windows are closed, the default behavior is to quit the app;
  /// however, if you subscribe, you control whether the app quits or not. If
  /// the user pressed Cmd + Q, or the developer called `app.quit()`, Electron
  /// will first try to close all the windows and then emit the `will-quit`
  /// event, and in this case the `window-all-closed` event would not be
  /// emitted.
  [<Emit "$0.on('window-all-closed',$1)">] abstract onWindowAllClosed: listener: (Event -> unit) -> App
  /// See onWindowAllClosed.
  [<Emit "$0.once('window-all-closed',$1)">] abstract onceWindowAllClosed: listener: (Event -> unit) -> App
  /// See onWindowAllClosed.
  [<Emit "$0.addListener('window-all-closed',$1)">] abstract addListenerWindowAllClosed: listener: (Event -> unit) -> App
  /// See onWindowAllClosed.
  [<Emit "$0.removeListener('window-all-closed',$1)">] abstract removeListenerWindowAllClosed: listener: (Event -> unit) -> App
  /// Emitted before the application starts closing its windows. Calling
  /// `event.preventDefault()` will prevent the default behavior, which is
  /// terminating the application.
  ///
  /// Note: If application quit was initiated by `autoUpdater.quitAndInstall()`,
  /// then `before-quit` is emitted *after* emitting the `close` event on all
  /// windows and closing them.
  ///
  /// Note: On Windows, this event will not be emitted if the app is closed due
  /// to a shutdown/restart of the system or a user logout.
  [<Emit "$0.on('before-quit',$1)">] abstract onBeforeQuit: listener: (Event -> unit) -> App
  /// See onBeforeQuit.
  [<Emit "$0.once('before-quit',$1)">] abstract onceBeforeQuit: listener: (Event -> unit) -> App
  /// See onBeforeQuit.
  [<Emit "$0.addListener('before-quit',$1)">] abstract addListenerBeforeQuit: listener: (Event -> unit) -> App
  /// See onBeforeQuit.
  [<Emit "$0.removeListener('before-quit',$1)">] abstract removeListenerBeforeQuit: listener: (Event -> unit) -> App
  /// Emitted when all windows have been closed and the application will quit.
  /// Calling `event.preventDefault()` will prevent the default behavior, which
  /// is terminating the application. See the description of `the
  /// window-all-closed` event for the differences between the `will-quit` and
  /// `window-all-closed` events.
  ///
  /// Note: On Windows, this event will not be emitted if the app is closed due
  /// to a shutdown/restart of the system or a user logout.
  [<Emit "$0.on('will-quit',$1)">] abstract onWillQuit: listener: (Event -> unit) -> App
  /// See onWillQuit.
  [<Emit "$0.once('will-quit',$1)">] abstract onceWillQuit: listener: (Event -> unit) -> App
  /// See onWillQuit.
  [<Emit "$0.addListener('will-quit',$1)">] abstract addListenerWillQuit: listener: (Event -> unit) -> App
  /// See onWillQuit.
  [<Emit "$0.removeListener('will-quit',$1)">] abstract removeListenerWillQuit: listener: (Event -> unit) -> App
  /// Emitted when the application is quitting.
  ///
  /// Note: On Windows, this event will not be emitted if the app is closed due
  /// to a shutdown/restart of the system or a user logout.
  ///
  /// Extra parameters:
  ///
  ///   - exitCode
  [<Emit "$0.on('quit',$1)">] abstract onQuit: listener: (Event -> int -> unit) -> App
  /// See onQuit.
  [<Emit "$0.once('quit',$1)">] abstract onceQuit: listener: (Event -> int -> unit) -> App
  /// See onQuit.
  [<Emit "$0.addListener('quit',$1)">] abstract addListenerQuit: listener: (Event -> int -> unit) -> App
  /// See onQuit.
  [<Emit "$0.removeListener('quit',$1)">] abstract removeListenerQuit: listener: (Event -> int -> unit) -> App
  /// [macOS] Emitted when the user wants to open a file with the application.
  /// The `open-file` event is usually emitted when the application is already
  /// open and the OS wants to reuse the application to open the file.
  /// `open-file` is also emitted when a file is dropped onto the dock and the
  /// application is not yet running. Make sure to listen for the `open-file`
  /// event very early in your application startup to handle this case (even
  /// before the `ready` event is emitted).
  ///
  /// You should call `event.preventDefault()` if you want to handle this event.
  ///
  /// On Windows, you have to parse `process.argv` (in the main process) to get
  /// the file path.
  ///
  /// Extra parameters:
  ///
  ///   - path
  [<Emit "$0.on('open-file',$1)">] abstract onOpenFile: listener: (Event -> string -> unit) -> App
  /// See onOpenFile.
  [<Emit "$0.once('open-file',$1)">] abstract onceOpenFile: listener: (Event -> string -> unit) -> App
  /// See onOpenFile.
  [<Emit "$0.addListener('open-file',$1)">] abstract addListenerOpenFile: listener: (Event -> string -> unit) -> App
  /// See onOpenFile.
  [<Emit "$0.removeListener('open-file',$1)">] abstract removeListenerOpenFile: listener: (Event -> string -> unit) -> App
  /// [macOS] Emitted when the user wants to open a URL with the application.
  /// Your application's Info.plist file must define the url scheme within the
  /// CFBundleURLTypes key, and set NSPrincipalClass to AtomApplication.
  ///
  /// You should call `event.preventDefault()` if you want to handle this event.
  ///
  /// Extra parameters:
  ///
  ///   - url
  [<Emit "$0.on('open-url',$1)">] abstract onOpenUrl: listener: (Event -> string -> unit) -> App
  /// See onOpenUrl.
  [<Emit "$0.once('open-url',$1)">] abstract onceOpenUrl: listener: (Event -> string -> unit) -> App
  /// See onOpenUrl.
  [<Emit "$0.addListener('open-url',$1)">] abstract addListenerOpenUrl: listener: (Event -> string -> unit) -> App
  /// See onOpenUrl.
  [<Emit "$0.removeListener('open-url',$1)">] abstract removeListenerOpenUrl: listener: (Event -> string -> unit) -> App
  /// [macOS] Emitted when the application is activated. Various actions can
  /// trigger this event, such as launching the application for the first time,
  /// attempting to re-launch the application when it's already running, or
  /// clicking on the application's dock or taskbar icon.
  ///
  /// Extra parameters:
  ///
  ///   - hasVisibleWindows
  [<Emit "$0.on('activate',$1)">] abstract onActivate: listener: (Event -> bool -> unit) -> App
  /// See onActivate.
  [<Emit "$0.once('activate',$1)">] abstract onceActivate: listener: (Event -> bool -> unit) -> App
  /// See onActivate.
  [<Emit "$0.addListener('activate',$1)">] abstract addListenerActivate: listener: (Event -> bool -> unit) -> App
  /// See onActivate.
  [<Emit "$0.removeListener('activate',$1)">] abstract removeListenerActivate: listener: (Event -> bool -> unit) -> App
  /// [macOS] Emitted during macOS Handoff when an activity from a different
  /// device wants to be resumed. You should call `event.preventDefault()` if
  /// you want to handle this event.
  ///
  /// A user activity can be continued only in an app that has the same
  /// developer Team ID as the activity's source app and that supports the
  /// activity's type. Supported activity types are specified in the app's
  /// Info.plist under the NSUserActivityTypes key.
  ///
  /// Extra parameters:
  ///
  ///   - type: A string identifying the activity. Maps to NSUserActivity.activityType.
  ///   - userInfo: Contains app-specific state stored by the activity on another device
  [<Emit "$0.on('continue-activity',$1)">] abstract onContinueActivity: listener: (Event -> string -> obj -> unit) -> App
  /// See onContinueActivity.
  [<Emit "$0.once('continue-activity',$1)">] abstract onceContinueActivity: listener: (Event -> string -> obj -> unit) -> App
  /// See onContinueActivity.
  [<Emit "$0.addListener('continue-activity',$1)">] abstract addListenerContinueActivity: listener: (Event -> string -> obj -> unit) -> App
  /// See onContinueActivity.
  [<Emit "$0.removeListener('continue-activity',$1)">] abstract removeListenerContinueActivity: listener: (Event -> string -> obj -> unit) -> App
  /// [macOS] Emitted during macOS Handoff before an activity from a different
  /// device wants to be resumed. You should call event.preventDefault() if you
  /// want to handle this event.
  ///
  /// Extra parameters:
  ///
  ///   - type: A string identifying the activity. Maps to NSUserActivity.activityType.
  [<Emit "$0.on('will-continue-activity',$1)">] abstract onWillContinueActivity: listener: (Event -> string -> unit) -> App
  /// See onWillContinueActivity.
  [<Emit "$0.once('will-continue-activity',$1)">] abstract onceWillContinueActivity: listener: (Event -> string -> unit) -> App
  /// See onWillContinueActivity.
  [<Emit "$0.addListener('will-continue-activity',$1)">] abstract addListenerWillContinueActivity: listener: (Event -> string -> unit) -> App
  /// See onWillContinueActivity.
  [<Emit "$0.removeListener('will-continue-activity',$1)">] abstract removeListenerWillContinueActivity: listener: (Event -> string -> unit) -> App
  /// [macOS] Emitted during macOS Handoff when an activity from a different
  /// device fails to be resumed.
  ///
  /// Extra parameters:
  ///
  ///   - type: A string identifying the activity. Maps to NSUserActivity.activityType.
  ///   - error: A string with the error's localized description
  [<Emit "$0.on('continue-activity-error',$1)">] abstract onContinueActivityError: listener: (Event -> string -> string -> unit) -> App
  /// See onContinueActivityError.
  [<Emit "$0.once('continue-activity-error',$1)">] abstract onceContinueActivityError: listener: (Event -> string -> string -> unit) -> App
  /// See onContinueActivityError.
  [<Emit "$0.addListener('continue-activity-error',$1)">] abstract addListenerContinueActivityError: listener: (Event -> string -> string -> unit) -> App
  /// See onContinueActivityError.
  [<Emit "$0.removeListener('continue-activity-error',$1)">] abstract removeListenerContinueActivityError: listener: (Event -> string -> string -> unit) -> App
  /// [macOS] Emitted during macOS Handoff after an activity from this device
  /// was successfully resumed on another one.
  ///
  /// Extra parameters:
  ///
  ///   - type: A string identifying the activity. Maps to NSUserActivity.activityType.
  ///   - userInfo: Contains app-specific state stored by the activity
  [<Emit "$0.on('activity-was-continued',$1)">] abstract onActivityWasContinued: listener: (Event -> string -> obj -> unit) -> App
  /// See onActivityWasContinued.
  [<Emit "$0.once('activity-was-continued',$1)">] abstract onceActivityWasContinued: listener: (Event -> string -> obj -> unit) -> App
  /// See onActivityWasContinued.
  [<Emit "$0.addListener('activity-was-continued',$1)">] abstract addListenerActivityWasContinued: listener: (Event -> string -> obj -> unit) -> App
  /// See onActivityWasContinued.
  [<Emit "$0.removeListener('activity-was-continued',$1)">] abstract removeListenerActivityWasContinued: listener: (Event -> string -> obj -> unit) -> App
  /// [macOS] Emitted when macOS Handoff is about to be resumed on another
  /// device. If you need to update the state to be transferred, you should call
  /// event.preventDefault() immediately, construct a new userInfo dictionary
  /// and call app.updateCurrentActiviy() in a timely manner. Otherwise, the
  /// operation will fail and `continue-activity-error` will be called.
  ///
  /// Extra parameters:
  ///
  ///   - type: A string identifying the activity. Maps to NSUserActivity.activityType. 
  ///   - userInfo: Contains app-specific state stored by the activity
  [<Emit "$0.on('update-activity-state',$1)">] abstract onUpdateActivityState: listener: (Event -> string -> obj option -> unit) -> App
  /// See onUpdateActivityState.
  [<Emit "$0.once('update-activity-state',$1)">] abstract onceUpdateActivityState: listener: (Event -> string -> obj option -> unit) -> App
  /// See onUpdateActivityState.
  [<Emit "$0.addListener('update-activity-state',$1)">] abstract addListenerUpdateActivityState: listener: (Event -> string -> obj option -> unit) -> App
  /// See onUpdateActivityState.
  [<Emit "$0.removeListener('update-activity-state',$1)">] abstract removeListenerUpdateActivityState: listener: (Event -> string -> obj option -> unit) -> App
  /// [macOS] Emitted when the user clicks the native macOS new tab button. The
  /// new tab button is only visible if the current BrowserWindow has a
  /// `tabbingIdentifier`
  [<Emit "$0.on('new-window-for-tab',$1)">] abstract onNewWindowForTab: listener: (Event -> unit) -> App
  /// See onNewWindowForTab.
  [<Emit "$0.once('new-window-for-tab',$1)">] abstract onceNewWindowForTab: listener: (Event -> unit) -> App
  /// See onNewWindowForTab.
  [<Emit "$0.addListener('new-window-for-tab',$1)">] abstract addListenerNewWindowForTab: listener: (Event -> unit) -> App
  /// See onNewWindowForTab.
  [<Emit "$0.removeListener('new-window-for-tab',$1)">] abstract removeListenerNewWindowForTab: listener: (Event -> unit) -> App
  /// Emitted when a browserWindow gets blurred.
  [<Emit "$0.on('browser-window-blur',$1)">] abstract onBrowserWindowBlur: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowBlur.
  [<Emit "$0.once('browser-window-blur',$1)">] abstract onceBrowserWindowBlur: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowBlur.
  [<Emit "$0.addListener('browser-window-blur',$1)">] abstract addListenerBrowserWindowBlur: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowBlur.
  [<Emit "$0.removeListener('browser-window-blur',$1)">] abstract removeListenerBrowserWindowBlur: listener: (Event -> BrowserWindow -> unit) -> App
  /// Emitted when a browserWindow gets focused.
  [<Emit "$0.on('browser-window-focus',$1)">] abstract onBrowserWindowFocus: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowFocus.
  [<Emit "$0.once('browser-window-focus',$1)">] abstract onceBrowserWindowFocus: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowFocus.
  [<Emit "$0.addListener('browser-window-focus',$1)">] abstract addListenerBrowserWindowFocus: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowFocus.
  [<Emit "$0.removeListener('browser-window-focus',$1)">] abstract removeListenerBrowserWindowFocus: listener: (Event -> BrowserWindow -> unit) -> App
  /// Emitted when a new browserWindow is created.
  [<Emit "$0.on('browser-window-created',$1)">] abstract onBrowserWindowCreated: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowCreated.
  [<Emit "$0.once('browser-window-created',$1)">] abstract onceBrowserWindowCreated: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowCreated.
  [<Emit "$0.addListener('browser-window-created',$1)">] abstract addListenerBrowserWindowCreated: listener: (Event -> BrowserWindow -> unit) -> App
  /// See onBrowserWindowCreated.
  [<Emit "$0.removeListener('browser-window-created',$1)">] abstract removeListenerBrowserWindowCreated: listener: (Event -> BrowserWindow -> unit) -> App
  /// Emitted when a new `webContents` is created.
  [<Emit "$0.on('web-contents-created',$1)">] abstract onWebContentsCreated: listener: (Event -> WebContents -> unit) -> App
  /// See onWebContentsCreated.
  [<Emit "$0.once('web-contents-created',$1)">] abstract onceWebContentsCreated: listener: (Event -> WebContents -> unit) -> App
  /// See onWebContentsCreated.
  [<Emit "$0.addListener('web-contents-created',$1)">] abstract addListenerWebContentsCreated: listener: (Event -> WebContents -> unit) -> App
  /// See onWebContentsCreated.
  [<Emit "$0.removeListener('web-contents-created',$1)">] abstract removeListenerWebContentsCreated: listener: (Event -> WebContents -> unit) -> App
  /// Emitted when failed to verify the `certificate` for `url`. To trust the
  /// certificate you should prevent the default behavior with
  /// event.preventDefault() and call `callback(true)`.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  ///   - url
  ///   - error: The error code
  ///   - certificate
  ///   - callback
  ///       - isTrusted: Whether to consider the certificate as trusted
  [<Emit "$0.on('certificate-error',$1)">] abstract onCertificateError: listener: (Event -> WebContents -> string -> string -> Certificate -> (bool -> unit) -> unit) -> App
  /// See onCertificateError.
  [<Emit "$0.once('certificate-error',$1)">] abstract onceCertificateError: listener: (Event -> WebContents -> string -> string -> Certificate -> (bool -> unit) -> unit) -> App
  /// See onCertificateError.
  [<Emit "$0.addListener('certificate-error',$1)">] abstract addListenerCertificateError: listener: (Event -> WebContents -> string -> string -> Certificate -> (bool -> unit) -> unit) -> App
  /// See onCertificateError.
  [<Emit "$0.removeListener('certificate-error',$1)">] abstract removeListenerCertificateError: listener: (Event -> WebContents -> string -> string -> Certificate -> (bool -> unit) -> unit) -> App
  /// Emitted when a client certificate is requested. The `url` corresponds to
  /// the navigation entry requesting the client certificate and `callback` can
  /// be called with an entry filtered from the list. Using
  /// event.preventDefault() prevents the application from using the first
  /// certificate from the store.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  ///   - url
  ///   - certificateList
  ///   - callback
  ///   - certificate
  [<Emit "$0.on('select-client-certificate',$1)">] abstract onSelectClientCertificate: listener: (Event -> WebContents -> string -> Certificate [] -> (Certificate -> unit) -> unit) -> App
  /// See onSelectClientCertificate.
  [<Emit "$0.once('select-client-certificate',$1)">] abstract onceSelectClientCertificate: listener: (Event -> WebContents -> string -> Certificate [] -> (Certificate -> unit) -> unit) -> App
  /// See onSelectClientCertificate.
  [<Emit "$0.addListener('select-client-certificate',$1)">] abstract addListenerSelectClientCertificate: listener: (Event -> WebContents -> string -> Certificate [] -> (Certificate -> unit) -> unit) -> App
  /// See onSelectClientCertificate.
  [<Emit "$0.removeListener('select-client-certificate',$1)">] abstract removeListenerSelectClientCertificate: listener: (Event -> WebContents -> string -> Certificate [] -> (Certificate -> unit) -> unit) -> App
  /// Emitted when webContents wants to do basic auth.
  ///
  /// The default behavior is to cancel all authentications. To override this
  /// you should prevent the default behavior with event.preventDefault() and
  /// call callback(username, password) with the credentials.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  ///   - request
  ///   - authInfo
  ///   - callback(username, password)
  [<Emit "$0.on('login',$1)">] abstract onLogin: listener: (Event -> WebContents -> LoginRequest -> AuthInfo -> (string -> string -> unit) -> unit) -> App
  /// See onLogin.
  [<Emit "$0.once('login',$1)">] abstract onceLogin: listener: (Event -> WebContents -> LoginRequest -> AuthInfo -> (string -> string -> unit) -> unit) -> App
  /// See onLogin.
  [<Emit "$0.addListener('login',$1)">] abstract addListenerLogin: listener: (Event -> WebContents -> LoginRequest -> AuthInfo -> (string -> string -> unit) -> unit) -> App
  /// See onLogin.
  [<Emit "$0.removeListener('login',$1)">] abstract removeListenerLogin: listener: (Event -> WebContents -> LoginRequest -> AuthInfo -> (string -> string -> unit) -> unit) -> App
  /// Emitted when the GPU process crashes or is killed.
  ///
  /// Extra parameters:
  ///
  ///   - killed
  [<Emit "$0.on('gpu-process-crashed',$1)">] abstract onGpuProcessCrashed: listener: (Event -> bool -> unit) -> App
  /// See onGpuProcessCrashed.
  [<Emit "$0.once('gpu-process-crashed',$1)">] abstract onceGpuProcessCrashed: listener: (Event -> bool -> unit) -> App
  /// See onGpuProcessCrashed.
  [<Emit "$0.addListener('gpu-process-crashed',$1)">] abstract addListenerGpuProcessCrashed: listener: (Event -> bool -> unit) -> App
  /// See onGpuProcessCrashed.
  [<Emit "$0.removeListener('gpu-process-crashed',$1)">] abstract removeListenerGpuProcessCrashed: listener: (Event -> bool -> unit) -> App
  /// Emitted when the renderer process of `webContents` crashes or is killed.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  ///   - killed
  [<Emit "$0.on('renderer-process-crashed',$1)">] abstract onRendererProcessCrashed: listener: (Event -> WebContents -> bool -> unit) -> App
  /// See onRendererProcessCrashed.
  [<Emit "$0.once('renderer-process-crashed',$1)">] abstract onceRendererProcessCrashed: listener: (Event -> WebContents -> bool -> unit) -> App
  /// See onRendererProcessCrashed.
  [<Emit "$0.addListener('renderer-process-crashed',$1)">] abstract addListenerRendererProcessCrashed: listener: (Event -> WebContents -> bool -> unit) -> App
  /// See onRendererProcessCrashed.
  [<Emit "$0.removeListener('renderer-process-crashed',$1)">] abstract removeListenerRendererProcessCrashed: listener: (Event -> WebContents -> bool -> unit) -> App
  /// [macOS, Windows] Emitted when Chrome's accessibility support changes. This
  /// event fires when assistive technologies, such as screen readers, are
  /// enabled or disabled. See
  /// https://www.chromium.org/developers/design-documents/accessibility for
  /// more details.
  ///
  /// Extra parameters:
  ///
  ///   - accessibilitySupportEnabled: true when Chrome's accessibility support is enabled, false otherwise.
  [<Emit "$0.on('accessibility-support-changed',$1)">] abstract onAccessibilitySupportChanged: listener: (Event -> bool -> unit) -> App
  /// See onAccessibilitySupportChanged.
  [<Emit "$0.once('accessibility-support-changed',$1)">] abstract onceAccessibilitySupportChanged: listener: (Event -> bool -> unit) -> App
  /// See onAccessibilitySupportChanged.
  [<Emit "$0.addListener('accessibility-support-changed',$1)">] abstract addListenerAccessibilitySupportChanged: listener: (Event -> bool -> unit) -> App
  /// See onAccessibilitySupportChanged.
  [<Emit "$0.removeListener('accessibility-support-changed',$1)">] abstract removeListenerAccessibilitySupportChanged: listener: (Event -> bool -> unit) -> App
  /// Emitted when Electron has created a new session.
  // TODO: verify that this has Event as first parameter, or only Session.
  [<Emit "$0.on('session-created',$1)">] abstract onSessionCreated: listener: (Event -> Session -> unit) -> App
  /// See onSessionCreated.
  [<Emit "$0.once('session-created',$1)">] abstract onceSessionCreated: listener: (Event -> Session -> unit) -> App
  /// See onSessionCreated.
  [<Emit "$0.addListener('session-created',$1)">] abstract addListenerSessionCreated: listener: (Event -> Session -> unit) -> App
  /// See onSessionCreated.
  [<Emit "$0.removeListener('session-created',$1)">] abstract removeListenerSessionCreated: listener: (Event -> Session -> unit) -> App
  /// This event will be emitted inside the primary instance of your application
  /// when a second instance has been executed and calls
  /// app.requestSingleInstanceLock(). Usually applications respond to this by
  /// making their primary window focused and non-minimized. This event is
  /// guaranteed to be emitted after the ready event of app gets emitted. Note:
  ///
  /// Note: Extra command line arguments might be added by Chromium, such as
  /// --original-process-start-time.
  ///
  /// Extra parameters:
  ///
  ///   - argv: The second instance's command line arguments
  ///   - workingDirectory: The second instance's working directory
  [<Emit "$0.on('second-instance',$1)">] abstract onSecondInstance: listener: (Event -> string [] -> string -> unit) -> App
  /// See onSecondInstance.
  [<Emit "$0.once('second-instance',$1)">] abstract onceSecondInstance: listener: (Event -> string [] -> string -> unit) -> App
  /// See onSecondInstance.
  [<Emit "$0.addListener('second-instance',$1)">] abstract addListenerSecondInstance: listener: (Event -> string [] -> string -> unit) -> App
  /// See onSecondInstance.
  [<Emit "$0.removeListener('second-instance',$1)">] abstract removeListenerSecondInstance: listener: (Event -> string [] -> string -> unit) -> App
  /// Emitted when desktopCapturer.getSources() is called in the renderer
  /// process of `webContents`. Calling event.preventDefault() will make it
  /// return empty sources.
  [<Emit "$0.on('desktop-capturer-get-sources',$1)">] abstract onDesktopCapturerGetSources: listener: (Event -> WebContents -> unit) -> App
  /// See onDesktopCapturerGetSources.
  [<Emit "$0.once('desktop-capturer-get-sources',$1)">] abstract onceDesktopCapturerGetSources: listener: (Event -> WebContents -> unit) -> App
  /// See onDesktopCapturerGetSources.
  [<Emit "$0.addListener('desktop-capturer-get-sources',$1)">] abstract addListenerDesktopCapturerGetSources: listener: (Event -> WebContents -> unit) -> App
  /// See onDesktopCapturerGetSources.
  [<Emit "$0.removeListener('desktop-capturer-get-sources',$1)">] abstract removeListenerDesktopCapturerGetSources: listener: (Event -> WebContents -> unit) -> App
  /// Emitted when remote.require() is called in the renderer process of
  /// `webContents`. Calling event.preventDefault() will prevent the module from
  /// being returned. Custom value can be returned by setting
  /// `event.returnValue`.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  ///   - moduleName
  [<Emit "$0.on('remote-require',$1)">] abstract onRemoteRequire: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteRequire.
  [<Emit "$0.once('remote-require',$1)">] abstract onceRemoteRequire: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteRequire.
  [<Emit "$0.addListener('remote-require',$1)">] abstract addListenerRemoteRequire: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteRequire.
  [<Emit "$0.removeListener('remote-require',$1)">] abstract removeListenerRemoteRequire: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// Emitted when remote.getGlobal() is called in the renderer process of
  /// `webContents`. Calling event.preventDefault() will prevent the global from
  /// being returned. Custom value can be returned by setting
  /// `event.returnValue`.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  ///   - globalName
  [<Emit "$0.on('remote-get-global',$1)">] abstract onRemoteGetGlobal: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteGetGlobal.
  [<Emit "$0.once('remote-get-global',$1)">] abstract onceRemoteGetGlobal: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteGetGlobal.
  [<Emit "$0.addListener('remote-get-global',$1)">] abstract addListenerRemoteGetGlobal: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteGetGlobal.
  [<Emit "$0.removeListener('remote-get-global',$1)">] abstract removeListenerRemoteGetGlobal: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// Emitted when remote.getBuiltin() is called in the renderer process of
  /// `webContents`. Calling event.preventDefault() will prevent the module from
  /// being returned. Custom value can be returned by setting
  /// `event.returnValue`.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  ///   - moduleName
  [<Emit "$0.on('remote-get-builtin',$1)">] abstract onRemoteGetBuiltin: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteGetBuiltin.
  [<Emit "$0.once('remote-get-builtin',$1)">] abstract onceRemoteGetBuiltin: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteGetBuiltin.
  [<Emit "$0.addListener('remote-get-builtin',$1)">] abstract addListenerRemoteGetBuiltin: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// See onRemoteGetBuiltin.
  [<Emit "$0.removeListener('remote-get-builtin',$1)">] abstract removeListenerRemoteGetBuiltin: listener: (ReturnValueEvent -> WebContents -> string -> unit) -> App
  /// Emitted when remote.getCurrentWindow() is called in the renderer process
  /// of `webContents`. Calling event.preventDefault() will prevent the object
  /// from being returned. Custom value can be returned by setting
  /// `event.returnValue`.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  [<Emit "$0.on('remote-get-current-window',$1)">] abstract onRemoteGetCurrentWindow: listener: (ReturnValueEvent -> WebContents -> unit) -> App
  /// See onRemoteGetCurrentWindow.
  [<Emit "$0.once('remote-get-current-window',$1)">] abstract onceRemoteGetCurrentWindow: listener: (ReturnValueEvent -> WebContents -> unit) -> App
  /// See onRemoteGetCurrentWindow.
  [<Emit "$0.addListener('remote-get-current-window',$1)">] abstract addListenerRemoteGetCurrentWindow: listener: (ReturnValueEvent -> WebContents -> unit) -> App
  /// See onRemoteGetCurrentWindow.
  [<Emit "$0.removeListener('remote-get-current-window',$1)">] abstract removeListenerRemoteGetCurrentWindow: listener: (ReturnValueEvent -> WebContents -> unit) -> App
  /// Emitted when remote.getCurrentWebContents() is called in the renderer
  /// process of `webContents`. Calling event.preventDefault() will prevent the
  /// object from being returned. Custom value can be returned by setting
  /// `event.returnValue`.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  [<Emit "$0.on('remote-get-current-web-contents',$1)">] abstract onRemoteGetCurrentWebContents: listener: (ReturnValueEvent -> WebContents -> unit) -> App
  /// See onRemoteGetCurrentWebContents.
  [<Emit "$0.once('remote-get-current-web-contents',$1)">] abstract onceRemoteGetCurrentWebContents: listener: (ReturnValueEvent -> WebContents -> unit) -> App
  /// See onRemoteGetCurrentWebContents.
  [<Emit "$0.addListener('remote-get-current-web-contents',$1)">] abstract addListenerRemoteGetCurrentWebContents: listener: (ReturnValueEvent -> WebContents -> unit) -> App
  /// See onRemoteGetCurrentWebContents.
  [<Emit "$0.removeListener('remote-get-current-web-contents',$1)">] abstract removeListenerRemoteGetCurrentWebContents: listener: (ReturnValueEvent -> WebContents -> unit) -> App
  /// Emitted when <webview>.getWebContents() is called in the renderer process
  /// of `webContents`. Calling event.preventDefault() will prevent the object
  /// from being returned. Custom value can be returned by setting
  /// `event.returnValue`.
  ///
  /// Extra parameters:
  ///
  ///   - webContents
  ///   - guestWebContents
  [<Emit "$0.on('remote-get-guest-web-contents',$1)">] abstract onRemoteGetGuestWebContents: listener: (ReturnValueEvent -> WebContents -> WebContents -> unit) -> App
  /// See onRemoteGetGuestWebContents.
  [<Emit "$0.once('remote-get-guest-web-contents',$1)">] abstract onceRemoteGetGuestWebContents: listener: (ReturnValueEvent -> WebContents -> WebContents -> unit) -> App
  /// See onRemoteGetGuestWebContents.
  [<Emit "$0.addListener('remote-get-guest-web-contents',$1)">] abstract addListenerRemoteGetGuestWebContents: listener: (ReturnValueEvent -> WebContents -> WebContents -> unit) -> App
  /// See onRemoteGetGuestWebContents.
  [<Emit "$0.removeListener('remote-get-guest-web-contents',$1)">] abstract removeListenerRemoteGetGuestWebContents: listener: (ReturnValueEvent -> WebContents -> WebContents -> unit) -> App
  /// Try to close all windows. The `before-quit` event will be emitted first.
  /// If all windows are successfully closed, the `will-quit` event will be
  /// emitted and by default the application will terminate.
  ///
  /// This method guarantees that all `beforeunload` and `unload` event handlers
  /// are correctly executed. It is possible that a window cancels the quitting
  /// by returning `false` in the `beforeunload` event handler.
  abstract quit: unit -> unit
  /// Exits immediately with `exitCode` (default 0). All windows will be closed
  /// immediately without asking the user, and the `before-quit` and `will-quit`
  /// events will not be emitted.
  abstract exit: ?exitCode: int -> unit
  /// Relaunches the app when current instance exits.
  ///
  /// By default, the new instance will use the same working directory and
  /// command line arguments with current instance. When options.args is
  /// specified, the args will be passed as command line arguments instead. When
  /// options.execPath is specified, the execPath will be executed for relaunch
  /// instead of current app.
  ///
  /// Note that this method does not quit the app when executed. You have to
  /// call app.quit or app.exit after calling app.relaunch to make the app
  /// restart.
  ///
  /// When app.relaunch is called multiple times, multiple instances will be
  /// started after current instance exited.
  abstract relaunch: ?options: RelaunchOptions -> unit
  /// Returns true if Electron has finished initializing, false otherwise.
  abstract isReady: unit -> bool
  /// Returns a promise that is fulfilled when Electron is initialized. May be
  /// used as a convenient alternative to checking app.isReady() and subscribing
  /// to the `ready` event if the app is not ready yet.
  abstract whenReady: unit -> Promise<unit>
  /// On Linux, focuses on the first visible window. On macOS, makes the
  /// application the active app. On Windows, focuses on the application's first
  /// window.
  abstract focus: unit -> unit
  /// [macOS] Hides all application windows without minimizing them.
  abstract hide: unit -> unit
  /// [macOS] Shows application windows after they were hidden. Does not
  /// automatically focus them.
  abstract show: unit -> unit
  /// Sets or creates a directory your app's logs which can then be manipulated
  /// with app.getPath() or app.setPath(pathName, newPath). `path` must be
  /// absolute.
  ///
  /// On macOS, this directory will be set by default to
  /// /Library/Logs/YourAppName, and on Linux and Windows it will be placed
  /// inside your userData directory.
  abstract setAppLogsPath: path: string -> unit
  /// Returns the current application directory.
  abstract getAppPath: unit -> string
  /// Returns the specified special directory or file. On failure, an `Error` is
  /// thrown.
  abstract getPath: name: AppPathName -> string
  /// Fetches a path's associated icon.
  ///
  /// On Windows, there a 2 kinds of icons: Icons associated with certain file
  /// extensions, like .mp3, .png, etc., and icons inside the file itself, like
  /// .exe, .dll, .ico.
  ///
  /// On Linux and macOS, icons depend on the application associated with file
  /// mime type.
  abstract getFileIcon: path: string * ?options: FileIconOptions -> Promise<NativeImage>
  /// Overrides the `path` to a special directory or file associated with
  /// `name`. If the path specifies a directory that does not exist, the
  /// directory will be created by this method. On failure an `Error` is thrown.
  ///
  /// By default, web pages' cookies and caches will be stored under the
  /// UserData directory. If you want to change this location, you have to
  /// override the UserData path before the `ready` event of the app module is
  /// emitted.
  abstract setPath: name: AppPathName * path: string -> unit
  /// Returns the version of the loaded application. If no version is found in
  /// the application's package.json file, the version of the current bundle or
  /// executable is returned.
  abstract getVersion: unit -> string
  /// Returns the current application's name, which is the name in the
  /// application's package.json file.
  ///
  /// Usually the `name` field of package.json is a short lower-cased name,
  /// according to the npm modules spec. You should usually also specify a
  /// `productName` field, which is your application's full capitalized name,
  /// and which will be preferred over `name` by Electron.
  abstract getName: unit -> string
  /// Overrides the current application's name.
  abstract setName: name: string -> unit
  /// Returns the current application locale. Possible return values are
  /// documented here: https://electronjs.org/docs/api/locales
  ///
  /// To set the locale, you'll want to use a command line switch at app
  /// startup, which may be found here:
  /// https://github.com/electron/electron/blob/master/docs/api/chrome-command-line-switches.md
  ///
  /// Note: When distributing your packaged app, you have to also ship the
  /// `locales` folder.
  ///
  /// Note: On Windows, you have to call it after the `ready` events gets
  /// emitted.
  abstract getLocale: unit -> string
  /// returns the operating system's locale two-letter ISO 3166 country code.
  /// The value is taken from native OS APIs.
  ///
  /// Note: When unable to detect locale country code, it returns empty string.
  abstract getLocaleCountryCode: unit -> string
  /// [macOS, Windows] Adds `path` to the recent documents list. This list is
  /// managed by the OS. On Windows, you can visit the list from the task bar,
  /// and on macOS, you can visit it from dock menu.
  abstract addRecentDocument: path: string -> unit
  /// [macOS, Windows] Clears the recent documents list.
  abstract clearRecentDocuments: unit -> unit
  /// This method sets the current executable as the default handler for a
  /// protocol (aka URI scheme). It allows you to integrate your app deeper into
  /// the operating system. Once registered, all links with `your-protocol://`
  /// will be opened with the current executable. The whole link, including
  /// protocol, will be passed to your application as a parameter.
  ///
  /// `protocol` is the name of your protocol, without ://
  ///
  /// Returns a value indicating whether the call succeeded.
  ///
  /// On Windows, you can provide optional parameters path (the path to your
  /// executable, defaults to process.execPath), and args, an array of arguments
  /// to be passed to your executable when it launches.
  ///
  /// Note: On macOS, you can only register protocols that have been added to
  /// your app's info.plist, which can not be modified at runtime. You can
  /// however change the file with a simple text editor or script during build
  /// time. Please refer to Apple's documentation for details.
  ///
  /// Note: In a Windows Store environment (when packaged as an `appx`) this API
  /// will return `true` for all calls but the registry key it sets won't be
  /// accessible by other applications.  In order to register your Windows Store
  /// application as a default protocol handler you must declare the protocol in
  /// your manifest:
  /// https://docs.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-protocol
  ///
  /// The API uses the Windows Registry and LSSetDefaultHandlerForURLScheme
  /// internally.
  abstract setAsDefaultProtocolClient: protocol: string * ?path: string * ?args: string [] -> bool
  /// This method checks if the current executable as the default handler for a
  /// protocol (aka URI scheme). If so, it will remove the app as the default
  /// handler.
  ///
  /// See `setAsDefaultProtocolClient` for more details. 
  abstract removeAsDefaultProtocolClient: protocol: string * ?path: string * ?args: string [] -> bool
  /// Returns a value indicating whether the current executable is the default
  /// handler for a protocol (aka URI scheme).
  ///
  /// Note: On macOS, you can use this method to check if the app has been
  /// registered as the default protocol handler for a protocol. You can also
  /// verify this by checking
  /// ~/Library/Preferences/com.apple.LaunchServices.plist on the macOS machine.
  /// Please refer to Apple's documentation for details.
  ///
  /// See `setAsDefaultProtocolClient` for more details. 
  abstract isDefaultProtocolClient: protocol: string * ?path: string * ?args: string [] -> bool
  /// [Windows] Adds `tasks` to the Tasks category of the JumpList on Windows.
  ///
  /// Note: If you'd like to customize the Jump List even more, use
  /// app.setJumpList(categories) instead.
  abstract setUserTasks: tasks: Task [] -> bool
  abstract getJumpListSettings: unit -> JumpListSettings
  /// Sets or removes a custom Jump List for the application.
  ///
  /// If categories is `null` the previously set custom Jump List (if any) will
  /// be replaced by the standard Jump List for the app (managed by Windows).
  ///
  /// Note: If a JumpListCategory object has neither the `type` nor the `name`
  /// property set then its `type` is assumed to be `tasks`. If the `name`
  /// property is set but the `type` property is omitted then the `type` is
  /// assumed to be `custom`.
  ///
  /// Note: Users can remove items from custom categories, and Windows will not
  /// allow a removed item to be added back into a custom category until after
  /// the next successful call to app.setJumpList(categories). Any attempt to
  /// re-add a removed item to a custom category earlier than that will result
  /// in the entire custom category being omitted from the Jump List. The list
  /// of removed items can be obtained using app.getJumpListSettings().
  abstract setJumpList: categories: JumpListCategory [] -> SetJumpListResult
  /// This method makes your application a Single Instance Application - instead
  /// of allowing multiple instances of your app to run, this will ensure that
  /// only a single instance of your app is running, and other instances signal
  /// this instance and exit.
  ///
  /// The return value of this method indicates whether or not this instance of
  /// your application successfully obtained the lock. If it failed to obtain
  /// the lock, you can assume that another instance of your application is
  /// already running with the lock and exit immediately.
  ///
  /// I.e. This method returns true if your process is the primary instance of
  /// your application and your app should continue loading. It returns false if
  /// your process should immediately quit as it has sent its parameters to
  /// another instance that has already acquired the lock.
  ///
  /// On macOS, the system enforces single instance automatically when users try
  /// to open a second instance of your app in Finder, and the `open-file` and
  /// `open-url` events will be emitted for that. However when users start your
  /// app in command line, the system's single instance mechanism will be
  /// bypassed, and you have to use this method to ensure single instance.
  abstract requestSingleInstanceLock: unit -> bool
  /// This method returns whether or not this instance of your app is currently
  /// holding the single instance lock. You can request the lock with
  /// app.requestSingleInstanceLock() and release with
  /// app.releaseSingleInstanceLock()
  abstract hasSingleInstanceLock: unit -> bool
  /// Releases all locks that were created by requestSingleInstanceLock. This
  /// will allow multiple instances of the application to once again run side by
  /// side.
  abstract releaseSingleInstanceLock: unit -> unit
  /// <summary>
  ///   [macOS] Creates an NSUserActivity and sets it as the current activity.
  ///   The activity is eligible for Handoff to another device afterward.
  /// </summary>
  /// <param name="type">
  ///   Uniquely identifies the activity. Maps to NSUserActivity.activityType.
  /// </param>
  /// <param name="userInfo">
  ///   App-specific state to store for use by another device.
  /// </param>
  /// <param name="webpageURL">
  ///   The web page to load in a browser if no suitable app is installed on the
  ///   resuming device. The scheme must be http or https.
  /// </param>
  abstract setUserActivity: ``type``: string * userInfo: obj option * ?webpageURL: string -> unit
  /// [macOS] Returns the type of the currently running activity.
  abstract getCurrentActivityType: unit -> string
  /// <summary>[macOS] Invalidates the current Handoff user activity.</summary>
  ///
  /// <param name="type">
  ///   Uniquely identifies the activity. Maps to NSUserActivity.activityType
  /// </param>
  abstract invalidateCurrentActivity: ``type``: string -> unit
  /// <summary>
  ///   [macOS] Updates the current activity if its type matches `type`, merging
  ///   the entries from `userInfo` into its current `userInfo` dictionary.
  /// </summary>
  /// <param name="type">
  ///   Uniquely identifies the activity. Maps to NSUserActivity.activityType.
  /// </param>
  /// <param name="userInfo">
  ///   App-specific state to store for use by another device.
  /// </param>
  abstract updateCurrentActivity: ``type``: string * userInfo: obj option -> unit
  /// Changes the Application User Model ID to `id`. More info:
  /// https://docs.microsoft.com/en-us/windows/desktop/shell/appids
  abstract setAppUserModelId: id: string -> unit
  /// [Linux] Imports the certificate in pkcs12 format into the platform
  /// certificate store. `callback` is called with the result of import
  /// operation. A value of `0` indicates success while any other value
  /// indicates failure according to Chromium net_error_list:
  /// https://cs.chromium.org/chromium/src/net/base/net_error_list.h
  abstract importCertificate: options: ImportCertificateOptions * callback: (int -> unit) -> unit
  /// Disables hardware acceleration for current app. This method can only be
  /// called before app is ready.
  abstract disableHardwareAcceleration: unit -> unit
  /// By default, Chromium disables 3D APIs (e.g. WebGL) until restart on a per
  /// domain basis if the GPU processes crashes too frequently. This function
  /// disables that behavior. This method can only be called before app is
  /// ready.
  abstract disableDomainBlockingFor3DAPIs: unit -> unit
  /// Returns objects that correspond to memory and CPU usage statistics of all
  /// the processes associated with the app.
  abstract getAppMetrics: unit -> ProcessMetric []
  /// Returns the Graphics Feature Status from chrome://gpu/.
  abstract getGPUFeatureStatus: unit -> GPUFeatureStatus
  /// For GpuInfoType.Complete, returns an object containing all the GPU
  /// Information in chromium's GPUInfo object. This includes the version and
  /// driver information that's shown on chrome://gpu page. For
  /// GpuInfoType.Basic, returns a subset of the complete info. Using Basic
  /// should be preferred if only basic information like vendorId or driverId is
  /// needed.
  abstract getGPUInfo: infoType: GpuInfoType -> Promise<obj>
  /// [Linux, macOS] Sets the counter badge for current app. Setting the count
  /// to 0 will hide the badge.
  ///
  /// On macOS, it shows on the dock icon. On Linux, it only works for Unity
  /// launcher.
  ///
  /// Note: Unity launcher requires the existence of a .desktop file to work.
  /// For more information please read Desktop Environment Integration:
  /// https://electronjs.org/docs/tutorial/desktop-environment-integration#unity-launcher
  abstract setBadgeCount: count: int -> bool
  /// Returns the current value displayed in the counter badge.
  abstract getBadgeCount: unit -> int
  /// [Linux] Whether the current desktop environment is Unity launcher.
  abstract isUnityRunning: unit -> bool
  /// [macOS, Windows] If you provided path and args options to
  /// app.setLoginItemSettings, then you need to pass the same arguments here
  /// for openAtLogin to be set correctly.
  abstract getLoginItemSettings: ?options: GetLoginItemSettingsOptions -> LoginItemSettings
  /// Set the app's login item settings.
  ///
  /// To work with Electron's autoUpdater on Windows, which uses Squirrel,
  /// you'll want to set the launch path to Update.exe, and pass arguments that
  /// specify your application name.
  abstract setLoginItemSettings: settings: SetLoginItemSettings -> unit
  /// `true` if Chrome's accessibility support is enabled, `false` otherwise.
  /// This property will be `true` if the use of assistive technologies, such as
  /// screen readers, has been detected. Setting this property to `true`
  /// manually enables Chrome's accessibility support, allowing developers to
  /// expose accessibility switch to users in application settings.
  ///
  /// See Chromium's accessibility docs for more details. Disabled by default.
  ///
  /// This API must be called after the `ready` event is emitted.
  ///
  /// Note: Rendering accessibility tree can significantly affect the
  /// performance of your app. It should not be enabled by default.
  abstract accessibilitySupportEnabled: bool with get, set
  /// [macOS, Linux] Show the app's about panel options. These options can be
  /// overridden with app.setAboutPanelOptions(options).
  abstract showAboutPanel: unit -> unit
  /// [macOS, Linux] Set the about panel options. This will override the values
  /// defined in the app's .plist file on MacOS. See the Apple docs for more
  /// details. On Linux, values must be set in order to be shown; there are no
  /// defaults.
  abstract setAboutPanelOptions: options: AboutPanelOptions -> unit
  /// Returns a value indicating whether or not the current OS version allows
  /// for native emoji pickers.
  abstract isEmojiPanelSupported: unit -> bool
  /// [macOS, Windows] Show the platform's native emoji picker.
  abstract showEmojiPanel: unit -> unit
  /// [macOS (Mac App Store)] Start accessing a security scoped resource. With
  /// this method Electron applications that are packaged for the Mac App Store
  /// may reach outside their sandbox to access files chosen by the user. See
  /// Apple's documentation for a description of how this system works.
  ///
  /// Note: The returned function MUST be called once you have finished
  /// accessing the security scoped file. If you do not remember to stop
  /// accessing the bookmark, kernel resources will be leaked and your app will
  /// lose its ability to reach outside the sandbox completely, until your app
  /// is restarted.
  abstract startAccessingSecurityScopedResource: bookmarkData: string -> (unit -> unit)
  abstract commandLine: CommandLine with get, set
  /// [macOS, Windows] Enables full sandbox mode on the app. This method can
  /// only be called before app is ready.
  abstract enableSandbox: unit -> unit
  /// [macOS] Indicates whether the application is currently running from the
  /// systems Application folder. Use in combination with
  /// app.moveToApplicationsFolder().
  abstract isInApplicationsFolder: unit -> bool
  /// [macOS] Returns a value indicating whether the move was successful. Please
  /// note that if the move is successful, your application will quit and
  /// relaunch.
  ///
  /// No confirmation dialog will be presented by default. If you wish to allow
  /// the user to confirm the operation, you may do so using the dialog API.
  ///
  /// Note: This method throws errors if anything other than the user causes the
  /// move to fail. For instance if the user cancels the authorization dialog,
  /// this method returns false. If we fail to perform the copy, then this
  /// method will throw an error. The message in the error should be informative
  /// and tell you exactly what went wrong
  abstract moveToApplicationsFolder: unit -> bool
  abstract dock: Dock with get, set
  /// Gets or sets the application menu on macOS, and each window's top menu on
  /// Windows and Linux.
  ///
  /// On Windows and Linux, you can use a `&` in the top-level item name to
  /// indicate which letter should get a generated accelerator. For example,
  /// using &File for the file menu would result in a generated Alt-F
  /// accelerator that opens the associated menu. The indicated character in the
  /// button label gets an underline. The & character is not displayed on the
  /// button label.
  ///
  /// Passing None will suppress the default menu. On Windows and Linux, this
  /// has the additional effect of removing the menu bar from the window.
  ///
  /// Note: The default menu will be created automatically if the app does not
  /// set one. It contains standard items such as File, Edit, View, Window and
  /// Help.
  ///
  /// Note: The Menu instance doesn't support dynamic addition or removal of
  /// menu items. Instance properties can still be dynamically modified.
  abstract applicationMenu: Menu option with get, set
  /// The user agent string Electron will use as a global fallback.
  ///
  /// This is the user agent that will be used when no user agent is set at the
  /// webContents or session level.  Useful for ensuring your entire app has the
  /// same user agent. Set to a custom value as early as possible in your app's
  /// initialization to ensure that your overridden value is used.
  abstract userAgentFallback: string with get, set
  /// Returns true if the app is packaged, false otherwise. For many apps, this
  /// property can be used to distinguish development and production
  /// environments.
  abstract isPackaged: bool with get, set
  /// A `Boolean` which when `true` disables the overrides that Electron has in
  /// place to ensure renderer processes are restarted on every navigation. The
  /// current default value for this property is `false`.
  ///
  /// The intention is for these overrides to become disabled by default and
  /// then at some point in the future this property will be removed. This
  /// property impacts which native modules you can use in the renderer process.
  /// For more information on the direction Electron is going with renderer
  /// process restarts and usage of native modules in the renderer process
  /// please check out this tracking issue:
  /// https://github.com/electron/electron/issues/18397
  abstract allowRendererProcessReuse: bool with get, set

type AutoUpdater =
  inherit EventEmitter<AutoUpdater>
  /// Emitted when there is an error while updating.
  [<Emit "$0.on('error',$1)">] abstract onError: listener: (Event -> Error -> unit) -> AutoUpdater
  /// See onError.
  [<Emit "$0.once('error',$1)">] abstract onceError: listener: (Event -> Error -> unit) -> AutoUpdater
  /// See onError.
  [<Emit "$0.addListener('error',$1)">] abstract addListenerError: listener: (Event -> Error -> unit) -> AutoUpdater
  /// See onError.
  [<Emit "$0.removeListener('error',$1)">] abstract removeListenerError: listener: (Event -> Error -> unit) -> AutoUpdater
  /// Emitted when checking if an update exists has started.
  [<Emit "$0.on('checking-for-update',$1)">] abstract onCheckingForUpdate: listener: (Event -> unit) -> AutoUpdater
  /// See onCheckingForUpdate.
  [<Emit "$0.once('checking-for-update',$1)">] abstract onceCheckingForUpdate: listener: (Event -> unit) -> AutoUpdater
  /// See onCheckingForUpdate.
  [<Emit "$0.addListener('checking-for-update',$1)">] abstract addListenerCheckingForUpdate: listener: (Event -> unit) -> AutoUpdater
  /// See onCheckingForUpdate.
  [<Emit "$0.removeListener('checking-for-update',$1)">] abstract removeListenerCheckingForUpdate: listener: (Event -> unit) -> AutoUpdater
  /// Emitted when there is an available update. The update is downloaded
  /// automatically.
  [<Emit "$0.on('update-available',$1)">] abstract onUpdateAvailable: listener: (Event -> unit) -> AutoUpdater
  /// See onUpdateAvailable.
  [<Emit "$0.once('update-available',$1)">] abstract onceUpdateAvailable: listener: (Event -> unit) -> AutoUpdater
  /// See onUpdateAvailable.
  [<Emit "$0.addListener('update-available',$1)">] abstract addListenerUpdateAvailable: listener: (Event -> unit) -> AutoUpdater
  /// See onUpdateAvailable.
  [<Emit "$0.removeListener('update-available',$1)">] abstract removeListenerUpdateAvailable: listener: (Event -> unit) -> AutoUpdater
  /// Emitted when there is no available update.
  [<Emit "$0.on('update-not-available',$1)">] abstract onUpdateNotAvailable: listener: (Event -> unit) -> AutoUpdater
  /// See onUpdateNotAvailable.
  [<Emit "$0.once('update-not-available',$1)">] abstract onceUpdateNotAvailable: listener: (Event -> unit) -> AutoUpdater
  /// See onUpdateNotAvailable.
  [<Emit "$0.addListener('update-not-available',$1)">] abstract addListenerUpdateNotAvailable: listener: (Event -> unit) -> AutoUpdater
  /// See onUpdateNotAvailable.
  [<Emit "$0.removeListener('update-not-available',$1)">] abstract removeListenerUpdateNotAvailable: listener: (Event -> unit) -> AutoUpdater
  /// Emitted when an update has been downloaded.
  ///
  /// On Windows only `releaseName` is available.
  ///
  /// Note: It is not strictly necessary to handle this event. A successfully
  /// downloaded update will still be applied the next time the application
  /// starts.
  ///
  /// Extra parameters:
  ///
  ///   - releaseNotes
  ///   - releaseName
  ///   - releaseDate
  ///   - updateUrl
  [<Emit "$0.on('update-downloaded',$1)">] abstract onUpdateDownloaded: listener: (Event -> string -> string -> DateTime -> string -> unit) -> AutoUpdater
  /// See onUpdateDownloaded.
  [<Emit "$0.once('update-downloaded',$1)">] abstract onceUpdateDownloaded: listener: (Event -> string -> string -> DateTime -> string -> unit) -> AutoUpdater
  /// See onUpdateDownloaded.
  [<Emit "$0.addListener('update-downloaded',$1)">] abstract addListenerUpdateDownloaded: listener: (Event -> string -> string -> DateTime -> string -> unit) -> AutoUpdater
  /// See onUpdateDownloaded.
  [<Emit "$0.removeListener('update-downloaded',$1)">] abstract removeListenerUpdateDownloaded: listener: (Event -> string -> string -> DateTime -> string -> unit) -> AutoUpdater
  /// This event is emitted after a user calls quitAndInstall().
  ///
  /// When this API is called, the `before-quit` event is not emitted before all
  /// windows are closed. As a result you should listen to this event if you
  /// wish to perform actions before the windows are closed while a process is
  /// quitting, as well as listening to `before-quit`.
  [<Emit "$0.on('before-quit-for-update',$1)">] abstract onBeforeQuitForUpdate: listener: (Event -> unit) -> AutoUpdater
  /// See onBeforeQuitForUpdate.
  [<Emit "$0.once('before-quit-for-update',$1)">] abstract onceBeforeQuitForUpdate: listener: (Event -> unit) -> AutoUpdater
  /// See onBeforeQuitForUpdate.
  [<Emit "$0.addListener('before-quit-for-update',$1)">] abstract addListenerBeforeQuitForUpdate: listener: (Event -> unit) -> AutoUpdater
  /// See onBeforeQuitForUpdate.
  [<Emit "$0.removeListener('before-quit-for-update',$1)">] abstract removeListenerBeforeQuitForUpdate: listener: (Event -> unit) -> AutoUpdater
  /// Sets the url and initialize the auto updater.
  abstract setFeedURL: options: AutoOpdateFeedOptions -> unit
  /// Returns the current update feed URL.
  abstract getFeedURL: unit -> string
  /// Asks the server whether there is an update. You must call `setFeedURL`
  /// before using this API.
  abstract checkForUpdates: unit -> unit
  /// Restarts the app and installs the update after it has been downloaded. It
  /// should only be called after `update-downloaded` has been emitted.
  ///
  /// Under the hood calling autoUpdater.quitAndInstall() will close all
  /// application windows first, and automatically call app.quit() after all
  /// windows have been closed.
  ///
  /// Note: It is not strictly necessary to call this function to apply an
  /// update, as a successfully downloaded update will always be applied the
  /// next time the application starts.
  abstract quitAndInstall: unit -> unit

type BluetoothDevice =
  abstract deviceName: string with get, set
  abstract deviceId: string with get, set

/// A BrowserView can be used to embed additional web content into a
/// BrowserWindow. It is like a child window, except that it is positioned
/// relative to its owning window.
type BrowserView =
  inherit EventEmitter<BrowserView>
  /// A WebContents object owned by this view.
  abstract webContents: WebContents with get, set
  /// An integer representing the unique ID of the view.
  abstract id: int with get, set
  /// Force closing the view, the `unload` and `beforeunload` events won't be
  /// emitted for the web page. After you're done with a view, call this
  /// function in order to free memory and other resources as soon as possible.
  abstract destroy: unit -> unit
  /// Indicates whether the view is destroyed.
  abstract isDestroyed: unit -> bool
  /// Sets whether the view's height and/or width will grow and shrink together
  /// with the window.
  abstract setAutoResize: options: AutoResizeOptions -> unit
  /// Sets the background color. Accepted formats: #aarrggbb, #rrggbb, #argb,
  /// #rgb.
  abstract setBackgroundColor: color: string -> unit

type BrowserViewStatic =
  /// Instantiates a BrowserWindow.
  [<EmitConstructor>] abstract Create: ?options: BrowserViewOptions -> BrowserView
  /// Returns all opened BrowserViews.
  abstract getAllViews: unit -> BrowserView []
  /// Returns the BrowserView that owns the given webContents or None if the
  /// contents are not owned by a BrowserView.
  abstract fromWebContents: webContents: WebContents -> BrowserView option
  /// Returns the BrowserView with the given id.
  abstract fromId: id: int -> BrowserView option

[<StringEnum; RequireQualifiedAccess>]
type AlwaysOnTopLevel =
  | Normal
  | Floating
  | [<CompiledName "torn-off-menu">] TornOffMenu
  | [<CompiledName "modal-panel">] ModalPanel
  | [<CompiledName "main-menu">] MainMenu
  | Status
  | [<CompiledName "pop-up-menu">] PopUpMenu
  | [<CompiledName "screen-saver">] ScreenSaver

[<StringEnum; RequireQualifiedAccess>]
type VibrancyType =
  | [<CompiledName("appearance-based")>] AppearanceBased
  | Light
  | Dark
  | [<CompiledName("titlebar")>] TitleBar
  | Selection
  | Menu
  | Popover
  | Sidebar
  | [<CompiledName("medium-light")>] MediumLight
  | [<CompiledName("ultra-dark")>] UltraDark

[<StringEnum; RequireQualifiedAccess>]
type SwipeDirection =
  | Up
  | Right
  | Down
  | Left

type BrowserWindow =
  inherit EventEmitter<BrowserWindow>
  /// Emitted when the document changed its title, calling
  /// event.preventDefault() will prevent the native window's title from
  /// changing. `explicitSet` is false when title is synthesized from file url.
  ///
  /// Extra parameters:
  ///
  ///   - title
  ///   - explicitSet
  [<Emit "$0.on('page-title-updated',$1)">] abstract onPageTitleUpdated: listener: (Event -> string -> bool -> unit) -> BrowserWindow
  /// See onPageTitleUpdated.
  [<Emit "$0.once('page-title-updated',$1)">] abstract oncePageTitleUpdated: listener: (Event -> string -> bool -> unit) -> BrowserWindow
  /// See onPageTitleUpdated.
  [<Emit "$0.addListener('page-title-updated',$1)">] abstract addListenerPageTitleUpdated: listener: (Event -> string -> bool -> unit) -> BrowserWindow
  /// See onPageTitleUpdated.
  [<Emit "$0.removeListener('page-title-updated',$1)">] abstract removeListenerPageTitleUpdated: listener: (Event -> string -> bool -> unit) -> BrowserWindow
  /// Emitted when the window is going to be closed. It's emitted before the
  /// `beforeunload` and `unload` event of the DOM. Calling
  /// event.preventDefault() will cancel the close.
  ///
  /// Usually you would want to use the `beforeunload` handler to decide whether
  /// the window should be closed, which will also be called when the window is
  /// reloaded. In Electron, returning any value other than undefined would
  /// cancel the close.
  [<Emit "$0.on('close',$1)">] abstract onClose: listener: (Event -> unit) -> BrowserWindow
  /// See onClose.
  [<Emit "$0.once('close',$1)">] abstract onceClose: listener: (Event -> unit) -> BrowserWindow
  /// See onClose.
  [<Emit "$0.addListener('close',$1)">] abstract addListenerClose: listener: (Event -> unit) -> BrowserWindow
  /// See onClose.
  [<Emit "$0.removeListener('close',$1)">] abstract removeListenerClose: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window is closed. After you have received this event you
  /// should remove the reference to the window and avoid using it any more.
  [<Emit "$0.on('closed',$1)">] abstract onClosed: listener: (Event -> unit) -> BrowserWindow
  /// See onClosed.
  [<Emit "$0.once('closed',$1)">] abstract onceClosed: listener: (Event -> unit) -> BrowserWindow
  /// See onClosed.
  [<Emit "$0.addListener('closed',$1)">] abstract addListenerClosed: listener: (Event -> unit) -> BrowserWindow
  /// See onClosed.
  [<Emit "$0.removeListener('closed',$1)">] abstract removeListenerClosed: listener: (Event -> unit) -> BrowserWindow
  /// [Windows] Emitted when window session is going to end due to force
  /// shutdown or machine restart or session log off.
  [<Emit "$0.on('session-end',$1)">] abstract onSessionEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onSessionEnd.
  [<Emit "$0.once('session-end',$1)">] abstract onceSessionEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onSessionEnd.
  [<Emit "$0.addListener('session-end',$1)">] abstract addListenerSessionEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onSessionEnd.
  [<Emit "$0.removeListener('session-end',$1)">] abstract removeListenerSessionEnd: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the web page becomes unresponsive.
  [<Emit "$0.on('unresponsive',$1)">] abstract onUnresponsive: listener: (Event -> unit) -> BrowserWindow
  /// See onUnresponsive.
  [<Emit "$0.once('unresponsive',$1)">] abstract onceUnresponsive: listener: (Event -> unit) -> BrowserWindow
  /// See onUnresponsive.
  [<Emit "$0.addListener('unresponsive',$1)">] abstract addListenerUnresponsive: listener: (Event -> unit) -> BrowserWindow
  /// See onUnresponsive.
  [<Emit "$0.removeListener('unresponsive',$1)">] abstract removeListenerUnresponsive: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the unresponsive web page becomes responsive again.
  [<Emit "$0.on('responsive',$1)">] abstract onResponsive: listener: (Event -> unit) -> BrowserWindow
  /// See onResponsive.
  [<Emit "$0.once('responsive',$1)">] abstract onceResponsive: listener: (Event -> unit) -> BrowserWindow
  /// See onResponsive.
  [<Emit "$0.addListener('responsive',$1)">] abstract addListenerResponsive: listener: (Event -> unit) -> BrowserWindow
  /// See onResponsive.
  [<Emit "$0.removeListener('responsive',$1)">] abstract removeListenerResponsive: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window loses focus.
  [<Emit "$0.on('blur',$1)">] abstract onBlur: listener: (Event -> unit) -> BrowserWindow
  /// See onBlur.
  [<Emit "$0.once('blur',$1)">] abstract onceBlur: listener: (Event -> unit) -> BrowserWindow
  /// See onBlur.
  [<Emit "$0.addListener('blur',$1)">] abstract addListenerBlur: listener: (Event -> unit) -> BrowserWindow
  /// See onBlur.
  [<Emit "$0.removeListener('blur',$1)">] abstract removeListenerBlur: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window gains focus.
  [<Emit "$0.on('focus',$1)">] abstract onFocus: listener: (Event -> unit) -> BrowserWindow
  /// See onFocus.
  [<Emit "$0.once('focus',$1)">] abstract onceFocus: listener: (Event -> unit) -> BrowserWindow
  /// See onFocus.
  [<Emit "$0.addListener('focus',$1)">] abstract addListenerFocus: listener: (Event -> unit) -> BrowserWindow
  /// See onFocus.
  [<Emit "$0.removeListener('focus',$1)">] abstract removeListenerFocus: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window is shown.
  [<Emit "$0.on('show',$1)">] abstract onShow: listener: (Event -> unit) -> BrowserWindow
  /// See onShow.
  [<Emit "$0.once('show',$1)">] abstract onceShow: listener: (Event -> unit) -> BrowserWindow
  /// See onShow.
  [<Emit "$0.addListener('show',$1)">] abstract addListenerShow: listener: (Event -> unit) -> BrowserWindow
  /// See onShow.
  [<Emit "$0.removeListener('show',$1)">] abstract removeListenerShow: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window is hidden.
  [<Emit "$0.on('hide',$1)">] abstract onHide: listener: (Event -> unit) -> BrowserWindow
  /// See onHide.
  [<Emit "$0.once('hide',$1)">] abstract onceHide: listener: (Event -> unit) -> BrowserWindow
  /// See onHide.
  [<Emit "$0.addListener('hide',$1)">] abstract addListenerHide: listener: (Event -> unit) -> BrowserWindow
  /// See onHide.
  [<Emit "$0.removeListener('hide',$1)">] abstract removeListenerHide: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the web page has been rendered (while not being shown) and
  /// window can be displayed without a visual flash.
  [<Emit "$0.on('ready-to-show',$1)">] abstract onReadyToShow: listener: (Event -> unit) -> BrowserWindow
  /// See onReadyToShow.
  [<Emit "$0.once('ready-to-show',$1)">] abstract onceReadyToShow: listener: (Event -> unit) -> BrowserWindow
  /// See onReadyToShow.
  [<Emit "$0.addListener('ready-to-show',$1)">] abstract addListenerReadyToShow: listener: (Event -> unit) -> BrowserWindow
  /// See onReadyToShow.
  [<Emit "$0.removeListener('ready-to-show',$1)">] abstract removeListenerReadyToShow: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when window is maximized.
  [<Emit "$0.on('maximize',$1)">] abstract onMaximize: listener: (Event -> unit) -> BrowserWindow
  /// See onMaximize.
  [<Emit "$0.once('maximize',$1)">] abstract onceMaximize: listener: (Event -> unit) -> BrowserWindow
  /// See onMaximize.
  [<Emit "$0.addListener('maximize',$1)">] abstract addListenerMaximize: listener: (Event -> unit) -> BrowserWindow
  /// See onMaximize.
  [<Emit "$0.removeListener('maximize',$1)">] abstract removeListenerMaximize: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window exits from a maximized state.
  [<Emit "$0.on('unmaximize',$1)">] abstract onUnmaximize: listener: (Event -> unit) -> BrowserWindow
  /// See onUnmaximize.
  [<Emit "$0.once('unmaximize',$1)">] abstract onceUnmaximize: listener: (Event -> unit) -> BrowserWindow
  /// See onUnmaximize.
  [<Emit "$0.addListener('unmaximize',$1)">] abstract addListenerUnmaximize: listener: (Event -> unit) -> BrowserWindow
  /// See onUnmaximize.
  [<Emit "$0.removeListener('unmaximize',$1)">] abstract removeListenerUnmaximize: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window is minimized.
  [<Emit "$0.on('minimize',$1)">] abstract onMinimize: listener: (Event -> unit) -> BrowserWindow
  /// See onMinimize.
  [<Emit "$0.once('minimize',$1)">] abstract onceMinimize: listener: (Event -> unit) -> BrowserWindow
  /// See onMinimize.
  [<Emit "$0.addListener('minimize',$1)">] abstract addListenerMinimize: listener: (Event -> unit) -> BrowserWindow
  /// See onMinimize.
  [<Emit "$0.removeListener('minimize',$1)">] abstract removeListenerMinimize: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window is restored from a minimized state.
  [<Emit "$0.on('restore',$1)">] abstract onRestore: listener: (Event -> unit) -> BrowserWindow
  /// See onRestore.
  [<Emit "$0.once('restore',$1)">] abstract onceRestore: listener: (Event -> unit) -> BrowserWindow
  /// See onRestore.
  [<Emit "$0.addListener('restore',$1)">] abstract addListenerRestore: listener: (Event -> unit) -> BrowserWindow
  /// See onRestore.
  [<Emit "$0.removeListener('restore',$1)">] abstract removeListenerRestore: listener: (Event -> unit) -> BrowserWindow
  /// [macOS, Windows] Emitted before the window is resized. Calling
  /// event.preventDefault() will prevent the window from being resized.
  ///
  /// Note that this is only emitted when the window is being resized manually.
  /// Resizing the window with setBounds/setSize will not emit this event.
  ///
  /// Extra parameters:
  ///
  ///   - newBounds: Size the window is being resized to.
  [<Emit "$0.on('will-resize',$1)">] abstract onWillResize: listener: (Event -> Rectangle -> unit) -> BrowserWindow
  /// See onWillResize.
  [<Emit "$0.once('will-resize',$1)">] abstract onceWillResize: listener: (Event -> Rectangle -> unit) -> BrowserWindow
  /// See onWillResize.
  [<Emit "$0.addListener('will-resize',$1)">] abstract addListenerWillResize: listener: (Event -> Rectangle -> unit) -> BrowserWindow
  /// See onWillResize.
  [<Emit "$0.removeListener('will-resize',$1)">] abstract removeListenerWillResize: listener: (Event -> Rectangle -> unit) -> BrowserWindow
  /// Emitted after the window has been resized.
  [<Emit "$0.on('resize',$1)">] abstract onResize: listener: (Event -> unit) -> BrowserWindow
  /// See onResize.
  [<Emit "$0.once('resize',$1)">] abstract onceResize: listener: (Event -> unit) -> BrowserWindow
  /// See onResize.
  [<Emit "$0.addListener('resize',$1)">] abstract addListenerResize: listener: (Event -> unit) -> BrowserWindow
  /// See onResize.
  [<Emit "$0.removeListener('resize',$1)">] abstract removeListenerResize: listener: (Event -> unit) -> BrowserWindow
  /// [Windows] Emitted before the window is moved. Calling
  /// event.preventDefault() will prevent the window from being moved. Note that
  /// this is only emitted when the window is being resized manually. Resizing
  /// the window with setBounds/setSize will not emit this event.
  ///
  /// Extra parameters:
  ///
  ///   - newBounds: Location the window is being moved to.
  [<Emit "$0.on('will-move',$1)">] abstract onWillMove: listener: (Event -> Rectangle -> unit) -> BrowserWindow
  /// See onWillMove.
  [<Emit "$0.once('will-move',$1)">] abstract onceWillMove: listener: (Event -> Rectangle -> unit) -> BrowserWindow
  /// See onWillMove.
  [<Emit "$0.addListener('will-move',$1)">] abstract addListenerWillMove: listener: (Event -> Rectangle -> unit) -> BrowserWindow
  /// See onWillMove.
  [<Emit "$0.removeListener('will-move',$1)">] abstract removeListenerWillMove: listener: (Event -> Rectangle -> unit) -> BrowserWindow
  /// Emitted when the window is being moved to a new position.
  ///
  /// Note: On macOS this event is an alias of `moved`.
  [<Emit "$0.on('move',$1)">] abstract onMove: listener: (Event -> unit) -> BrowserWindow
  /// See onMove.
  [<Emit "$0.once('move',$1)">] abstract onceMove: listener: (Event -> unit) -> BrowserWindow
  /// See onMove.
  [<Emit "$0.addListener('move',$1)">] abstract addListenerMove: listener: (Event -> unit) -> BrowserWindow
  /// See onMove.
  [<Emit "$0.removeListener('move',$1)">] abstract removeListenerMove: listener: (Event -> unit) -> BrowserWindow
  /// [macOS] Emitted once when the window is moved to a new position.
  [<Emit "$0.on('moved',$1)">] abstract onMoved: listener: (Event -> unit) -> BrowserWindow
  /// See onMoved.
  [<Emit "$0.once('moved',$1)">] abstract onceMoved: listener: (Event -> unit) -> BrowserWindow
  /// See onMoved.
  [<Emit "$0.addListener('moved',$1)">] abstract addListenerMoved: listener: (Event -> unit) -> BrowserWindow
  /// See onMoved.
  [<Emit "$0.removeListener('moved',$1)">] abstract removeListenerMoved: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window enters a full-screen state.
  [<Emit "$0.on('enter-full-screen',$1)">] abstract onEnterFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onEnterFullScreen.
  [<Emit "$0.once('enter-full-screen',$1)">] abstract onceEnterFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onEnterFullScreen.
  [<Emit "$0.addListener('enter-full-screen',$1)">] abstract addListenerEnterFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onEnterFullScreen.
  [<Emit "$0.removeListener('enter-full-screen',$1)">] abstract removeListenerEnterFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window leaves a full-screen state.
  [<Emit "$0.on('leave-full-screen',$1)">] abstract onLeaveFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onLeaveFullScreen.
  [<Emit "$0.once('leave-full-screen',$1)">] abstract onceLeaveFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onLeaveFullScreen.
  [<Emit "$0.addListener('leave-full-screen',$1)">] abstract addListenerLeaveFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onLeaveFullScreen.
  [<Emit "$0.removeListener('leave-full-screen',$1)">] abstract removeListenerLeaveFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window enters a full-screen state triggered by HTML API.
  [<Emit "$0.on('enter-html-full-screen',$1)">] abstract onEnterHtmlFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onEnterHtmlFullScreen.
  [<Emit "$0.once('enter-html-full-screen',$1)">] abstract onceEnterHtmlFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onEnterHtmlFullScreen.
  [<Emit "$0.addListener('enter-html-full-screen',$1)">] abstract addListenerEnterHtmlFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onEnterHtmlFullScreen.
  [<Emit "$0.removeListener('enter-html-full-screen',$1)">] abstract removeListenerEnterHtmlFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// Emitted when the window leaves a full-screen state triggered by HTML API.
  [<Emit "$0.on('leave-html-full-screen',$1)">] abstract onLeaveHtmlFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onLeaveHtmlFullScreen.
  [<Emit "$0.once('leave-html-full-screen',$1)">] abstract onceLeaveHtmlFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onLeaveHtmlFullScreen.
  [<Emit "$0.addListener('leave-html-full-screen',$1)">] abstract addListenerLeaveHtmlFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// See onLeaveHtmlFullScreen.
  [<Emit "$0.removeListener('leave-html-full-screen',$1)">] abstract removeListenerLeaveHtmlFullScreen: listener: (Event -> unit) -> BrowserWindow
  /// [macOS] Emitted when the window is set or unset to show always on top of
  /// other windows.
  ///
  /// Extra parameters:
  ///
  ///   - isAlwaysOnTop
  [<Emit "$0.on('always-on-top-changed',$1)">] abstract onAlwaysOnTopChanged: listener: (Event -> bool -> unit) -> BrowserWindow
  /// See onAlwaysOnTopChanged.
  [<Emit "$0.once('always-on-top-changed',$1)">] abstract onceAlwaysOnTopChanged: listener: (Event -> bool -> unit) -> BrowserWindow
  /// See onAlwaysOnTopChanged.
  [<Emit "$0.addListener('always-on-top-changed',$1)">] abstract addListenerAlwaysOnTopChanged: listener: (Event -> bool -> unit) -> BrowserWindow
  /// See onAlwaysOnTopChanged.
  [<Emit "$0.removeListener('always-on-top-changed',$1)">] abstract removeListenerAlwaysOnTopChanged: listener: (Event -> bool -> unit) -> BrowserWindow
  /// [Windows, Linux] Emitted when an App Command is invoked. These are
  /// typically related to keyboard media keys or browser commands, as well as
  /// the "Back" button built into some mice on Windows. Commands are
  /// lowercased, underscores are replaced with hyphens, and the APPCOMMAND_
  /// prefix is stripped off. e.g. APPCOMMAND_BROWSER_BACKWARD is emitted as
  /// browser-backward. The following app commands are explicitly supported on
  /// Linux: browser-backward, browser-forward
  ///
  /// Extra parameters:
  ///
  ///   - command
  [<Emit "$0.on('app-command',$1)">] abstract onAppCommand: listener: (Event -> string -> unit) -> BrowserWindow
  /// See onAppCommand.
  [<Emit "$0.once('app-command',$1)">] abstract onceAppCommand: listener: (Event -> string -> unit) -> BrowserWindow
  /// See onAppCommand.
  [<Emit "$0.addListener('app-command',$1)">] abstract addListenerAppCommand: listener: (Event -> string -> unit) -> BrowserWindow
  /// See onAppCommand.
  [<Emit "$0.removeListener('app-command',$1)">] abstract removeListenerAppCommand: listener: (Event -> string -> unit) -> BrowserWindow
  /// [macOS] Emitted when scroll wheel event phase has begun.
  [<Emit "$0.on('scroll-touch-begin',$1)">] abstract onScrollTouchBegin: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchBegin.
  [<Emit "$0.once('scroll-touch-begin',$1)">] abstract onceScrollTouchBegin: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchBegin.
  [<Emit "$0.addListener('scroll-touch-begin',$1)">] abstract addListenerScrollTouchBegin: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchBegin.
  [<Emit "$0.removeListener('scroll-touch-begin',$1)">] abstract removeListenerScrollTouchBegin: listener: (Event -> unit) -> BrowserWindow
  /// [macOS] Emitted when scroll wheel event phase has ended.
  [<Emit "$0.on('scroll-touch-end',$1)">] abstract onScrollTouchEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchEnd.
  [<Emit "$0.once('scroll-touch-end',$1)">] abstract onceScrollTouchEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchEnd.
  [<Emit "$0.addListener('scroll-touch-end',$1)">] abstract addListenerScrollTouchEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchEnd.
  [<Emit "$0.removeListener('scroll-touch-end',$1)">] abstract removeListenerScrollTouchEnd: listener: (Event -> unit) -> BrowserWindow
  /// [macOS] Emitted when scroll wheel event phase filed upon reaching the edge
  /// of element.
  [<Emit "$0.on('scroll-touch-edge',$1)">] abstract onScrollTouchEdge: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchEdge.
  [<Emit "$0.once('scroll-touch-edge',$1)">] abstract onceScrollTouchEdge: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchEdge.
  [<Emit "$0.addListener('scroll-touch-edge',$1)">] abstract addListenerScrollTouchEdge: listener: (Event -> unit) -> BrowserWindow
  /// See onScrollTouchEdge.
  [<Emit "$0.removeListener('scroll-touch-edge',$1)">] abstract removeListenerScrollTouchEdge: listener: (Event -> unit) -> BrowserWindow
  /// [macOS] Emitted on 3-finger swipe.
  [<Emit "$0.on('swipe',$1)">] abstract onSwipe: listener: (Event -> SwipeDirection -> unit) -> BrowserWindow
  /// See onSwipe.
  [<Emit "$0.once('swipe',$1)">] abstract onceSwipe: listener: (Event -> SwipeDirection -> unit) -> BrowserWindow
  /// See onSwipe.
  [<Emit "$0.addListener('swipe',$1)">] abstract addListenerSwipe: listener: (Event -> SwipeDirection -> unit) -> BrowserWindow
  /// See onSwipe.
  [<Emit "$0.removeListener('swipe',$1)">] abstract removeListenerSwipe: listener: (Event -> SwipeDirection -> unit) -> BrowserWindow
  /// [macOS] Emitted when the window opens a sheet.
  [<Emit "$0.on('sheet-begin',$1)">] abstract onSheetBegin: listener: (Event -> unit) -> BrowserWindow
  /// See onSheetBegin.
  [<Emit "$0.once('sheet-begin',$1)">] abstract onceSheetBegin: listener: (Event -> unit) -> BrowserWindow
  /// See onSheetBegin.
  [<Emit "$0.addListener('sheet-begin',$1)">] abstract addListenerSheetBegin: listener: (Event -> unit) -> BrowserWindow
  /// See onSheetBegin.
  [<Emit "$0.removeListener('sheet-begin',$1)">] abstract removeListenerSheetBegin: listener: (Event -> unit) -> BrowserWindow
  /// [macOS] Emitted when the window has closed a sheet.
  [<Emit "$0.on('sheet-end',$1)">] abstract onSheetEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onSheetEnd.
  [<Emit "$0.once('sheet-end',$1)">] abstract onceSheetEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onSheetEnd.
  [<Emit "$0.addListener('sheet-end',$1)">] abstract addListenerSheetEnd: listener: (Event -> unit) -> BrowserWindow
  /// See onSheetEnd.
  [<Emit "$0.removeListener('sheet-end',$1)">] abstract removeListenerSheetEnd: listener: (Event -> unit) -> BrowserWindow
  /// [macOS] Emitted when the native new tab button is clicked.
  [<Emit "$0.on('new-window-for-tab',$1)">] abstract onNewWindowForTab: listener: (Event -> unit) -> BrowserWindow
  /// See onNewWindowForTab.
  [<Emit "$0.once('new-window-for-tab',$1)">] abstract onceNewWindowForTab: listener: (Event -> unit) -> BrowserWindow
  /// See onNewWindowForTab.
  [<Emit "$0.addListener('new-window-for-tab',$1)">] abstract addListenerNewWindowForTab: listener: (Event -> unit) -> BrowserWindow
  /// See onNewWindowForTab.
  [<Emit "$0.removeListener('new-window-for-tab',$1)">] abstract removeListenerNewWindowForTab: listener: (Event -> unit) -> BrowserWindow
  /// A WebContents object this window owns. All web page related events and
  /// operations will be done via it.
  abstract webContents: WebContents with get, set
  /// An integer representing the unique ID of the window.
  abstract id: int with get, set
  /// Force closing the window, the `unload` and `beforeunload` event won't be
  /// emitted for the web page, and `close` event will also not be emitted for
  /// this window, but it guarantees the `closed` event will be emitted.
  abstract destroy: unit -> unit
  /// Try to close the window. This has the same effect as a user manually
  /// clicking the close button of the window. The web page may cancel the close
  /// though (see the `close` event).
  abstract close: unit -> unit
  /// Focuses on the window.
  abstract focus: unit -> unit
  /// Removes focus from the window.
  abstract blur: unit -> unit
  /// Indicates whether the window is focused.
  abstract isFocused: unit -> bool
  /// Indicates whether the window is destroyed.
  abstract isDestroyed: unit -> bool
  /// Shows and gives focus to the window.
  abstract show: unit -> unit
  /// Shows the window but doesn't focus on it.
  abstract showInactive: unit -> unit
  /// Hides the window.
  abstract hide: unit -> unit
  /// Indicates whether the window is visible to the user.
  abstract isVisible: unit -> bool
  /// Indicates whether current window is a modal window.
  abstract isModal: unit -> bool
  /// Maximizes the window. This will also show (but not focus) the window if it
  /// isn't being displayed already.
  abstract maximize: unit -> unit
  /// Unmaximizes the window.
  abstract unmaximize: unit -> unit
  /// Indicates whether the window is maximized.
  abstract isMaximized: unit -> bool
  /// Minimizes the window. On some platforms the minimized window will be shown
  /// in the Dock.
  abstract minimize: unit -> unit
  /// Restores the window from minimized state to its previous state.
  abstract restore: unit -> unit
  /// Indicates whether the window is minimized.
  abstract isMinimized: unit -> bool
  /// Sets whether the window should be in fullscreen mode.
  abstract setFullScreen: flag: bool -> unit
  /// Indicates whether the window is in fullscreen mode.
  abstract isFullScreen: unit -> bool
  /// [macOS] Enters or leaves simple fullscreen mode.
  ///
  /// Simple fullscreen mode emulates the native fullscreen behavior found in
  /// versions of Mac OS X prior to Lion (10.7).
  abstract setSimpleFullScreen: flag: bool -> unit
  /// [macOS] Indicates whether the window is in simple (pre-Lion) fullscreen
  /// mode.
  abstract isSimpleFullScreen: unit -> bool
  /// Indicates whether the window is in normal state (not maximized, not
  /// minimized, not in fullscreen mode).
  abstract isNormal: unit -> bool
  /// <summary>
  ///   [macOS] This will make a window maintain an aspect ratio. The extra size
  ///   allows a developer to have space, specified in pixels, not included
  ///   within the aspect ratio calculations. This API already takes into
  ///   account the difference between a window's size and its content size.
  ///
  ///   Consider a normal window with an HD video player and associated
  ///   controls. Perhaps there are 15 pixels of controls on the left edge, 25
  ///   pixels of controls on the right edge and 50 pixels of controls below the
  ///   player. In order to maintain a 16:9 aspect ratio (standard aspect ratio
  ///   for HD @1920x1080) within the player itself we would call this function
  ///   with arguments of 16/9 and [ 40, 50 ]. The second argument doesn't care
  ///   where the extra width and height are within the content view--only that
  ///   they exist. Sum any extra width and height areas you have within the
  ///   overall content view.
  ///
  ///   Calling this function with a value of 0 will remove any previously set
  ///   aspect ratios.
  /// </summary>
  /// <param name="aspectRatio">
  ///   The aspect ratio to maintain for some portion of the content view.
  /// </param>
  /// <param name="ertraSize">
  ///   The extra size not to be included while maintaining the aspect ratio.
  /// </param>
  abstract setAspectRatio: aspectRatio: float * extraSize: Size -> unit
  /// Sets the background color of the window as a hexadecimal value, like
  /// #66CD00 or #FFF or #80FFFFFF (alpha is supported if transparent is true).
  /// Default is #FFF (white).
  abstract setBackgroundColor: backgroundColor: string -> unit
  /// <summary>
  ///   [macOS] Uses Quick Look to preview a file at a given path.
  /// </summary>
  /// <param name="path">
  ///   The absolute path to the file to preview with QuickLook. This is
  ///   important as Quick Look uses the file name and file extension on the
  ///   path to determine the content type of the file to open.
  /// </param>
  /// <param name="displayName">
  ///   The name of the file to display on the Quick Look modal view. This is
  ///   purely visual and does not affect the content type of the file. Defaults
  ///   to path.
  /// </param>
  abstract previewFile: path: string * ?displayName: string -> unit
  /// [macOS] Closes the currently open Quick Look panel.
  abstract closeFilePreview: unit -> unit
  /// Resizes and moves the window to the supplied bounds. Any properties that
  /// are not supplied will default to their current values. animate is macOS
  /// only.
  abstract setBounds: bounds: Rectangle * ?animate: bool -> unit
  abstract getBounds: unit -> Rectangle
  /// Resizes and moves the window's client area (e.g. the web page) to the
  /// supplied bounds. animate is macOS only.
  abstract setContentBounds: bounds: Rectangle * ?animate: bool -> unit
  abstract getContentBounds: unit -> Rectangle
  /// Returns a Rectangle that contains the window bounds of the normal state.
  ///
  /// Note: whatever the current state of the window : maximized, minimized or
  /// in fullscreen, this function always returns the position and size of the
  /// window in normal state. In normal state, getBounds and getNormalBounds
  /// returns the same Rectangle.
  abstract getNormalBounds: unit -> Rectangle
  /// Disable or enable the window.
  abstract setEnabled: enable: bool -> unit
  /// Resizes the window. If width or height are below any set minimum size
  /// constraints the window will snap to its minimum size. animate is macOS only.
  abstract setSize: width: int * height: int * ?animate: bool -> unit
  /// Returns the window's width and height.
  abstract getSize: unit -> int * int
  /// Resizes the window's client area (e.g. the web page) to width and height.
  /// animate is macOS only.
  abstract setContentSize: width: int * height: int * ?animate: bool -> unit
  /// Returns the window's client area's width and height.
  abstract getContentSize: unit -> int * int
  /// Sets the minimum size of window to width and height.
  abstract setMinimumSize: width: int * height: int -> unit
  /// Returns the window's minimum width and height.
  abstract getMinimumSize: unit -> int * int
  /// Sets the maximum size of window to width and height.
  abstract setMaximumSize: width: int * height: int -> unit
  /// Returns the window's maximum width and height.
  abstract getMaximumSize: unit -> int * int
  /// Sets whether the window can be manually resized by user.
  abstract setResizable: resizable: bool -> unit
  /// Indicates whether the window can be manually resized by user.
  abstract isResizable: unit -> bool
  /// [macOS, Windows] Sets whether the window can be moved by user. On Linux
  /// does nothing.
  abstract setMovable: movable: bool -> unit
  /// [macOS, Windows] Indicates whether the window can be moved by user. On
  /// Linux always returns true.
  abstract isMovable: unit -> bool
  /// [macOS, Windows] Sets whether the window can be manually minimized by
  /// user. On Linux does nothing.
  abstract setMinimizable: minimizable: bool -> unit
  /// [macOS, Windows] Indicates whether the window can be manually minimized by
  /// user. On Linux always returns true.
  abstract isMinimizable: unit -> bool
  /// [macOS, Windows] Sets whether the window can be manually maximized by
  /// user. On Linux does nothing.
  abstract setMaximizable: maximizable: bool -> unit
  /// [Windows, macOS] Indicates whether the window can be manually maximized by
  /// user. On Linux always returns true.
  abstract isMaximizable: unit -> bool
  /// Sets whether the maximize/zoom window button toggles fullscreen mode or
  /// maximizes the window.
  abstract setFullScreenable: fullscreenable: bool -> unit
  /// Indicates whether the maximize/zoom window button toggles fullscreen mode
  /// or maximizes the window.
  abstract isFullScreenable: unit -> bool
  /// [macOS, Windows] Sets whether the window can be manually closed by user.
  /// On Linux does nothing.
  abstract setClosable: closable: bool -> unit
  /// [macOS, Windows] Indicates whether the window can be manually closed by
  /// user. On Linux always returns true.
  abstract isClosable: unit -> bool
  /// Sets whether the window should show always on top of other windows. After
  /// setting this, the window is still a normal window, not a toolbox window
  /// which can not be focused on.
  abstract setAlwaysOnTop: flag: bool * ?level: AlwaysOnTopLevel * ?relativeLevel: int -> unit
  /// Indicates whether the window is always on top of other windows.
  abstract isAlwaysOnTop: unit -> bool
  /// Moves window to top(z-order) regardless of focus
  abstract moveTop: unit -> unit
  /// Moves window to the center of the screen.
  abstract center: unit -> unit
  /// Moves window to x and y. animate is macOS only.
  abstract setPosition: x: int * y: int * ?animate: bool -> unit
  /// Returns the window's current position.
  abstract getPosition: unit -> int * int
  /// Changes the title of native window.
  abstract setTitle: title: string -> unit
  /// Returns the title of the native window.
  ///
  /// Note: The title of the web page can be different from the title of the
  /// native window.
  abstract getTitle: unit -> string
  /// [macOS] Changes the attachment point for sheets on macOS. By default,
  /// sheets are attached just below the window frame, but you may want to
  /// display them beneath a HTML-rendered toolbar.
  abstract setSheetOffset: offsetY: float * ?offsetX: float -> unit
  /// Starts or stops flashing the window to attract user's attention.
  abstract flashFrame: flag: bool -> unit
  /// Makes the window not show in the taskbar.
  abstract setSkipTaskbar: skip: bool -> unit
  /// Enters or leaves the kiosk mode.
  abstract setKiosk: flag: bool -> unit
  /// Indicates whether the window is in kiosk mode.
  abstract isKiosk: unit -> bool
  /// The native type of the handle is HWND on Windows, NSView* on macOS, and
  /// Window (unsigned long) on Linux.
  abstract getNativeWindowHandle: unit -> Buffer
  /// [Windows] Hooks a windows message. The callback is called when the message
  /// is received in the WndProc.
  abstract hookWindowMessage: message: int * callback: (unit -> unit) -> unit  // TODO: is unit -> unit correct?
  /// [Windows] Indicates whether the message is hooked.
  abstract isWindowMessageHooked: message: int -> bool
  /// [Windows] Unhook the window message.
  abstract unhookWindowMessage: message: int -> unit
  /// [Windows] Unhooks all of the window messages.
  abstract unhookAllWindowMessages: unit -> unit
  /// [macOS] Sets the pathname of the file the window represents, and the icon
  /// of the file will show in window's title bar.
  abstract setRepresentedFilename: filename: string -> unit
  /// [macOS] Returns the pathname of the file the window represents.
  abstract getRepresentedFilename: unit -> string option
  /// [macOS] Specifies whether the window’s document has been edited, and the
  /// icon in title bar will become gray when set to true.
  abstract setDocumentEdited: edited: bool -> unit
  /// [macOS] Indicates whether the window's document has been edited.
  abstract isDocumentEdited: unit -> bool
  abstract focusOnWebView: unit -> unit
  abstract blurWebView: unit -> unit
  /// Captures a snapshot of the page within rect. Omitting rect will capture
  /// the whole visible page.
  abstract capturePage: ?rect: Rectangle -> Promise<NativeImage>
  /// Returns a promise that will resolve when the page has finished loading
  /// (see `did-finish-load`), and rejects if the page fails to load (see
  /// `did-fail-load`).
  ///
  /// Same as webContents.loadURL(url[, options]). The url can be a remote
  /// address (e.g. http://) or a path to a local HTML file using the file://
  /// protocol. To ensure that file URLs are properly formatted, it is
  /// recommended to use Node's url.format method.
  abstract loadURL: url: string * ?options: LoadURLOptions -> Promise<unit>
  /// Returns a promise that will resolve when the page has finished loading
  /// (see `did-finish-load`), and rejects if the page fails to load (see
  /// `did-fail-load`).
  ///
  /// Same as webContents.loadFile, filePath should be a path to an HTML file
  /// relative to the root of your application.  See the webContents docs for
  /// more information.
  abstract loadFile: filePath: string * ?options: LoadFileOptions -> Promise<unit>
  /// Same as webContents.reload.
  abstract reload: unit -> unit
  /// [Windows, Linux] Sets the menu as the window's menu bar.
  abstract setMenu: menu: Menu -> unit
  /// [Windows, Linux] Remove the window's menu bar.
  abstract removeMenu: unit -> unit
  /// Sets progress value in progress bar. Valid range is [0, 1.0].
  ///
  /// Remove progress bar when progress < 0; Change to indeterminate mode when
  /// progress > 1.
  ///
  /// On Linux platform, only supports Unity desktop environment, you need to
  /// specify the *.desktop file name to desktopName field in package.json. By
  /// default, it will assume app.getName().desktop.
  ///
  /// On Windows, a mode can be passed. If you call setProgressBar without a
  /// mode set (but with a value within the valid range), ProgressBarMode.Normal
  /// will be assumed.
  abstract setProgressBar: progress: float * ?options: ProgressBarOptions -> unit
  /// <summary>
  ///   [Windows] Sets or clears a 16 x 16 pixel overlay onto the current
  ///   taskbar icon, usually used to convey some sort of application status or
  ///   to passively notify the user.
  /// </summary>
  /// <param name="overlay">
  ///   The icon to display on the bottom right corner of the taskbar icon. If
  ///   this parameter is null, the overlay is cleared
  /// </param>
  /// <param name="description">
  ///    A description that will be provided to Accessibility screen readers
  /// </param>
  abstract setOverlayIcon: overlay: NativeImage option * description: string -> unit
  /// [macOS] Sets whether the window should have a shadow. On Windows and Linux
  /// does nothing.
  abstract setHasShadow: hasShadow: bool -> unit
  /// [macOS] Indicates whether the window has a shadow. On Windows and Linux
  /// always returns true.
  abstract hasShadow: unit -> bool
  /// [Windows, macOS] Sets the opacity of the window. On Linux does nothing.
  abstract setOpacity: opacity: float -> unit
  /// [Windows, macOS] Returns a number between 0.0 (fully transparent) and 1.0 (fully opaque).
  abstract getOpacity: unit -> float
  /// [Windows, Linux] Sets a shape on the window. Passing an empty array
  /// reverts the window to being rectangular.
  ///
  /// Setting a window shape determines the area within the window where the
  /// system permits drawing and user interaction. Outside of the given region,
  /// no pixels will be drawn and no mouse events will be registered. Mouse
  /// events outside of the region will not be received by that window, but will
  /// fall through to whatever is behind the window.
  abstract setShape: rects: Rectangle [] -> bool
  /// [Windows] Add a thumbnail toolbar with a specified set of buttons to the
  /// thumbnail image of a window in a taskbar button layout. Returns true if
  /// the thumbnail has been added successfully.
  ///
  /// The number of buttons in thumbnail toolbar should be no greater than 7 due
  /// to the limited room. Once you setup the thumbnail toolbar, the toolbar
  /// cannot be removed due to the platform's limitation. But you can call the
  /// API with an empty array to clean the buttons.
  ///
  abstract setThumbarButtons: buttons: ThumbarButton [] -> bool
  /// [Windows] Sets the region of the window to show as the thumbnail image
  /// displayed when hovering over the window in the taskbar. You can reset the
  /// thumbnail to be the entire window by specifying an empty region: { x: 0,
  /// y: 0, width: 0, height: 0 }.
  abstract setThumbnailClip: region: Rectangle -> unit
  /// [Windows] Sets the toolTip that is displayed when hovering over the window
  /// thumbnail in the taskbar.
  abstract setThumbnailToolTip: toolTip: string -> unit
  /// Sets the properties for the window's taskbar button.
  ///
  /// Note: relaunchCommand and relaunchDisplayName must always be set together.
  /// If one of those properties is not set, then neither will be used.
  abstract setAppDetails: options: AppDetailsOptions -> unit
  /// [macOS] Same as webContents.showDefinitionForSelection().
  abstract showDefinitionForSelection: unit -> unit
  /// [Windows, Linux] Changes window icon.
  abstract setIcon: icon: NativeImage -> unit
  /// [macOS] Sets whether the window traffic light buttons should be visible.
  ///
  /// This cannot be called when titleBarStyle is set to customButtonsOnHover.
  abstract setWindowButtonVisibility: visible: bool -> unit
  /// Sets whether the window menu bar should hide itself automatically. Once
  /// set the menu bar will only show when users press the single Alt key.
  ///
  /// If the menu bar is already visible, calling setAutoHideMenuBar(true) won't
  /// hide it immediately.
  abstract setAutoHideMenuBar: hide: bool -> unit
  /// Indicates whether menu bar automatically hides itself.
  abstract isMenuBarAutoHide: unit -> bool
  /// [Windows, Linux] Sets whether the menu bar should be visible. If the menu
  /// bar is auto-hide, users can still bring up the menu bar by pressing the
  /// single Alt key.
  abstract setMenuBarVisibility: visible: bool -> unit
  /// Indicates whether the menu bar is visible.
  abstract isMenuBarVisible: unit -> bool
  /// [macOS, Linux] Sets whether the window should be visible on all workspaces.
  ///
  /// Note: This API does nothing on Windows.
  abstract setVisibleOnAllWorkspaces: visible: bool * ?options: VisibleOnAllWorkspacesOptions -> unit
  /// [macOS, Linux] Indicates whether the window is visible on all workspaces.
  ///
  /// Note: This API always returns false on Windows.
  abstract isVisibleOnAllWorkspaces: unit -> bool
  /// Makes the window ignore all mouse events.
  ///
  /// All mouse events happened in this window will be passed to the window
  /// below this window, but if this window has focus, it will still receive
  /// keyboard events.
  abstract setIgnoreMouseEvents: ignore: bool * ?options: IgnoreMouseEventsOptions -> unit
  /// [macOS, Windows] Prevents the window contents from being captured by other
  /// apps.
  ///
  /// On macOS it sets the NSWindow's sharingType to NSWindowSharingNone. On
  /// Windows it calls SetWindowDisplayAffinity with WDA_MONITOR.
  abstract setContentProtection: enable: bool -> unit
  /// [Windows] Changes whether the window can be focused.
  abstract setFocusable: focusable: bool -> unit
  /// Sets parent as current window's parent window, passing None will turn
  /// current window into a top-level window.
  abstract setParentWindow: parent: BrowserWindow option -> unit
  /// Returns the parent window.
  abstract getParentWindow: unit -> BrowserWindow option
  /// Returns all child windows.
  abstract getChildWindows: unit -> BrowserWindow []
  /// [macOS] Controls whether to hide cursor when typing.
  abstract setAutoHideCursor: autoHide: bool -> unit
  /// [macOS] Selects the previous tab when native tabs are enabled and there
  /// are other tabs in the window.
  abstract selectPreviousTab: unit -> unit
  /// [macOS] Selects the next tab when native tabs are enabled and there are
  /// other tabs in the window.
  abstract selectNextTab: unit -> unit
  /// [macOS] Merges all windows into one window with multiple tabs when native
  /// tabs are enabled and there is more than one open window.
  abstract mergeAllWindows: unit -> unit
  /// [macOS] Moves the current tab into a new window if native tabs are enabled
  /// and there is more than one tab in the current window.
  abstract moveTabToNewWindow: unit -> unit
  /// [macOS] Toggles the visibility of the tab bar if native tabs are enabled
  /// and there is only one tab in the current window.
  abstract toggleTabBar: unit -> unit
  /// [macOS] Adds a window as a tab on this window, after the tab for the
  /// window instance.
  abstract addTabbedWindow: browserWindow: BrowserWindow -> unit
  /// [macOS] Adds a vibrancy effect to the browser window. Passing None will
  /// remove the vibrancy effect on the window.
  abstract setVibrancy: ``type``: VibrancyType option -> unit
  /// [macOS] Sets the touchBar layout for the current window. Specifying None
  /// clears the touch bar. This method only has an effect if the machine has a
  /// touch bar and is running on macOS 10.12.1+.
  ///
  /// Note: The TouchBar API is currently experimental and may change or be
  /// removed in future Electron releases.
  abstract setTouchBar: touchBar: TouchBar option -> unit
  /// Attach browserView to win. If there is some other browserViews was
  /// attached they will be removed from this window.
  abstract setBrowserView: browserView: BrowserView -> unit
  /// Returns an BrowserView what is attached. Returns null if none is attached.
  /// Throw error if multiple BrowserViews is attached.
  abstract getBrowserView: unit -> BrowserView option
  /// Replacement API for setBrowserView supporting work with multi browser
  /// views.
  abstract addBrowserView: browserView: BrowserView -> unit
  abstract removeBrowserView: browserView: BrowserView -> unit
  /// Returns array of BrowserView what was an attached with addBrowserView or
  /// setBrowserView.
  ///
  /// Note: The BrowserView API is currently experimental and may change or be
  /// removed in future Electron releases.
  abstract getBrowserViews: unit -> BrowserView []
  /// [macOS] Determines whether the window is excluded from the application’s
  /// Windows menu. `false` by default.
  abstract excludedFromShownWindowsMenu: bool with get, set

type BrowserWindowStatic =
  /// Instantiates a BrowserWindow.
  [<EmitConstructor>] abstract Create: ?options: BrowserWindowOptions -> BrowserWindow
  /// Returns all opened browser windows.
  abstract getAllWindows: unit -> BrowserWindow []
  /// Returns the window that is focused in this application.
  abstract getFocusedWindow: unit -> BrowserWindow option
  /// Returns the window that owns the given webContents.
  abstract fromWebContents: webContents: WebContents -> BrowserWindow
  /// Returns the window that owns the given browserView, or None if the given
  /// view is not attached to any window.
  abstract fromBrowserView: browserView: BrowserView -> BrowserWindow option
  /// Returns the window with the given id.
  abstract fromId: id: int -> BrowserWindow option
  /// Adds Chrome extension located at `path`, and returns extension's name. The
  /// method will also not return if the extension's manifest is missing or
  /// incomplete.
  ///
  /// Note: This API cannot be called before the `ready` event of the app module
  /// is emitted.
  abstract addExtension: path: string -> unit
  /// Remove a Chrome extension by name.
  ///
  /// Note: This API cannot be called before the `ready` event of the app module
  /// is emitted.
  abstract removeExtension: name: string -> unit
  /// Returns an object where the keys are the extension names and each value is
  /// an object containing `name` and `version` properties.
  ///
  /// Note: This API cannot be called before the `ready` event of the app module
  /// is emitted.
  abstract getExtensions: unit -> obj
  /// Adds DevTools extension located at `path`, and returns extension's name.
  ///
  /// The extension will be remembered so you only need to call this API once,
  /// this API is not for programming use. If you try to add an extension that
  /// has already been loaded, this method will not return and instead log a
  /// warning to the console.
  ///
  /// The method will also not return if the extension's manifest is missing or
  /// incomplete.
  ///
  /// Note: This API cannot be called before the `ready` event of the app module
  /// is emitted.
  abstract addDevToolsExtension: path: string -> unit
  /// Remove a DevTools extension by name.
  ///
  /// Note: This API cannot be called before the `ready` event of the app module
  /// is emitted.
  abstract removeDevToolsExtension: name: string -> unit
  /// Returns an object where the keys are the extension names and each value is
  /// an object containing `name` and `version` properties.
  ///
  /// Note: This API cannot be called before the `ready` event of the app module
  /// is emitted.
  abstract getDevToolsExtensions: unit -> obj

type BrowserWindowProxy =
  inherit EventEmitter<BrowserWindowProxy>
  /// Removes focus from the child window.
  abstract blur: unit -> unit
  /// Forcefully closes the child window without calling its unload event.
  abstract close: unit -> unit
  /// Evaluates the code in the child window.
  abstract eval: code: string -> unit
  /// Focuses the child window (brings the window to front).
  abstract focus: unit -> unit
  /// Invokes the print dialog on the child window.
  abstract print: unit -> unit
  /// Sends a message to the child window with the specified origin or * for no
  /// origin preference.
  abstract postMessage: message: string * targetOrigin: string -> unit
  /// Set to true after the child window gets closed.
  abstract closed: bool with get, set

type Certificate =
  /// PEM encoded data
  abstract data: string with get, set
  /// Issuer principal
  abstract issuer: CertificatePrincipal with get, set
  /// Issuer's Common Name
  abstract issuerName: string with get, set
  /// Issuer certificate (if not self-signed)
  abstract issuerCert: Certificate with get, set
  /// Subject principal
  abstract subject: CertificatePrincipal with get, set
  /// Subject's Common Name
  abstract subjectName: string with get, set
  /// Hex value represented string
  abstract serialNumber: string with get, set
  /// Start date of the certificate being valid in seconds
  abstract validStart: float with get, set
  /// End date of the certificate being valid in seconds
  abstract validExpiry: float with get, set
  /// Fingerprint of the certificate
  abstract fingerprint: string with get, set

type CertificatePrincipal =
  /// Common Name.
  abstract commonName: string with get, set
  /// Organization names.
  abstract organizations: string [] with get, set
  /// Organization Unit names.
  abstract organizationUnits: string [] with get, set
  /// Locality.
  abstract locality: string with get, set
  /// Country or region.
  abstract country: string with get, set
  /// State or province.
  abstract state: string with get, set

type ClientRequest =
  inherit Node.Stream.Writable<obj>
  inherit EventEmitter<ClientRequest>
  /// Provides the HTTP response message.
  [<Emit "$0.on('response',$1)">] abstract onResponse: listener: (IncomingMessage -> unit) -> ClientRequest  // TODO: Should IncomingMessage be second argument, and first argument be Event?
  /// See onResponse.
  [<Emit "$0.once('response',$1)">] abstract onceResponse: listener: (IncomingMessage -> unit) -> ClientRequest
  /// See onResponse.
  [<Emit "$0.addListener('response',$1)">] abstract addListenerResponse: listener: (IncomingMessage -> unit) -> ClientRequest
  /// See onResponse.
  [<Emit "$0.removeListener('response',$1)">] abstract removeListenerResponse: listener: (IncomingMessage -> unit) -> ClientRequest
  /// Emitted when an authenticating proxy is asking for user credentials.
  ///
  /// The callback function is expected to be called back with username and
  /// password.
  ///
  /// Providing empty credentials will cancel the request and report an
  /// authentication error on the response object.
  [<Emit "$0.on('login',$1)">] abstract onLogin: listener: (AuthInfo -> (string -> string -> unit) -> unit) -> ClientRequest
  /// See onLogin.
  [<Emit "$0.once('login',$1)">] abstract onceLogin: listener: (AuthInfo -> (string -> string -> unit) -> unit) -> ClientRequest
  /// See onLogin.
  [<Emit "$0.addListener('login',$1)">] abstract addListenerLogin: listener: (AuthInfo -> (string -> string -> unit) -> unit) -> ClientRequest
  /// See onLogin.
  [<Emit "$0.removeListener('login',$1)">] abstract removeListenerLogin: listener: (AuthInfo -> (string -> string -> unit) -> unit) -> ClientRequest
  /// Emitted just after the last chunk of the request's data has been written
  /// into the `request` object.
  [<Emit "$0.on('finish',$1)">] abstract onFinish: listener: (Event -> unit) -> ClientRequest
  /// See onFinish.
  [<Emit "$0.once('finish',$1)">] abstract onceFinish: listener: (Event -> unit) -> ClientRequest
  /// See onFinish.
  [<Emit "$0.addListener('finish',$1)">] abstract addListenerFinish: listener: (Event -> unit) -> ClientRequest
  /// See onFinish.
  [<Emit "$0.removeListener('finish',$1)">] abstract removeListenerFinish: listener: (Event -> unit) -> ClientRequest
  /// Emitted when the request is aborted. Will not be fired if the request is
  /// already closed.
  [<Emit "$0.on('abort',$1)">] abstract onAbort: listener: (Event -> unit) -> ClientRequest
  /// See onAbort.
  [<Emit "$0.once('abort',$1)">] abstract onceAbort: listener: (Event -> unit) -> ClientRequest
  /// See onAbort.
  [<Emit "$0.addListener('abort',$1)">] abstract addListenerAbort: listener: (Event -> unit) -> ClientRequest
  /// See onAbort.
  [<Emit "$0.removeListener('abort',$1)">] abstract removeListenerAbort: listener: (Event -> unit) -> ClientRequest
  /// Emitted when the `net` module fails to issue a network request. Typically
  /// when the request object emits an error event, a close event will
  /// subsequently follow and no response object will be provided.
  [<Emit "$0.on('error',$1)">] abstract onError: listener: (Error -> unit) -> ClientRequest
  /// See onError.
  [<Emit "$0.once('error',$1)">] abstract onceError: listener: (Error -> unit) -> ClientRequest
  /// See onError.
  [<Emit "$0.addListener('error',$1)">] abstract addListenerError: listener: (Error -> unit) -> ClientRequest
  /// See onError.
  [<Emit "$0.removeListener('error',$1)">] abstract removeListenerError: listener: (Error -> unit) -> ClientRequest
  /// Emitted as the last event in the HTTP request-response transaction. The
  /// close event indicates that no more events will be emitted on either the
  /// request or response objects.
  [<Emit "$0.on('close',$1)">] abstract onClose: listener: (Event -> unit) -> ClientRequest
  /// See onClose.
  [<Emit "$0.once('close',$1)">] abstract onceClose: listener: (Event -> unit) -> ClientRequest
  /// See onClose.
  [<Emit "$0.addListener('close',$1)">] abstract addListenerClose: listener: (Event -> unit) -> ClientRequest
  /// See onClose.
  [<Emit "$0.removeListener('close',$1)">] abstract removeListenerClose: listener: (Event -> unit) -> ClientRequest
  /// Emitted when there is redirection and the mode is manual. Calling
  /// request.followRedirect will continue with the redirection.
  ///
  /// Parameters:
  ///   - statusCode
  ///   - method
  ///   - redirectUrl
  ///   - responseHeaders
  [<Emit "$0.on('redirect',$1)">] abstract onRedirect: listener: (int -> string -> string -> obj option -> unit) -> ClientRequest
  /// See onRedirect.
  [<Emit "$0.once('redirect',$1)">] abstract onceRedirect: listener: (int -> string -> string -> obj option -> unit) -> ClientRequest
  /// See onRedirect.
  [<Emit "$0.addListener('redirect',$1)">] abstract addListenerRedirect: listener: (int -> string -> string -> obj option -> unit) -> ClientRequest
  /// See onRedirect.
  [<Emit "$0.removeListener('redirect',$1)">] abstract removeListenerRedirect: listener: (int -> string -> string -> obj option -> unit) -> ClientRequest
  /// Indicates whether the request will use HTTP chunked transfer encoding or
  /// not. Defaults to false. The property is readable and writable, however it
  /// can be set only before the first write operation as the HTTP headers are
  /// not yet put on the wire. Trying to set the chunkedEncoding property after
  /// the first write will throw an error.
  ///
  /// Using chunked encoding is strongly recommended if you need to send a large
  /// request body as data will be streamed in small chunks instead of being
  /// internally buffered inside Electron process memory.
  abstract chunkedEncoding: bool with get, set
  /// Adds an extra HTTP header. The header name will issued as it is without
  /// lowercasing. It can be called only before first write. Calling this method
  /// after the first write will throw an error. If the passed value is not a
  /// string, its toString() method will be called to obtain the final value.
  abstract setHeader: name: string * value: obj -> unit
  /// Returns the value of a previously set extra header.
  abstract getHeader: name: string -> obj
  /// Removes a previously set extra header name. This method can be called only
  /// before first write. Trying to call it after the first write will throw an
  /// error.
  abstract removeHeader: name: string -> unit
  /// <summary>
  ///   Adds a chunk of data to the request body. The first write operation may
  ///   cause the request headers to be issued on the wire. After the first
  ///   write operation, it is not allowed to add or remove a custom header.
  /// </summary>
  /// <param name="chunk">
  ///   A chunk of the request body's data. It is converted into a Buffer using
  ///   the specified encoding.
  /// </param>
  /// <param name="encoding">
  ///   Used to convert string chunks into Buffer objects. Defaults to "utf-8".
  /// </param>
  /// <param name="callback">
  ///   Called after the write operation ends. This is essentially a dummy
  ///   function introduced in the purpose of keeping similarity with the
  ///   Node.js API. It is called asynchronously in the next tick after chunk
  ///   content have been delivered to the Chromium networking layer. Contrary
  ///   to the Node.js implementation, it is not guaranteed that chunk content
  ///   have been flushed on the wire before callback is called.
  /// </param>
  abstract write: chunk: string * ?encoding: string * ?callback: (Event -> unit) -> unit
  /// <summary>
  ///   Adds a chunk of data to the request body. The first write operation may
  ///   cause the request headers to be issued on the wire. After the first
  ///   write operation, it is not allowed to add or remove a custom header.
  /// </summary>
  /// <param name="chunk">A chunk of the request body's data.</param>
  /// <param name="callback">
  ///   Called after the write operation ends. This is essentially a dummy
  ///   function introduced in the purpose of keeping similarity with the
  ///   Node.js API. It is called asynchronously in the next tick after chunk
  ///   content have been delivered to the Chromium networking layer. Contrary
  ///   to the Node.js implementation, it is not guaranteed that chunk content
  ///   have been flushed on the wire before callback is called.
  /// </param>
  abstract write: chunk: Buffer * ?callback: (Event -> unit) -> unit
  /// Sends the last chunk of the request data. Subsequent write or end
  /// operations will not be allowed. The `finish` event is emitted just after
  /// the end operation.
  abstract ``end``: ?callback: (Event -> unit) -> unit
  /// Sends the last chunk of the request data. Subsequent write or end
  /// operations will not be allowed. The `finish` event is emitted just after
  /// the end operation.
  abstract ``end``: chunk: string * ?encoding: string * ?callback: (Event -> unit) -> unit
  /// Sends the last chunk of the request data. Subsequent write or end
  /// operations will not be allowed. The `finish` event is emitted just after
  /// the end operation.
  abstract ``end``: chunk: Buffer * ?callback: (Event -> unit) -> unit
  /// Cancels an ongoing HTTP transaction. If the request has already emitted
  /// the `close` event, the abort operation will have no effect. Otherwise an
  /// ongoing event will emit `abort` and `close` events. Additionally, if there
  /// is an ongoing response object,it will emit the `aborted` event.
  abstract abort: unit -> unit
  /// Continues any deferred redirection request when the redirection mode is
  /// RedirectMode.Manual.
  abstract followRedirect: unit -> unit
  /// You can use this method in conjunction with POST requests to get the
  /// progress of a file upload or other data transfer.
  abstract getUploadProgress: unit -> UploadProgress

[<StringEnum; RequireQualifiedAccess>]
type RedirectMode =
  | Follow
  | Error
  | Manual

type ClientRequestOptions =
  /// The HTTP request method. Defaults to the GET method.
  abstract method: string
  /// The request URL. Must be provided in the absolute form with the protocol
  /// scheme specified as http or https.
  abstract url: string
  /// The Session instance with which the request is associated.
  abstract session: Session
  /// The name of the partition with which the request is associated. Defaults
  /// to the empty string. The session option prevails on partition. Thus if a
  /// session is explicitly specified, partition is ignored.
  abstract partition: string
  /// The protocol scheme in the form 'scheme:'. Currently supported values are
  /// 'http:' or 'https:'. Defaults to 'http:'.
  abstract protocol: string
  /// The server host provided as a concatenation of the hostname and the port
  /// number 'hostname:port'.
  abstract host: string
  /// The server host name.
  abstract hostname: string
  /// The server's listening port number.
  abstract port: int
  /// The path part of the request URL.
  abstract path: string
  /// The redirect mode for this request. Defaults to RedirectMode.Follow. When
  /// mode is RedirectMode.Error, any redirection will be aborted. When mode is
  /// RedirectMode.Manual the redirection will be deferred until
  /// request.followRedirect is invoked. Listen for the `redirect` event in this
  /// mode to get more details about the redirect request.
  abstract redirect: RedirectMode


type ClientRequestStatic =
  [<EmitConstructor>] abstract Create: options: U2<string, ClientRequestOptions> -> ClientRequest

[<StringEnum; RequireQualifiedAccess>]
type ClipboardType =
  | Clipboard
  // Only available on Linux
  | Selection

type Clipboard =
  inherit EventEmitter<Clipboard>
  /// Returns the content in the clipboard as plain text.
  abstract readText: ?``type``: ClipboardType -> string
  /// Writes the `text` into the clipboard as plain text.
  abstract writeText: text: string * ?``type``: ClipboardType -> unit
  /// Returns the content in the clipboard as markup.
  abstract readHTML: ?``type``: ClipboardType -> string
  /// Writes `markup` to the clipboard.
  abstract writeHTML: markup: string * ?``type``: ClipboardType -> unit
  /// Returns the image content in the clipboard.
  abstract readImage: ?``type``: ClipboardType -> NativeImage
  /// Writes `image` to the clipboard.
  abstract writeImage: image: NativeImage * ?``type``: ClipboardType -> unit
  /// Returns the content in the clipboard as RTF.
  abstract readRTF: ?``type``: ClipboardType -> string
  /// Writes the `text` into the clipboard in RTF.
  abstract writeRTF: text: string * ?``type``: ClipboardType -> unit
  /// [macOS, Windows] Returns an object containing title and url keys
  /// representing the bookmark in the clipboard. The title and url values will
  /// be empty strings when the bookmark is unavailable.
  abstract readBookmark: unit -> ClipboardBookmark
  /// [macOS, Windows] Writes the title and url into the clipboard as a
  /// bookmark.
  ///
  /// Note: Most apps on Windows don't support pasting bookmarks into them so
  /// you can use clipboard.write to write both a bookmark and fallback text to
  /// the clipboard.
  abstract writeBookmark: title: string * url: string * ?``type``: ClipboardType -> unit
  /// [macOS] Returns the text on the find pasteboard. This method uses
  /// synchronous IPC when called from the renderer process. The cached value is
  /// reread from the find pasteboard whenever the application is activated.
  abstract readFindText: unit -> string
  /// [macOS] Writes the `text` into the find pasteboard as plain text. This
  /// method uses synchronous IPC when called from the renderer process.
  abstract writeFindText: text: string -> unit
  /// Clears the clipboard content.
  abstract clear: ?``type``: ClipboardType -> unit
  /// Returns the supported formats for the clipboard.
  abstract availableFormats: ?``type``: ClipboardType -> string []
  /// Returns a value indicating whether the clipboard supports the specified format.
  abstract has: format: string * ?``type``: ClipboardType -> bool
  /// Reads `format` type from the clipboard.
  abstract read: format: string -> string
  /// Reads `format` type from the clipboard.
  abstract readBuffer: format: string -> Buffer
  /// Writes the `buffer` into the clipboard as `format`.
  abstract writeBuffer: format: string * buffer: Buffer * ?``type``: ClipboardType -> unit
  /// Writes `data` to the clipboard.
  abstract write: data: ClipboardData * ?``type``: ClipboardType -> unit

type TraceBufferUsage =
  abstract value: float with get, set
  abstract percentage: float with get, set

type ContentTracing =
  inherit EventEmitter<ContentTracing>
  /// Get a set of category groups. The category groups can change as new code
  /// paths are reached.
  ///
  /// The returned promise resolves with an array of category groups once all
  /// child processes have acknowledged the getCategories request.
  abstract getCategories: unit -> Promise<string []>
  /// Start recording on all processes.
  ///
  /// Recording begins immediately locally and asynchronously on child processes
  /// as soon as they receive the EnableRecording request.
  ///
  /// The returned promise is resolved once all child processes have
  /// acknowledged the `startRecording` request.
  abstract startRecording: options: TraceCategoriesAndOptions -> Promise<unit>
  /// Start recording on all processes.
  ///
  /// Recording begins immediately locally and asynchronously on child processes
  /// as soon as they receive the EnableRecording request.
  ///
  /// The returned promise is resolved once all child processes have
  /// acknowledged the `startRecording` request.
  abstract startRecording: options: TraceConfig -> Promise<unit>
  /// Stop recording on all processes.
  ///
  /// Child processes typically cache trace data and only rarely flush and send
  /// trace data back to the main process. This helps to minimize the runtime
  /// overhead of tracing since sending trace data over IPC can be an expensive
  /// operation. So, to end tracing, we must asynchronously ask all child
  /// processes to flush any pending trace data.
  ///
  /// Trace data will be written into `resultFilePath` if it is not empty or
  /// into a temporary file.
  ///
  /// The returned promise resolves with a file that contains the traced data
  /// once all child processes have acknowledged the stopRecording request
  abstract stopRecording: resultFilePath: string -> Promise<string>
  /// Get the maximum usage across processes of trace buffer as a percentage of
  /// the full state.
  abstract getTraceBufferUsage: unit -> Promise<TraceBufferUsage>

type Cookie =
  /// The name of the cookie.
  abstract name: string with get, set
  /// The value of the cookie.
  abstract value: string with get, set
  /// The domain of the cookie; this will be normalized with a preceding dot so
  /// that it's also valid for subdomains.
  abstract domain: string option with get, set
  /// The path of the cookie.
  abstract path: string option with get, set
  /// Whether the cookie is marked as secure.
  abstract secure: bool option with get, set
  /// Whether the cookie is marked as HTTP only.
  abstract httpOnly: bool option with get, set
  /// Whether the cookie is a host-only cookie; this will only be true if no
  /// domain was passed.
  abstract hostOnly: bool option with get, set
  /// Whether the cookie is a session cookie or a persistent cookie with an
  /// expiration date.
  abstract session: bool option with get, set
  /// The expiration date of the cookie as the number of seconds since the UNIX
  /// epoch. Not provided for session cookies.
  abstract expirationDate: float option with get, set

[<StringEnum; RequireQualifiedAccess>]
type CookieChangedCause =
  /// The cookie was changed directly by a consumer's action.
  | Explicit
  /// The cookie was automatically removed due to an insert operation that
  /// overwrote it.
  | Overwrite
  /// The cookie was automatically removed as it expired.
  | Expired
  /// The cookie was automatically evicted during garbage collection.
  | Evicted
  /// The cookie was overwritten with an already-expired expiration date.
  | [<CompiledName("expired-overwrite")>] ExpiredOverwrite

type Cookies =
  inherit EventEmitter<Cookies>
  /// Emitted when a cookie is changed because it was added, edited, removed, or
  /// expired.
  ///
  /// Extra parameters:
  ///   - cookie: The cookie that was changed
  ///   - cause: The cause of the change
  ///   - removed: true if the cookie was removed, false otherwise
  [<Emit "$0.on('changed',$1)">] abstract onChanged: listener: (Event -> Cookie -> CookieChangedCause -> bool -> unit) -> Cookies
  /// See onChanged.
  [<Emit "$0.once('changed',$1)">] abstract onceChanged: listener: (Event -> Cookie -> CookieChangedCause -> bool -> unit) -> Cookies
  /// See onChanged.
  [<Emit "$0.addListener('changed',$1)">] abstract addListenerChanged: listener: (Event -> Cookie -> CookieChangedCause -> bool -> unit) -> Cookies
  /// See onChanged.
  [<Emit "$0.removeListener('changed',$1)">] abstract removeListenerChanged: listener: (Event -> Cookie -> CookieChangedCause -> bool -> unit) -> Cookies
  /// Sends a request to get all cookies matching filter, and resolves a promise
  /// with the response.
  abstract get: filter: GetCookiesFilter -> Promise<Cookie []>
  /// Sets a cookie with details.
  abstract set: details: SetCookieDetails -> Promise<unit>
  /// Removes the cookies matching `url` and `name`
  abstract remove: url: string * name: string -> Promise<unit>
  /// Writes any unwritten cookies data to disk.
  abstract flushStore: unit -> Promise<unit>

type CPUUsage =
  /// Percentage of CPU used since the last call to `getCPUUsage`. First call
  /// returns 0.
  abstract percentCPUUsage: float with get, set
  /// The number of average idle cpu wakeups per second since the last call to
  /// `getCPUUsage`. First call returns 0. Will always return 0 on Windows.
  abstract idleWakeupsPerSecond: float with get, set

type CrashReport =
  abstract date: DateTime with get, set
  abstract id: string with get, set

type CrashReporter =
  inherit EventEmitter<CrashReporter>
  /// You are required to call this method before using any other crashReporter
  /// APIs and in each process (main/renderer) from which you want to collect
  /// crash reports. You can pass different options to crashReporter.start when
  /// calling from different processes.
  ///
  /// For more information, see the Electron docs:
  /// https://electronjs.org/docs/api/crash-reporter
  abstract start: options: CrashReporterStartOptions -> unit
  /// Returns the date and ID of the last crash report. Only crash reports that
  /// have been uploaded will be returned; even if a crash report is present on
  /// disk it will not be returned until it is uploaded. In the case that there
  /// are no uploaded reports, None is returned.
  abstract getLastCrashReport: unit -> CrashReport option
  /// Returns all uploaded crash reports.
  abstract getUploadedReports: unit -> CrashReport []
  /// [Linux, macOS] Returns a value indicating whether reports should be
  /// submitted to the server. Set through the `start` method or
  /// `setUploadToServer`.
  ///
  /// Note: This API can only be called from the main process.
  abstract getUploadToServer: unit -> bool
  /// [Linux, macOS] Sets whether reports should be submitted to the server.
  ///
  /// This would normally be controlled by user preferences. This has no effect
  /// if called before `start` is called.
  ///
  /// Note: This API can only be called from the main process.
  abstract setUploadToServer: uploadToServer: bool -> unit
  /// [macOS] Set an extra parameter to be sent with the crash report. The
  /// values specified here will be sent in addition to any values set via the
  /// `extra` option when `start` was called. This API is only available on
  /// macOS, if you need to add/update extra parameters on Linux and Windows
  /// after your first call to `start` you can call `start` again with the
  /// updated `extra` options.
  ///
  /// Both `key` and `value` must be less than 64 characters long.
  abstract addExtraParameter: key: string * value: string -> unit
  /// [macOS] Remove a extra parameter from the current set of parameters so
  /// that it will not be sent with the crash report.
  abstract removeExtraParameter: key: string -> unit
  /// See all of the current parameters being passed to the crash reporter.
  abstract getParameters: unit -> unit

type CustomScheme =
  /// Custom schemes to be registered with options.
  abstract scheme: string with get, set
  abstract privileges: CustomSchemePrivileges with get, set

type Debugger =
  inherit EventEmitter<Debugger>
  /// Emitted when debugging session is terminated. This happens either when
  /// webContents is closed or devtools is invoked for the attached webContents.
  [<Emit "$0.on('detach',$1)">] abstract onDetach: listener: (Event -> string -> unit) -> Debugger
  /// See onDetach.
  [<Emit "$0.once('detach',$1)">] abstract onceDetach: listener: (Event -> string -> unit) -> Debugger
  /// See onDetach.
  [<Emit "$0.addListener('detach',$1)">] abstract addListenerDetach: listener: (Event -> string -> unit) -> Debugger
  /// See onDetach.
  [<Emit "$0.removeListener('detach',$1)">] abstract removeListenerDetach: listener: (Event -> string -> unit) -> Debugger
  /// Emitted whenever debugging target issues instrumentation event.
  [<Emit "$0.on('message',$1)">] abstract onMessage: listener: (Event -> string -> obj option -> unit) -> Debugger
  /// See onMessage.
  [<Emit "$0.once('message',$1)">] abstract onceMessage: listener: (Event -> string -> obj option -> unit) -> Debugger
  /// See onMessage.
  [<Emit "$0.addListener('message',$1)">] abstract addListenerMessage: listener: (Event -> string -> obj option -> unit) -> Debugger
  /// See onMessage.
  [<Emit "$0.removeListener('message',$1)">] abstract removeListenerMessage: listener: (Event -> string -> obj option -> unit) -> Debugger
  /// Attaches the debugger to the webContents.
  abstract attach: ?protocolVersion: string -> unit
  /// Indicates whether a debugger is attached to the webContents.
  abstract isAttached: unit -> bool
  /// Detaches the debugger from the webContents.
  abstract detach: unit -> unit
  /// <summary>
  ///   Send given command to the debugging target. Returns a promise that
  ///   resolves with the response defined by the 'returns' attribute of the
  ///   command description in the remote debugging protocol or is rejected
  ///   indicating the failure of the command.
  /// </summary>
  /// <param name="method">
  ///   Method name, should be one of the methods defined by the remote
  ///   debugging protocol.
  /// </param>
  /// <param name="commandParams">Object with request parameters.</param>
  abstract sendCommand: method: string * ?commandParams: obj -> Promise<obj option>

type DesktopCapturer =
  inherit EventEmitter<DesktopCapturer>
  /// Each returned DesktopCapturerSource represents a screen or an individual
  /// window that can be captured.
  abstract getSources: options: GetDesktopCapturerSourcesOptions -> Promise<DesktopCapturerSource []>

type DesktopCapturerSource =
  /// The identifier of a window or screen that can be used as a
  /// chromeMediaSourceId constraint when calling
  /// [navigator.webkitGetUserMedia]. The format of the identifier will be
  /// window:XX or screen:XX, where XX is a random generated number.
  abstract id: string with get, set
  /// A screen source will be named either Entire Screen or Screen <index>,
  /// while the name of a window source will match the window title.
  abstract name: string with get, set
  /// A thumbnail image. There is no guarantee that the size of the thumbnail is
  /// the same as the `thumbnailSize` specified in the `options` passed to
  /// desktopCapturer.getSources. The actual size depends on the scale of the
  /// screen or window.
  abstract thumbnail: NativeImage with get, set
  /// A unique identifier that will correspond to the `id` of the matching
  /// returned by the Screen API. On some platforms, this is equivalent to the
  /// XX portion of the `id` field and on others it will differ. It will be an
  /// empty string if not available.
  abstract display_id: string with get, set
  /// An icon image of the application that owns the window or null if the
  /// source has a type screen. The size of the icon is not known in advance and
  /// depends on what the application provides.
  abstract appIcon: NativeImage option with get, set

type OpenDialogResult =
  /// Whether or not the dialog was canceled.
  abstract canceled: bool with get, set
  /// An array of file paths chosen by the user. If the dialog is cancelled this
  /// will be an empty array.
  abstract filePaths: string [] with get, set  // TODO: check that this is an empty array and not undefined/null when cancelled, also update `bookmarks` accordingly
  /// [macOS Mac App Store only] An array matching the filePaths array of base64
  /// encoded strings which contains security scoped bookmark data.
  /// securityScopedBookmarks must be enabled for this to be populated.
  abstract bookmarks: string [] with get, set

type SaveDialogResult =
  /// Whether or not the dialog was canceled.
  abstract canceled: bool with get, set
  /// If the dialog is canceled this will be None.
  abstract filePath : string option with get, set
  /// [macOS Mac App Store only] Base64 encoded string which contains the
  /// security scoped bookmark data for the saved file. securityScopedBookmarks
  /// must be enabled for this to be present.
  abstract bookmarks: string [] with get, set

type MessageBoxResult =
  /// The index of the clicked button.
  abstract response: int with get, set
  /// The checked state of the checkbox if checkboxLabel was set. Otherwise
  /// false
  abstract checkboxChecked: bool with get, set

type Dialog =
  inherit EventEmitter<Dialog>
  /// Returns the file paths chosen by the user, or None if the dialog was
  /// canceled.
  abstract showOpenDialogSync: options: OpenDialogOptions -> string [] option  // TODO: is the return type correct? Is it empty or undefined when cancelled?
  /// Returns an array of file paths chosen by the user.
  ///
  /// The `browserWindow` argument allows the dialog to attach itself to a
  /// parent window, making it modal.
  abstract showOpenDialogSync: browserWindow: BrowserWindow * options: OpenDialogOptions -> string [] option  // TODO: is the return type correct? Is it empty or undefined when cancelled?
  /// Display an Open dialog and returns the file paths chosen by the user.
  abstract showOpenDialog: options: OpenDialogOptions -> Promise<OpenDialogResult>
  /// Display an Open dialog and returns the file paths chosen by the user.
  ///
  /// The `browserWindow` argument allows the dialog to attach itself to a
  /// parent window, making it modal.
  abstract showOpenDialog: browserWindow: BrowserWindow * options: OpenDialogOptions -> Promise<OpenDialogResult>
  /// Returns the path of the file chosen by the user, or None if the dialog was
  /// canceled.
  ///
  /// Note: On macOS, using the asynchronous version is recommended to avoid
  /// issues when expanding and collapsing the dialog.
  abstract showSaveDialogSync: options: SaveDialogOptions -> string option
  /// Returns the path of the file chosen by the user, or None if the dialog was
  /// canceled.
  ///
  /// The `browserWindow` argument allows the dialog to attach itself to a
  /// parent window, making it modal.
  ///
  /// Note: On macOS, using the asynchronous version is recommended to avoid
  /// issues when expanding and collapsing the dialog.
  abstract showSaveDialogSync: browserWindow: BrowserWindow * options: SaveDialogOptions -> string option
  /// Display a Save dialog and returns the file path chosen by the user.
  abstract showSaveDialog: options: SaveDialogOptions -> Promise<SaveDialogResult>
  /// Display a Save dialog and returns the file path chosen by the user.
  ///
  /// The `browserWindow` argument allows the dialog to attach itself to a
  /// parent window, making it modal.
  abstract showSaveDialog: browserWindow: BrowserWindow * options: SaveDialogOptions -> Promise<SaveDialogResult>
  /// Shows a message box, it will block the process until the message box is
  /// closed. It returns the index of the clicked button.
  abstract showMessageBoxSync: options: MessageBoxOptions -> int
  /// Shows a message box, it will block the process until the message box is
  /// closed. It returns the index of the clicked button.
  ///
  /// The browserWindow argument allows the dialog to attach itself to a parent
  /// window, making it modal.
  abstract showMessageBoxSync: browserWindow: BrowserWindow * options: MessageBoxOptions -> int
  /// Shows a message box, it will block the process until the message box is closed.
  abstract showMessageBox: options: MessageBoxOptions -> Promise<MessageBoxResult>
  /// Shows a message box, it will block the process until the message box is closed.
  ///
  /// The browserWindow argument allows the dialog to attach itself to a parent
  /// window, making it modal.
  abstract showMessageBox: browserWindow: BrowserWindow * options: MessageBoxOptions -> Promise<MessageBoxResult>
  /// Displays a modal dialog that shows an error message.
  ///
  /// This API can be called safely before the `ready` event the app module
  /// emits, it is usually used to report errors in early stage of startup. If
  /// called before the app `ready` event on Linux, the message will be emitted
  /// to stderr, and no GUI dialog will appear.
  abstract showErrorBox: title: string * content: string -> unit
  /// [macOS, Windows] Displays a modal dialog that shows a message and
  /// certificate information, and gives the user the option of
  /// trusting/importing the certificate.
  abstract showCertificateTrustDialog: options: CertificateTrustDialogOptions -> Promise<unit>
  /// [macOS] Displays a modal dialog that shows a message and certificate
  /// information, and gives the user the option of trusting/importing the
  /// certificate.
  abstract showCertificateTrustDialog: browserWindow: BrowserWindow * options: CertificateTrustDialogOptions -> Promise<unit>


[<StringEnum; RequireQualifiedAccess>]
type DisplayTouchSupport =
  | Available
  | Unavailable
  | Unknown

[<StringEnum; RequireQualifiedAccess>]
type DisplayAccelerometerSupport =
  | Available
  | Unavailable
  | Unknown

type Display =
  /// Unique identifier associated with the display.
  abstract id: int with get, set
  /// Can be 0, 90, 180, 270, represents screen rotation in clock-wise degrees.
  abstract rotation: int with get, set
  /// Output device's pixel scale factor.
  abstract scaleFactor: float with get, set
  abstract touchSupport: DisplayTouchSupport with get, set
  /// Whether or not the display is a monochrome display.
  abstract monochrome: bool with get, set
  abstract accelerometerSupport: DisplayAccelerometerSupport with get, set
  /// Represents a color space (three-dimensional object which contains all
  /// realizable color combinations) for the purpose of color conversions
  abstract colorSpace: string with get, set
  /// The number of bits per pixel.
  abstract colorDepth: int with get, set
  /// The number of bits per color component.
  abstract depthPerComponent: int with get, set
  abstract bounds: Rectangle with get, set
  abstract size: Size with get, set
  abstract workArea: Rectangle with get, set
  abstract workAreaSize: Size with get, set
  /// `true` for an internal display and `false` for an external display
  abstract ``internal``: bool with get, set

[<StringEnum; RequireQualifiedAccess>]
type DownloadItemState =
  | Progressing
  | Completed
  | Cancelled
  | Interrupted

[<StringEnum; RequireQualifiedAccess>]
type DownloadItemDoneState =
  /// The download completed successfully.
  | Completed
  /// The download has been cancelled.
  | Cancelled
  /// The download has interrupted and can not resume.
  | Interrupted

[<StringEnum; RequireQualifiedAccess>]
type DownloadItemUpdatedState =
  /// The download is in-progress.
  | Progressing
  /// The download has interrupted and can be resumed.
  | Interrupted

type DownloadItem =
  inherit EventEmitter<DownloadItem>
  /// Emitted when the download has been updated and is not done.
  [<Emit "$0.on('updated',$1)">] abstract onUpdated: listener: (Event -> DownloadItemUpdatedState -> unit) -> DownloadItem
  /// See onUpdated.
  [<Emit "$0.once('updated',$1)">] abstract onceUpdated: listener: (Event -> DownloadItemUpdatedState -> unit) -> DownloadItem
  /// See onUpdated.
  [<Emit "$0.addListener('updated',$1)">] abstract addListenerUpdated: listener: (Event -> DownloadItemUpdatedState -> unit) -> DownloadItem
  /// See onUpdated.
  [<Emit "$0.removeListener('updated',$1)">] abstract removeListenerUpdated: listener: (Event -> DownloadItemUpdatedState -> unit) -> DownloadItem
  /// Emitted when the download is in a terminal state. This includes a
  /// completed download, a cancelled download (via downloadItem.cancel()), and
  /// interrupted download that can't be resumed.
  [<Emit "$0.on('done',$1)">] abstract onDone: listener: (Event -> DownloadItemDoneState -> unit) -> DownloadItem
  /// See onDone.
  [<Emit "$0.once('done',$1)">] abstract onceDone: listener: (Event -> DownloadItemDoneState -> unit) -> DownloadItem
  /// See onDone.
  [<Emit "$0.addListener('done',$1)">] abstract addListenerDone: listener: (Event -> DownloadItemDoneState -> unit) -> DownloadItem
  /// See onDone.
  [<Emit "$0.removeListener('done',$1)">] abstract removeListenerDone: listener: (Event -> DownloadItemDoneState -> unit) -> DownloadItem
  /// Set the save file path of the download item.
  ///
  /// The API is only available in session's `will-download` callback function.
  /// If user doesn't set the save path via the API, Electron will use the
  /// original routine to determine the save path (usually prompts a save
  /// dialog).
  abstract setSavePath: path: string -> unit
  /// Returns the save path of the download item. This will be either the path
  /// set via downloadItem.setSavePath(path) or the path selected from the shown
  /// save dialog.
  abstract getSavePath: unit -> string
  /// Set the save file dialog options.
  ///
  /// This API allows the user to set custom options for the save dialog that
  /// opens for the download item by default. The API is only available in
  /// session's `will-download` callback function.
  abstract setSaveDialogOptions: options: SaveDialogOptions -> unit
  /// Returns the object previously set by
  /// downloadItem.setSaveDialogOptions(options)
  abstract getSaveDialogOptions: unit -> SaveDialogOptions
  /// Pauses the download.
  abstract pause: unit -> unit
  /// Indicates whether the download is paused.
  abstract isPaused: unit -> bool
  /// Resumes the download that has been paused.
  ///
  /// Note: To enable resumable downloads the server you are downloading from
  /// must support range requests and provide both Last-Modified and ETag header
  /// values. Otherwise resume() will dismiss previously received bytes and
  /// restart the download from the beginning.
  abstract resume: unit -> unit
  /// Indicates whether the download can resume.
  abstract canResume: unit -> bool
  /// Cancels the download operation.
  abstract cancel: unit -> unit
  /// Returns the origin url where the item is downloaded from.
  abstract getURL: unit -> string
  /// Returns the files mime type.
  abstract getMimeType: unit -> string
  /// Indicates whether the download has user gesture.
  abstract hasUserGesture: unit -> bool
  /// Returns the file name of the download item.
  ///
  /// Note: The file name is not always the same as the actual one saved in
  /// local disk. If user changes the file name in a prompted download saving
  /// dialog, the actual name of saved file will be different.
  abstract getFilename: unit -> string
  /// Returns the total size in bytes of the download item.
  ///
  /// If the size is unknown, it returns 0.
  abstract getTotalBytes: unit -> int
  /// Returns the received bytes of the download item.
  abstract getReceivedBytes: unit -> int
  /// Returns the Content-Disposition field from the response header.
  abstract getContentDisposition: unit -> string
  /// Returns the current state.
  abstract getState: unit -> DownloadItemState
  /// Returns the complete url chain of the item including any redirects.
  ///
  /// May be useful to resume a cancelled item when session is restarted.
  abstract getURLChain: unit -> string []
  /// Returns the Last-Modified header value.
  ///
  /// May be useful to resume a cancelled item when session is restarted.
  abstract getLastModifiedTime: unit -> string
  /// Returns the ETag header value.
  ///
  /// May be useful to resume a cancelled item when session is restarted.
  abstract getETag: unit -> string
  /// Returns the number of seconds since the UNIX epoch when the download was
  /// started.
  ///
  /// May be useful to resume a cancelled item when session is restarted.
  abstract getStartTime: unit -> float
  

type FileFilter =
  abstract name: string with get, set
  /// Extensions without dots or wildcards (e.g. "png", but not ".png" or
  /// "*.png"). To show all files, use "*".
  abstract extensions: string [] with get, set

type GlobalShortcut =
  inherit EventEmitter<GlobalShortcut>
  /// Registers a global shortcut. The callback is called when the registered
  /// shortcut is pressed by the user. The returned value indicates whether or
  /// not the shortcut was registered successfully.
  ///
  /// When the accelerator is already taken by other applications, this call
  /// will silently fail. This behavior is intended by operating systems, since
  /// they don't want applications to fight for global shortcuts.
  abstract register: accelerator: string * callback: (unit -> unit) -> bool
  /// Registers multiple global shortcuts. The callback is called when any of
  /// the registered shortcuts are pressed by the user.
  ///
  /// When a given accelerator is already taken by other applications, this call
  /// will silently fail. This behavior is intended by operating systems, since
  /// they don't want applications to fight for global shortcuts.
  abstract registerAll: accelerators: string [] * callback: (Event -> unit) -> unit
  /// Returns a value indicating whether this application has registered the
  /// accelerator.
  ///
  /// When the accelerator is already taken by other applications, this call
  /// will still return false. This behavior is intended by operating systems,
  /// since they don't want applications to fight for global shortcuts.
  abstract isRegistered: accelerator: string -> bool
  /// Unregisters the global shortcut of `accelerator`.
  abstract unregister: accelerator: string -> unit
  /// Unregisters all of the global shortcuts.
  abstract unregisterAll: unit -> unit

type GPUFeatureStatus =
  /// Canvas.
  abstract ``2d_canvas``: string with get, set
  /// Flash.
  abstract flash_3d: string with get, set
  /// Flash Stage3D.
  abstract flash_stage3d: string with get, set
  /// Flash Stage3D Baseline profile.
  abstract flash_stage3d_baseline: string with get, set
  /// Compositing.
  abstract gpu_compositing: string with get, set
  /// Multiple Raster Threads.
  abstract multiple_raster_threads: string with get, set
  /// Native GpuMemoryBuffers.
  abstract native_gpu_memory_buffers: string with get, set
  /// Rasterization.
  abstract rasterization: string with get, set
  /// Video Decode.
  abstract video_decode: string with get, set
  /// Video Encode.
  abstract video_encode: string with get, set
  /// VPx Video Decode.
  abstract vpx_decode: string with get, set
  /// WebGL.
  abstract webgl: string with get, set
  /// WebGL2.
  abstract webgl2: string with get, set

type InAppPurchase =
  inherit EventEmitter<InAppPurchase>
  /// Emitted when one or more transactions have been updated.
  [<Emit "$0.on('transactions-updated',$1)">] abstract onTransactionsUpdated: listener: (Event -> Transaction [] -> unit) -> InAppPurchase
  /// See onTransactionsUpdated.
  [<Emit "$0.once('transactions-updated',$1)">] abstract onceTransactionsUpdated: listener: (Event -> Transaction [] -> unit) -> InAppPurchase
  /// See onTransactionsUpdated.
  [<Emit "$0.addListener('transactions-updated',$1)">] abstract addListenerTransactionsUpdated: listener: (Event -> Transaction [] -> unit) -> InAppPurchase
  /// See onTransactionsUpdated.
  [<Emit "$0.removeListener('transactions-updated',$1)">] abstract removeListenerTransactionsUpdated: listener: (Event -> Transaction [] -> unit) -> InAppPurchase
  /// <summary>
  ///   Purchases a product.
  ///
  ///   You should listen for the `transactions-updated` event as soon as
  ///   possible and certainly before you call `purchaseProduct`.
  ///
  ///   Returns a promise that resolves with `true` if the product is valid and
  ///   added to the payment queue.
  /// </summary>
  /// <param name="productID">
  ///   The identifiers of the product to purchase. (The identifier of
  ///   com.example.app.product1 is product1).
  /// </param>
  /// <param name="quantity">
  ///   The number of items the user wants to purchase.
  /// </param>
  abstract purchaseProduct: productID: string * ?quantity: int -> Promise<bool>
  /// <summary>
  ///   Retrieves the product descriptions.
  /// </summary>
  /// <param name="productIDs">The identifiers of the products to get.</param>
  abstract getProducts: productIDs: string [] -> Promise<Product []>
  /// Indicates whether a user can make a payment.
  abstract canMakePayments: unit -> bool
  /// Returns the path to the receipt.
  abstract getReceiptURL: unit -> string
  /// Completes all pending transactions.
  abstract finishAllTransactions: unit -> unit
  /// Completes the pending transactions corresponding to the ISO formatted
  /// date.
  abstract finishTransactionByDate: date: string -> unit

type IncomingMessage =
  inherit Node.Stream.Readable<obj>
  inherit EventEmitter<IncomingMessage>
  /// The `data` event is the usual method of transferring response data into
  /// applicative code.
  [<Emit "$0.on('data',$1)">] abstract onData: listener: (Buffer -> unit) -> IncomingMessage
  /// See onData.
  [<Emit "$0.once('data',$1)">] abstract onceData: listener: (Buffer -> unit) -> IncomingMessage
  /// See onData.
  [<Emit "$0.addListener('data',$1)">] abstract addListenerData: listener: (Buffer -> unit) -> IncomingMessage
  /// See onData.
  [<Emit "$0.removeListener('data',$1)">] abstract removeListenerData: listener: (Buffer -> unit) -> IncomingMessage
  /// Indicates that response body has ended.
  [<Emit "$0.on('end',$1)">] abstract onEnd: listener: (Event -> unit) -> IncomingMessage
  /// See onEnd.
  [<Emit "$0.once('end',$1)">] abstract onceEnd: listener: (Event -> unit) -> IncomingMessage
  /// See onEnd.
  [<Emit "$0.addListener('end',$1)">] abstract addListenerEnd: listener: (Event -> unit) -> IncomingMessage
  /// See onEnd.
  [<Emit "$0.removeListener('end',$1)">] abstract removeListenerEnd: listener: (Event -> unit) -> IncomingMessage
  /// Emitted when a request has been canceled during an ongoing HTTP
  /// transaction.
  [<Emit "$0.on('aborted',$1)">] abstract onAborted: listener: (Event -> unit) -> IncomingMessage
  /// See onAborted.
  [<Emit "$0.once('aborted',$1)">] abstract onceAborted: listener: (Event -> unit) -> IncomingMessage
  /// See onAborted.
  [<Emit "$0.addListener('aborted',$1)">] abstract addListenerAborted: listener: (Event -> unit) -> IncomingMessage
  /// See onAborted.
  [<Emit "$0.removeListener('aborted',$1)">] abstract removeListenerAborted: listener: (Event -> unit) -> IncomingMessage
  /// Emitted when an error was encountered while streaming response data
  /// events. For instance, if the server closes the underlying while the
  /// response is still streaming, an `error` event will be emitted on the
  /// response object and a `close` event will subsequently follow on the
  /// request object.
  [<Emit "$0.on('error',$1)">] abstract onError: listener: (Event -> Error -> unit) -> IncomingMessage  // TODO: is Event correct, or only Error?
  /// See onError.
  [<Emit "$0.once('error',$1)">] abstract onceError: listener: (Event -> Error -> unit) -> IncomingMessage
  /// See onError.
  [<Emit "$0.addListener('error',$1)">] abstract addListenerError: listener: (Event -> Error -> unit) -> IncomingMessage
  /// See onError.
  [<Emit "$0.removeListener('error',$1)">] abstract removeListenerError: listener: (Event -> Error -> unit) -> IncomingMessage
  /// The HTTP response status code.
  abstract statusCode: int with get, set
  /// The HTTP status message.
  abstract statusMessage: string with get, set
  /// the response HTTP headers. The headers object is formatted as follows: 1)
  /// All header names are lowercased. 2) Each header name produces an
  /// array-valued property on the headers object. 3) Each header value is
  /// pushed into the array associated with its header name.
  abstract headers: obj option with get, set
  /// The HTTP protocol version number. Typical values are '1.0' or '1.1'.
  /// Additionally `httpVersionMajor` and `httpVersionMinor` are two
  /// Integer-valued readable properties that return respectively the HTTP major
  /// and minor version numbers.
  abstract httpVersion: string with get, set
  /// The HTTP protocol major version number.
  abstract httpVersionMajor: int with get, set
  /// The HTTP protocol minor version number.
  abstract httpVersionMinor: int with get, set

type IOCounters =
  /// Then number of I/O other operations.
  abstract otherOperationCount: float with get, set
  /// Then number of I/O other transfers.
  abstract otherTransferCount: float with get, set
  /// The number of I/O read operations.
  abstract readOperationCount: float with get, set
  /// The number of I/O read transfers.
  abstract readTransferCount: float with get, set
  /// The number of I/O write operations.
  abstract writeOperationCount: float with get, set
  /// The number of I/O write transfers.
  abstract writeTransferCount: float with get, set

type IpcMain =
  inherit EventEmitter<IpcMain>
  /// Listens to channel, when a new message arrives listener would be called
  /// with listener(event, args...).
  abstract on: channel: string * listener: (IpcMainEvent -> obj [] -> unit) -> IpcMain
  /// Adds a one time listener function for the event. This listener is invoked
  /// only the next time a message is sent to channel, after which it is
  /// removed.
  abstract once: channel: string * listener: (IpcMainEvent -> obj [] -> unit) -> IpcMain
  /// Removes listeners of the specified channel.
  abstract removeAllListeners: channel: string -> IpcMain
  /// Removes the specified listener from the listener array for the specified
  /// channel.
  abstract removeListener: channel: string * listener: (IpcMainEvent -> obj [] -> unit) -> IpcMain

type IpcRenderer =
  inherit EventEmitter<IpcRenderer>
  /// Listens to channel, when a new message arrives listener would be called
  /// with listener(event, args...).
  abstract on: channel: string * listener: (IpcRendererEvent -> obj [] -> unit) -> IpcRenderer
  /// Adds a one time listener function for the event. This listener is invoked
  /// only the next time a message is sent to channel, after which it is
  /// removed.
  abstract once: channel: string * listener: (IpcRendererEvent -> obj [] -> unit) -> IpcRenderer
  /// Removes all listeners, or those of the specified channel.
  abstract removeAllListeners: channel: string -> IpcRenderer
  /// Removes the specified listener from the listener array for the specified
  /// channel.
  abstract removeListener: channel: string * listener: (IpcRendererEvent -> obj [] -> unit) -> IpcRenderer
  /// Send a message to the main process asynchronously via channel, you can
  /// also send arbitrary arguments. Arguments will be serialized in JSON
  /// internally and hence no functions or prototype chain will be included.
  ///
  /// The main process handles it by listening for channel with ipcMain module.
  abstract send: channel: string * [<ParamArray>] args: obj [] -> unit
  /// Send a message to the main process synchronously via channel, you can also
  /// send arbitrary arguments. Arguments will be serialized in JSON internally
  /// and hence no functions or prototype chain will be included.
  ///
  /// The main process handles it by listening for channel with ipcMain module,
  /// and replies by setting event.returnValue.
  ///
  /// Note: Sending a synchronous message will block the whole renderer process,
  /// unless you know what you are doing you should never use it.
  abstract sendSync: channel: string * [<ParamArray>] args: obj [] -> obj option
  /// Sends a message to a window with `webContentsId` via `channel`.
  abstract sendTo: webContentsId: int * channel: string * [<ParamArray>] args: obj [] -> unit

[<StringEnum; RequireQualifiedAccess>]
type JumpListCategoryType =
  /// Items in this category will be placed into the standard Tasks category.
  /// There can be only one such category, and it will always be displayed at
  /// the bottom of the Jump List.
  | Tasks
  /// Displays a list of files frequently opened by the app. The name of the
  /// category and its items are set by Windows.
  | Frequent
  /// Displays a list of files recently opened by the app. The name of the
  /// category and its items are set by Windows. Items may be added to this
  /// category indirectly using app.addRecentDocument(path).
  | Recent
  /// Displays tasks or file links. JumpListCategory.name must be set by the
  /// app.
  | Custom

type JumpListCategory =
  abstract ``type``: JumpListCategoryType with get, set
  /// Must be set if `type` JumpListCategoryType.Custom, otherwise it should be
  /// omitted.
  abstract name: string with get, set
  /// Must be set if `type` is JumpListCategoryType.Tasks or
  /// JumpListCategoryType.Custom, otherwise it should be omitted.
  abstract items: JumpListItem [] with get, set

[<StringEnum; RequireQualifiedAccess>]
type JumpListItemType =
  /// A task will launch an app with specific arguments.
  | Task
  /// Can be used to separate items in the standard Tasks category.
  | Separator
  /// A file link will open a file using the app that created the Jump List, for
  /// this to work the app must be registered as a handler for the file type
  /// (though it doesn't have to be the default handler).
  | File

type JumpListItem =
  abstract ``type``: JumpListItemType option with get, set
  /// Path of the file to open, should only be set if `type` is
  /// JumpListItemType.File.
  abstract path: string option with get, set
  /// Path of the program to execute, usually you should specify
  /// `process.execPath` which opens the current program. Should only be set if
  /// `type` is JumpListItemType.Task.
  abstract program: string option with get, set
  /// The command line arguments when `program` is executed. Should only be set
  /// if `type` is JumpListItemType.Task.
  abstract args: string option with get, set
  /// The text to be displayed for the item in the Jump List. Should only be set
  /// if `type` is JumpListItemType.Task.
  abstract title: string option with get, set
  /// Description of the task (displayed in a tooltip). Should only be set if
  /// `type` is JumpListItemType.Task.
  abstract description: string option with get, set
  /// The absolute path to an icon to be displayed in a Jump List, which can be
  /// an arbitrary resource file that contains an icon (e.g. .ico, .exe, .dll).
  /// You can usually specify `process.execPath` to show the program icon.
  abstract iconPath: string option with get, set
  /// The index of the icon in the resource file. If a resource file contains
  /// multiple icons this value can be used to specify the zero-based index of
  /// the icon that should be displayed for this task. If a resource file
  /// contains only one icon, this property should be set to zero.
  abstract iconIndex: int option with get, set
  /// The working directory. Default is empty.
  abstract workingDirectory: string option with get, set

type MemoryUsageDetails =
  abstract count: int with get, set
  abstract size: float with get, set
  abstract liveSize: float with get, set

type Menu =
  /// Emitted when menu.popup() is called.
  [<Emit "$0.on('menu-will-show',$1)">] abstract onMenuWillShow: listener: (Event -> unit) -> Menu
  /// See onMenuWillShow.
  [<Emit "$0.once('menu-will-show',$1)">] abstract onceMenuWillShow: listener: (Event -> unit) -> Menu
  /// See onMenuWillShow.
  [<Emit "$0.addListener('menu-will-show',$1)">] abstract addListenerMenuWillShow: listener: (Event -> unit) -> Menu
  /// See onMenuWillShow.
  [<Emit "$0.removeListener('menu-will-show',$1)">] abstract removeListenerMenuWillShow: listener: (Event -> unit) -> Menu
  /// Emitted when a popup is closed either manually or with menu.closePopup().
  [<Emit "$0.on('menu-will-close',$1)">] abstract onMenuWillClose: listener: (Event -> unit) -> Menu
  /// See onMenuWillClose.
  [<Emit "$0.once('menu-will-close',$1)">] abstract onceMenuWillClose: listener: (Event -> unit) -> Menu
  /// See onMenuWillClose.
  [<Emit "$0.addListener('menu-will-close',$1)">] abstract addListenerMenuWillClose: listener: (Event -> unit) -> Menu
  /// See onMenuWillClose.
  [<Emit "$0.removeListener('menu-will-close',$1)">] abstract removeListenerMenuWillClose: listener: (Event -> unit) -> Menu
  /// Pops up this menu as a context menu in the BrowserWindow.
  abstract popup: ?options: PopupOptions -> unit
  /// Closes the context menu in the browserWindow.
  abstract closePopup: ?browserWindow: BrowserWindow -> unit
  /// Appends the menuItem to the menu.
  abstract append: menuItem: MenuItem -> unit
  /// Returns the item with the specified id.
  abstract getMenuItemById: id: string -> MenuItem option
  /// Inserts the menuItem to the `pos` position of the menu.
  abstract insert: pos: int * menuItem: MenuItem -> unit
  /// Returns the menu's items.
  abstract items: MenuItem [] with get, set

type MenuStatic =
  [<EmitConstructor>] abstract Create: unit -> Menu
  /// [macOS] Sends the `action` to the first responder of application. This is
  /// used for emulating default macOS menu behaviors. Usually you would use the
  /// `role` property of a MenuItem.
  ///
  /// See the macOS Cocoa Event Handling Guide for more information on macOS'
  /// native actions:
  /// https://developer.apple.com/library/archive/documentation/Cocoa/Conceptual/EventOverview/EventArchitecture/EventArchitecture.html#//apple_ref/doc/uid/10000060i-CH3-SW7
  abstract sendActionToFirstResponder: action: string -> unit
  /// Generally, the template is an array of MenuItemOptions for constructing a
  /// MenuItem.
  ///
  /// You can also attach other fields to the elements of the template and they
  /// will become properties of the constructed menu items.
  abstract buildFromTemplate: template: U2<MenuItemOptions, MenuItem> [] -> Menu

type MenuItem =
  /// The item's unique id, this property can be dynamically changed.
  abstract id: string option with get, set
  /// The item's visible label, this property can be dynamically changed.
  abstract label: string option with get, set
  /// Fired when the MenuItem receives a click event. It can be called with
  /// menuItem.click(event, focusedWindow, focusedWebContents).
  abstract click: (MenuItem -> BrowserWindow -> Event -> unit) option with get, set
  /// The menu item's submenu, if present.
  abstract submenu: Menu option with get, set
  /// The type of the item.
  abstract ``type``: MenuItemType option with get, set
  /// The item's role, if set.
  abstract role: MenuItemRole option with get, set
  /// The item's accelerator, if set.
  abstract accelerator: string option with get, set
  /// The item's icon, if set.
  abstract icon: U2<NativeImage, string> option with get, set
  /// The item's sublabel, this property can be dynamically changed.
  abstract sublabel: string option with get, set
  /// Indicates whether the item is enabled, this property can be dynamically
  /// changed.
  abstract enabled: bool option with get, set
  /// Indicates whether the item is visible, this property can be dynamically
  /// changed.
  abstract visible: bool option with get, set
  /// Indicates whether the item is checked, this property can be dynamically
  /// changed.
  ///
  /// A MenuItemType.Checkbox menu item will toggle the `checked` property on
  /// and off when selected.
  ///
  /// A MenuItemType.Radio menu item will turn on its `checked` property when
  /// clicked, and will turn off that property for all adjacent items in the
  /// same menu.
  ///
  /// You can add a `click` function for additional behavior.
  abstract ``checked``: bool option with get, set
  /// Indicates if the accelerator should be registered with the system or just
  /// displayed, this property can be dynamically changed.
  abstract registerAccelerator: bool option with get, set
  /// An item's sequential unique id.
  abstract commandId: int option with get, set
  /// A Menu that the item is a part of.
  abstract menu: Menu option with get, set

type MenuItemStatic =
  [<EmitConstructor>] abstract Create: options: MenuItemOptions -> MenuItem

type MimeTypedBuffer =
  /// The actual Buffer content.
  abstract data: Buffer with get, set
  /// The mimeType of the Buffer that you are sending.
  abstract mimeType: string with get, set
  abstract charset: string with get, set

type NativeImage =
  /// Returns a Buffer that contains the image's PNG encoded data.
  abstract toPNG: ?options: ToPNGOptions -> Buffer
  /// Returns a Buffer that contains the image's JPEG encoded data. `quality`
  /// must be between 0 and 100.
  abstract toJPEG: quality: int -> Buffer
  /// Returns a Buffer that contains a copy of the image's raw bitmap pixel
  /// data.
  abstract toBitmap: ?options: ToBitmapOptions -> Buffer
  /// Returns the data URL of the image.
  abstract toDataURL: ?options: ToDataURLOptions -> string
  /// Returns a Buffer that contains the image's raw bitmap pixel data.
  ///
  /// The difference between getBitmap() and toBitmap() is, getBitmap() does not
  /// copy the bitmap data, so you have to use the returned Buffer immediately
  /// in current event loop tick, otherwise the data might be changed or
  /// destroyed.
  abstract getBitmap: ?options: GetBitmapOptions -> Buffer
  /// Returns a Buffer that stores C pointer to underlying native handle of the
  /// image. On macOS, a pointer to NSImage instance would be returned.
  ///
  /// [macOS] Notice that the returned pointer is a weak pointer to the
  /// underlying native image instead of a copy, so you must ensure that the
  /// associated nativeImage instance is kept around.
  abstract getNativeHandle: unit -> Buffer
  /// Indicates whether the image is empty.
  abstract isEmpty: unit -> bool
  abstract getSize: unit -> Size
  /// Marks the image as a template image.
  abstract setTemplateImage: option: bool -> unit
  /// Indicates whether the image is a template image.
  abstract isTemplateImage: unit -> bool
  /// Returns a cropped image.
  abstract crop: rect: Rectangle -> NativeImage
  /// Returns a resized image.
  ///
  /// If only the height or the width are specified then the current aspect
  /// ratio will be preserved in the resized image.
  abstract resize: options: ResizeOptions -> NativeImage
  /// Returns the image's aspect ratio.
  abstract getAspectRatio: unit -> float
  /// Add an image representation for a specific scale factor. This can be used
  /// to explicitly add different scale factor representations to an image. This
  /// can be called on empty images.
  abstract addRepresentation: options: AddRepresentationOptions -> unit

type NativeImageStatic =
  /// Creates an empty NativeImage instance.
  abstract createEmpty: unit -> NativeImage
  /// Creates a new NativeImage instance from a file located at `path`. This
  /// method returns an empty image if the path does not exist, cannot be read,
  /// or is not a valid image.
  abstract createFromPath: path: string -> NativeImage
  /// Creates a new `NativeImage` instance from `buffer` that contains the raw
  /// bitmap pixel data returned by `toBitmap()`. The specific format is
  /// platform-dependent.
  abstract createFromBitmap: buffer: Buffer * ?options: NativeImageFromBufferOptions -> NativeImage
  /// Creates a new NativeImage instance from buffer. Tries to decode as PNG or
  /// JPEG first.
  abstract createFromBuffer: buffer: Buffer * ?options: NativeImageFromBufferOptions -> NativeImage
  /// Creates a new NativeImage instance from `dataURL`.
  abstract createFromDataURL: dataURL: string -> NativeImage
  /// [macOS] Creates a new NativeImage instance from the NSImage that maps to
  /// the given image name. See here for a list of possible values:
  /// https://developer.apple.com/documentation/appkit/nsimagename?language=objc
  ///
  /// The first hslShift number is the absolute hue value for the image. 0 and 1
  /// map to 0 and 360 on the hue color wheel (red).
  ///
  /// The second hslShift number is a saturation shift for the image, with the
  /// following key values: 0 = remove all color. 0.5 = leave unchanged. 1 =
  /// fully saturate the image.
  ///
  /// The third hslShift number is a lightness shift for the image, with the
  /// following key values: 0 = remove all lightness (make all pixels black).
  /// 0.5 = leave unchanged. 1 = full lightness (make all pixels white).
  ///
  /// In some cases, the NSImageName doesn't match its string representation;
  /// one example of this is NSFolderImageName, whose string representation
  /// would actually be NSFolder. Therefore, you'll need to determine the
  /// correct string representation for your image before passing it in. See the
  /// Electron documentation of this method for more information.
  abstract createFromNamedImage: imageName: string * hslShift: float * float * float -> NativeImage

type Net =
  inherit EventEmitter<Net>
  /// Creates a ClientRequest instance using the provided options which are
  /// directly forwarded to the ClientRequest constructor. The net.request
  /// method would be used to issue both secure and insecure HTTP requests
  /// according to the specified protocol scheme in the options object.
  abstract request: options: U2<ClientRequestOptions, string> -> ClientRequest

type NetLog =
  inherit EventEmitter<NetLog>
  /// Starts recording network events to `path`.
  abstract startLogging: path: string -> unit
  /// Stops recording network events. If not called, net logging will
  /// automatically end when app quits.
  ///
  /// Returns a promise that resolves with a file path to which network logs
  /// were recorded.
  abstract stopLogging: unit -> Promise<string>
  /// Indicates whether network logs are recorded.
  abstract currentlyLogging: bool with get, set
  /// The path to the current log file.
  abstract currentlyLoggingPath: string option with get, set

type Notification =
  inherit EventEmitter<Notification>
  /// Emitted when the notification is shown to the user, note this could be
  /// fired multiple times as a notification can be shown multiple times through
  /// the show() method.
  [<Emit "$0.on('show',$1)">] abstract onShow: listener: (Event -> unit) -> Notification
  /// See onShow.
  [<Emit "$0.once('show',$1)">] abstract onceShow: listener: (Event -> unit) -> Notification
  /// See onShow.
  [<Emit "$0.addListener('show',$1)">] abstract addListenerShow: listener: (Event -> unit) -> Notification
  /// See onShow.
  [<Emit "$0.removeListener('show',$1)">] abstract removeListenerShow: listener: (Event -> unit) -> Notification
  /// Emitted when the notification is clicked by the user.
  [<Emit "$0.on('click',$1)">] abstract onClick: listener: (Event -> unit) -> Notification
  /// See onClick.
  [<Emit "$0.once('click',$1)">] abstract onceClick: listener: (Event -> unit) -> Notification
  /// See onClick.
  [<Emit "$0.addListener('click',$1)">] abstract addListenerClick: listener: (Event -> unit) -> Notification
  /// See onClick.
  [<Emit "$0.removeListener('click',$1)">] abstract removeListenerClick: listener: (Event -> unit) -> Notification
  /// Emitted when the notification is closed by manual intervention from the
  /// user.
  ///
  /// This event is not guaranteed to be emitted in all cases where the
  /// notification is closed.
  [<Emit "$0.on('close',$1)">] abstract onClose: listener: (Event -> unit) -> Notification
  /// See onClose.
  [<Emit "$0.once('close',$1)">] abstract onceClose: listener: (Event -> unit) -> Notification
  /// See onClose.
  [<Emit "$0.addListener('close',$1)">] abstract addListenerClose: listener: (Event -> unit) -> Notification
  /// See onClose.
  [<Emit "$0.removeListener('close',$1)">] abstract removeListenerClose: listener: (Event -> unit) -> Notification
  /// [macOS] Emitted when the user clicks the "Reply" button on a notification
  /// with hasReply: true. The string contains the user input.
  [<Emit "$0.on('reply',$1)">] abstract onReply: listener: (Event -> string -> unit) -> Notification
  /// See onReply.
  [<Emit "$0.once('reply',$1)">] abstract onceReply: listener: (Event -> string -> unit) -> Notification
  /// See onReply.
  [<Emit "$0.addListener('reply',$1)">] abstract addListenerReply: listener: (Event -> string -> unit) -> Notification
  /// See onReply.
  [<Emit "$0.removeListener('reply',$1)">] abstract removeListenerReply: listener: (Event -> string -> unit) -> Notification
  /// [macOS] Called with the index of the action that was activated.
  [<Emit "$0.on('action',$1)">] abstract onAction: listener: (Event -> int -> unit) -> Notification
  /// See onAction.
  [<Emit "$0.once('action',$1)">] abstract onceAction: listener: (Event -> int -> unit) -> Notification
  /// See onAction.
  [<Emit "$0.addListener('action',$1)">] abstract addListenerAction: listener: (Event -> int -> unit) -> Notification
  /// See onAction.
  [<Emit "$0.removeListener('action',$1)">] abstract removeListenerAction: listener: (Event -> int -> unit) -> Notification
  /// Immediately shows the notification to the user, please note this means
  /// unlike the HTML5 Notification implementation, instantiating a new
  /// Notification does not immediately show it to the user, you need to call
  /// this method before the OS will display it.
  ///
  /// If the notification has been shown before, this method will dismiss the
  /// previously shown notification and create a new one with identical
  /// properties.
  abstract show: unit -> unit
  /// Dismisses the notification.
  abstract close: unit -> unit

type NotificationStatic =
  [<EmitConstructor>] abstract Create: options: NotificationOptions -> Notification
  /// Indicates whether or not desktop notifications are supported on the
  /// current system
  abstract isSupported: unit -> bool

type NotificationAction =
  /// The type of action, can be button.
  abstract ``type``: string with get, set
  /// The label for the given action.
  abstract text: string with get, set

type Point =
  abstract x: int with get, set
  abstract y: int with get, set


[<StringEnum; RequireQualifiedAccess>]
type PowerIdleState =
  | Active
  | Idle
  /// Available on supported systems only.
  | Locked
  | Unknown

type PowerMonitor =
  inherit EventEmitter<PowerMonitor>
  /// Emitted when the system is suspending.
  [<Emit "$0.on('suspend',$1)">] abstract onSuspend: listener: (Event -> unit) -> PowerMonitor
  /// See onSuspend.
  [<Emit "$0.once('suspend',$1)">] abstract onceSuspend: listener: (Event -> unit) -> PowerMonitor
  /// See onSuspend.
  [<Emit "$0.addListener('suspend',$1)">] abstract addListenerSuspend: listener: (Event -> unit) -> PowerMonitor
  /// See onSuspend.
  [<Emit "$0.removeListener('suspend',$1)">] abstract removeListenerSuspend: listener: (Event -> unit) -> PowerMonitor
  /// Emitted when system is resuming.
  [<Emit "$0.on('resume',$1)">] abstract onResume: listener: (Event -> unit) -> PowerMonitor
  /// See onResume.
  [<Emit "$0.once('resume',$1)">] abstract onceResume: listener: (Event -> unit) -> PowerMonitor
  /// See onResume.
  [<Emit "$0.addListener('resume',$1)">] abstract addListenerResume: listener: (Event -> unit) -> PowerMonitor
  /// See onResume.
  [<Emit "$0.removeListener('resume',$1)">] abstract removeListenerResume: listener: (Event -> unit) -> PowerMonitor
  /// [Windows] Emitted when the system changes to AC power.
  [<Emit "$0.on('on-ac',$1)">] abstract onOnAc: listener: (Event -> unit) -> PowerMonitor
  /// See onOnAc.
  [<Emit "$0.once('on-ac',$1)">] abstract onceOnAc: listener: (Event -> unit) -> PowerMonitor
  /// See onOnAc.
  [<Emit "$0.addListener('on-ac',$1)">] abstract addListenerOnAc: listener: (Event -> unit) -> PowerMonitor
  /// See onOnAc.
  [<Emit "$0.removeListener('on-ac',$1)">] abstract removeListenerOnAc: listener: (Event -> unit) -> PowerMonitor
  /// [Windows] Emitted when system changes to battery power.
  [<Emit "$0.on('on-battery',$1)">] abstract onOnBattery: listener: (Event -> unit) -> PowerMonitor
  /// See onOnBattery.
  [<Emit "$0.once('on-battery',$1)">] abstract onceOnBattery: listener: (Event -> unit) -> PowerMonitor
  /// See onOnBattery.
  [<Emit "$0.addListener('on-battery',$1)">] abstract addListenerOnBattery: listener: (Event -> unit) -> PowerMonitor
  /// See onOnBattery.
  [<Emit "$0.removeListener('on-battery',$1)">] abstract removeListenerOnBattery: listener: (Event -> unit) -> PowerMonitor
  /// [Linux, macOS] Emitted when the system is about to reboot or shut down. If
  /// the event handler invokes e.preventDefault(), Electron will attempt to
  /// delay system shutdown in order for the app to exit cleanly. If
  /// e.preventDefault() is called, the app should exit as soon as possible by
  /// calling something like app.quit().
  [<Emit "$0.on('shutdown',$1)">] abstract onShutdown: listener: (Event -> unit) -> PowerMonitor
  /// See onShutdown.
  [<Emit "$0.once('shutdown',$1)">] abstract onceShutdown: listener: (Event -> unit) -> PowerMonitor
  /// See onShutdown.
  [<Emit "$0.addListener('shutdown',$1)">] abstract addListenerShutdown: listener: (Event -> unit) -> PowerMonitor
  /// See onShutdown.
  [<Emit "$0.removeListener('shutdown',$1)">] abstract removeListenerShutdown: listener: (Event -> unit) -> PowerMonitor
  /// [macOS, Windows] Emitted when the system is about to lock the screen.
  [<Emit "$0.on('lock-screen',$1)">] abstract onLockScreen: listener: (Event -> unit) -> PowerMonitor
  /// See onLockScreen.
  [<Emit "$0.once('lock-screen',$1)">] abstract onceLockScreen: listener: (Event -> unit) -> PowerMonitor
  /// See onLockScreen.
  [<Emit "$0.addListener('lock-screen',$1)">] abstract addListenerLockScreen: listener: (Event -> unit) -> PowerMonitor
  /// See onLockScreen.
  [<Emit "$0.removeListener('lock-screen',$1)">] abstract removeListenerLockScreen: listener: (Event -> unit) -> PowerMonitor
  /// [macOS, Windows] Emitted as soon as the systems screen is unlocked.
  [<Emit "$0.on('unlock-screen',$1)">] abstract onUnlockScreen: listener: (Event -> unit) -> PowerMonitor
  /// See onUnlockScreen.
  [<Emit "$0.once('unlock-screen',$1)">] abstract onceUnlockScreen: listener: (Event -> unit) -> PowerMonitor
  /// See onUnlockScreen.
  [<Emit "$0.addListener('unlock-screen',$1)">] abstract addListenerUnlockScreen: listener: (Event -> unit) -> PowerMonitor
  /// See onUnlockScreen.
  [<Emit "$0.removeListener('unlock-screen',$1)">] abstract removeListenerUnlockScreen: listener: (Event -> unit) -> PowerMonitor
  /// Calculate the system idle state. idleThreshold is the amount of time (in
  /// seconds) before considered idle.
  abstract getSystemIdleState: idleThreshold: int -> PowerIdleState
  /// Calculate system idle time in seconds.
  abstract getSystemIdleTime: unit -> int


[<StringEnum; RequireQualifiedAccess>]
type PowerSaveBlockerType =
  /// Prevent the application from being suspended. Keeps system active but
  /// allows screen to be turned off. Example use cases: downloading a file or
  /// playing audio.
  | [<CompiledName("prevent-app-suspension")>] PreventAppSuspension
  /// Prevent the display from going to sleep. Keeps system and screen active.
  /// Example use case: playing video.
  | [<CompiledName("prevent-display-sleep")>] PreventDisplaySleep

type PowerSaveBlocker =
  inherit EventEmitter<PowerSaveBlocker>
  /// Starts preventing the system from entering lower-power mode. Returns an
  /// integer identifying the power save blocker.
  ///
  /// Note: PowerSaveBlockerType.PreventDisplaySleep has higher precedence over
  /// PowerSaveBlockerType.PreventAppSuspension. Only the highest precedence
  /// type takes effect. In other words, PreventDisplaySleep always takes
  /// precedence over PreventAppSuspension.
  ///
  /// For example, an API calling A requests for PreventAppSuspension, and
  /// another calling B requests for PreventDisplaySleep. PreventDisplaySleep
  /// will be used until B stops its request. After that, PreventAppSuspension
  /// is used.
  abstract start: ``type``: PowerSaveBlockerType -> int
  /// Stops the specified power save blocker.
  abstract stop: id: int -> unit
  /// Indicates whether the corresponding powerSaveBlocker has started.
  abstract isStarted: id: int -> bool

type PrinterInfo =
  abstract name: string with get, set
  abstract description: string with get, set
  abstract status: int with get, set
  abstract isDefault: bool with get, set

[<StringEnum; RequireQualifiedAccess>]
type ProcessType =
  /// The main process.
  | Browser
  /// A renderer process.
  | Renderer
  /// A web worker.
  | Worker

type ProcessVersions =
  inherit Node.Base.ProcessVersions
  /// Chrome's version string.
  abstract chrome: string with get
  /// Electron's version string.
  abstract electron: string with get

type Process =
  inherit Node.Process.Process
  inherit EventEmitter<Process>
  /// Emitted when Electron has loaded its internal initialization script and is
  /// beginning to load the web page or the main script.
  ///
  /// It can be used by the preload script to add removed Node global symbols
  /// back to the global scope when node integration is turned off:
  [<Emit "$0.on('loaded',$1)">] abstract onLoaded: listener: (Event -> unit) -> Process
  [<Emit "$0.once('loaded',$1)">] abstract onceLoaded: listener: (Event -> unit) -> Process
  [<Emit "$0.addListener('loaded',$1)">] abstract addListenerLoaded: listener: (Event -> unit) -> Process
  [<Emit "$0.removeListener('loaded',$1)">] abstract removeListenerLoaded: listener: (Event -> unit) -> Process
  /// When app is started by being passed as parameter to the default app, this
  /// property is Some true in the main process, otherwise it is None.
  abstract defaultApp: bool option with get, set
  /// true when the current renderer context is the "main" renderer frame. If
  /// you want the ID of the current frame you should use webFrame.routingId.
  abstract isMainFrame: bool with get, set
  /// For Mac App Store build, this property is true, for other builds it is
  /// None.
  abstract mas: bool option with get, set
  /// Controls ASAR support inside your application. Setting this to true will
  /// disable the support for asar archives in Node's built-in modules.
  abstract noAsar: bool with get, set
  /// Controls whether or not deprecation warnings are printed to stderr.
  /// Setting this to true will silence deprecation warnings. This property is
  /// used instead of the --no-deprecation command line flag.
  abstract noDeprecation: bool with get, set
  /// Controls whether or not deprecation warnings are printed to stderr when
  /// formerly callback-based APIs converted to Promises are invoked using
  /// callbacks. Setting this to true will enable deprecation warnings.
  abstract enablePromiseAPIs: bool with get, set
  /// The path to the resources directory.
  abstract resourcesPath: string with get, set
  /// When the renderer process is sandboxed, this property is true, otherwise
  /// it is None.
  abstract sandboxed: bool option with get, set
  /// Controls whether or not deprecation warnings will be thrown as exceptions.
  /// Setting this to true will throw errors for deprecations. This property is
  /// used instead of the --throw-deprecation command line flag.
  abstract throwDeprecation: bool with get, set
  /// Controls whether or not deprecations printed to stderr include their stack
  /// trace. Setting this to true will print stack traces for deprecations. This
  /// property is instead of the --trace-deprecation command line flag.
  abstract traceDeprecation: bool with get, set
  /// Controls whether or not process warnings printed to stderr include their
  /// stack trace. Setting this to true will print stack traces for process
  /// warnings (including deprecations). This property is instead of the
  /// --trace-warnings command line flag.
  abstract traceProcessWarnings: bool with get, set
  /// The current process's type.
  abstract ``type``: ProcessType with get, set
  abstract versions: ProcessVersions with get, set
  /// If the app is running as a Windows Store app (appx), this property is
  /// true, for otherwise it is None.
  abstract windowsStore: bool option with get, set
  /// Causes the main thread of the current process crash.
  abstract crash: unit -> unit
  /// Indicates the creation time of the application. The time is represented as
  /// number of milliseconds since epoch. It returns None if it is unable to get
  /// the process creation time.
  abstract getCreationTime: unit -> float option
  abstract getCPUUsage: unit -> CPUUsage
  /// [Windows, Linux]
  abstract getIOCounters: unit -> IOCounters
  /// Returns an object with V8 heap statistics. Note that all statistics are
  /// reported in Kilobytes.
  abstract getHeapStatistics: unit -> HeapStatistics
  /// Returns an object giving memory usage statistics about the current
  /// process. Note that all statistics are reported in Kilobytes. This api
  /// should be called after app ready.
  abstract getProcessMemoryInfo: unit -> Promise<ProcessMemoryInfo>
  /// Returns an object giving memory usage statistics about the entire  Note
  /// that all statistics are reported in Kilobytes.
  abstract getSystemMemoryInfo: unit -> SystemMemoryInfo  // TODO: does this return Promise, too?
  /// Returns the version of the host operating system.
  ///
  /// Note: It returns the actual operating system version instead of kernel
  /// version on macOS unlike `os.release()`.
  abstract getSystemVersion: unit -> string
  /// Takes a V8 heap snapshot and saves it to `filePath`.
  abstract takeHeapSnapshot: filePath: string -> bool
  /// Causes the main thread of the current process hang.
  abstract hang: unit -> unit
  /// [macOS, Linux] Sets the file descriptor soft limit to maxDescriptors or
  /// the OS hard limit, whichever is lower for the current process.
  abstract setFdLimit: maxDescriptors: int -> unit

type ProcessMetric =
  /// Process id of the process.
  abstract pid: int with get, set
  /// Process type (Browser or Tab or GPU etc).
  abstract ``type``: string with get, set
  /// CPU usage of the process.
  abstract cpu: CPUUsage with get, set

type Product =
  /// The string that identifies the product to the Apple App Store.
  abstract productIdentifier: string with get, set
  /// A description of the product.
  abstract localizedDescription: string with get, set
  /// The name of the product.
  abstract localizedTitle: string with get, set
  /// A string that identifies the version of the content.
  abstract contentVersion: string with get, set
  /// The total size of the content, in bytes.
  abstract contentLengths: int [] with get, set
  /// The cost of the product in the local currency.
  abstract price: float with get, set
  /// The locale formatted price of the product.
  abstract formattedPrice: string with get, set
  /// Indicates whether the App Store has downloadable content for this product.
  /// true if at least one file has been associated with the product.
  abstract isDownloadable: bool with get, set

type Protocol =
  inherit EventEmitter<Protocol>
  /// Registers the scheme as standard, secure, bypasses content security policy
  /// for resources, allows registering ServiceWorker and supports fetch API.
  ///
  /// Note: This method can only be used before the ready event of the app
  /// module gets emitted and can be called only once.
  ///
  /// Specify a privilege with the value of true to enable the capability.
  ///
  /// A standard scheme adheres to what RFC 3986 calls generic URI syntax. For
  /// example `http` and `https` are standard schemes, while `file` is not.
  ///
  /// Registering a scheme as standard, will allow relative and absolute
  /// resources to be resolved correctly when served. Otherwise the scheme will
  /// behave like the file protocol, but without the ability to resolve relative
  /// URLs.
  ///
  /// Registering a scheme as standard will allow access to files through the
  /// FileSystem API. Otherwise the renderer will throw a security error for the
  /// scheme.
  ///
  /// By default web storage apis (localStorage, sessionStorage, webSQL,
  /// indexedDB, cookies) are disabled for non standard schemes. So in general
  /// if you want to register a custom protocol to replace the http protocol,
  /// you have to register it as a standard scheme.
  abstract registerSchemesAsPrivileged: customSchemes: CustomScheme [] -> unit
  /// Registers a protocol of scheme that will send the file as a response. The
  /// handler will be called with handler(request, callback) when a request is
  /// going to be created with scheme. completion will be called with
  /// completion(null) when scheme is successfully registered or
  /// completion(error) when failed.
  ///
  /// To handle the request, the handler's callback should be called with either
  /// the file's path or an object that has a `path` property, e.g.
  /// callback(filePath) or callback({ path: filePath }). The object may also
  /// have a `headers` property which gives a map of headers to values for the
  /// response headers, e.g. callback({ path: filePath, headers:
  /// {"Content-Security-Policy": "default-src 'none'"]}).
  ///
  /// When callback is called with nothing, a number, or an object that has an
  /// `error` property, the request will fail with the error number you
  /// specified. For the available error numbers you can use, please see the net
  /// error list.
  ///
  /// By default the scheme is treated like http:, which is parsed differently
  /// than protocols that follow the "generic URI syntax" like file:, so you
  /// probably want to call protocol.registerSchemesAsPrivileged to have your scheme
  /// treated as a standard scheme.
  abstract registerFileProtocol: scheme: string * handler: (RegisterFileProtocolRequest -> (obj -> unit) -> unit) * ?completion: (Error option -> unit) -> unit
  /// Registers a protocol of `scheme` that will send a Buffer as a response.
  ///
  /// The usage is the same with registerFileProtocol, except for the handler's
  /// callback signature.
  abstract registerBufferProtocol: scheme: string * handler: (RegisterBufferProtocolRequest -> (Buffer -> unit) -> unit) * ?completion: (Error -> unit) -> unit
  /// Registers a protocol of `scheme` that will send a Buffer as a response.
  ///
  /// The usage is the same with registerFileProtocol, except for the handler's
  /// callback signature.
  abstract registerBufferProtocol: scheme: string * handler: (RegisterBufferProtocolRequest -> (MimeTypedBuffer -> unit) -> unit) * ?completion: (Error -> unit) -> unit
  /// Registers a protocol of `scheme` that will send a string as a response.
  ///
  /// The usage is the same with registerFileProtocol, except for the handler's
  /// callback signature.
  abstract registerStringProtocol: scheme: string * handler: (RegisterStringProtocolRequest -> (string -> unit) -> unit) * ?completion: (Error option -> unit) -> unit
  /// Registers a protocol of `scheme` that will send a string as a response.
  ///
  /// The usage is the same with registerFileProtocol, except for the handler's
  /// callback signature.
  abstract registerStringProtocol: scheme: string * handler: (RegisterStringProtocolRequest -> (MimeTypedBuffer -> unit) -> unit) * ?completion: (Error option -> unit) -> unit
  /// Registers a protocol of `scheme` that will send an HTTP request as a
  /// response.
  ///
  /// The usage is the same with registerFileProtocol, except for the handler's
  /// callback signature.
  ///
  /// By default the HTTP request will reuse the current session. If you want
  /// the request to have a different session you should set `session` to
  /// `null`.
  ///
  /// For POST requests the uploadData object must be provided.
  abstract registerHttpProtocol: scheme: string * handler: (RegisterHttpProtocolRequest -> (RedirectRequest -> unit) -> unit) * ?completion: (Error option -> unit) -> unit
  /// Registers a protocol of `scheme` that will send a `Readable` as a
  /// response.
  ///
  /// The usage is similar to the other register{Any}Protocol, except for the
  /// handler's callback signature.
  abstract registerStreamProtocol: scheme: string * handler: (RegisterStreamProtocolRequest -> (Node.Stream.Readable<'a> -> unit) -> unit) * ?completion: (Error -> unit) -> unit
  /// Registers a protocol of `scheme` that will send a `Readable` as a
  /// response.
  ///
  /// The usage is similar to the other register{Any}Protocol, except for the
  /// handler's callback signature.
  abstract registerStreamProtocol: scheme: string * handler: (RegisterStreamProtocolRequest -> (StreamProtocolResponse<'a> -> unit) -> unit) * ?completion: (Error -> unit) -> unit
  /// Unregisters the custom protocol of `scheme`.
  abstract unregisterProtocol: scheme: string * ?completion: (Error -> unit) -> unit
  /// Indicates whether there is already a handler for `scheme`.
  abstract isProtocolHandled: scheme: string -> Promise<bool>
  /// Intercepts `scheme` protocol and uses `handler` as the protocol's new
  /// handler which sends a file as a response.
  abstract interceptFileProtocol: scheme: string * handler: (InterceptFileProtocolRequest -> (string -> unit) -> unit) * ?completion: (Error option -> unit) -> unit
  /// Intercepts `scheme` protocol and uses `handler` as the protocol's new
  /// handler which sends a string as a response.
  abstract interceptStringProtocol: scheme: string * handler: (InterceptStringProtocolRequest -> (string -> unit) -> unit) * ?completion: (Error option -> unit) -> unit
  /// Intercepts `scheme` protocol and uses `handler` as the protocol's new
  /// handler which sends a Buffer as a response.
  abstract interceptBufferProtocol: scheme: string * handler: (InterceptBufferProtocolRequest -> (Buffer -> unit) -> unit) * ?completion: (Error option -> unit) -> unit
  /// Intercepts `scheme` protocol and uses `handler` as the protocol's new
  /// handler which sends a new HTTP request as a response.
  abstract interceptHttpProtocol: scheme: string * handler: (InterceptHttpProtocolRequest -> (RedirectRequest -> unit) -> unit) * ?completion: (Error option -> unit) -> unit
  /// Same as protocol.registerStreamProtocol, except that it replaces an
  /// existing protocol handler.
  abstract interceptStreamProtocol: scheme: string * handler: (InterceptStreamProtocolRequest -> (U2<Node.Stream.Readable<'a>, StreamProtocolResponse<'a>> -> unit) -> unit) * ?completion: (Error -> unit) -> unit
  /// Remove the interceptor installed for `scheme` and restore its original
  /// handler.
  abstract uninterceptProtocol: scheme: string * ?completion: (Error -> unit) -> unit

type Rectangle =
  /// The x coordinate of the origin of the rectangle.
  abstract x: int with get, set
  /// The y coordinate of the origin of the rectangle.
  abstract y: int with get, set
  /// The width of the rectangle.
  abstract width: int with get, set
  /// The height of the rectangle.
  abstract height: int with get, set

[<StringEnum; RequireQualifiedAccess>]
type ReferrerPolicy =
  | Default
  | [<CompiledName("unsafe-url")>] UnsafeUrl
  | [<CompiledName("no-referrer-when-downgrade")>] NoReferrerWhenDowngrade
  | [<CompiledName("no-referrer")>] NoReferrer
  | Origin
  | [<CompiledName("strict-origin-when-cross-origin")>] StrictOriginWhenCrossOrigin
  | [<CompiledName("same-origin")>] SameOrigin
  | [<CompiledName("strict-origin")>] StrictOrigin

type Referrer =
  /// HTTP Referrer URL.
  abstract url: string with get, set
  /// See the Referrer-Policy spec for more details on the meaning of these
  /// values:
  /// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
  abstract policy: ReferrerPolicy with get, set

type Remote =
  inherit MainInterface
  /// Returns the object returned by require(module) in the main process.
  /// Modules specified by their relative path will resolve relative to the
  /// entrypoint of the main process.
  abstract require: ``module``: string -> obj option
  /// Returns the window to which this web page belongs.
  ///
  /// Note: Do not use removeAllListeners on BrowserWindow. Use of this can
  /// remove all blur listeners, disable click events on touch bar buttons, and
  /// other unintended consequences.
  abstract getCurrentWindow: unit -> BrowserWindow
  /// Returns the web contents of this web page.
  abstract getCurrentWebContents: unit -> WebContents
  /// Returns the global variable of name (e.g. global[name]) in the main
  /// process.
  abstract getGlobal: name: string -> obj option
  /// The `process` object in the main process. This is the same as
  /// remote.getGlobal('process') but is cached.
  abstract ``process``: Process with get, set

[<StringEnum; RequireQualifiedAccess>]
type DisplayMetricChangeType =
  | Bounds
  | WorkArea
  | ScaleFactor
  | Rotation

type Screen =
  inherit EventEmitter<Screen>
  /// Emitted when a display has been added.
  [<Emit "$0.on('display-added',$1)">] abstract onDisplayAdded: listener: (Event -> Display -> unit) -> Screen
  /// See onDisplayAdded.
  [<Emit "$0.once('display-added',$1)">] abstract onceDisplayAdded: listener: (Event -> Display -> unit) -> Screen
  /// See onDisplayAdded.
  [<Emit "$0.addListener('display-added',$1)">] abstract addListenerDisplayAdded: listener: (Event -> Display -> unit) -> Screen
  /// See onDisplayAdded.
  [<Emit "$0.removeListener('display-added',$1)">] abstract removeListenerDisplayAdded: listener: (Event -> Display -> unit) -> Screen
  /// Emitted when a display has been removed.
  [<Emit "$0.on('display-removed',$1)">] abstract onDisplayRemoved: listener: (Event -> Display -> unit) -> Screen
  /// See onDisplayRemoved.
  [<Emit "$0.once('display-removed',$1)">] abstract onceDisplayRemoved: listener: (Event -> Display -> unit) -> Screen
  /// See onDisplayRemoved.
  [<Emit "$0.addListener('display-removed',$1)">] abstract addListenerDisplayRemoved: listener: (Event -> Display -> unit) -> Screen
  /// See onDisplayRemoved.
  [<Emit "$0.removeListener('display-removed',$1)">] abstract removeListenerDisplayRemoved: listener: (Event -> Display -> unit) -> Screen
  /// Emitted when one or more metrics change in a display.
  [<Emit "$0.on('display-metrics-changed',$1)">] abstract onDisplayMetricsChanged: listener: (Event -> Display -> DisplayMetricChangeType [] -> unit) -> Screen
  /// See onDisplayMetricsChanged.
  [<Emit "$0.once('display-metrics-changed',$1)">] abstract onceDisplayMetricsChanged: listener: (Event -> Display -> DisplayMetricChangeType [] -> unit) -> Screen
  /// See onDisplayMetricsChanged.
  [<Emit "$0.addListener('display-metrics-changed',$1)">] abstract addListenerDisplayMetricsChanged: listener: (Event -> Display -> DisplayMetricChangeType [] -> unit) -> Screen
  /// See onDisplayMetricsChanged.
  [<Emit "$0.removeListener('display-metrics-changed',$1)">] abstract removeListenerDisplayMetricsChanged: listener: (Event -> Display -> DisplayMetricChangeType [] -> unit) -> Screen
  /// The current absolute position of the mouse pointer.
  abstract getCursorScreenPoint: unit -> Point
  /// Returns the primary display.
  abstract getPrimaryDisplay: unit -> Display
  /// Returns the displays that are currently available.
  abstract getAllDisplays: unit -> Display []
  /// Returns the display nearest the specified point.
  abstract getDisplayNearestPoint: point: Point -> Display
  /// Returns the display that most closely intersects the provided bounds.
  abstract getDisplayMatching: rect: Rectangle -> Display
  /// [Windows] Converts a screen physical point to a screen DIP point. The DPI
  /// scale is performed relative to the display containing the physical point.
  abstract screenToDipPoint: point: Point -> Point
  /// [Windows] Converts a screen DIP point to a screen physical point. The DPI
  /// scale is performed relative to the display containing the DIP point.
  abstract dipToScreenPoint: point: Point -> Point
  /// [Windows] Converts a screen physical rect to a screen DIP rect. The DPI
  /// scale is performed relative to the display nearest to `window`. If
  /// `window` is None, scaling will be performed to the display nearest to
  /// `rect`.
  abstract screenToDipRect: window: BrowserWindow option * rect: Rectangle -> Rectangle
  /// [Windows] Converts a screen DIP rect to a screen physical rect. The DPI
  /// scale is performed relative to the display nearest to `window`. If
  /// `window` is nullNone, scaling will be performed to the display nearest to
  /// `rect`.
  abstract dipToScreenRect: window: BrowserWindow option * rect: Rectangle -> Rectangle

type ScrubberItem =
  /// The text to appear in this item.
  abstract label: string option with get, set
  /// The image to appear in this item.
  abstract icon: NativeImage option with get, set

type SegmentedControlSegment =
  /// The text to appear in this segment.
  abstract label: string option with get, set
  /// The image to appear in this segment.
  abstract icon: NativeImage option with get, set
  /// Whether this segment is selectable. Default: true.
  abstract enabled: bool option with get, set

[<StringEnum; RequireQualifiedAccess>]
type PermissionRequestHandlerPermission =
  | Media
  | Geolocation
  | Notifications
  | MidiSysex
  | PointerLock
  | Fullscreen
  | OpenExternal

[<StringEnum; RequireQualifiedAccess>]
type PermissionCheckHandlerPermission =
  | Media

type Session =
  inherit EventEmitter<Session>
  /// Emitted when Electron is about to download item in webContents.
  ///
  /// Calling event.preventDefault() will cancel the download and the
  /// DownloadItem will not be available from next tick of the process.
  [<Emit "$0.on('will-download',$1)">] abstract onWillDownload: listener: (Event -> DownloadItem -> WebContents -> unit) -> Session
  /// See onWillDownload.
  [<Emit "$0.once('will-download',$1)">] abstract onceWillDownload: listener: (Event -> DownloadItem -> WebContents -> unit) -> Session
  /// See onWillDownload.
  [<Emit "$0.addListener('will-download',$1)">] abstract addListenerWillDownload: listener: (Event -> DownloadItem -> WebContents -> unit) -> Session
  /// See onWillDownload.
  [<Emit "$0.removeListener('will-download',$1)">] abstract removeListenerWillDownload: listener: (Event -> DownloadItem -> WebContents -> unit) -> Session
  /// Returns the session's current cache size, in bytes.
  abstract getCacheSize: unit -> Promise<int>
  /// Clears the session’s HTTP cache.
  ///
  /// Returns a promise that resolves when the cache clear operation is
  /// complete.
  abstract clearCache: unit -> Promise<unit>
  /// Clears the data of web storages.
  ///
  /// Returns a promise that resolves when the storage data has been cleared.
  abstract clearStorageData: ?options: ClearStorageDataOptions -> Promise<unit>
  /// Writes any unwritten DOMStorage data to disk.
  abstract flushStorageData: unit -> unit
  /// Sets the proxy settings.
  ///
  /// Returns a promise that resolves when the proxy setting process is
  /// complete.
  ///
  /// See the Electron documentation for details:
  /// https://electronjs.org/docs/api/session#sessetproxyconfig
  abstract setProxy: config: ProxyConfig -> Promise<unit>
  /// Returns a promise that resolves with the proxy information for `url`.
  abstract resolveProxy: url: string -> Promise<string>
  /// Sets download saving directory. By default, the download directory will be
  /// the Downloads under the respective app folder.
  abstract setDownloadPath: path: string -> unit
  /// Emulates network with the given configuration for the session.
  abstract enableNetworkEmulation: options: EnableNetworkEmulationOptions -> unit
  /// Disables any network emulation already active for the session. Resets to
  /// the original network configuration.
  abstract disableNetworkEmulation: unit -> unit
  /// Sets the certificate verify proc for `session`, the `proc` will be called
  /// with proc(request, callback) whenever a server certificate verification is
  /// requested. Calling callback(0) accepts the certificate, calling
  /// callback(-2) rejects it, calling callback(-3) uses the verification result
  /// from chromium. See more codes here:
  /// https://cs.chromium.org/chromium/src/net/base/net_error_list.h
  ///
  /// Calling setCertificateVerifyProc(None) will revert back to default
  /// certificate verify proc.
  abstract setCertificateVerifyProc: proc: (CertificateVerifyProcRequest -> (int -> unit) -> unit) option -> unit
  /// Sets the handler which can be used to respond to permission requests for
  /// the session. Calling callback(true) will allow the permission and
  /// callback(false) will reject it. To clear the handler, call
  /// setPermissionRequestHandler(None).
  ///
  /// The WebContents parameter is the WebContents requesting the permission.
  /// Please note that if the request comes from a subframe you should use
  /// PermissionRequestHandlerDetails.requestingUrl to check the request origin.
  abstract setPermissionRequestHandler: handler: (WebContents -> PermissionRequestHandlerPermission -> (bool -> unit) -> PermissionRequestHandlerDetails -> unit) option -> unit
  /// Sets the handler which can be used to respond to permission checks for the
  /// session. Returning true will allow the permission and false will reject
  /// it. To clear the handler, call setPermissionCheckHandler(null). The third
  /// handler argument is `requestingOrigin`, the origin URL of the permission
  /// check.
  ///
  /// The WebContents parameter is the WebContents requesting the permission.
  /// Please note that if the request comes from a subframe you should use
  /// PermissionRequestHandlerDetails.requestingUrl to check the request origin.
  abstract setPermissionCheckHandler: handler: (WebContents -> PermissionCheckHandlerPermission -> string -> PermissionCheckHandlerDetails -> bool) option -> unit
  /// Clears the host resolver cache.
  ///
  /// Returns a promise that resolves when the operation is complete.
  abstract clearHostResolverCache: unit -> Promise<unit>
  /// Dynamically sets whether to always send credentials for HTTP NTLM or
  /// Negotiate authentication. `domains` is a comma-separated list of servers
  /// for which integrated authentication is enabled.
  abstract allowNTLMCredentialsForDomains: domains: string -> unit
  /// <summary>
  ///   Overrides the `userAgent` and `acceptLanguages` for this session.
  ///
  ///   This doesn't affect existing WebContents, and each WebContents can use
  ///   webContents.setUserAgent to override the session-wide user agent.
  /// </summary>
  /// <param name="userAgent"></param>
  /// <param name="acceptLanguages">
  ///   A comma separated ordered list of language codes, for example
  ///   "en-US,fr,de,ko,zh-CN,ja".
  /// </param>
  abstract setUserAgent: userAgent: string * ?acceptLanguages: string -> unit
  /// Returns the user agent for this session.
  abstract getUserAgent: unit -> string
  /// <param name="identifier">Valid UUID</param>
  abstract getBlobData: identifier: string -> Promise<Buffer>
  /// Allows resuming cancelled or interrupted downloads from previous Session.
  /// The API will generate a DownloadItem that can be accessed with the
  /// `will-download` event. The DownloadItem will not have any WebContents
  /// associated with it and the initial state will be interrupted. The download
  /// will start only when the resume API is called on the DownloadItem.
  abstract createInterruptedDownload: options: CreateInterruptedDownloadOptions -> unit
  /// Clears the session’s HTTP authentication cache.
  ///
  /// Returns a promise that resolves when the session’s HTTP authentication
  /// cache has been cleared.
  abstract clearAuthCache: unit -> Promise<unit>
  /// Adds scripts that will be executed on ALL web contents that are associated
  /// with this session just before normal `preload` scripts run.
  abstract setPreloads: preloads: string [] -> unit
  /// Returns an array of paths to preload scripts that have been registered.
  abstract getPreloads: unit -> string []
  /// A Cookies object for this session.
  abstract cookies: Cookies with get, set
  /// A WebRequest object for this session.
  abstract webRequest: WebRequest with get, set
  /// A Protocol object for this session.
  abstract protocol: Protocol with get, set
  /// A NetLog object for this session.
  abstract netLog: NetLog with get, set

type SessionStatic =
  /// Returns a session instance from `partition` string. When there is an
  /// existing Session with the same `partition`, it will be returned; otherwise
  /// a new Session instance will be created with `options`.
  ///
  /// If partition starts with `persist:`, the page will use a persistent
  /// session available to all pages in the app with the same partition. if
  /// there is no `persist:` prefix, the page will use an in-memory session. If
  /// the partition is empty then default session of the app will be returned.
  ///
  /// To create a Session with `options`, you have to ensure the Session with
  /// the partition has never been used before. There is no way to change the
  /// options of an existing Session object.
  abstract fromPartition: partition: string * ?options: FromPartitionOptions -> Session
  /// The default session object of the app.
  abstract defaultSession: Session with get, set

[<StringEnum; RequireQualifiedAccess>]
type WriteShortcutLinkOperation =
  /// Creates a new shortcut, overwriting if necessary.
  | Create
  /// Updates specified properties only on an existing shortcut.
  | Update
  /// Overwrites an existing shortcut, fails if the shortcut doesn't exist.
  | Replace

type Shell =
  /// Show the given file in a file manager. If possible, select the file.
  abstract showItemInFolder: fullPath: string -> unit
  /// Open the given file in the desktop's default manner. Returns a value
  /// indicating whether the item was successfully opened.
  abstract openItem: fullPath: string -> bool
  /// Open the given external protocol URL in the desktop's default manner. (For
  /// example, mailto: URLs in the user's default mail agent). `url` must be max
  /// 2081 characters on Windows. Returns a value indicating whether an
  /// application was available to open the URL.
  abstract openExternalSync: url: string * ?options: OpenExternalOptions -> bool
  /// Open the given external protocol URL in the desktop's default manner. (For
  /// example, mailto: URLs in the user's default mail agent).
  abstract openExternal: url: string * ?options: OpenExternalOptions -> Promise<unit>
  /// Move the given file to trash and returns a value indicating whether the
  /// item was successfully moved to the trash.
  abstract moveItemToTrash: fullPath: string -> bool
  /// Play the beep sound.
  abstract beep: unit -> unit
  /// [Windows] Creates or updates a shortcut link at shortcutPath. Returns a
  /// value indicating whether the shortcut was created successfully.
  abstract writeShortcutLink: shortcutPath: string * operation: WriteShortcutLinkOperation * options: ShortcutDetails -> bool
  /// [Windows] Creates or updates a shortcut link at shortcutPath. Returns a
  /// value indicating whether the shortcut was created successfully.
  abstract writeShortcutLink: shortcutPath: string * options: ShortcutDetails -> bool
  /// [Windows] Resolves the shortcut link at shortcutPath. An exception will be
  /// thrown when any error happens.
  abstract readShortcutLink: shortcutPath: string -> ShortcutDetails

type ShortcutDetails =
  /// The target to launch from this shortcut.
  abstract target: string with get, set
  /// The working directory. Default is empty.
  abstract cwd: string option with get, set
  /// The arguments to be applied to target when launching from this shortcut.
  /// Default is empty.
  abstract args: string option with get, set
  /// The description of the shortcut. Default is empty.
  abstract description: string option with get, set
  /// The path to the icon, can be a DLL or EXE. `icon` and `iconIndex` have to
  /// be set together. Default is empty, which uses the target's icon.
  abstract icon: string option with get, set
  /// The resource ID of icon when `icon` is a DLL or EXE. Default is 0.
  abstract iconIndex: int option with get, set
  /// The Application User Model ID. Default is empty.
  abstract appUserModelId: string option with get, set

type Size =
  abstract height: int with get, set
  abstract width: int with get, set

type StreamProtocolResponse<'a> =
  /// The HTTP response code.
  abstract statusCode: int with get, set
  /// An object containing the response headers.
  abstract headers: obj with get, set
  /// A Node.js readable stream representing the response body.
  abstract data: Node.Stream.Readable<'a> with get, set

[<StringEnum; RequireQualifiedAccess>]
type SetAppearance =
  | Dark
  | Light

[<StringEnum; RequireQualifiedAccess>]
type GetAppearance =
  | Dark
  | Light
  | Unknown

[<StringEnum; RequireQualifiedAccess>]
type MediaAccessType =
  | Microphone
  | Camera

[<StringEnum; RequireQualifiedAccess>]
type MediaAccessStatus =
  | [<CompiledName("not-determined")>] NotDetermined
  | Granted
  | Denied
  | Restricted
  | Unknown

[<StringEnum; RequireQualifiedAccess>]
type UserDefaultValueType =
  | String
  | Boolean
  | Integer
  | Float
  | Double
  | Url
  | Array
  | Dictionary

[<StringEnum; RequireQualifiedAccess>]
type SystemPrefsColorWin =
  /// ("3d-dark-shadow") Dark shadow for three-dimensional display elements.
  | [<CompiledName("3d-dark-shadow")>] DarkShadow3D
  /// ("3d-face") Face color for three-dimensional display elements and for
  /// dialog box backgrounds.
  | [<CompiledName("3d-face")>] Face3D
  /// ("3d-highlight") Highlight color for three-dimensional display elements.
  | [<CompiledName("3d-highlight")>] Highlight3D
  /// ("3d-light") Light color for three-dimensional display elements.
  | [<CompiledName("3d-light")>] Light3D
  /// ("3d-shadow") Shadow color for three-dimensional display elements.
  | [<CompiledName("3d-shadow")>] Shadow3D
  /// Active window border.
  | [<CompiledName("active-border")>] ActiveBorder
  /// Active window title bar. Specifies the left side color in the color
  /// gradient of an active window's title bar if the gradient effect is
  /// enabled.
  | [<CompiledName("active-caption")>] ActiveCaption
  /// Right side color in the color gradient of an active window's title bar.
  | [<CompiledName("active-caption-gradient")>] ActiveCaptionGradient
  /// Background color of multiple document interface (MDI) applications.
  | [<CompiledName("app-workspace")>] AppWorkspace
  /// Text on push buttons.
  | [<CompiledName("button-text")>] ButtonText
  /// Text in caption, size box, and scroll bar arrow box.
  | [<CompiledName("caption-text")>] CaptionText
  /// Desktop background color.
  | [<CompiledName("desktop")>] Desktop
  /// Grayed (disabled) text.
  | [<CompiledName("disabled-text")>] DisabledText
  /// Item(s) selected in a control.
  | [<CompiledName("highlight")>] Highlight
  /// Text of item(s) selected in a control.
  | [<CompiledName("highlight-text")>] HighlightText
  /// Color for a hyperlink or hot-tracked item.
  | [<CompiledName("hotlight")>] Hotlight
  /// Inactive window border.
  | [<CompiledName("inactive-border")>] InactiveBorder
  /// Inactive window caption. Specifies the left side color in the color
  /// gradient of an inactive window's title bar if the gradient effect is
  /// enabled.
  | [<CompiledName("inactive-caption")>] InactiveCaption
  /// Right side color in the color gradient of an inactive window's title bar.
  | [<CompiledName("inactive-caption-gradient")>] InactiveCaptionGradient
  /// Color of text in an inactive caption.
  | [<CompiledName("inactive-caption-text")>] InactiveCaptionText
  /// Background color for tooltip controls.
  | [<CompiledName("info-background")>] InfoBackground
  /// Text color for tooltip controls.
  | [<CompiledName("info-text")>] InfoText
  /// Menu background.
  | [<CompiledName("menu")>] Menu
  /// The color used to highlight menu items when the menu appears as a flat
  /// menu.
  | [<CompiledName("menu-highlight")>] MenuHighlight
  /// The background color for the menu bar when menus appear as flat menus.
  | [<CompiledName("menubar")>] MenuBar
  /// Text in menus.
  | [<CompiledName("menu-text")>] MenuText
  /// Scroll bar gray area.
  | [<CompiledName("scrollbar")>] Scrollbar
  /// Window background.
  | [<CompiledName("window")>] Window
  /// Window frame.
  | [<CompiledName("window-frame")>] WindowFrame
  /// Text in windows.
  | [<CompiledName("window-text")>] WindowText

[<StringEnum; RequireQualifiedAccess>]
type SystemPrefsColorMac =
  /// The text on a selected surface in a list or table.
  | [<CompiledName("alternate-selected-control-text")>] AlternateSelectedControlText
  /// The background of a large interface element, such as a browser or table.
  | [<CompiledName("control-background")>] ControlBackground
  /// The surface of a control.
  | [<CompiledName("control")>] Control
  /// The text of a control that isn’t disabled.
  | [<CompiledName("control-text")>] ControlText
  /// The text of a control that’s disabled.
  | [<CompiledName("disabled-control-text")>] DisabledControlText
  /// The color of a find indicator.
  | [<CompiledName("find-highlight")>] FindHighlight
  /// The gridlines of an interface element such as a table.
  | [<CompiledName("grid")>] Grid
  /// The text of a header cell in a table.
  | [<CompiledName("header-text")>] HeaderText
  /// The virtual light source onscreen.
  | [<CompiledName("highlight")>] Highlight
  /// The ring that appears around the currently focused control when using the
  /// keyboard for interface navigation.
  | [<CompiledName("keyboard-focus-indicator")>] KeyboardFocusIndicator
  /// The text of a label containing primary content.
  | [<CompiledName("label")>] Label
  /// A link to other content.
  | [<CompiledName("link")>] Link
  /// A placeholder string in a control or text view.
  | [<CompiledName("placeholder-text")>] PlaceholderText
  /// The text of a label of lesser importance than a tertiary label such as
  /// watermark text.
  | [<CompiledName("quaternary-label")>] QuaternaryLabel
  /// The background of a scrubber in the Touch Bar.
  | [<CompiledName("scrubber-textured-background")>] ScrubberTexturedBackground
  /// The text of a label of lesser importance than a normal label such as a
  /// label used to represent a subheading or additional information.
  | [<CompiledName("secondary-label")>] SecondaryLabel
  /// The background for selected content in a key window or view.
  | [<CompiledName("selected-content-background")>] SelectedContentBackground
  /// The surface of a selected control.
  | [<CompiledName("selected-control")>] SelectedControl
  /// The text of a selected control.
  | [<CompiledName("selected-control-text")>] SelectedControlText
  /// The text of a selected menu.
  | [<CompiledName("selected-menu-item")>] SelectedMenuItem
  /// The background of selected text.
  | [<CompiledName("selected-text-background")>] SelectedTextBackground
  /// Selected text.
  | [<CompiledName("selected-text")>] SelectedText
  /// A separator between different sections of content.
  | [<CompiledName("separator")>] Separator
  /// The virtual shadow cast by a raised object onscreen.
  | [<CompiledName("shadow")>] Shadow
  /// The text of a label of lesser importance than a secondary label such as a
  /// label used to represent disabled text.
  | [<CompiledName("tertiary-label")>] TertiaryLabel
  /// Text background.
  | [<CompiledName("text-background")>] TextBackground
  ///  The text in a document.
  | [<CompiledName("text")>] Text
  /// The background behind a document's content.
  | [<CompiledName("under-page-background")>] UnderPageBackground
  /// The selected content in a non-key window or view.
  | [<CompiledName("unemphasized-selected-content-background")>] UnemphasizedSelectedContentBackground
  /// A background for selected text in a non-key window or view.
  | [<CompiledName("unemphasized-selected-text-background")>] UnemphasizedSelectedTextBackground
  /// Selected text in a non-key window or view.
  | [<CompiledName("unemphasized-selected-text")>] UnemphasizedSelectedText
  /// The background of a window.
  | [<CompiledName("window-background")>] WindowBackground
  /// The text in the window's titlebar area. 
  | [<CompiledName("window-frame-text")>] WindowFrameText

[<StringEnum; RequireQualifiedAccess>]
type SystemPrefsSystemColor =
  | Blue
  | Brown
  | Gray
  | Green
  | Orange
  | Pink
  | Purple
  | Red
  | Yellow

type SystemAnimationSettings =
  /// True if rich animations should be rendered. Looks at session type (e.g.
  /// remote desktop) and accessibility settings to give guidance for heavy
  /// animations.
  abstract shouldRenderRichAnimation: bool with get, set
  /// Determines on a per-platform basis whether scroll animations (e.g.
  /// produced by home/end key) should be enabled.
  abstract scrollAnimationsEnabledBySystem: bool with get, set
  /// Determines whether the user desires reduced motion based on platform APIs.
  abstract prefersReducedMotion: bool with get, set

type SystemPreferences =
  inherit EventEmitter<SystemPreferences>
  /// [Windows] Called with the new RGBA color the user assigned to be their
  /// system accent color.
  [<Emit "$0.on('accent-color-changed',$1)">] abstract onAccentColorChanged: listener: (Event -> string -> unit) -> SystemPreferences
  /// See onAccentColorChanged.
  [<Emit "$0.once('accent-color-changed',$1)">] abstract onceAccentColorChanged: listener: (Event -> string -> unit) -> SystemPreferences
  /// See onAccentColorChanged.
  [<Emit "$0.addListener('accent-color-changed',$1)">] abstract addListenerAccentColorChanged: listener: (Event -> string -> unit) -> SystemPreferences
  /// See onAccentColorChanged.
  [<Emit "$0.removeListener('accent-color-changed',$1)">] abstract removeListenerAccentColorChanged: listener: (Event -> string -> unit) -> SystemPreferences
  /// [Windows]
  [<Emit "$0.on('color-changed',$1)">] abstract onColorChanged: listener: (Event -> unit) -> SystemPreferences
  /// See onColorChanged.
  [<Emit "$0.once('color-changed',$1)">] abstract onceColorChanged: listener: (Event -> unit) -> SystemPreferences
  /// See onColorChanged.
  [<Emit "$0.addListener('color-changed',$1)">] abstract addListenerColorChanged: listener: (Event -> unit) -> SystemPreferences
  /// See onColorChanged.
  [<Emit "$0.removeListener('color-changed',$1)">] abstract removeListenerColorChanged: listener: (Event -> unit) -> SystemPreferences
  /// [Windows] Called with true if an inverted color scheme (a high contrast
  /// color scheme with light text and dark backgrounds) is being used, false
  /// otherwise.
  [<Emit "$0.on('inverted-color-scheme-changed',$1)">] abstract onInvertedColorSchemeChanged: listener: (Event -> bool -> unit) -> SystemPreferences
  /// See onInvertedColorSchemeChanged.
  [<Emit "$0.once('inverted-color-scheme-changed',$1)">] abstract onceInvertedColorSchemeChanged: listener: (Event -> bool -> unit) -> SystemPreferences
  /// See onInvertedColorSchemeChanged.
  [<Emit "$0.addListener('inverted-color-scheme-changed',$1)">] abstract addListenerInvertedColorSchemeChanged: listener: (Event -> bool -> unit) -> SystemPreferences
  /// See onInvertedColorSchemeChanged.
  [<Emit "$0.removeListener('inverted-color-scheme-changed',$1)">] abstract removeListenerInvertedColorSchemeChanged: listener: (Event -> bool -> unit) -> SystemPreferences
  /// [Windows] Called with true if a high contrast theme is being used, false
  /// otherwise.
  [<Emit "$0.on('high-contrast-color-scheme-changed',$1)">] abstract onHighContrastColorSchemeChanged: listener: (Event -> bool -> unit) -> SystemPreferences
  /// See onHighContrastColorSchemeChanged.
  [<Emit "$0.once('high-contrast-color-scheme-changed',$1)">] abstract onceHighContrastColorSchemeChanged: listener: (Event -> bool -> unit) -> SystemPreferences
  /// See onHighContrastColorSchemeChanged.
  [<Emit "$0.addListener('high-contrast-color-scheme-changed',$1)">] abstract addListenerHighContrastColorSchemeChanged: listener: (Event -> bool -> unit) -> SystemPreferences
  /// See onHighContrastColorSchemeChanged.
  [<Emit "$0.removeListener('high-contrast-color-scheme-changed',$1)">] abstract removeListenerHighContrastColorSchemeChanged: listener: (Event -> bool -> unit) -> SystemPreferences
  /// [macOS]
  ///
  /// Note: This event is only emitted after you have called
  /// startAppLevelAppearanceTrackingOS
  [<Emit "$0.on('appearance-changed',$1)">] abstract onAppearanceChanged: listener: (SetAppearance -> unit) -> SystemPreferences
  /// See onAppearanceChanged.
  [<Emit "$0.once('appearance-changed',$1)">] abstract onceAppearanceChanged: listener: (SetAppearance -> unit) -> SystemPreferences
  /// See onAppearanceChanged.
  [<Emit "$0.addListener('appearance-changed',$1)">] abstract addListenerAppearanceChanged: listener: (SetAppearance -> unit) -> SystemPreferences
  /// See onAppearanceChanged.
  [<Emit "$0.removeListener('appearance-changed',$1)">] abstract removeListenerAppearanceChanged: listener: (SetAppearance -> unit) -> SystemPreferences
  /// [macOS] Whether the system is in Dark Mode.
  abstract isDarkMode: unit -> bool
  /// [macOS] Whether the Swipe between pages setting is on.
  abstract isSwipeTrackingFromScrollEventsEnabled: unit -> bool
  /// [macOS] Posts `event` as native notifications of macOS. The userInfo is an
  /// object that contains the user information dictionary sent along with the
  /// notification. Set deliverImmediately=true to post notifications
  /// immediately even when the subscribing app is inactive.
  abstract postNotification: event: string * userInfo: obj option * ?deliverImmediately: bool -> unit
  /// [macOS] Posts `event` as native notifications of macOS. The userInfo is an
  /// object that contains the user information dictionary sent along with the
  /// notification.
  abstract postLocalNotification: event: string * userInfo: obj option -> unit
  /// [macOS] Posts `event` as native notifications of macOS. The userInfo is an
  /// object that contains the user information dictionary sent along with the
  /// notification.
  abstract postWorkspaceNotification: event: string * userInfo: obj option -> unit
  /// [macOS] Subscribes to native notifications of macOS, callback will be
  /// called with callback(event, userInfo) when the corresponding event
  /// happens. The userInfo is an object that contains the user information
  /// dictionary sent along with the notification.
  ///
  /// The id of the subscriber is returned, which can be used to unsubscribe the
  /// event. Under the hood this API subscribes to
  /// NSDistributedNotificationCenter.
  abstract subscribeNotification: event: string * callback: (string -> obj option -> unit) -> int
  /// [macOS] Same as subscribeNotification, but uses NSNotificationCenter for
  /// local defaults. This is necessary for events such as
  /// NSUserDefaultsDidChangeNotification.
  abstract subscribeLocalNotification: event: string * callback: (string -> obj option -> unit) -> int
  /// [macOS] Same as subscribeNotification, but uses
  /// NSWorkspace.sharedWorkspace.notificationCenter. This is necessary for
  /// events such as NSWorkspaceDidActivateApplicationNotification.
  abstract subscribeWorkspaceNotification: event: string * callback: (string -> obj option -> unit) -> int
  /// [macOS] Removes the subscriber with `id`.
  abstract unsubscribeNotification: id: int -> unit
  /// [macOS] Same as unsubscribeNotification, but removes the subscriber from
  /// NSNotificationCenter.
  abstract unsubscribeLocalNotification: id: int -> unit
  /// [macOS] Same as unsubscribeNotification, but removes the subscriber from
  /// NSWorkspace.sharedWorkspace.notificationCenter.
  abstract unsubscribeWorkspaceNotification: id: int -> unit
  /// [macOS] Add the specified defaults to your application's NSUserDefaults.
  abstract registerDefaults: defaults: obj option -> unit
  /// [macOS] Returns the value of `key` in NSUserDefaults.
  abstract getUserDefault: key: string * ``type``: UserDefaultValueType -> obj option
  /// [macOS] Set the value of key in NSUserDefaults. Note that `type` should
  /// match actual type of value. An exception is thrown if they don't.
  abstract setUserDefault: key: string * ``type``: UserDefaultValueType * value: string -> unit
  /// [macOS] Removes the key in NSUserDefaults. This can be used to restore the
  /// default or global value of a key previously set with setUserDefault.
  abstract removeUserDefault: key: string -> unit
  /// [Windows] Returns true if DWM composition (Aero Glass) is enabled, and
  /// false otherwise.
  abstract isAeroGlassEnabled: unit -> bool
  /// [Windows, macOS] Returns the user's current system wide accent color
  /// preference in RGBA hexadecimal form.
  ///
  /// This API is only available on macOS 10.14 Mojave or newer.
  abstract getAccentColor: unit -> string
  /// [Windows] Returns the system color setting in RGB hexadecimal form
  /// (#ABCDEF). See the Windows docs for more details.
  abstract getColor: color: SystemPrefsColorWin -> string
  /// [macOS] Returns the system color setting in RGB hexadecimal form
  /// (#ABCDEF). See the MacOS docs for more details.
  abstract getColor: color: SystemPrefsColorMac -> string
  /// [macOS] Returns one of several standard system colors that automatically
  /// adapt to vibrancy and changes in accessibility settings like 'Increase
  /// contrast' and 'Reduce transparency'. See Apple Documentation for more
  /// details.
  abstract getSystemColor: color: SystemPrefsSystemColor -> string
  /// [Windows] Returns true if an inverted color scheme (a high contrast color
  /// scheme with light text and dark backgrounds) is active, false otherwise.
  abstract isInvertedColorScheme: unit -> bool
  /// [Windows] Returns true if a high contrast theme is active, false
  /// otherwise.
  abstract isHighContrastColorScheme: unit -> bool
  /// [macOS] Gets the macOS appearance setting that is currently applied to
  /// your application, maps to NSApplication.effectiveAppearance.
  abstract getEffectiveAppearance: unit -> GetAppearance
  /// [macOS] Gets the macOS appearance setting that you have declared you want
  /// for your application, maps to NSApplication.appearance. You can use the
  /// setAppLevelAppearance API to set this value.
  abstract getAppLevelAppearance: unit -> GetAppearance option
  /// [macOS] Sets the appearance setting for your application, this should
  /// override the system default and override the value of
  /// getEffectiveAppearance.
  abstract setAppLevelAppearance: appearance: SetAppearance option -> unit
  /// [macOS] Returns a value indicating whether or not this device has the
  /// ability to use Touch ID.
  ///
  /// Note: This API will return `false` on macOS systems older than Sierra
  /// 10.12.2.
  abstract canPromptTouchID: unit -> bool
  /// <summary>
  ///   Returns a promise that resolves if the user has successfully
  ///   authenticated with Touch ID.
  ///
  ///   This API itself will not protect your user data; rather, it is a
  ///   mechanism to allow you to do so. Native apps will need to set Access
  ///   Control Constants like `kSecAccessControlUserPresence` on the their
  ///   keychain entry so that reading it would auto-prompt for Touch ID
  ///   biometric consent. This could be done with
  ///   [`node-keytar`](https://github.com/atom/node-keytar), such that one
  ///   would store an encryption key with `node-keytar` and only fetch it if
  ///   `promptTouchID()` resolves.
  ///
  ///   Note: This API will return a rejected Promise on macOS systems older
  ///   than Sierra 10.12.2.
  /// </summary>
  /// <param name="reason">
  ///   The reason you are asking for Touch ID authentication
  /// </param>
  abstract promptTouchID: reason: string -> Promise<unit>
  /// <summary>
  ///   [macOS] Returns true if the current process is a trusted accessibility
  ///   client and false if it is not.
  /// </summary>
  /// <param name="prompt">
  ///   whether or not the user will be informed via prompt if the current
  ///   process is untrusted.
  /// </param>
  abstract isTrustedAccessibilityClient: prompt: bool -> bool
  /// [macOS] This user consent was not required until macOS 10.14 Mojave, so
  /// this method will always return MediaAccessStatus.Granted if your system is
  /// running 10.13 High Sierra or lower.
  abstract getMediaAccessStatus: mediaType: string -> MediaAccessStatus
  /// Returns a promise that resolves with true if consent was granted and false
  /// if it was denied. If an invalid mediaType is passed, the promise will be
  /// rejected. If an access request was denied and later is changed through the
  /// System Preferences pane, a restart of the app will be required for the new
  /// permissions to take effect. If access has already been requested and
  /// denied, it must be changed through the preference pane; an alert will not
  /// pop up and the promise will resolve with the existing access status.
  ///
  /// Important: In order to properly leverage this API, you must set the
  /// NSMicrophoneUsageDescription and NSCameraUsageDescription strings in your
  /// app's Info.plist file. The values for these keys will be used to populate
  /// the permission dialogs so that the user will be properly informed as to
  /// the purpose of the permission request. See Electron Application
  /// Distribution for more information about how to set these in the context of
  /// Electron.
  ///
  /// This user consent was not required until macOS 10.14 Mojave, so this
  /// method will always return true if your system is running 10.13 High Sierra
  /// or lower.
  abstract askForMediaAccess: mediaType: MediaAccessType -> Promise<bool>
  /// Returns an object with system animation settings.
  abstract getAnimationSettings: unit -> SystemAnimationSettings

type Task =
  /// Path of the program to execute, usually you should specify
  /// process.execPath which opens the current program.
  abstract program: string with get, set
  /// The command line arguments when program is executed.
  abstract arguments: string with get, set
  /// The string to be displayed in a JumpList.
  abstract title: string with get, set
  /// Description of this task.
  abstract description: string with get, set
  /// The absolute path to an icon to be displayed in a JumpList, which can be
  /// an arbitrary resource file that contains an icon. You can usually specify
  /// process.execPath to show the icon of the program.
  abstract iconPath: string with get, set
  /// The icon index in the icon file. If an icon file consists of two or more
  /// icons, set this value to identify the icon. If an icon file consists of
  /// one icon, this value is 0.
  abstract iconIndex: int with get, set
  /// The working directory. Default is empty.
  abstract workingDirectory: string with get, set

[<StringEnum; RequireQualifiedAccess>]
type ThumbarButtonFlag =
  /// The button is active and available to the user.
  | Enabled
  /// The button is disabled. It is present, but has a visual state indicating
  /// it will not respond to user action.
  | Disabled
  /// When the button is clicked, the thumbnail window closes immediately.
  | [<CompiledName("dismissonclick")>] DismissOnClick
  /// Do not draw a button border, use only the image.
  | [<CompiledName("nobackground")>] NoBackground
  /// The button is not shown to the user.
  | Hidden
  /// The button is enabled but not interactive; no pressed button state is
  /// drawn. This value is intended for instances where the button is used in a
  /// notification.
  | [<CompiledName("noninteractive")>] NonInteractive

type ThumbarButton =
  /// The icon showing in thumbnail toolbar.
  abstract icon: NativeImage with get, set
  abstract click: (Event -> unit) with get, set
  /// The text of the button's tooltip.
  abstract tooltip: string option with get, set
  /// Control specific states and behaviors of the button. By default, it is
  /// [ThumbarButtonFlag.Enabled].
  abstract flags: ThumbarButtonFlag [] with get, set

type ITouchBarItem =
  interface end

type TouchBarButton =
  inherit EventEmitter<TouchBarButton>
  inherit ITouchBarItem
  /// The button's current text. Changing this value immediately updates the
  /// button in the touch bar.
  abstract label: string with get, set
  /// A hex code representing the button's current background color. Changing
  /// this value immediately updates the button in the touch bar.
  abstract backgroundColor: string with get, set
  /// The button's current icon. Changing this value immediately updates the
  /// button in the touch bar.
  abstract icon: NativeImage with get, set

type TouchBarButtonStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarButtonOptions -> TouchBarButton

type TouchBarColorPicker =
  inherit EventEmitter<TouchBarColorPicker>
  inherit ITouchBarItem
  /// the color picker's available colors to select. Changing this value
  /// immediately updates the color picker in the touch bar.
  abstract availableColors: string [] with get, set
  /// The color picker's currently selected color. Changing this value
  /// immediately updates the color picker in the touch bar.
  abstract selectedColor: string with get, set

type TouchBarColorPickerStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarColorPickerOptions -> TouchBarColorPicker

type TouchBarGroup =
  inherit EventEmitter<TouchBarGroup>
  inherit ITouchBarItem

type TouchBarGroupStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarGroupOptions -> TouchBarGroup

type TouchBarLabel =
  inherit EventEmitter<TouchBarLabel>
  inherit ITouchBarItem
  /// The label's current text. Changing this value immediately updates the
  /// label in the touch bar.
  abstract label: string with get, set
  /// A hex code representing the label's current text color. Changing this
  /// value immediately updates the label in the touch bar.
  abstract textColor: string with get, set

type TouchBarLabelStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarLabelOptions -> TouchBarLabel

type TouchBarPopover =
  inherit EventEmitter<TouchBarPopover>
  inherit ITouchBarItem
  /// The popover's current button text. Changing this value immediately updates
  /// the popover in the touch bar.
  abstract label: string with get, set
  /// The popover's current button icon. Changing this value immediately updates
  /// the popover in the touch bar.
  abstract icon: NativeImage with get, set

type TouchBarPopoverStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarPopoverOptions -> TouchBarPopover

[<StringEnum; RequireQualifiedAccess>]
type TouchBarScrubberStyle =
  /// Maps to [NSScrubberSelectionStyle roundedBackgroundStyle].
  | Background
  /// Maps to [NSScrubberSelectionStyle outlineOverlayStyle].
  | Outline

[<StringEnum; RequireQualifiedAccess>]
type TouchBarScrubberMode =
  /// Maps to NSScrubberModeFixed.
  | Fixed
  /// Maps to NSScrubberModeFree.
  | Free

type TouchBarScrubber =
  inherit EventEmitter<TouchBarScrubber>
  inherit ITouchBarItem
  /// The items in this scrubber. Updating this value immediately updates the
  /// control in the touch bar. Updating deep properties inside this array does
  /// not update the touch bar.
  abstract items: ScrubberItem [] with get, set
  /// The style that selected items in the scrubber should have. Updating this
  /// value immediately updates the control in the touch bar.
  abstract selectedStyle: TouchBarScrubberStyle option with get, set
  /// The style that selected items in the scrubber should have. This style is
  /// overlayed on top of the scrubber item instead of being placed behind it.
  /// Updating this value immediately updates the control in the touch bar.
  abstract overlayStyle: TouchBarScrubberStyle option with get, set
  /// Whether to show the left / right selection arrows in this scrubber.
  /// Updating this value immediately updates the control in the touch bar.
  abstract showArrowButtons: bool with get, set
  /// The mode of this scrubber. Updating this value immediately updates the
  /// control in the touch bar.
  abstract mode: TouchBarScrubberMode with get, set
  /// Whether this scrubber is continuous or not. Updating this value
  /// immediately updates the control in the touch bar.
  abstract continuous: bool with get, set

type TouchBarScrubberStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarScrubberOptions -> TouchBarScrubber

type TouchBarSegmentedControl =
  inherit EventEmitter<TouchBarSegmentedControl>
  inherit ITouchBarItem
  /// The controls current segment style. Updating this value immediately
  /// updates the control in the touch bar.
  abstract segmentStyle: string with get, set
  /// The segments in this control. Updating this value immediately updates the
  /// control in the touch bar. Updating deep properties inside this array does
  /// not update the touch bar.
  abstract segments: SegmentedControlSegment [] with get, set
  /// The currently selected segment. Changing this value immediately updates
  /// the control in the touch bar. User interaction with the touch bar will
  /// update this value automatically.
  abstract selectedIndex: int option with get, set

type TouchBarSegmentedControlStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarSegmentedControlOptions -> TouchBarSegmentedControl

type TouchBarSlider =
  inherit EventEmitter<TouchBarSlider>
  inherit ITouchBarItem
  /// The slider's current text. Changing this value immediately updates the
  /// slider in the touch bar.
  abstract label: string with get, set
  /// The slider's current value. Changing this value immediately updates the
  /// slider in the touch bar.
  abstract value: int with get, set
  /// The slider's current minimum value. Changing this value immediately
  /// updates the slider in the touch bar.
  abstract minValue: int with get, set
  /// The slider's current maximum value. Changing this value immediately
  /// updates the slider in the touch bar.
  abstract maxValue: int with get, set

type TouchBarSliderStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarSliderOptions -> TouchBarSlider

type TouchBarSpacer =
  inherit EventEmitter<TouchBarSpacer>
  inherit ITouchBarItem

type TouchBarSpacerStatic =
  [<EmitConstructor>] abstract Create: options: TouchBarSpacerOptions -> TouchBarSpacer

type TouchBar =
  inherit EventEmitter<TouchBar>
  /// A TouchBarItem that will replace the "esc" button on the touch bar when
  /// set. Setting to None restores the default "esc" button. Changing this
  /// value immediately updates the escape item in the touch bar.
  abstract escapeItem: ITouchBarItem option with get, set

type TouchBarStatic =
  /// Creates a new touch bar with the specified items. Use
  /// BrowserWindow.setTouchBar to add the TouchBar to a window.
  ///
  /// Note: The TouchBar API is currently experimental and may change or be
  /// removed in future Electron releases.
  ///
  /// Tip: If you don't have a MacBook with Touch Bar, you can use Touch Bar
  /// Simulator to test Touch Bar usage in your app.
  [<EmitConstructor>] abstract Create: options: TouchBarOptions -> TouchBar
  abstract TouchBarButton: TouchBarButtonStatic with get, set
  abstract TouchBarColorPicker: TouchBarColorPickerStatic with get, set
  abstract TouchBarGroup: TouchBarGroupStatic with get, set
  abstract TouchBarLabel: TouchBarLabelStatic with get, set
  abstract TouchBarPopover: TouchBarPopoverStatic with get, set
  abstract TouchBarScrubber: TouchBarScrubberStatic with get, set
  abstract TouchBarSegmentedControl: TouchBarSegmentedControlStatic with get, set
  abstract TouchBarSlider: TouchBarSliderStatic with get, set
  abstract TouchBarSpacer: TouchBarSpacerStatic with get, set

type TraceCategoriesAndOptions =
  /// A filter to control what category groups should be traced. A filter can
  /// have an optional `-` prefix to exclude category groups that contain a
  /// matching category. Having both included and excluded category patterns in
  /// the same list is not supported. Examples: `test_MyTest*`,
  /// `test_MyTest*,test_OtherStuff`, `-excluded_category1,-excluded_category2`.
  abstract categoryFilter: string with get, set
  /// Controls what kind of tracing is enabled, it is a comma-delimited sequence
  /// of the following strings: `record-until-full`, `record-continuously`,
  /// `trace-to-console`, `enable-sampling`, `enable-systrace`. Example:
  /// "record-until-full,enable-sampling". The first 3 options are trace
  /// recording modes and hence mutually exclusive. If more than one trace
  /// recording modes appear in the traceOptions string, the last one takes
  /// precedence. If none of the trace recording modes are specified, recording
  /// mode is `record-until-full`. The trace option will first be reset to the
  /// default option (record_mode set to record-until-full, enable_sampling and
  /// enable_systrace set to false) before options parsed from traceOptions are
  /// applied on it.
  abstract traceOptions: string with get, set

type TraceConfig =
  abstract excluded_categories: string [] with get, set
  abstract included_categories: string [] with get, set
  abstract memory_dump_config: obj option with get, set

[<StringEnum; RequireQualifiedAccess>]
type TransactionState =
  | Purchasing
  | Purchased
  | Failed
  | Restored
  | Deferred

type Transaction =
  /// A string that uniquely identifies a successful payment transaction.
  abstract transactionIdentifier: string with get, set
  /// The date the transaction was added to the App Store’s payment queue.
  abstract transactionDate: string with get, set
  /// The identifier of the restored transaction by the App Store.
  abstract originalTransactionIdentifier: string with get, set
  /// The transaction state.
  abstract transactionState: TransactionState with get, set
  /// The error code if an error occurred while processing the transaction.
  abstract errorCode: int with get, set
  /// The error message if an error occurred while processing the transaction.
  abstract errorMessage: string with get, set
  abstract payment: Payment with get, set

[<StringEnum; RequireQualifiedAccess>]
type HighlightMode =
  /// Highlight the tray icon when it is clicked and also when its context menu
  /// is open.
  | Selection
  /// Always highlight the tray icon.
  | Always
  /// Never highlight the tray icon.
  | Never

type Tray =
  inherit EventEmitter<Tray>
  /// Emitted when the tray icon is clicked. The listener gets the bounds of the
  /// tray icon and the position of the event.
  [<Emit "$0.on('click',$1)">] abstract onClick: listener: (TrayInputEvent -> Rectangle -> Point -> unit) -> Tray
  /// See onClick.
  [<Emit "$0.once('click',$1)">] abstract onceClick: listener: (TrayInputEvent -> Rectangle -> Point -> unit) -> Tray
  /// See onClick.
  [<Emit "$0.addListener('click',$1)">] abstract addListenerClick: listener: (TrayInputEvent -> Rectangle -> Point -> unit) -> Tray
  /// See onClick.
  [<Emit "$0.removeListener('click',$1)">] abstract removeListenerClick: listener: (TrayInputEvent -> Rectangle -> Point -> unit) -> Tray
  /// [macOS, Windows] Emitted when the tray icon is right clicked. The listener
  /// gets the bounds of the tray icon.
  [<Emit "$0.on('right-click',$1)">] abstract onRightClick: listener: (TrayInputEvent -> Rectangle -> unit) -> Tray
  /// See onRightClick.
  [<Emit "$0.once('right-click',$1)">] abstract onceRightClick: listener: (TrayInputEvent -> Rectangle -> unit) -> Tray
  /// See onRightClick.
  [<Emit "$0.addListener('right-click',$1)">] abstract addListenerRightClick: listener: (TrayInputEvent -> Rectangle -> unit) -> Tray
  /// See onRightClick.
  [<Emit "$0.removeListener('right-click',$1)">] abstract removeListenerRightClick: listener: (TrayInputEvent -> Rectangle -> unit) -> Tray
  /// [macOS, Windows] Emitted when the tray icon is double clicked. The
  /// listener gets the bounds of the tray icon.
  [<Emit "$0.on('double-click',$1)">] abstract onDoubleClick: listener: (TrayInputEvent -> Rectangle -> unit) -> Tray
  /// See onDoubleClick.
  [<Emit "$0.once('double-click',$1)">] abstract onceDoubleClick: listener: (TrayInputEvent -> Rectangle -> unit) -> Tray
  /// See onDoubleClick.
  [<Emit "$0.addListener('double-click',$1)">] abstract addListenerDoubleClick: listener: (TrayInputEvent -> Rectangle -> unit) -> Tray
  /// See onDoubleClick.
  [<Emit "$0.removeListener('double-click',$1)">] abstract removeListenerDoubleClick: listener: (TrayInputEvent -> Rectangle -> unit) -> Tray
  /// [Windows] Emitted when the tray balloon shows.
  [<Emit "$0.on('balloon-show',$1)">] abstract onBalloonShow: listener: (Event -> unit) -> Tray
  /// See onBalloonShow.
  [<Emit "$0.once('balloon-show',$1)">] abstract onceBalloonShow: listener: (Event -> unit) -> Tray
  /// See onBalloonShow.
  [<Emit "$0.addListener('balloon-show',$1)">] abstract addListenerBalloonShow: listener: (Event -> unit) -> Tray
  /// See onBalloonShow.
  [<Emit "$0.removeListener('balloon-show',$1)">] abstract removeListenerBalloonShow: listener: (Event -> unit) -> Tray
  /// [Windows] Emitted when the tray balloon is clicked.
  [<Emit "$0.on('balloon-click',$1)">] abstract onBalloonClick: listener: (Event -> unit) -> Tray
  /// See onBalloonClick.
  [<Emit "$0.once('balloon-click',$1)">] abstract onceBalloonClick: listener: (Event -> unit) -> Tray
  /// See onBalloonClick.
  [<Emit "$0.addListener('balloon-click',$1)">] abstract addListenerBalloonClick: listener: (Event -> unit) -> Tray
  /// See onBalloonClick.
  [<Emit "$0.removeListener('balloon-click',$1)">] abstract removeListenerBalloonClick: listener: (Event -> unit) -> Tray
  /// [Windows] Emitted when the tray balloon is closed because of timeout or
  /// user manually closes it.
  [<Emit "$0.on('balloon-closed',$1)">] abstract onBalloonClosed: listener: (Event -> unit) -> Tray
  /// See onBalloonClosed.
  [<Emit "$0.once('balloon-closed',$1)">] abstract onceBalloonClosed: listener: (Event -> unit) -> Tray
  /// See onBalloonClosed.
  [<Emit "$0.addListener('balloon-closed',$1)">] abstract addListenerBalloonClosed: listener: (Event -> unit) -> Tray
  /// See onBalloonClosed.
  [<Emit "$0.removeListener('balloon-closed',$1)">] abstract removeListenerBalloonClosed: listener: (Event -> unit) -> Tray
  /// [macOS] Emitted when any dragged items are dropped on the tray icon.
  [<Emit "$0.on('drop',$1)">] abstract onDrop: listener: (Event -> unit) -> Tray
  /// See onDrop.
  [<Emit "$0.once('drop',$1)">] abstract onceDrop: listener: (Event -> unit) -> Tray
  /// See onDrop.
  [<Emit "$0.addListener('drop',$1)">] abstract addListenerDrop: listener: (Event -> unit) -> Tray
  /// See onDrop.
  [<Emit "$0.removeListener('drop',$1)">] abstract removeListenerDrop: listener: (Event -> unit) -> Tray
  /// [macOS] Emitted when dragged files are dropped in the tray icon. The
  /// listener receives the paths of the dropped files.
  [<Emit "$0.on('drop-files',$1)">] abstract onDropFiles: listener: (Event -> string [] -> unit) -> Tray
  /// See onDropFiles.
  [<Emit "$0.once('drop-files',$1)">] abstract onceDropFiles: listener: (Event -> string [] -> unit) -> Tray
  /// See onDropFiles.
  [<Emit "$0.addListener('drop-files',$1)">] abstract addListenerDropFiles: listener: (Event -> string [] -> unit) -> Tray
  /// See onDropFiles.
  [<Emit "$0.removeListener('drop-files',$1)">] abstract removeListenerDropFiles: listener: (Event -> string [] -> unit) -> Tray
  /// [macOS] Emitted when dragged text is dropped in the tray icon. The
  /// listener receives the dropped text string.
  [<Emit "$0.on('drop-text',$1)">] abstract onDropText: listener: (Event -> string -> unit) -> Tray
  /// See onDropText.
  [<Emit "$0.once('drop-text',$1)">] abstract onceDropText: listener: (Event -> string -> unit) -> Tray
  /// See onDropText.
  [<Emit "$0.addListener('drop-text',$1)">] abstract addListenerDropText: listener: (Event -> string -> unit) -> Tray
  /// See onDropText.
  [<Emit "$0.removeListener('drop-text',$1)">] abstract removeListenerDropText: listener: (Event -> string -> unit) -> Tray
  /// [macOS] Emitted when a drag operation enters the tray icon.
  [<Emit "$0.on('drag-enter',$1)">] abstract onDragEnter: listener: (Event -> unit) -> Tray
  /// See onDragEnter.
  [<Emit "$0.once('drag-enter',$1)">] abstract onceDragEnter: listener: (Event -> unit) -> Tray
  /// See onDragEnter.
  [<Emit "$0.addListener('drag-enter',$1)">] abstract addListenerDragEnter: listener: (Event -> unit) -> Tray
  /// See onDragEnter.
  [<Emit "$0.removeListener('drag-enter',$1)">] abstract removeListenerDragEnter: listener: (Event -> unit) -> Tray
  /// [macOS] Emitted when a drag operation exits the tray icon.
  [<Emit "$0.on('drag-leave',$1)">] abstract onDragLeave: listener: (Event -> unit) -> Tray
  /// See onDragLeave.
  [<Emit "$0.once('drag-leave',$1)">] abstract onceDragLeave: listener: (Event -> unit) -> Tray
  /// See onDragLeave.
  [<Emit "$0.addListener('drag-leave',$1)">] abstract addListenerDragLeave: listener: (Event -> unit) -> Tray
  /// See onDragLeave.
  [<Emit "$0.removeListener('drag-leave',$1)">] abstract removeListenerDragLeave: listener: (Event -> unit) -> Tray
  /// [macOS] Emitted when a drag operation ends on the tray or ends at another
  /// location.
  [<Emit "$0.on('drag-end',$1)">] abstract onDragEnd: listener: (Event -> unit) -> Tray
  /// See onDragEnd.
  [<Emit "$0.once('drag-end',$1)">] abstract onceDragEnd: listener: (Event -> unit) -> Tray
  /// See onDragEnd.
  [<Emit "$0.addListener('drag-end',$1)">] abstract addListenerDragEnd: listener: (Event -> unit) -> Tray
  /// See onDragEnd.
  [<Emit "$0.removeListener('drag-end',$1)">] abstract removeListenerDragEnd: listener: (Event -> unit) -> Tray
  /// [macOS] Emitted when the mouse enters the tray icon. The listener receives
  /// the position of the event.
  [<Emit "$0.on('mouse-enter',$1)">] abstract onMouseEnter: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseEnter.
  [<Emit "$0.once('mouse-enter',$1)">] abstract onceMouseEnter: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseEnter.
  [<Emit "$0.addListener('mouse-enter',$1)">] abstract addListenerMouseEnter: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseEnter.
  [<Emit "$0.removeListener('mouse-enter',$1)">] abstract removeListenerMouseEnter: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// [macOS] Emitted when the mouse exits the tray icon. The listener receives
  /// the position of the event.
  [<Emit "$0.on('mouse-leave',$1)">] abstract onMouseLeave: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseLeave.
  [<Emit "$0.once('mouse-leave',$1)">] abstract onceMouseLeave: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseLeave.
  [<Emit "$0.addListener('mouse-leave',$1)">] abstract addListenerMouseLeave: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseLeave.
  [<Emit "$0.removeListener('mouse-leave',$1)">] abstract removeListenerMouseLeave: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// [macOS] Emitted when the mouse moves in the tray icon. The listener
  /// receives the position of the event.
  [<Emit "$0.on('mouse-move',$1)">] abstract onMouseMove: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseMove.
  [<Emit "$0.once('mouse-move',$1)">] abstract onceMouseMove: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseMove.
  [<Emit "$0.addListener('mouse-move',$1)">] abstract addListenerMouseMove: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// See onMouseMove.
  [<Emit "$0.removeListener('mouse-move',$1)">] abstract removeListenerMouseMove: listener: (TrayInputEvent -> Point -> unit) -> Tray
  /// Destroys the tray icon immediately.
  abstract destroy: unit -> unit
  /// Sets the image associated with this tray icon.
  abstract setImage: image: U2<NativeImage, string> -> unit
  /// [macOS] Sets the image associated with this tray icon when pressed on
  /// macOS.
  abstract setPressedImage: image: U2<NativeImage, string> -> unit
  /// Sets the hover text for this tray icon.
  abstract setToolTip: toolTip: string -> unit
  /// [macOS] Sets the title displayed aside of the tray icon in the status bar
  /// (Support ANSI colors).
  abstract setTitle: title: string -> unit
  /// [macOS] Returns the title displayed next to the tray icon in the status
  /// bar.
  abstract getTitle: unit -> string
  /// [macOS] Sets when the tray's icon background becomes highlighted (in
  /// blue). Default is HighlightMode.Selection.
  ///
  /// Note: You can use highlightMode with a BrowserWindow by toggling between
  /// HighlightMode.Never and HighlightMode.Always modes when the window
  /// visibility changes.
  abstract setHighlightMode: mode: HighlightMode -> unit
  /// [macOS] Sets the option to ignore double click events. Ignoring these
  /// events allows you to detect every individual click of the tray icon. This
  /// value is set to false by default.
  abstract setIgnoreDoubleClickEvents: ignore: bool -> unit
  /// [macOS] Returns a value indicating whether double click events will be
  /// ignored.
  abstract getIgnoreDoubleClickEvents: unit -> bool
  /// [Windows] Displays a tray balloon.
  abstract displayBalloon: options: DisplayBalloonOptions -> unit
  /// [macOS, Windows] Pops up the context menu of the tray icon. When `menu` is
  /// passed, the menu will be shown instead of the tray icon's context menu.
  ///
  /// The `position` is only available on Windows, and it is (0, 0) by default.
  abstract popUpContextMenu: ?menu: Menu * ?position: Point -> unit
  /// Sets the context menu for this icon.
  abstract setContextMenu: menu: Menu option -> unit
  /// [macOS, Windows] Returns the bounds of this tray icon.
  abstract getBounds: unit -> Rectangle
  /// Returns a value indicating whether the tray icon is destroyed.
  abstract isDestroyed: unit -> bool

type TrayStatic =
  /// Creates a new tray icon associated with the image.
  [<EmitConstructor>] abstract Create: image: U2<NativeImage, string> -> Tray

type UploadBlob =
  /// blob.
  abstract ``type``: string with get, set
  /// UUID of blob data to upload.
  abstract blobUUID: string with get, set

type UploadData =
  /// Content being sent.
  abstract bytes: Buffer with get, set
  /// Path of file being uploaded.
  abstract file: string with get, set
  /// UUID of blob data. Use session.getBlobData method to retrieve the data.
  abstract blobUUID: string with get, set

type UploadFile =
  /// file.
  abstract ``type``: string with get, set
  /// Path of file to be uploaded.
  abstract filePath: string with get, set
  /// Defaults to 0.
  abstract offset: int with get, set
  /// Number of bytes to read from offset. Defaults to 0.
  abstract length: int with get, set
  /// Last Modification time in number of seconds since the UNIX epoch.
  abstract modificationTime: float with get, set

type UploadRawData =
  /// rawData.
  abstract ``type``: string with get, set
  /// Data to be uploaded.
  abstract bytes: Buffer with get, set

[<StringEnum; RequireQualifiedAccess>]
type WindowDisposition =
  | Default
  | [<CompiledName("foreground-tab")>] ForegroundTab
  | [<CompiledName("background-tab")>] BackgroundTab
  | [<CompiledName("new-window")>] NewWindow
  | [<CompiledName("save-to-disk")>] SaveToDisk
  | Other

[<StringEnum; RequireQualifiedAccess>]
type WebContentType =
  | BackgroundPage
  | Window
  | BrowserView
  | Remote
  | Webview
  | Offscreen

[<StringEnum; RequireQualifiedAccess>]
type WebContentSaveType =
  /// Save only the HTML of the page.
  | [<CompiledName("HTMLOnly")>] HtmlOnly
  /// Save complete-html page.
  | [<CompiledName("HTMLComplete")>] HtmlComplete
  /// Save complete-html page as MHTML.
  | [<CompiledName("MHTML")>] Mhtml

[<StringEnum; RequireQualifiedAccess>]
type WebRtcIpHandlingPolicy =
  /// Exposes user's public and local IPs. This is the default behavior. When
  /// this policy is used, WebRTC has the right to enumerate all interfaces and
  /// bind them to discover public interfaces.
  | Default
  /// Exposes user's public IP, but does not expose user's local IP. When this
  /// policy is used, WebRTC should only use the default route used by http.
  /// This doesn't expose any local addresses.
  | [<CompiledName("default_public_interface_only")>] DefaultPublicInterfaceOnly
  /// Exposes user's public and local IPs. When this policy is used, WebRTC
  /// should only use the default route used by http. This also exposes the
  /// associated default private address. Default route is the route chosen by
  /// the OS on a multi-homed endpoint.
  | [<CompiledName("default_public_and_private_interfaces")>] DefaultPublicAndPrivateInterfaces
  /// Does not expose public or local IPs. When this policy is used, WebRTC
  /// should only use TCP to contact peers or servers unless the proxy server
  /// supports UDP.
  | [<CompiledName("disable_non_proxied_udp")>] DisableNonProxiedUdp

[<StringEnum; RequireQualifiedAccess>]
type StopFindInPageAction =
  /// Clear the selection.
  | ClearSelection
  /// Translate the selection into a normal selection.
  | KeepSelection
  /// Focus and click the selection node.
  | ActivateSelection

[<StringEnum; RequireQualifiedAccess>]
type CursorType =
  | Default
  | Crosshair
  | Pointer
  | Text
  | Wait
  | Help
  | [<CompiledName("e-resize")>] EResize
  | [<CompiledName("n-resize")>] NResize
  | [<CompiledName("ne-resize")>] NEResize
  | [<CompiledName("nw-resize")>] NWResize
  | [<CompiledName("s-resize")>] SResize
  | [<CompiledName("se-resize")>] SEResize
  | [<CompiledName("sw-resize")>] SWResize
  | [<CompiledName("w-resize")>] WResize
  | [<CompiledName("ns-resize")>] NSResize
  | [<CompiledName("ew-resize")>] EWResize
  | [<CompiledName("nesw-resize")>] NESWResize
  | [<CompiledName("nwse-resize")>] NWSEResize
  | [<CompiledName("col-resize")>] ColResize
  | [<CompiledName("row-resize")>] RowResize
  | [<CompiledName("m-panning")>] MPanning
  | [<CompiledName("e-panning")>] EPanning
  | [<CompiledName("n-panning")>] NPanning
  | [<CompiledName("ne-panning")>] NEPanning
  | [<CompiledName("nw-panning")>] NWPanning
  | [<CompiledName("s-panning")>] SPanning
  | [<CompiledName("se-panning")>] SEPanning
  | [<CompiledName("sw-panning")>] SWPanning
  | [<CompiledName("w-panning")>] WPanning
  | Move
  | [<CompiledName("vertical-text")>] VerticalText
  | Cell
  | [<CompiledName("context-menu")>] ContextMenu
  | Alias
  | Progress
  | [<CompiledName("nodrop")>] NoDrop
  | Copy
  | None
  | [<CompiledName("not-allowed")>] NotAllowed
  | [<CompiledName("zoom-in")>] ZoomIn
  | [<CompiledName("zoom-out")>] ZoomOut
  | Grab
  | Grabbing
  | Custom

[<StringEnum; RequireQualifiedAccess>]
type InputEventType =
  | MouseDown
  | MouseUp
  | MouseEnter
  | MouseLeave
  | ContextMenu
  | MouseWheel
  | MouseMove
  | KeyDown
  | KeyUp
  | Char

[<StringEnum; RequireQualifiedAccess>]
type InputEventModifier =
  | Shift
  | Control
  | Alt
  | Meta
  | IsKeypad
  | IsAutoRepeat
  | LeftButtonDown
  | MiddleButtonDown
  | RightButtonDown
  | CapsLock
  | NumLock
  | Left
  | Right

type SendInputEvent =
  /// The type of the event.
  abstract ``type``: InputEventType with get, set
  /// An array of modifiers of the event.
  abstract modifiers: InputEventModifier [] with get, set

type SendKeyboardEvent =
  inherit SendInputEvent
  /// The character that will be sent as the keyboard event. Must be a valid
  /// accelerator key. (You can use `!!Helpers.Key.A` etc. if you want.)
  abstract keyCode: string with get, set

[<StringEnum; RequireQualifiedAccess>]
type SendMouseEventButton =
  | Left
  | Middle
  | Right

type SendMouseEvent =
  inherit SendInputEvent
  abstract x: int with get, set
  abstract y: int with get, set
  abstract button: SendMouseEventButton with get, set
  abstract globalX: int with get, set
  abstract globalY: int with get, set
  abstract movementX: int with get, set
  abstract movementY: int with get, set
  abstract clickCount: int with get, set

type SendMouseWheelEvent =
  inherit SendInputEvent
  abstract deltaX: int with get, set
  abstract deltaY: int with get, set
  abstract wheelTicksX: int with get, set
  abstract wheelTicksY: int with get, set
  abstract accelerationRatioX: int with get, set
  abstract accelerationRatioY: int with get, set
  abstract hasPreciseScrollingDeltas: bool with get, set
  abstract canScroll: bool with get, set



type WebContents =
  inherit EventEmitter<WebContents>
  /// Emitted when the navigation is done, i.e. the spinner of the tab has
  /// stopped spinning, and the `onload` event was dispatched.
  [<Emit "$0.on('did-finish-load',$1)">] abstract onDidFinishLoad: listener: (Event -> unit) -> WebContents
  /// See onDidFinishLoad.
  [<Emit "$0.once('did-finish-load',$1)">] abstract onceDidFinishLoad: listener: (Event -> unit) -> WebContents
  /// See onDidFinishLoad.
  [<Emit "$0.addListener('did-finish-load',$1)">] abstract addListenerDidFinishLoad: listener: (Event -> unit) -> WebContents
  /// See onDidFinishLoad.
  [<Emit "$0.removeListener('did-finish-load',$1)">] abstract removeListenerDidFinishLoad: listener: (Event -> unit) -> WebContents
  /// This event is like did-finish-load but emitted when the load failed or was
  /// cancelled, e.g. window.stop() is invoked. The full list of error codes and
  /// their meaning is available here:
  /// https://cs.chromium.org/chromium/src/net/base/net_error_list.h
  ///
  /// Additional parameters:
  ///   - errorCode
  ///   - errorDescription
  ///   - validatedURL
  ///   - isMainFrame
  ///   - frameProcessId
  ///   - frameRoutingId
  [<Emit "$0.on('did-fail-load',$1)">] abstract onDidFailLoad: listener: (Event -> int -> string -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFailLoad.
  [<Emit "$0.once('did-fail-load',$1)">] abstract onceDidFailLoad: listener: (Event -> int -> string -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFailLoad.
  [<Emit "$0.addListener('did-fail-load',$1)">] abstract addListenerDidFailLoad: listener: (Event -> int -> string -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFailLoad.
  [<Emit "$0.removeListener('did-fail-load',$1)">] abstract removeListenerDidFailLoad: listener: (Event -> int -> string -> string -> bool -> int -> int -> unit) -> WebContents
  /// Emitted when a frame has done navigation.
  ///
  /// Additional parameters:
  ///   - isMainFrame
  ///   - frameProcessId
  ///   - frameRoutingId
  [<Emit "$0.on('did-frame-finish-load',$1)">] abstract onDidFrameFinishLoad: listener: (Event -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFrameFinishLoad.
  [<Emit "$0.once('did-frame-finish-load',$1)">] abstract onceDidFrameFinishLoad: listener: (Event -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFrameFinishLoad.
  [<Emit "$0.addListener('did-frame-finish-load',$1)">] abstract addListenerDidFrameFinishLoad: listener: (Event -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFrameFinishLoad.
  [<Emit "$0.removeListener('did-frame-finish-load',$1)">] abstract removeListenerDidFrameFinishLoad: listener: (Event -> bool -> int -> int -> unit) -> WebContents
  /// Corresponds to the points in time when the spinner of the tab started
  /// spinning.
  [<Emit "$0.on('did-start-loading',$1)">] abstract onDidStartLoading: listener: (Event -> unit) -> WebContents
  /// See onDidStartLoading.
  [<Emit "$0.once('did-start-loading',$1)">] abstract onceDidStartLoading: listener: (Event -> unit) -> WebContents
  /// See onDidStartLoading.
  [<Emit "$0.addListener('did-start-loading',$1)">] abstract addListenerDidStartLoading: listener: (Event -> unit) -> WebContents
  /// See onDidStartLoading.
  [<Emit "$0.removeListener('did-start-loading',$1)">] abstract removeListenerDidStartLoading: listener: (Event -> unit) -> WebContents
  /// Corresponds to the points in time when the spinner of the tab stopped
  /// spinning.
  [<Emit "$0.on('did-stop-loading',$1)">] abstract onDidStopLoading: listener: (Event -> unit) -> WebContents
  /// See onDidStopLoading.
  [<Emit "$0.once('did-stop-loading',$1)">] abstract onceDidStopLoading: listener: (Event -> unit) -> WebContents
  /// See onDidStopLoading.
  [<Emit "$0.addListener('did-stop-loading',$1)">] abstract addListenerDidStopLoading: listener: (Event -> unit) -> WebContents
  /// See onDidStopLoading.
  [<Emit "$0.removeListener('did-stop-loading',$1)">] abstract removeListenerDidStopLoading: listener: (Event -> unit) -> WebContents
  /// Emitted when the document in the given frame is loaded.
  [<Emit "$0.on('dom-ready',$1)">] abstract onDomReady: listener: (Event -> unit) -> WebContents
  /// See onDomReady.
  [<Emit "$0.once('dom-ready',$1)">] abstract onceDomReady: listener: (Event -> unit) -> WebContents
  /// See onDomReady.
  [<Emit "$0.addListener('dom-ready',$1)">] abstract addListenerDomReady: listener: (Event -> unit) -> WebContents
  /// See onDomReady.
  [<Emit "$0.removeListener('dom-ready',$1)">] abstract removeListenerDomReady: listener: (Event -> unit) -> WebContents
  /// Fired when page title is set during navigation. explicitSet is false when
  /// title is synthesized from file url.
  ///
  /// Additional parameters:
  ///   - title
  ///   - explicitSet
  [<Emit "$0.on('page-title-updated',$1)">] abstract onPageTitleUpdated: listener: (Event -> string -> bool -> unit) -> WebContents
  /// See onPageTitleUpdated.
  [<Emit "$0.once('page-title-updated',$1)">] abstract oncePageTitleUpdated: listener: (Event -> string -> bool -> unit) -> WebContents
  /// See onPageTitleUpdated.
  [<Emit "$0.addListener('page-title-updated',$1)">] abstract addListenerPageTitleUpdated: listener: (Event -> string -> bool -> unit) -> WebContents
  /// See onPageTitleUpdated.
  [<Emit "$0.removeListener('page-title-updated',$1)">] abstract removeListenerPageTitleUpdated: listener: (Event -> string -> bool -> unit) -> WebContents
  /// Emitted when page receives favicon urls. Called with the URLs.
  [<Emit "$0.on('page-favicon-updated',$1)">] abstract onPageFaviconUpdated: listener: (Event -> string [] -> unit) -> WebContents
  /// See onPageFaviconUpdated.
  [<Emit "$0.once('page-favicon-updated',$1)">] abstract oncePageFaviconUpdated: listener: (Event -> string [] -> unit) -> WebContents
  /// See onPageFaviconUpdated.
  [<Emit "$0.addListener('page-favicon-updated',$1)">] abstract addListenerPageFaviconUpdated: listener: (Event -> string [] -> unit) -> WebContents
  /// See onPageFaviconUpdated.
  [<Emit "$0.removeListener('page-favicon-updated',$1)">] abstract removeListenerPageFaviconUpdated: listener: (Event -> string [] -> unit) -> WebContents
  /// Emitted when the page requests to open a new window for a url. It could be
  /// requested by window.open or an external link like <a target='_blank'>.
  ///
  /// By default a new BrowserWindow will be created for the url.
  ///
  /// Calling event.preventDefault() will prevent Electron from automatically
  /// creating a new BrowserWindow. If you call event.preventDefault() and
  /// manually create a new BrowserWindow then you must set event.newGuest to
  /// reference the new BrowserWindow instance, failing to do so may result in
  /// unexpected behavior.
  ///
  /// Additional parameters:
  ///   - url
  ///   - frameName
  ///   - disposition
  ///   - options: The options which will be used for creating the new BrowserWindow
  ///   - additionalFeatures: The non-standard features (features not handled by Chromium or Electron) given to window.open()
  ///   - referrer: The referrer that will be passed to the new window. May or may not result in the Referer header being sent, depending on the referrer policy.
  [<Emit "$0.on('new-window',$1)">] abstract onNewWindow: listener: (Event -> string -> string -> WindowDisposition -> BrowserWindowOptions -> string [] -> Referrer -> unit) -> WebContents
  /// See onNewWindow.
  [<Emit "$0.once('new-window',$1)">] abstract onceNewWindow: listener: (Event -> string -> string -> WindowDisposition -> BrowserWindowOptions -> string [] -> Referrer -> unit) -> WebContents
  /// See onNewWindow.
  [<Emit "$0.addListener('new-window',$1)">] abstract addListenerNewWindow: listener: (Event -> string -> string -> WindowDisposition -> BrowserWindowOptions -> string [] -> Referrer -> unit) -> WebContents
  /// See onNewWindow.
  [<Emit "$0.removeListener('new-window',$1)">] abstract removeListenerNewWindow: listener: (Event -> string -> string -> WindowDisposition -> BrowserWindowOptions -> string [] -> Referrer -> unit) -> WebContents
  /// Emitted when a user or the page wants to start navigation. It can happen
  /// when the window.location object is changed or a user clicks a link in the
  /// page. The listener receives the URL.
  ///
  /// This event will not emit when the navigation is started programmatically
  /// with APIs like webContents.loadURL and webContents.back.
  ///
  /// It is also not emitted for in-page navigations, such as clicking anchor
  /// links or updating the window.location.hash. Use did-navigate-in-page event
  /// for this purpose.
  ///
  /// Calling event.preventDefault() will prevent the navigation.
  [<Emit "$0.on('will-navigate',$1)">] abstract onWillNavigate: listener: (Event -> string -> unit) -> WebContents
  /// See onWillNavigate.
  [<Emit "$0.once('will-navigate',$1)">] abstract onceWillNavigate: listener: (Event -> string -> unit) -> WebContents
  /// See onWillNavigate.
  [<Emit "$0.addListener('will-navigate',$1)">] abstract addListenerWillNavigate: listener: (Event -> string -> unit) -> WebContents
  /// See onWillNavigate.
  [<Emit "$0.removeListener('will-navigate',$1)">] abstract removeListenerWillNavigate: listener: (Event -> string -> unit) -> WebContents
  /// Emitted when any frame (including main) starts navigating. isInplace will
  /// be true for in-page navigations.
  ///
  /// Additional parameters:
  ///   - url
  ///   - isInPlace
  ///   - isMainFrame
  ///   - frameProcessId
  ///   - frameRoutingId
  [<Emit "$0.on('did-start-navigation',$1)">] abstract onDidStartNavigation: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onDidStartNavigation.
  [<Emit "$0.once('did-start-navigation',$1)">] abstract onceDidStartNavigation: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onDidStartNavigation.
  [<Emit "$0.addListener('did-start-navigation',$1)">] abstract addListenerDidStartNavigation: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onDidStartNavigation.
  [<Emit "$0.removeListener('did-start-navigation',$1)">] abstract removeListenerDidStartNavigation: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// Emitted as a server side redirect occurs during navigation. For example a
  /// 302 redirect.
  ///
  /// This event will be emitted after did-start-navigation and always before
  /// the did-redirect-navigation event for the same navigation.
  ///
  /// Calling event.preventDefault() will prevent the navigation (not just the
  /// redirect).
  ///
  /// Additional parameters:
  ///   - url
  ///   - isInPlace
  ///   - isMainFrame
  ///   - frameProcessId
  ///   - frameRoutingId
  [<Emit "$0.on('will-redirect',$1)">] abstract onWillRedirect: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onWillRedirect.
  [<Emit "$0.once('will-redirect',$1)">] abstract onceWillRedirect: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onWillRedirect.
  [<Emit "$0.addListener('will-redirect',$1)">] abstract addListenerWillRedirect: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onWillRedirect.
  [<Emit "$0.removeListener('will-redirect',$1)">] abstract removeListenerWillRedirect: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// Emitted after a server side redirect occurs during navigation. For example
  /// a 302 redirect.
  ///
  /// This event can not be prevented, if you want to prevent redirects you
  /// should checkout out the `will-redirect` event.
  ///
  /// Additional parameters:
  ///   - url
  ///   - isInPlace
  ///   - isMainFrame
  ///   - frameProcessId
  ///   - frameRoutingId
  [<Emit "$0.on('did-redirect-navigation',$1)">] abstract onDidRedirectNavigation: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onDidRedirectNavigation.
  [<Emit "$0.once('did-redirect-navigation',$1)">] abstract onceDidRedirectNavigation: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onDidRedirectNavigation.
  [<Emit "$0.addListener('did-redirect-navigation',$1)">] abstract addListenerDidRedirectNavigation: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// See onDidRedirectNavigation.
  [<Emit "$0.removeListener('did-redirect-navigation',$1)">] abstract removeListenerDidRedirectNavigation: listener: (Event -> string -> bool -> bool -> int -> int -> unit) -> WebContents
  /// Emitted when a main frame navigation is done.
  ///
  /// This event is not emitted for in-page navigations, such as clicking anchor
  /// links or updating the window.location.hash. Use `did-navigate-in-page`
  /// event for this purpose.
  ///
  /// Additional parameters:
  ///   - url
  ///   - httpResponseCode: -1 for non HTTP navigations
  ///   - httpStatusText: empty for non HTTP navigations
  [<Emit "$0.on('did-navigate',$1)">] abstract onDidNavigate: listener: (Event -> string -> int -> string -> unit) -> WebContents
  /// See onDidNavigate.
  [<Emit "$0.once('did-navigate',$1)">] abstract onceDidNavigate: listener: (Event -> string -> int -> string -> unit) -> WebContents
  /// See onDidNavigate.
  [<Emit "$0.addListener('did-navigate',$1)">] abstract addListenerDidNavigate: listener: (Event -> string -> int -> string -> unit) -> WebContents
  /// See onDidNavigate.
  [<Emit "$0.removeListener('did-navigate',$1)">] abstract removeListenerDidNavigate: listener: (Event -> string -> int -> string -> unit) -> WebContents
  /// Emitted when any frame navigation is done.
  ///
  /// This event is not emitted for in-page navigations, such as clicking anchor
  /// links or updating the window.location.hash. Use `did-navigate-in-page`
  /// event for this purpose.
  ///
  /// Additional parameters:
  ///   - url
  ///   - httpResponseCode: -1 for non HTTP navigations
  ///   - httpStatusText: empty for non HTTP navigations
  ///   - isMainFrame
  ///   - frameProcessId
  ///   - frameRoutingId
  [<Emit "$0.on('did-frame-navigate',$1)">] abstract onDidFrameNavigate: listener: (Event -> string -> int -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFrameNavigate.
  [<Emit "$0.once('did-frame-navigate',$1)">] abstract onceDidFrameNavigate: listener: (Event -> string -> int -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFrameNavigate.
  [<Emit "$0.addListener('did-frame-navigate',$1)">] abstract addListenerDidFrameNavigate: listener: (Event -> string -> int -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidFrameNavigate.
  [<Emit "$0.removeListener('did-frame-navigate',$1)">] abstract removeListenerDidFrameNavigate: listener: (Event -> string -> int -> string -> bool -> int -> int -> unit) -> WebContents
  /// Emitted when an in-page navigation happened in any frame.
  ///
  /// When in-page navigation happens, the page URL changes but does not cause
  /// navigation outside of the page. Examples of this occurring are when anchor
  /// links are clicked or when the DOM `hashchange` event is triggered.
  ///
  /// Additional parameters:
  ///   - url
  ///   - isMainFrame
  ///   - frameProcessId
  ///   - frameRoutingId
  [<Emit "$0.on('did-navigate-in-page',$1)">] abstract onDidNavigateInPage: listener: (Event -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidNavigateInPage.
  [<Emit "$0.once('did-navigate-in-page',$1)">] abstract onceDidNavigateInPage: listener: (Event -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidNavigateInPage.
  [<Emit "$0.addListener('did-navigate-in-page',$1)">] abstract addListenerDidNavigateInPage: listener: (Event -> string -> bool -> int -> int -> unit) -> WebContents
  /// See onDidNavigateInPage.
  [<Emit "$0.removeListener('did-navigate-in-page',$1)">] abstract removeListenerDidNavigateInPage: listener: (Event -> string -> bool -> int -> int -> unit) -> WebContents
  /// Emitted when a beforeunload event handler is attempting to cancel a page
  /// unload.
  ///
  /// Calling event.preventDefault() will ignore the beforeunload event handler
  /// and allow the page to be unloaded.
  [<Emit "$0.on('will-prevent-unload',$1)">] abstract onWillPreventUnload: listener: (Event -> unit) -> WebContents
  /// See onWillPreventUnload.
  [<Emit "$0.once('will-prevent-unload',$1)">] abstract onceWillPreventUnload: listener: (Event -> unit) -> WebContents
  /// See onWillPreventUnload.
  [<Emit "$0.addListener('will-prevent-unload',$1)">] abstract addListenerWillPreventUnload: listener: (Event -> unit) -> WebContents
  /// See onWillPreventUnload.
  [<Emit "$0.removeListener('will-prevent-unload',$1)">] abstract removeListenerWillPreventUnload: listener: (Event -> unit) -> WebContents
  /// Emitted when the renderer process crashes or is killed. Called with true
  /// if killed, false if crashed.
  [<Emit "$0.on('crashed',$1)">] abstract onCrashed: listener: (Event -> bool -> unit) -> WebContents
  /// See onCrashed.
  [<Emit "$0.once('crashed',$1)">] abstract onceCrashed: listener: (Event -> bool -> unit) -> WebContents
  /// See onCrashed.
  [<Emit "$0.addListener('crashed',$1)">] abstract addListenerCrashed: listener: (Event -> bool -> unit) -> WebContents
  /// See onCrashed.
  [<Emit "$0.removeListener('crashed',$1)">] abstract removeListenerCrashed: listener: (Event -> bool -> unit) -> WebContents
  /// Emitted when the web page becomes unresponsive.
  [<Emit "$0.on('unresponsive',$1)">] abstract onUnresponsive: listener: (Event -> unit) -> WebContents
  /// See onUnresponsive.
  [<Emit "$0.once('unresponsive',$1)">] abstract onceUnresponsive: listener: (Event -> unit) -> WebContents
  /// See onUnresponsive.
  [<Emit "$0.addListener('unresponsive',$1)">] abstract addListenerUnresponsive: listener: (Event -> unit) -> WebContents
  /// See onUnresponsive.
  [<Emit "$0.removeListener('unresponsive',$1)">] abstract removeListenerUnresponsive: listener: (Event -> unit) -> WebContents
  /// Emitted when the unresponsive web page becomes responsive again.
  [<Emit "$0.on('responsive',$1)">] abstract onResponsive: listener: (Event -> unit) -> WebContents
  /// See onResponsive.
  [<Emit "$0.once('responsive',$1)">] abstract onceResponsive: listener: (Event -> unit) -> WebContents
  /// See onResponsive.
  [<Emit "$0.addListener('responsive',$1)">] abstract addListenerResponsive: listener: (Event -> unit) -> WebContents
  /// See onResponsive.
  [<Emit "$0.removeListener('responsive',$1)">] abstract removeListenerResponsive: listener: (Event -> unit) -> WebContents
  /// Emitted when a plugin process has crashed. Called with the name and version.
  [<Emit "$0.on('plugin-crashed',$1)">] abstract onPluginCrashed: listener: (Event -> string -> string -> unit) -> WebContents
  /// See onPluginCrashed.
  [<Emit "$0.once('plugin-crashed',$1)">] abstract oncePluginCrashed: listener: (Event -> string -> string -> unit) -> WebContents
  /// See onPluginCrashed.
  [<Emit "$0.addListener('plugin-crashed',$1)">] abstract addListenerPluginCrashed: listener: (Event -> string -> string -> unit) -> WebContents
  /// See onPluginCrashed.
  [<Emit "$0.removeListener('plugin-crashed',$1)">] abstract removeListenerPluginCrashed: listener: (Event -> string -> string -> unit) -> WebContents
  /// Emitted when webContents is destroyed.
  [<Emit "$0.on('destroyed',$1)">] abstract onDestroyed: listener: (Event -> unit) -> WebContents
  /// See onDestroyed.
  [<Emit "$0.once('destroyed',$1)">] abstract onceDestroyed: listener: (Event -> unit) -> WebContents
  /// See onDestroyed.
  [<Emit "$0.addListener('destroyed',$1)">] abstract addListenerDestroyed: listener: (Event -> unit) -> WebContents
  /// See onDestroyed.
  [<Emit "$0.removeListener('destroyed',$1)">] abstract removeListenerDestroyed: listener: (Event -> unit) -> WebContents
  /// Emitted before dispatching the keydown and keyup events in the page.
  /// Calling event.preventDefault will prevent the page keydown/keyup events
  /// and the menu shortcuts.
  ///
  /// To only prevent the menu shortcuts, use setIgnoreMenuShortcuts.
  [<Emit "$0.on('before-input-event',$1)">] abstract onBeforeInputEvent: listener: (Event -> BeforeInputEventData -> unit) -> WebContents
  /// See onBeforeInputEvent.
  [<Emit "$0.once('before-input-event',$1)">] abstract onceBeforeInputEvent: listener: (Event -> BeforeInputEventData -> unit) -> WebContents
  /// See onBeforeInputEvent.
  [<Emit "$0.addListener('before-input-event',$1)">] abstract addListenerBeforeInputEvent: listener: (Event -> BeforeInputEventData -> unit) -> WebContents
  /// See onBeforeInputEvent.
  [<Emit "$0.removeListener('before-input-event',$1)">] abstract removeListenerBeforeInputEvent: listener: (Event -> BeforeInputEventData -> unit) -> WebContents
  /// Emitted when the window enters a full-screen state triggered by HTML API.
  [<Emit "$0.on('enter-html-full-screen',$1)">] abstract onEnterHtmlFullScreen: listener: (Event -> unit) -> WebContents
  /// See onEnterHtmlFullScreen.
  [<Emit "$0.once('enter-html-full-screen',$1)">] abstract onceEnterHtmlFullScreen: listener: (Event -> unit) -> WebContents
  /// See onEnterHtmlFullScreen.
  [<Emit "$0.addListener('enter-html-full-screen',$1)">] abstract addEnterHtmlFullScreen: listener: (Event -> unit) -> WebContents
  /// See onEnterHtmlFullScreen.
  [<Emit "$0.removeListener('enter-html-full-screen',$1)">] abstract removeEnterHtmlFullScreen: listener: (Event -> unit) -> WebContents
  /// Emitted when the window leaves a full-screen state triggered by HTML API.
  [<Emit "$0.on('leave-html-full-screen',$1)">] abstract onLeaveHtmlFullScreen: listener: (Event -> unit) -> WebContents
  /// See onLeaveHtmlFullScreen.
  [<Emit "$0.once('leave-html-full-screen',$1)">] abstract onceLeaveHtmlFullScreen: listener: (Event -> unit) -> WebContents
  /// See onLeaveHtmlFullScreen.
  [<Emit "$0.addListener('leave-html-full-screen',$1)">] abstract addLeaveHtmlFullScreen: listener: (Event -> unit) -> WebContents
  /// See onLeaveHtmlFullScreen.
  [<Emit "$0.removeListener('leave-html-full-screen',$1)">] abstract removeLeaveHtmlFullScreen: listener: (Event -> unit) -> WebContents
  /// Emitted when DevTools is opened.
  [<Emit "$0.on('devtools-opened',$1)">] abstract onDevtoolsOpened: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsOpened.
  [<Emit "$0.once('devtools-opened',$1)">] abstract onceDevtoolsOpened: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsOpened.
  [<Emit "$0.addListener('devtools-opened',$1)">] abstract addListenerDevtoolsOpened: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsOpened.
  [<Emit "$0.removeListener('devtools-opened',$1)">] abstract removeListenerDevtoolsOpened: listener: (Event -> unit) -> WebContents
  /// Emitted when DevTools is closed.
  [<Emit "$0.on('devtools-closed',$1)">] abstract onDevtoolsClosed: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsClosed.
  [<Emit "$0.once('devtools-closed',$1)">] abstract onceDevtoolsClosed: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsClosed.
  [<Emit "$0.addListener('devtools-closed',$1)">] abstract addListenerDevtoolsClosed: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsClosed.
  [<Emit "$0.removeListener('devtools-closed',$1)">] abstract removeListenerDevtoolsClosed: listener: (Event -> unit) -> WebContents
  /// Emitted when DevTools is focused / opened.
  [<Emit "$0.on('devtools-focused',$1)">] abstract onDevtoolsFocused: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsFocused.
  [<Emit "$0.once('devtools-focused',$1)">] abstract onceDevtoolsFocused: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsFocused.
  [<Emit "$0.addListener('devtools-focused',$1)">] abstract addListenerDevtoolsFocused: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsFocused.
  [<Emit "$0.removeListener('devtools-focused',$1)">] abstract removeListenerDevtoolsFocused: listener: (Event -> unit) -> WebContents
  /// Emitted when failed to verify the certificate for `url`.
  ///
  /// The usage is the same with the `certificate-error` event of `app`.
  ///
  /// Additional parameters:
  ///   - url
  ///   - error
  ///   - certificate
  ///   - callback: call with true if the certificate can be trusted.
  [<Emit "$0.on('certificate-error',$1)">] abstract onCertificateError: listener: (Event -> string -> string -> Certificate -> (bool -> unit) -> unit) -> WebContents
  /// See onCertificateError.
  [<Emit "$0.once('certificate-error',$1)">] abstract onceCertificateError: listener: (Event -> string -> string -> Certificate -> (bool -> unit) -> unit) -> WebContents
  /// See onCertificateError.
  [<Emit "$0.addListener('certificate-error',$1)">] abstract addListenerCertificateError: listener: (Event -> string -> string -> Certificate -> (bool -> unit) -> unit) -> WebContents
  /// See onCertificateError.
  [<Emit "$0.removeListener('certificate-error',$1)">] abstract removeListenerCertificateError: listener: (Event -> string -> string -> Certificate -> (bool -> unit) -> unit) -> WebContents
  /// Emitted when a client certificate is requested.
  ///
  /// The usage is the same with the `select-client-certificate` event of `app`.
  ///
  /// Additional parameters:
  ///   - url
  ///   - certificateList
  ///   - callback: call with a certificate from the given list to select it
  [<Emit "$0.on('select-client-certificate',$1)">] abstract onSelectClientCertificate: listener: (Event -> string -> Certificate [] -> (Certificate -> unit) -> unit) -> WebContents
  /// See onSelectClientCertificate.
  [<Emit "$0.once('select-client-certificate',$1)">] abstract onceSelectClientCertificate: listener: (Event -> string -> Certificate [] -> (Certificate -> unit) -> unit) -> WebContents
  /// See onSelectClientCertificate.
  [<Emit "$0.addListener('select-client-certificate',$1)">] abstract addListenerSelectClientCertificate: listener: (Event -> string -> Certificate [] -> (Certificate -> unit) -> unit) -> WebContents
  /// See onSelectClientCertificate.
  [<Emit "$0.removeListener('select-client-certificate',$1)">] abstract removeListenerSelectClientCertificate: listener: (Event -> string -> Certificate [] -> (Certificate -> unit) -> unit) -> WebContents
  /// Emitted when webContents wants to do basic auth.
  ///
  /// The usage is the same with the `login` event of `app`.
  ///
  /// Additional parameters:
  ///
  ///   - request
  ///   - authInfo
  ///   - callback(username, password)
  [<Emit "$0.on('login',$1)">] abstract onLogin: listener: (Event -> LoginRequest -> AuthInfo -> (string -> string -> unit) -> unit) -> WebContents
  /// See onLogin.
  [<Emit "$0.once('login',$1)">] abstract onceLogin: listener: (Event -> LoginRequest -> AuthInfo -> (string -> string -> unit) -> unit) -> WebContents
  /// See onLogin.
  [<Emit "$0.addListener('login',$1)">] abstract addListenerLogin: listener: (Event -> LoginRequest -> AuthInfo -> (string -> string -> unit) -> unit) -> WebContents
  /// See onLogin.
  [<Emit "$0.removeListener('login',$1)">] abstract removeListenerLogin: listener: (Event -> LoginRequest -> AuthInfo -> (string -> string -> unit) -> unit) -> WebContents
  /// Emitted when a result is available for [webContents.findInPage] request.
  [<Emit "$0.on('found-in-page',$1)">] abstract onFoundInPage: listener: (Event -> FoundInPageResult -> unit) -> WebContents
  /// See onFoundInPage.
  [<Emit "$0.once('found-in-page',$1)">] abstract onceFoundInPage: listener: (Event -> FoundInPageResult -> unit) -> WebContents
  /// See onFoundInPage.
  [<Emit "$0.addListener('found-in-page',$1)">] abstract addListenerFoundInPage: listener: (Event -> FoundInPageResult -> unit) -> WebContents
  /// See onFoundInPage.
  [<Emit "$0.removeListener('found-in-page',$1)">] abstract removeListenerFoundInPage: listener: (Event -> FoundInPageResult -> unit) -> WebContents
  /// Emitted when media starts playing.
  [<Emit "$0.on('media-started-playing',$1)">] abstract onMediaStartedPlaying: listener: (Event -> unit) -> WebContents
  /// See onMediaStartedPlaying.
  [<Emit "$0.once('media-started-playing',$1)">] abstract onceMediaStartedPlaying: listener: (Event -> unit) -> WebContents
  /// See onMediaStartedPlaying.
  [<Emit "$0.addListener('media-started-playing',$1)">] abstract addListenerMediaStartedPlaying: listener: (Event -> unit) -> WebContents
  /// See onMediaStartedPlaying.
  [<Emit "$0.removeListener('media-started-playing',$1)">] abstract removeListenerMediaStartedPlaying: listener: (Event -> unit) -> WebContents
  /// Emitted when media is paused or done playing.
  [<Emit "$0.on('media-paused',$1)">] abstract onMediaPaused: listener: (Event -> unit) -> WebContents
  /// See onMediaPaused.
  [<Emit "$0.once('media-paused',$1)">] abstract onceMediaPaused: listener: (Event -> unit) -> WebContents
  /// See onMediaPaused.
  [<Emit "$0.addListener('media-paused',$1)">] abstract addListenerMediaPaused: listener: (Event -> unit) -> WebContents
  /// See onMediaPaused.
  [<Emit "$0.removeListener('media-paused',$1)">] abstract removeListenerMediaPaused: listener: (Event -> unit) -> WebContents
  /// Emitted when a page's theme color changes. This is usually due to
  /// encountering a meta tag.
  ///
  /// The listener gets the theme color in format "#rrggbb". It is None when no
  /// theme color is set.
  [<Emit "$0.on('did-change-theme-color',$1)">] abstract onDidChangeThemeColor: listener: (Event -> string option -> unit) -> WebContents
  /// See onDidChangeThemeColor.
  [<Emit "$0.once('did-change-theme-color',$1)">] abstract onceDidChangeThemeColor: listener: (Event -> string option -> unit) -> WebContents
  /// See onDidChangeThemeColor.
  [<Emit "$0.addListener('did-change-theme-color',$1)">] abstract addListenerDidChangeThemeColor: listener: (Event -> string option -> unit) -> WebContents
  /// See onDidChangeThemeColor.
  [<Emit "$0.removeListener('did-change-theme-color',$1)">] abstract removeListenerDidChangeThemeColor: listener: (Event -> string option -> unit) -> WebContents
  /// Emitted when mouse moves over a link or the keyboard moves the focus to a
  /// link. The listener receives the url.
  [<Emit "$0.on('update-target-url',$1)">] abstract onUpdateTargetUrl: listener: (Event -> string -> unit) -> WebContents
  /// See onUpdateTargetUrl.
  [<Emit "$0.once('update-target-url',$1)">] abstract onceUpdateTargetUrl: listener: (Event -> string -> unit) -> WebContents
  /// See onUpdateTargetUrl.
  [<Emit "$0.addListener('update-target-url',$1)">] abstract addListenerUpdateTargetUrl: listener: (Event -> string -> unit) -> WebContents
  /// See onUpdateTargetUrl.
  [<Emit "$0.removeListener('update-target-url',$1)">] abstract removeListenerUpdateTargetUrl: listener: (Event -> string -> unit) -> WebContents
  /// Emitted when the cursor's type changes. If the type is CursorType.Custom,
  /// the `image` parameter will hold the custom cursor image, and `scale`,
  /// `size` and `hotspot` will hold additional information about the custom
  /// cursor.
  ///
  /// Additional parameters:
  ///
  ///   - type
  ///   - image
  ///   - scale: scaling factor for the custom cursor
  ///   - size: the size of the image
  ///   - hotspot: coordinates of the custom cursor's hotspot
  [<Emit "$0.on('cursor-changed',$1)">] abstract onCursorChanged: listener: (Event -> CursorType -> NativeImage option -> float option -> Size option -> Point option -> unit) -> WebContents
  /// See onCursorChanged.
  [<Emit "$0.once('cursor-changed',$1)">] abstract onceCursorChanged: listener: (Event -> CursorType -> NativeImage option -> float option -> Size option -> Point option -> unit) -> WebContents
  /// See onCursorChanged.
  [<Emit "$0.addListener('cursor-changed',$1)">] abstract addListenerCursorChanged: listener: (Event -> CursorType -> NativeImage option option -> float option -> Size option -> Point -> unit) -> WebContents
  /// See onCursorChanged.
  [<Emit "$0.removeListener('cursor-changed',$1)">] abstract removeListenerCursorChanged: listener: (Event -> CursorType -> NativeImage option -> float option -> Size option -> Point option -> unit) -> WebContents
  /// Emitted when there is a new context menu that needs to be handled.
  [<Emit "$0.on('context-menu',$1)">] abstract onContextMenu: listener: (Event -> ContextMenuParams -> unit) -> WebContents
  /// See onContextMenu.
  [<Emit "$0.once('context-menu',$1)">] abstract onceContextMenu: listener: (Event -> ContextMenuParams -> unit) -> WebContents
  /// See onContextMenu.
  [<Emit "$0.addListener('context-menu',$1)">] abstract addListenerContextMenu: listener: (Event -> ContextMenuParams -> unit) -> WebContents
  /// See onContextMenu.
  [<Emit "$0.removeListener('context-menu',$1)">] abstract removeListenerContextMenu: listener: (Event -> ContextMenuParams -> unit) -> WebContents
  /// Emitted when bluetooth device needs to be selected on call to
  /// navigator.bluetooth.requestDevice. To use navigator.bluetooth api
  /// webBluetooth should be enabled. If event.preventDefault is not called,
  /// first available device will be selected. callback should be called with
  /// deviceId to be selected, passing empty string to callback will cancel the
  /// request.
  [<Emit "$0.on('select-bluetooth-device',$1)">] abstract onSelectBluetoothDevice: listener: (Event -> BluetoothDevice [] -> (string -> unit) -> unit) -> WebContents
  /// See onSelectBluetoothDevice.
  [<Emit "$0.once('select-bluetooth-device',$1)">] abstract onceSelectBluetoothDevice: listener: (Event -> BluetoothDevice [] -> (string -> unit) -> unit) -> WebContents
  /// See onSelectBluetoothDevice.
  [<Emit "$0.addListener('select-bluetooth-device',$1)">] abstract addListenerSelectBluetoothDevice: listener: (Event -> BluetoothDevice [] -> (string -> unit) -> unit) -> WebContents
  /// See onSelectBluetoothDevice.
  [<Emit "$0.removeListener('select-bluetooth-device',$1)">] abstract removeListenerSelectBluetoothDevice: listener: (Event -> BluetoothDevice [] -> (string -> unit) -> unit) -> WebContents
  /// Emitted when a new frame is generated. Only the dirty area is passed in
  /// the buffer.
  ///
  /// Additional parameters:
  ///
  ///   - dirtyRect
  ///   - image
  [<Emit "$0.on('paint',$1)">] abstract onPaint: listener: (Event -> Rectangle -> NativeImage -> unit) -> WebContents
  /// See onPaint.
  [<Emit "$0.once('paint',$1)">] abstract oncePaint: listener: (Event -> Rectangle -> NativeImage -> unit) -> WebContents
  /// See onPaint.
  [<Emit "$0.addListener('paint',$1)">] abstract addListenerPaint: listener: (Event -> Rectangle -> NativeImage -> unit) -> WebContents
  /// See onPaint.
  [<Emit "$0.removeListener('paint',$1)">] abstract removeListenerPaint: listener: (Event -> Rectangle -> NativeImage -> unit) -> WebContents
  /// Emitted when the devtools window instructs the webContents to reload
  [<Emit "$0.on('devtools-reload-page',$1)">] abstract onDevtoolsReloadPage: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsReloadPage.
  [<Emit "$0.once('devtools-reload-page',$1)">] abstract onceDevtoolsReloadPage: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsReloadPage.
  [<Emit "$0.addListener('devtools-reload-page',$1)">] abstract addListenerDevtoolsReloadPage: listener: (Event -> unit) -> WebContents
  /// See onDevtoolsReloadPage.
  [<Emit "$0.removeListener('devtools-reload-page',$1)">] abstract removeListenerDevtoolsReloadPage: listener: (Event -> unit) -> WebContents
  /// Emitted when the associated window logs a console message. Will not be
  /// emitted for windows with offscreen rendering enabled.
  ///
  /// Additional parameters:
  ///
  ///   - level
  ///   - message
  ///   - line
  ///   - sourceId
  [<Emit "$0.on('console-message',$1)">] abstract onConsoleMessage: listener: (Event -> int -> string -> int -> string -> unit) -> WebContents
  /// See onConsoleMessage.
  [<Emit "$0.once('console-message',$1)">] abstract onceConsoleMessage: listener: (Event -> int -> string -> int -> string -> unit) -> WebContents
  /// See onConsoleMessage.
  [<Emit "$0.addListener('console-message',$1)">] abstract addListenerConsoleMessage: listener: (Event -> int -> string -> int -> string -> unit) -> WebContents
  /// See onConsoleMessage.
  [<Emit "$0.removeListener('console-message',$1)">] abstract removeListenerConsoleMessage: listener: (Event -> int -> string -> int -> string -> unit) -> WebContents
  /// Emitted when the preload script preloadPath throws an unhandled exception
  /// error.
  ///
  /// Additional parameters:
  ///
  ///   - preloadPath
  ///   - error
  [<Emit "$0.on('preload-error',$1)">] abstract onPreloadError: listener: (Event -> string -> Error -> unit) -> WebContents
  /// See onPreloadError.
  [<Emit "$0.once('preload-error',$1)">] abstract oncePreloadError: listener: (Event -> string -> Error -> unit) -> WebContents
  /// See onPreloadError.
  [<Emit "$0.addListener('preload-error',$1)">] abstract addListenerPreloadError: listener: (Event -> string -> Error -> unit) -> WebContents
  /// See onPreloadError.
  [<Emit "$0.removeListener('preload-error',$1)">] abstract removeListenerPreloadError: listener: (Event -> string -> Error -> unit) -> WebContents
  /// Emitted when the renderer process sends an asynchronous message via
  /// ipcRenderer.send().
  ///
  /// Additional parameters:
  ///
  ///   - channel
  ///   - args
  [<Emit "$0.on('ipc-message',$1)">] abstract onIpcMessage: listener: (Event -> string -> obj [] -> unit) -> WebContents
  /// See onIpcMessage.
  [<Emit "$0.once('ipc-message',$1)">] abstract onceIpcMessage: listener: (Event -> string -> obj [] -> unit) -> WebContents
  /// See onIpcMessage.
  [<Emit "$0.addListener('ipc-message',$1)">] abstract addListenerIpcMessage: listener: (Event -> string -> obj [] -> unit) -> WebContents
  /// See onIpcMessage.
  [<Emit "$0.removeListener('ipc-message',$1)">] abstract removeListenerIpcMessage: listener: (Event -> string -> obj [] -> unit) -> WebContents
  /// Emitted when the renderer process sends a synchronous message via
  /// ipcRenderer.sendSync().
  ///
  /// Additional parameters:
  ///
  ///   - channel
  ///   - args
  [<Emit "$0.on('ipc-message-sync',$1)">] abstract onIpcMessageSync: listener: (Event -> string -> obj [] -> unit) -> WebContents
  /// See onIpcMessageSync.
  [<Emit "$0.once('ipc-message-sync',$1)">] abstract onceIpcMessageSync: listener: (Event -> string -> obj [] -> unit) -> WebContents
  /// See onIpcMessageSync.
  [<Emit "$0.addListener('ipc-message-sync',$1)">] abstract addListenerIpcMessageSync: listener: (Event -> string -> obj [] -> unit) -> WebContents
  /// See onIpcMessageSync.
  [<Emit "$0.removeListener('ipc-message-sync',$1)">] abstract removeListenerIpcMessageSync: listener: (Event -> string -> obj [] -> unit) -> WebContents
  /// Emitted when desktopCapturer.getSources() is called in the renderer
  /// process. Calling event.preventDefault() will make it return empty sources.
  [<Emit "$0.on('desktop-capturer-get-sources',$1)">] abstract onDesktopCapturerGetSources: listener: (Event -> unit) -> WebContents
  /// See onDesktopCapturerGetSources.
  [<Emit "$0.once('desktop-capturer-get-sources',$1)">] abstract onceDesktopCapturerGetSources: listener: (Event -> unit) -> WebContents
  /// See onDesktopCapturerGetSources.
  [<Emit "$0.addListener('desktop-capturer-get-sources',$1)">] abstract addListenerDesktopCapturerGetSources: listener: (Event -> unit) -> WebContents
  /// See onDesktopCapturerGetSources.
  [<Emit "$0.removeListener('desktop-capturer-get-sources',$1)">] abstract removeListenerDesktopCapturerGetSources: listener: (Event -> unit) -> WebContents
  /// Emitted when remote.require() is called in the renderer process. The
  /// listener is passed the module name. Calling event.preventDefault() will
  /// prevent the module from being returned. Custom value can be returned by
  /// setting event.returnValue.
  [<Emit "$0.on('remote-require',$1)">] abstract onRemoteRequire: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteRequire.
  [<Emit "$0.once('remote-require',$1)">] abstract onceRemoteRequire: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteRequire.
  [<Emit "$0.addListener('remote-require',$1)">] abstract addListenerRemoteRequire: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteRequire.
  [<Emit "$0.removeListener('remote-require',$1)">] abstract removeListenerRemoteRequire: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// Emitted when remote.getGlobal() is called in the renderer process. The
  /// listener is passed the global name. Calling event.preventDefault() will
  /// prevent the global from being returned. Custom value can be returned by
  /// setting event.returnValue.
  [<Emit "$0.on('remote-get-global',$1)">] abstract onRemoteGetGlobal: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteGetGlobal.
  [<Emit "$0.once('remote-get-global',$1)">] abstract onceRemoteGetGlobal: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteGetGlobal.
  [<Emit "$0.addListener('remote-get-global',$1)">] abstract addListenerRemoteGetGlobal: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteGetGlobal.
  [<Emit "$0.removeListener('remote-get-global',$1)">] abstract removeListenerRemoteGetGlobal: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// Emitted when remote.getBuiltin() is called in the renderer process. The
  /// listener is passed the module name. Calling event.preventDefault() will
  /// prevent the module from being returned. Custom value can be returned by
  /// setting event.returnValue.
  [<Emit "$0.on('remote-get-builtin',$1)">] abstract onRemoteGetBuiltin: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteGetBuiltin.
  [<Emit "$0.once('remote-get-builtin',$1)">] abstract onceRemoteGetBuiltin: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteGetBuiltin.
  [<Emit "$0.addListener('remote-get-builtin',$1)">] abstract addListenerRemoteGetBuiltin: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// See onRemoteGetBuiltin.
  [<Emit "$0.removeListener('remote-get-builtin',$1)">] abstract removeListenerRemoteGetBuiltin: listener: (ReturnValueEvent -> string -> unit) -> WebContents
  /// Emitted when remote.getCurrentWindow() is called in the renderer process.
  /// Calling event.preventDefault() will prevent the object from being
  /// returned. Custom value can be returned by setting event.returnValue.
  [<Emit "$0.on('remote-get-current-window',$1)">] abstract onRemoteGetCurrentWindow: listener: (ReturnValueEvent -> unit) -> WebContents
  /// See onRemoteGetCurrentWindow.
  [<Emit "$0.once('remote-get-current-window',$1)">] abstract onceRemoteGetCurrentWindow: listener: (ReturnValueEvent -> unit) -> WebContents
  /// See onRemoteGetCurrentWindow.
  [<Emit "$0.addListener('remote-get-current-window',$1)">] abstract addListenerRemoteGetCurrentWindow: listener: (ReturnValueEvent -> unit) -> WebContents
  /// See onRemoteGetCurrentWindow.
  [<Emit "$0.removeListener('remote-get-current-window',$1)">] abstract removeListenerRemoteGetCurrentWindow: listener: (ReturnValueEvent -> unit) -> WebContents
  /// Emitted when remote.getCurrentWebContents() is called in the renderer
  /// process. Calling event.preventDefault() will prevent the object from being
  /// returned. Custom value can be returned by setting event.returnValue.
  [<Emit "$0.on('remote-get-current-web-contents',$1)">] abstract onRemoteGetCurrentWebContents: listener: (ReturnValueEvent -> unit) -> WebContents
  /// See onRemoteGetCurrentWebContents.
  [<Emit "$0.once('remote-get-current-web-contents',$1)">] abstract onceRemoteGetCurrentWebContents: listener: (ReturnValueEvent -> unit) -> WebContents
  /// See onRemoteGetCurrentWebContents.
  [<Emit "$0.addListener('remote-get-current-web-contents',$1)">] abstract addListenerRemoteGetCurrentWebContents: listener: (ReturnValueEvent -> unit) -> WebContents
  /// See onRemoteGetCurrentWebContents.
  [<Emit "$0.removeListener('remote-get-current-web-contents',$1)">] abstract removeListenerRemoteGetCurrentWebContents: listener: (ReturnValueEvent -> unit) -> WebContents
  /// Loads the url in the window. The url must contain the protocol prefix,
  /// e.g. the http:// or file://. If the load should bypass http cache then use
  /// the pragma header to achieve it.
  ///
  /// The promise will resolve when the page has finished loading (see
  /// did-finish-load), and rejects if the page fails to load (see
  /// did-fail-load).
  abstract loadURL: url: string * ?options: LoadURLOptions -> Promise<unit>
  /// Loads the given file in the window, filePath should be a path to an HTML
  /// file relative to the root of your application.
  abstract loadFile: filePath: string * ?options: LoadFileOptions -> Promise<unit>
  /// Initiates a download of the resource at url without navigating. The
  /// will-download event of `session` will be triggered.
  abstract downloadURL: url: string -> unit
  /// Returns the URL of the current web page.
  abstract getURL: unit -> string
  /// Returns the title of the current web page.
  abstract getTitle: unit -> string
  /// Indicates whether the web page is destroyed.
  abstract isDestroyed: unit -> bool
  /// Focuses the web page.
  abstract focus: unit -> unit
  /// Indicates whether the web page is focused.
  abstract isFocused: unit -> bool
  /// Indicates whether web page is still loading resources.
  abstract isLoading: unit -> bool
  /// Indicates whether the main frame (and not just iframes or frames within
  /// it) is still loading.
  abstract isLoadingMainFrame: unit -> bool
  /// Indicates whether the web page is waiting for a first-response from the
  /// main resource of the page.
  abstract isWaitingForResponse: unit -> bool
  /// Stops any pending navigation.
  abstract stop: unit -> unit
  /// Reloads the current web page.
  abstract reload: unit -> unit
  /// Reloads current page and ignores cache.
  abstract reloadIgnoringCache: unit -> unit
  /// Indicates whether the browser can go back to previous web page.
  abstract canGoBack: unit -> bool
  /// Indicates hether the browser can go forward to next web page.
  abstract canGoForward: unit -> bool
  /// Indicates whether the web page can go to `offset`.
  abstract canGoToOffset: offset: int -> bool
  /// Clears the navigation history.
  abstract clearHistory: unit -> unit
  /// Makes the browser go back a web page.
  abstract goBack: unit -> unit
  /// Makes the browser go forward a web page.
  abstract goForward: unit -> unit
  /// Navigates browser to the specified absolute web page index.
  abstract goToIndex: index: int -> unit
  /// Navigates to the specified offset from the "current entry".
  abstract goToOffset: offset: int -> unit
  /// Indicates whether the renderer process has crashed.
  abstract isCrashed: unit -> bool
  /// Overrides the user agent for this web page.
  abstract setUserAgent: userAgent: string -> unit
  /// Returns the user agent for this web page.
  abstract getUserAgent: unit -> string
  /// Injects CSS into the current web page.
  abstract insertCSS: css: string -> unit
  /// Evaluates `code` in page.
  ///
  /// The returned promise resolves with the result of the executed code or is
  /// rejected if the result of the code is a rejected promise.
  ///
  /// In the browser window some HTML APIs like `requestFullScreen` can only be
  /// invoked by a gesture from the user. Setting `userGesture` to `true` will
  /// remove this limitation.
  abstract executeJavaScript: code: string * ?userGesture: bool -> Promise<obj option>
  /// Ignore application menu shortcuts while this web contents is focused.
  abstract setIgnoreMenuShortcuts: ignore: bool -> unit
  /// Mute the audio on the current web page.
  abstract setAudioMuted: muted: bool -> unit
  /// Indicates whether this page has been muted.
  abstract isAudioMuted: unit -> bool
  /// Indicates whether audio is currently playing.
  abstract isCurrentlyAudible: unit -> bool
  /// Changes the zoom factor to the specified factor. Zoom factor is zoom
  /// percent divided by 100, so 300% = 3.0.
  abstract setZoomFactor: factor: float -> unit
  /// Returns the current zoom factor.
  abstract getZoomFactor: unit -> float
  /// Changes the zoom level to the specified level. The original size is 0 and
  /// each increment above or below represents zooming 20% larger or smaller to
  /// default limits of 300% and 50% of original size, respectively. The formula
  /// for this is scale := 1.2 ^ level.
  abstract setZoomLevel: level: float -> unit
  /// Returns the current zoom level.
  abstract getZoomLevel: unit -> float
  /// Sets the maximum and minimum pinch-to-zoom level.
  ///
  /// Note: Visual zoom is disabled by default in Electron. To re-enable it,
  /// call contents.setVisualZoomLevelLimits(1, 3)
  abstract setVisualZoomLevelLimits: minimumLevel: float * maximumLevel: float -> unit
  /// Sets the maximum and minimum layout-based (i.e. non-visual) zoom level.
  abstract setLayoutZoomLevelLimits: minimumLevel: float * maximumLevel: float -> unit
  /// Executes the editing command undo in web page.
  abstract undo: unit -> unit
  /// Executes the editing command redo in web page.
  abstract redo: unit -> unit
  /// Executes the editing command cut in web page.
  abstract cut: unit -> unit
  /// Executes the editing command copy in web page.
  abstract copy: unit -> unit
  /// Copy the image at the given position to the clipboard.
  abstract copyImageAt: x: int * y: int -> unit
  /// Executes the editing command paste in web page.
  abstract paste: unit -> unit
  /// Executes the editing command pasteAndMatchStyle in web page.
  abstract pasteAndMatchStyle: unit -> unit
  /// Executes the editing command delete in web page.
  abstract delete: unit -> unit
  /// Executes the editing command selectAll in web page.
  abstract selectAll: unit -> unit
  /// Executes the editing command unselect in web page.
  abstract unselect: unit -> unit
  /// Executes the editing command replace in web page.
  abstract replace: text: string -> unit
  /// Executes the editing command replaceMisspelling in web page.
  abstract replaceMisspelling: text: string -> unit
  /// Inserts text to the focused element.
  abstract insertText: text: string -> unit
  /// Starts a request to find all matches for the text in the web page. The
  /// result of the request can be obtained by subscribing to `found-in-page`
  /// event. Returns the request id used for the request.
  abstract findInPage: text: string * ?options: FindInPageOptions -> int
  /// Stops any findInPage request for the webContents with the provided action.
  abstract stopFindInPage: action: StopFindInPageAction -> unit
  /// Captures a snapshot of the page within rect. Omitting rect will capture
  /// the whole visible page.
  abstract capturePage: ?rect: Rectangle -> Promise<NativeImage>
  /// Get the system printer list.
  abstract getPrinters: unit -> PrinterInfo []
  /// Prints window's web page. The callback indicates whether the print call
  /// was successful.
  ///
  /// When `silent` is set to true, Electron will pick the system's default
  /// printer if `deviceName` is empty and the default settings for printing.
  ///
  /// Calling window.print() in web page is equivalent to calling
  /// webContents.print({ silent: false, printBackground: false, deviceName:
  /// ''}).
  ///
  /// Use `page-break-before: always;` CSS style to force to print to a new
  /// page.
  abstract print: ?options: PrintOptions * ?callback: (bool -> unit) -> unit
  /// Prints window's web page as PDF with Chromium's preview printing custom
  /// settings.
  ///
  /// Returns a promise that resolves with the generated PDF data.
  ///
  /// The landscape will be ignored if `@page` CSS at-rule is used in the web
  /// page.
  ///
  /// Use `page-break-before: always;` CSS style to force to print to a new
  /// page.
  abstract printToPDF: options: PrintToPDFOptions -> Promise<Buffer>
  /// Adds the specified path to DevTools workspace. Must be used after DevTools
  /// creation.
  abstract addWorkSpace: path: string -> unit
  /// Removes the specified path from DevTools workspace.
  abstract removeWorkSpace: path: string -> unit
  /// Uses the devToolsWebContents as the target WebContents to show devtools.
  ///
  /// The devToolsWebContents must not have done any navigation, and it should
  /// not be used for other purposes after the call.
  ///
  /// By default Electron manages the devtools by creating an internal
  /// WebContents with native view, which developers have very limited control
  /// of. With the setDevToolsWebContents method, developers can use any
  /// WebContents to show the devtools in it, including BrowserWindow and
  /// BrowserView.
  ///
  /// Note that closing the devtools does not destroy the devToolsWebContents,
  /// it is caller's responsibility to destroy devToolsWebContents.
  abstract setDevToolsWebContents: devToolsWebContents: WebContents -> unit
  /// Opens the devtools.
  abstract openDevTools: ?options: OpenDevToolsOptions -> unit
  /// Closes the devtools.
  abstract closeDevTools: unit -> unit
  /// Indicates whether the devtools is opened.
  abstract isDevToolsOpened: unit -> bool
  /// Indicates whether the devtools view is focused.
  abstract isDevToolsFocused: unit -> bool
  /// Toggles the developer tools.
  abstract toggleDevTools: unit -> unit
  /// Starts inspecting element at position (x, y).
  abstract inspectElement: x: int * y: int -> unit
  /// Opens the developer tools for the shared worker context.
  abstract inspectSharedWorker: unit -> unit
  /// Opens the developer tools for the service worker context.
  abstract inspectServiceWorker: unit -> unit
  /// Send an asynchronous message to renderer process via `channel`, you can
  /// also send arbitrary arguments. Arguments will be serialized in JSON
  /// internally and hence no functions or prototype chain will be included.
  ///
  /// The renderer process can handle the message by listening to channel with
  /// the ipcRenderer module.
  abstract send: channel: string * [<ParamArray>] args: obj [] -> unit
  /// Send an asynchronous message to a specific frame in a renderer process via
  /// `channel`. Arguments will be serialized as JSON internally and as such no
  /// functions or prototype chains will be included.
  ///
  /// The renderer process can handle the message by listening to channel with
  /// the ipcRenderer module.
  ///
  /// If you want to get the frameId of a given renderer context you should use
  /// the webFrame.routingId value.
  abstract sendToFrame: frameId: int * channel: string * [<ParamArray>] args: obj [] -> unit
  /// Enable device emulation with the given parameters.
  abstract enableDeviceEmulation: parameters: DeviceEmulationParameters -> unit
  /// Disable device emulation enabled by webContents.enableDeviceEmulation.
  abstract disableDeviceEmulation: unit -> unit
  /// Sends an input event to the page. `event` may be a instance of
  /// SendKeyboardEvent, SendMouseEvent, or SendMouseWheelEvent.
  ///
  /// Note: The BrowserWindow containing the
  /// contents needs to be focused for sendInputEvent() to work.
  abstract sendInputEvent: event: SendInputEvent -> unit
  /// Begin subscribing for presentation events and captured frames, the
  /// callback will be called with callback(image, dirtyRect) when there is a
  /// presentation event.
  ///
  /// The `image` is an instance of NativeImage that stores the captured frame.
  ///
  /// The `dirtyRect` describes which part of the page was repainted.
  abstract beginFrameSubscription: callback: (NativeImage -> Rectangle -> unit) -> unit
  /// Begin subscribing for presentation events and captured frames, the
  /// callback will be called with callback(image, dirtyRect) when there is a
  /// presentation event.
  ///
  /// The `image` is an instance of NativeImage that stores the captured frame.
  ///
  /// The `dirtyRect` describes which part of the page was repainted. If
  /// onlyDirty is set to true, image will only contain the repainted area.
  abstract beginFrameSubscription: onlyDirty: bool * callback: (NativeImage -> Rectangle -> unit) -> unit
  /// End subscribing for frame presentation events.
  abstract endFrameSubscription: unit -> unit
  /// Sets the item as dragging item for current drag-drop operation.
  abstract startDrag: item: DraggedItem -> unit
  /// Returns a promise that resolves if the page is saved.
  abstract savePage: fullPath: string * saveType: WebContentSaveType -> Promise<unit>
  /// [macOS] Shows pop-up dictionary that searches the selected word on the
  /// page.
  abstract showDefinitionForSelection: unit -> unit
  /// Indicates whether offscreen rendering is enabled.
  abstract isOffscreen: unit -> bool
  /// If offscreen rendering is enabled and not painting, start painting.
  abstract startPainting: unit -> unit
  /// If offscreen rendering is enabled and painting, stop painting.
  abstract stopPainting: unit -> unit
  /// If offscreen rendering is enabled returns whether it is currently
  /// painting.
  abstract isPainting: unit -> bool
  /// If offscreen rendering is enabled sets the frame rate to the specified
  /// number. Only values between 1 and 60 are accepted.
  abstract setFrameRate: fps: int -> unit
  /// If offscreen rendering is enabled returns the current frame rate.
  abstract getFrameRate: unit -> int
  /// Schedules a full repaint of the window this web contents is in.
  ///
  /// If offscreen rendering is enabled invalidates the frame and generates a
  /// new one through the 'paint' event.
  abstract invalidate: unit -> unit
  /// Returns the WebRTC IP Handling Policy.
  abstract getWebRTCIPHandlingPolicy: unit -> string
  /// Setting the WebRTC IP handling policy allows you to control which IPs are
  /// exposed via WebRTC. See BrowserLeaks for more details:
  /// https://browserleaks.com/webrtc
  abstract setWebRTCIPHandlingPolicy: policy: WebRtcIpHandlingPolicy -> unit
  /// Returns the operating system pid of the associated renderer process.
  abstract getOSProcessId: unit -> int
  /// Returns the Chromium internal pid of the associated renderer. Can be
  /// compared to the frameProcessId passed by frame specific navigation events
  /// (e.g. did-frame-navigate)
  abstract getProcessId: unit -> int
  /// Takes a V8 heap snapshot and saves it to filePath.
  abstract takeHeapSnapshot: filePath: string -> Promise<unit>
  /// Controls whether or not this WebContents will throttle animations and
  /// timers when the page becomes backgrounded. This also affects the Page
  /// Visibility API.
  abstract setBackgroundThrottling: allowed: bool -> unit
  /// The type of the webContent
  abstract getType: unit -> WebContentType
  /// The unique ID of this WebContents.
  abstract id: int with get, set
  /// A Session used by this webContents.
  abstract session: Session with get, set
  /// A WebContents instance that might own this WebContents.
  abstract hostWebContents: WebContents option with get, set
  /// A WebContents of DevTools for this WebContents.
  ///
  /// Note: Users should never store this object because it may become null when
  /// the DevTools has been closed.
  abstract devToolsWebContents: WebContents option with get, set
  /// A Debugger instance for this webContents.
  abstract debugger: Debugger with get, set

type WebContentsStatic =
  /// Returns all WebContents instances. This will contain web contents for all
  /// windows, webviews, opened devtools, and devtools extension background
  /// pages.
  abstract getAllWebContents: unit -> WebContents []
  /// Returns the web contents that is focused in this application, otherwise
  /// returns None.
  abstract getFocusedWebContents: unit -> WebContents option
  /// Returns a WebContents instance with the given ID.
  abstract fromId: id: int -> WebContents option

type WebFrame =
  inherit EventEmitter<WebFrame>
  /// Changes the zoom factor to the specified factor. Zoom factor is zoom
  /// percent divided by 100, so 300% = 3.0.
  abstract setZoomFactor: factor: float -> unit
  /// Returns the current zoom factor.
  abstract getZoomFactor: unit -> float
  /// Changes the zoom level to the specified level. The original size is 0 and
  /// each increment above or below represents zooming 20% larger or smaller to
  /// default limits of 300% and 50% of original size, respectively.
  abstract setZoomLevel: level: float -> unit
  /// Returns the current zoom level.
  abstract getZoomLevel: unit -> float
  /// Sets the maximum and minimum pinch-to-zoom level.
  ///
  /// Note: Visual zoom is disabled by default in Electron. To re-enable it,
  /// call: webFrame.setVisualZoomLevelLimits(1, 3)
  abstract setVisualZoomLevelLimits: minimumLevel: float * maximumLevel: float -> unit
  /// Sets the maximum and minimum layout-based (i.e. non-visual) zoom level.
  abstract setLayoutZoomLevelLimits: minimumLevel: float * maximumLevel: float -> unit
  /// Sets a provider for spell checking in input fields and text areas. The
  /// provider must be an object that has a spellCheck method that accepts an
  /// array of individual words for spellchecking. The spellCheck function runs
  /// asynchronously and calls the callback function with an array of misspelt
  /// words when complete. An example of using node-spellchecker as provider:
  abstract setSpellCheckProvider: language: string * provider: SpellCheckProvider -> unit
  /// Inserts the specified CSS source code as a style sheet in the document.99
  abstract insertCSS: css: string -> unit
  /// Inserts text to the focused element.
  abstract insertText: text: string -> unit
  /// Evaluates `code` in page.
  ///
  /// Returns a promise that resolves with the result of the executed code or is
  /// rejected if the result of the code is a rejected promise.
  ///
  /// In the browser window some HTML APIs like requestFullScreen can only be
  /// invoked by a gesture from the user. Setting userGesture to true will
  /// remove this limitation.
  abstract executeJavaScript: code: string * ?userGesture: bool -> Promise<obj option>
  /// <summary>
  ///   Work like executeJavaScript but evaluates scripts in an isolated
  ///   context.
  ///
  ///   Returns a promise that resolves with the result of the executed code or
  ///   is rejected if the result of the code is a rejected promise.
  ///
  ///   In the browser window some HTML APIs like requestFullScreen can only be
  ///   invoked by a gesture from the user. Setting userGesture to true will
  ///   remove this limitation.
  /// </summary>
  /// <param name="worldId">
  ///   The ID of the world to run the javascript in, 0 is the default world,
  ///   999 is the world used by Electrons contextIsolation feature. Chrome
  ///   extensions reserve the range of IDs in `[1 << 20, 1 << 29)`. You can
  ///   provide any integer here.
  /// </param>
  /// <param name="scripts"></param>
  /// <param name="userGesture">Default is false</param>
  abstract executeJavaScriptInIsolatedWorld: worldId: int * scripts: WebSource [] * ?userGesture: bool -> Promise<obj option>
  /// <summary>
  ///   Set the content security policy of the isolated world.
  /// </summary>
  /// <param name="worldId">
  ///   The ID of the world to run the javascript in, 0 is the default world,
  ///   999 is the world used by Electrons contextIsolation feature. Chrome
  ///   extensions reserve the range of IDs in `[1 << 20, 1 << 29)`. You can
  ///   provide any integer here.
  /// </param>
  /// <param name="csp"></param>
  abstract setIsolatedWorldContentSecurityPolicy: worldId: int * csp: string -> unit
  /// <summary>
  ///   Set the name of the isolated world. Useful in devtools.
  /// </summary>
  /// <param name="worldId">
  ///   The ID of the world to run the javascript in, 0 is the default world,
  ///   999 is the world used by Electrons contextIsolation feature. Chrome
  ///   extensions reserve the range of IDs in `[1 << 20, 1 << 29)`. You can
  ///   provide any integer here.
  /// </param>
  /// <param name="name"></param>
  abstract setIsolatedWorldHumanReadableName: worldId: int * name: string -> unit
  /// <summary>
  ///   Set the security origin of the isolated world.
  /// </summary>
  /// <param name="worldId">
  ///   The ID of the world to run the javascript in, 0 is the default world,
  ///   999 is the world used by Electrons contextIsolation feature. Chrome
  ///   extensions reserve the range of IDs in `[1 << 20, 1 << 29)`. You can
  ///   provide any integer here.
  /// </param>
  /// <param name="securityOrigin"></param>
  abstract setIsolatedWorldSecurityOrigin: worldId: int * securityOrigin: string -> unit
  /// <summary>
  ///   Set the security origin, content security policy and name of the
  ///   isolated world.
  /// </summary>
  /// <param name="worldId">
  ///   The ID of the world to run the javascript in, 0 is the default world,
  ///   999 is the world used by Electrons contextIsolation feature. Chrome
  ///   extensions reserve the range of IDs in `[1 << 20, 1 << 29)`. You can
  ///   provide any integer here.
  /// </param>
  /// <param name="info"></param>
  abstract setIsolatedWorldInfo: worldId: int * info: IsolatedWorldInfo -> unit
  /// Returns an object describing usage information of Blink's internal memory
  /// caches.
  abstract getResourceUsage: unit -> ResourceUsage
  /// Attempts to free memory that is no longer being used (like images from a
  /// previous navigation).
  ///
  /// Note that blindly calling this method probably makes Electron slower since
  /// it will have to refill these emptied caches, you should only call it if an
  /// event in your app has occurred that makes you think your page is actually
  /// using less memory (i.e. you have navigated from a super heavy page to a
  /// mostly empty one, and intend to stay there).
  abstract clearCache: unit -> unit
  /// <summary>
  ///   Returns the frame element in webFrame's document selected by selector,
  ///   None would be returned if selector does not select a frame or if the
  ///   frame is not in the current renderer process.
  /// </summary>
  /// <param name="selector">CSS selector for a frame element</param>
  abstract getFrameForSelector: selector: string -> WebFrame option
  /// Returns a child of webFrame with the supplied name, None would be returned
  /// if there's no such frame or if the frame is not in the current renderer
  /// process.
  abstract findFrameByName: name: string -> WebFrame option
  /// <summary>
  ///   Returns the WebFrame that has the supplied routingId, None if not found.
  /// </summary>
  /// <param name="selector">
  ///   The unique frame id in the current renderer process. Routing IDs can be
  ///   retrieved from WebFrame instances (webFrame.routingId) and are also
  ///   passed by frame specific WebContents navigation events (e.g.
  ///   did-frame-navigate)
  /// </param>
  abstract findFrameByRoutingId: routingId: int -> WebFrame option
  /// A WebFrame representing top frame in frame hierarchy to which webFrame
  /// belongs, the property would be None if top frame is not in the current
  /// renderer process.
  abstract top: WebFrame option with get, set
  /// A WebFrame representing the frame which opened webFrame, the property
  /// would be None if there's no opener or opener is not in the current
  /// renderer process.
  abstract opener: WebFrame option with get, set
  /// A WebFrame representing parent frame of webFrame, the property would be
  /// None if webFrame is top or parent is not in the current renderer process.
  abstract parent: WebFrame option with get, set
  /// A WebFrame representing the first child frame of webFrame, the property
  /// would be None if webFrame has no children or if first child is not in the
  /// current renderer process.
  abstract firstChild: WebFrame option with get, set
  /// A WebFrame representing next sibling frame, the property would be None if
  /// webFrame is the last frame in its parent or if the next sibling is not in
  /// the current renderer process.
  abstract nextSibling: WebFrame option with get, set
  /// The unique frame id in the current renderer process. Distinct WebFrame
  /// instances that refer to the same underlying frame will have the same
  /// routingId.
  abstract routingId: int with get, set

type WebRequest =
  inherit EventEmitter<WebRequest>
  /// The listener will be called with listener(details, callback) when a
  /// request is about to occur.
  ///
  /// The callback has to be called with an OnBeforeRequestResponse object.
  abstract onBeforeRequest: listener: (OnBeforeRequestDetails -> (OnBeforeRequestResponse -> unit) -> unit) option -> unit
  /// The listener will be called with listener(details, callback) when a
  /// request is about to occur.
  ///
  /// The callback has to be called with an OnBeforeRequestResponse object.
  abstract onBeforeRequest: filter: OnBeforeRequestFilter * listener: (OnBeforeRequestDetails -> (OnBeforeRequestResponse -> unit) -> unit) option -> unit
  /// The listener will be called with listener(details, callback) before
  /// sending an HTTP request, once the request headers are available. This may
  /// occur after a TCP connection is made to the server, but before any http
  /// data is sent.
  ///
  /// The callback has to be called with an OnBeforeSendHeadersResponse object.
  abstract onBeforeSendHeaders: listener: (OnBeforeSendHeadersDetails -> (OnBeforeSendHeadersResponse -> unit) -> unit) option -> unit
  /// The listener will be called with listener(details, callback) before
  /// sending an HTTP request, once the request headers are available. This may
  /// occur after a TCP connection is made to the server, but before any http
  /// data is sent.
  ///
  /// The callback has to be called with an OnBeforeSendHeadersResponse object.
  abstract onBeforeSendHeaders: filter: OnBeforeSendHeadersFilter * listener: (OnBeforeSendHeadersDetails -> (OnBeforeSendHeadersResponse -> unit) -> unit) option -> unit
  /// The listener will be called with listener(details) just before a request
  /// is going to be sent to the server, modifications of previous
  /// onBeforeSendHeaders response are visible by the time this listener is
  /// fired.
  abstract onSendHeaders: listener: (OnSendHeadersDetails -> unit) option -> unit
  /// The listener will be called with listener(details) just before a request
  /// is going to be sent to the server, modifications of previous
  /// onBeforeSendHeaders response are visible by the time this listener is
  /// fired.
  abstract onSendHeaders: filter: OnSendHeadersFilter * listener: (OnSendHeadersDetails -> unit) option -> unit
  /// The listener will be called with listener(details, callback) when HTTP
  /// response headers of a request have been received.
  ///
  /// The callback has to be called with an OnHeadersReceivedResponse object.
  abstract onHeadersReceived: listener: (OnHeadersReceivedDetails -> (OnHeadersReceivedResponse -> unit) -> unit) option -> unit
  /// The listener will be called with listener(details, callback) when HTTP
  /// response headers of a request have been received.
  ///
  /// The callback has to be called with an OnHeadersReceivedResponse object.
  abstract onHeadersReceived: filter: OnHeadersReceivedFilter * listener: (OnHeadersReceivedDetails -> (OnHeadersReceivedResponse -> unit) -> unit) option -> unit
  /// The listener will be called with listener(details) when first byte of the
  /// response body is received. For HTTP requests, this means that the status
  /// line and response headers are available.
  abstract onResponseStarted: listener: (OnResponseStartedDetails -> unit) option -> unit
  /// The listener will be called with listener(details) when first byte of the
  /// response body is received. For HTTP requests, this means that the status
  /// line and response headers are available.
  abstract onResponseStarted: filter: OnResponseStartedFilter * listener: (OnResponseStartedDetails -> unit) option -> unit
  /// The listener will be called with listener(details) when a server initiated
  /// redirect is about to occur.
  abstract onBeforeRedirect: listener: (OnBeforeRedirectDetails -> unit) option -> unit
  /// The listener will be called with listener(details) when a server initiated
  /// redirect is about to occur.
  abstract onBeforeRedirect: filter: OnBeforeRedirectFilter * listener: (OnBeforeRedirectDetails -> unit) option -> unit
  /// The listener will be called with listener(details) when a request is
  /// completed.
  abstract onCompleted: listener: (OnCompletedDetails -> unit) option -> unit
  /// The listener will be called with listener(details) when a request is
  /// completed.
  abstract onCompleted: filter: OnCompletedFilter * listener: (OnCompletedDetails -> unit) option -> unit
  /// The listener will be called with listener(details) when an error occurs.
  abstract onErrorOccurred: listener: (OnErrorOccurredDetails -> unit) option -> unit
  /// The listener will be called with listener(details) when an error occurs.
  abstract onErrorOccurred: filter: OnErrorOccurredFilter * listener: (OnErrorOccurredDetails -> unit) option -> unit

type WebSource =
  abstract code: string with get, set
  abstract url: string with get, set
  /// Default is 1.
  abstract startLine: int with get, set

type AboutPanelOptions =
  /// The app's name.
  abstract applicationName: string with get, set
  /// The app's version.
  abstract applicationVersion: string with get, set
  /// Copyright information.
  abstract copyright: string with get, set
  /// [macOS] The app's build version number.
  abstract version: string with get, set
  /// [macOS] Credit information.
  abstract credits: string with get, set
  /// [Linux] The app's website.
  abstract website: string with get, set
  /// [Linux] Path to the app's icon.
  abstract iconPath: string with get, set

type AddRepresentationOptions =
  /// The scale factor to add the image representation for.
  abstract scaleFactor: float with get, set
  /// Defaults to 0. Required if a bitmap buffer is specified as `buffer`.
  abstract width: int with get, set
  /// Defaults to 0. Required if a bitmap buffer is specified as `buffer`.
  abstract height: int with get, set
  /// The buffer containing the raw image data.
  abstract buffer: Buffer with get, set
  /// The data URL containing either a base 64 encoded PNG or JPEG image.
  abstract dataURL: string with get, set

type AppDetailsOptions =
  /// Window's App User Model ID. It has to be set, otherwise the other options
  /// will have no effect.
  /// https://docs.microsoft.com/en-us/windows/desktop/properties/props-system-appusermodel-id
  abstract appId: string with get, set
  /// Window's Relaunch Icon.
  /// https://docs.microsoft.com/en-us/windows/desktop/properties/props-system-appusermodel-relaunchiconresource
  abstract appIconPath: string with get, set
  /// Index of the icon in appIconPath. Ignored when appIconPath is not set.
  /// Default is 0.
  abstract appIconIndex: int with get, set
  /// Window's Relaunch Command.
  /// https://docs.microsoft.com/en-us/windows/desktop/properties/props-system-appusermodel-relaunchcommand
  abstract relaunchCommand: string with get, set
  /// Window's Relaunch Display Name.
  /// https://docs.microsoft.com/en-us/windows/desktop/properties/props-system-appusermodel-relaunchdisplaynameresource
  abstract relaunchDisplayName: string with get, set

type AuthInfo =
  abstract isProxy: bool with get, set
  abstract scheme: string with get, set
  abstract host: string with get, set
  abstract port: int with get, set
  abstract realm: string with get, set

type AutoResizeOptions =
  /// If true, the view's width will grow and shrink together with the window.
  /// false by default.
  abstract width: bool with get, set
  /// If true, the view's height will grow and shrink together with the window.
  /// false by default.
  abstract height: bool with get, set
  /// If `true`, the view's x position and width will grow and shrink
  /// proportionly with the window. `false` by default.
  abstract vertical: bool with get, set
  /// If `true`, the view's y position and height will grow and shrink
  /// proportinaly with the window. `false` by default.
  abstract horizontal: bool with get, set


type GetBitmapOptions =
  /// Defaults to 1.0.
  abstract scaleFactor: float with get, set

type BrowserViewOptions =
  /// Settings of web page's features.
  abstract webPreferences: WebPreferences with get, set

[<StringEnum; RequireQualifiedAccess>]
type TitleBarStyle =
  /// Results in the standard gray opaque Mac title bar.
  | Default
  /// Results in a hidden title bar and a full size content window, yet the
  /// title bar still has the standard window controls ("traffic lights") in the
  /// top left.
  | Hidden
  /// Results in a hidden title bar with an alternative look where the traffic
  /// light buttons are slightly more inset from the window edge.
  | HiddenInset
  /// Draw custom close, and minimize buttons on macOS frameless windows. These
  /// buttons will not display unless hovered over in the top left of the
  /// window. These custom buttons prevent issues with mouse events that occur
  /// with the standard window toolbar buttons. Note: This option is currently
  /// experimental.
  | CustomButtonsOnHover

[<StringEnum; RequireQualifiedAccess>]
type BrowserWindowStyle =
  /// [Linux, macOS] On macOS, places the window at the desktop background
  /// window level (kCGDesktopWindowLevel - 1). Note that desktop window will
  /// not receive focus, keyboard or mouse events, but you can use
  /// globalShortcut to receive input sparingly.
  | Desktop
  /// [Linux]
  | Dock
  /// [Linux; Windows]
  | Toolbar
  /// [Linux]
  | Splash
  /// [Linux]
  | Notification
  /// [macOS] adds metal gradient appearance (NSTexturedBackgroundWindowMask)
  | Textured

type BrowserWindowOptions =
  /// Window's width in pixels. Default is 800.
  abstract width: int with get, set
  /// Window's height in pixels. Default is 600.
  abstract height: int with get, set
  /// Window's left offset from screen. Required if `y` is used. Default is to
  /// center the window.
  abstract x: int with get, set
  /// Window's top offset from screen. Required if `x` is used. Default is to
  /// center the window.
  abstract y: int with get, set
  /// The width and height would be used as web page's size, which means the
  /// actual window's size will include window frame's size and be slightly
  /// larger. Default is false.
  abstract useContentSize: bool with get, set
  /// Show window in the center of the screen.
  abstract center: bool with get, set
  /// Window's minimum width. Default is 0.
  abstract minWidth: int with get, set
  /// Window's minimum height. Default is 0.
  abstract minHeight: int with get, set
  /// Window's maximum width. Default is no limit.
  abstract maxWidth: int with get, set
  /// Window's maximum height. Default is no limit.
  abstract maxHeight: int with get, set
  /// Whether window is resizable. Default is true.
  abstract resizable: bool with get, set
  /// Whether window is movable. This is not implemented on Linux. Default is
  /// true.
  abstract movable: bool with get, set
  /// Whether window is minimizable. This is not implemented on Linux. Default
  /// is true.
  abstract minimizable: bool with get, set
  /// Whether window is maximizable. This is not implemented on Linux. Default
  /// is true.
  abstract maximizable: bool with get, set
  /// Whether window is closable. This is not implemented on Linux. Default is
  /// true.
  abstract closable: bool with get, set
  /// Whether the window can be focused. Default is true. On Windows setting
  /// focusable: false also implies setting skipTaskbar: true. On Linux setting
  /// focusable: false makes the window stop interacting with wm, so the window
  /// will always stay on top in all workspaces.
  abstract focusable: bool with get, set
  /// Whether the window should always stay on top of other windows. Default is
  /// false.
  abstract alwaysOnTop: bool with get, set
  /// Whether the window should show in fullscreen. When explicitly set to false
  /// the fullscreen button will be hidden or disabled on macOS. Default is
  /// false.
  abstract fullscreen: bool with get, set
  /// Whether the window can be put into fullscreen mode. On macOS, also whether
  /// the maximize/zoom button should toggle full screen mode or maximize
  /// window. Default is true.
  abstract fullscreenable: bool with get, set
  /// Use pre-Lion fullscreen on macOS. Default is false.
  abstract simpleFullscreen: bool with get, set
  /// Whether to show the window in taskbar. Default is false.
  abstract skipTaskbar: bool with get, set
  /// The kiosk mode. Default is false.
  abstract kiosk: bool with get, set
  /// Default window title. Default is "Electron". If the HTML tag `<title>` is
  /// defined in the HTML file loaded by loadURL(), this property will be
  /// ignored.
  abstract title: string with get, set
  /// The window icon. On Windows it is recommended to use ICO icons to get best
  /// visual effects, you can also leave it undefined so the executable's icon
  /// will be used.
  abstract icon: U2<NativeImage, string> with get, set
  /// Whether window should be shown when created. Default is true.
  abstract show: bool with get, set
  /// Specify false to create a frameless window. Default is true.
  abstract frame: bool with get, set
  /// Specify parent window. Default is null.
  abstract parent: BrowserWindow option with get, set
  /// Whether this is a modal window. This only works when the window is a child
  /// window. Default is false.
  abstract modal: bool with get, set
  /// Whether the web view accepts a single mouse-down event that simultaneously
  /// activates the window. Default is false.
  abstract acceptFirstMouse: bool with get, set
  /// Whether to hide cursor when typing. Default is false.
  abstract disableAutoHideCursor: bool with get, set
  /// Auto hide the menu bar unless the Alt key is pressed. Default is false.
  abstract autoHideMenuBar: bool with get, set
  /// Enable the window to be resized larger than screen. Default is false.
  abstract enableLargerThanScreen: bool with get, set
  /// Window's background color as a hexadecimal value, like #66CD00 or #FFF or
  /// #80FFFFFF (alpha in #AARRGGBB format is supported if transparent is set to
  /// true). Default is #FFF (white).
  abstract backgroundColor: string with get, set
  /// Whether window should have a shadow. This is only implemented on macOS.
  /// Default is true.
  abstract hasShadow: bool with get, set
  /// Set the initial opacity of the window, between 0.0 (fully transparent) and
  /// 1.0 (fully opaque). This is only implemented on Windows and macOS.
  abstract opacity: float with get, set
  /// Forces using dark theme for the window, only works on some GTK+3 desktop
  /// environments. Default is false.
  abstract darkTheme: bool with get, set
  /// Makes the window transparent. Default is false.
  abstract transparent: bool with get, set
  /// The type of window.
  abstract ``type``: BrowserWindowStyle with get, set
  /// The style of window title bar. Default is TitleBarStyle.Default.
  abstract titleBarStyle: TitleBarStyle with get, set
  /// Shows the title in the title bar in full screen mode on macOS for all
  /// titleBarStyle options. Default is false.
  abstract fullscreenWindowTitle: bool with get, set
  /// Use `WS_THICKFRAME` style for frameless windows on Windows, which adds
  /// standard window frame. Setting it to false will remove window shadow and
  /// window animations. Default is true.
  abstract thickFrame: bool with get, set
  /// Add a type of vibrancy effect to the window, only on macOS. Please note
  /// that using frame: false in combination with a vibrancy value requires that
  /// you use a non-default titleBarStyle as well.
  abstract vibrancy: VibrancyType with get, set
  /// Controls the behavior on macOS when option-clicking the green stoplight
  /// button on the toolbar or by clicking the Window > Zoom menu item. If true,
  /// the window will grow to the preferred width of the web page when zoomed,
  /// false will cause it to zoom to the width of the screen. This will also
  /// affect the behavior when calling maximize() directly. Default is false.
  abstract zoomToPageWidth: bool with get, set
  /// Tab group name, allows opening the window as a native tab on macOS 10.12+.
  /// Windows with the same tabbing identifier will be grouped together. This
  /// also adds a native new tab button to your window's tab bar and allows your
  /// app and window to receive the new-window-for-tab event.
  abstract tabbingIdentifier: string with get, set
  /// Settings of web page's features.
  abstract webPreferences: WebPreferences with get, set

type CertificateTrustDialogOptions =
  /// The certificate to trust/import.
  abstract certificate: Certificate with get, set
  /// [macOS] The message to display to the user.
  abstract message: string with get, set

type CertificateVerifyProcRequest =
  abstract hostname: string with get, set
  abstract certificate: Certificate with get, set
  /// Verification result from chromium.
  abstract verificationResult: string with get, set
  /// Error code.
  abstract errorCode: int with get, set

[<StringEnum; RequireQualifiedAccess>]
type StorageType =
  | [<CompiledName("appcache")>] AppCache
  | Cookies
  | [<CompiledName("filesystem")>] FileSystem
  | [<CompiledName("indexdb")>] IndexDb
  | [<CompiledName("localstorage")>] LocalStorage
  | [<CompiledName("shadercache")>] ShaderCache
  | [<CompiledName("websql")>] WebSql
  | [<CompiledName("serviceworkers")>] ServiceWorkers
  | [<CompiledName("cachestorage")>] CacheStorage

[<StringEnum; RequireQualifiedAccess>]
type StorageQuota =
  | Temporary
  | Persistent
  | Syncable

type ClearStorageDataOptions =
  /// Should follow window.location.origin’s representation scheme://host:port.
  abstract origin: string with get, set
  /// The types of storages to clear.
  abstract storages: StorageType [] with get, set
  /// The types of quotas to clear.
  abstract quotas: StorageQuota [] with get, set

type CommandLine =
  /// Append a switch (with optional value) to Chromium's command line.
  ///
  /// Note: This will not affect process.argv, and is mainly used by developers
  /// to control some low-level Chromium behaviors.
  ///
  /// Parameters:
  abstract appendSwitch: switch: string -> value: string -> unit
  /// Append an argument to Chromium's command line. The argument will be quoted
  /// correctly.
  ///
  /// Note: This will not affect process.argv.
  abstract appendArgument: value:string -> unit
  /// Indicates whether the command-line switch is present.
  abstract hasSwitch: switch:string -> bool
  /// Returns the command-line switch value.
  ///
  /// Note: When the switch is not present, it returns empty string.
  abstract getSwitchValue: switch: string -> string

type ProxyConfig =
  /// The URL associated with the PAC file.
  abstract pacScript: string with get, set
  /// Rules indicating which proxies to use.
  abstract proxyRules: string with get, set
  /// Rules indicating which URLs should bypass the proxy settings.
  abstract proxyBypassRules: string with get, set

[<StringEnum; RequireQualifiedAccess>]
type ContextMenuMediaType =
  | None
  | Image
  | Audio
  | Video
  | Canvas
  | File
  | Plugin

[<StringEnum; RequireQualifiedAccess>]
type ContextMenuSourceType =
  | None
  | Mouse
  | Keyboard
  | Touch
  | TouchMenu

[<StringEnum; RequireQualifiedAccess>]
type ContextMenuInputFieldType =
  | None
  | PlainText
  | Password
  | Other

type ContextMenuParams =
  /// x coordinate.
  abstract x: int with get, set
  /// y coordinate.
  abstract y: int with get, set
  /// URL of the link that encloses the node the context menu was invoked on.
  abstract linkURL: string with get, set
  /// Text associated with the link. May be an empty string if the contents of
  /// the link are an image.
  abstract linkText: string with get, set
  /// URL of the top level page that the context menu was invoked on.
  abstract pageURL: string with get, set
  /// URL of the subframe that the context menu was invoked on.
  abstract frameURL: string with get, set
  /// Source URL for the element that the context menu was invoked on. Elements
  /// with source URLs are images, audio and video.
  abstract srcURL: string with get, set
  /// Type of the node the context menu was invoked on. Can be none, image,
  /// audio, video, canvas, file or plugin.
  abstract mediaType: ContextMenuMediaType with get, set
  /// Whether the context menu was invoked on an image which has non-empty
  /// contents.
  abstract hasImageContents: bool with get, set
  /// Whether the context is editable.
  abstract isEditable: bool with get, set
  /// Text of the selection that the context menu was invoked on.
  abstract selectionText: string with get, set
  /// Title or alt text of the selection that the context was invoked on.
  abstract titleText: string with get, set
  /// The misspelled word under the cursor, if any.
  abstract misspelledWord: string with get, set
  /// The character encoding of the frame on which the menu was invoked.
  abstract frameCharset: string with get, set
  /// If the context menu was invoked on an input field, the type of that field.
  abstract inputFieldType: ContextMenuInputFieldType with get, set
  /// Input source that invoked the context menu.
  abstract menuSourceType: ContextMenuSourceType with get, set
  /// The flags for the media element the context menu was invoked on.
  abstract mediaFlags: ContextMenuMediaFlags with get, set
  /// These flags indicate whether the renderer believes it is able to perform
  /// the corresponding action.
  abstract editFlags: ContextMenuEditFlags with get, set

type CrashReporterStartOptions =
  abstract companyName: string with get, set
  /// URL that crash reports will be sent to as POST.
  abstract submitURL: string with get, set
  /// Defaults to app.getName().
  abstract productName: string with get, set
  /// Whether crash reports should be sent to the server Default is true.
  abstract uploadToServer: bool with get, set
  /// Default is false.
  abstract ignoreSystemCrashHandler: bool with get, set
  /// An object you can define that will be sent along with the report. Only
  /// string properties are sent correctly. Nested objects are not supported and
  /// the property names and values must be less than 64 characters long.
  abstract extra: obj with get, set
  /// Directory to store the crashreports temporarily (only used when the crash
  /// reporter is started via process.crashReporter.start).
  abstract crashesDirectory: string with get, set

type NativeImageFromBufferOptions =
  /// Required for bitmap buffers.
  abstract width: int with get, set
  /// Required for bitmap buffers.
  abstract height: int with get, set
  /// Defaults to 1.0.
  abstract scaleFactor: float with get, set

type CreateInterruptedDownloadOptions =
  /// Absolute path of the download.
  abstract path: string with get, set
  /// Complete URL chain for the download.
  abstract urlChain: string [] with get, set
  abstract mimeType: string with get, set
  /// Start range for the download.
  abstract offset: int with get, set
  /// Total length of the download.
  abstract length: int with get, set
  /// Last-Modified header value.
  abstract lastModified: string with get, set
  /// ETag header value.
  abstract eTag: string with get, set
  /// Time when download was started in number of seconds since UNIX epoch.
  abstract startTime: float with get, set

type ClipboardData =
  abstract text: string with get, set
  abstract html: string with get, set
  abstract image: NativeImage with get, set
  abstract rtf: string with get, set
  /// The title of the url at `text`.
  abstract bookmark: string with get, set

type SetCookieDetails =
  /// The url to associate the cookie with. If invalid, the promise returned
  /// when setting the cookie will be rejected.
  abstract url: string with get, set
  /// The name of the cookie. Empty by default if omitted.
  abstract name: string with get, set
  /// The value of the cookie. Empty by default if omitted.
  abstract value: string with get, set
  /// The domain of the cookie; this will be normalized with a preceding dot so
  /// that it's also valid for subdomains. Empty by default if omitted.
  abstract domain: string with get, set
  /// The path of the cookie. Empty by default if omitted.
  abstract path: string with get, set
  /// Whether the cookie should be marked as Secure. Defaults to false.
  abstract secure: bool with get, set
  /// Whether the cookie should be marked as HTTP only. Defaults to false.
  abstract httpOnly: bool with get, set
  /// The expiration date of the cookie as the number of seconds since the UNIX
  /// epoch. If omitted then the cookie becomes a session cookie and will not be
  /// retained between sessions.
  abstract expirationDate: float with get, set

type DisplayBalloonOptions =
  abstract icon: U2<NativeImage, string> with get, set
  abstract title: string with get, set
  abstract content: string with get, set

[<StringEnum; RequireQualifiedAccess>]
type DockBounceType =
  | Critical
  | Informational

type Dock =
  /// Returns an ID representing the request.
  ///
  /// [macOS] When DockBounceType.Critical is passed, the dock icon will bounce
  /// until either the application becomes active or the request is canceled.
  ///
  /// When DockBounceType.Informational is passed, the dock icon will bounce for
  /// one second. However, the request remains active until either the
  /// application becomes active or the request is canceled.
  abstract bounce: DockBounceType -> int
  /// [macOS] Cancel the bounce of `id`.
  abstract cancelBounce: id: int -> unit
  /// [macOS] Bounces the Downloads stack if the `filePath` is inside the
  /// Downloads folder.
  abstract downloadFinished: filePath: string -> unit
  /// [macOS] Sets the string to be displayed in the dock’s badging area.
  abstract setBadge: text: string -> unit
  /// [macOS] Returns the badge string of the dock.
  abstract getBadge: unit -> string
  /// [macOS] Hides the dock icon.
  abstract hide: unit -> unit
  /// [macOS] Shows the dock icon. The promise resolves when the dock icon is
  /// shown.
  abstract show: unit -> Promise<unit>
  /// [macOS] Indicates whether the dock icon is visible. The app.dock.show()
  /// call is asynchronous so this method might not return true immediately
  /// after that call.
  abstract isVisible: unit -> bool
  /// [macOS] Sets the application's dock menu. More information:
  /// https://developer.apple.com/design/human-interface-guidelines/macos/menus/dock-menus/
  abstract setMenu: menu: Menu -> unit
  /// [macOS] Sets the image associated with this dock icon.
  abstract setIcon: image: U2<NativeImage, string> -> unit

type EnableNetworkEmulationOptions =
  /// Whether to emulate network outage. Defaults to false.
  abstract offline: bool with get, set
  /// RTT in ms. Defaults to 0 which will disable latency throttling.
  abstract latency: float with get, set
  /// Download rate in Bps. Defaults to 0 which will disable download
  /// throttling.
  abstract downloadThroughput: float with get, set
  /// Upload rate in Bps. Defaults to 0 which will disable upload throttling.
  abstract uploadThroughput: float with get, set

[<StringEnum; RequireQualifiedAccess>]
type AutoUpdateFeedServerType =
  | Json
  | Default

type AutoOpdateFeedOptions =
  abstract url: string with get, set
  /// [macOS] HTTP request headers.
  abstract headers: obj with get, set
  /// [macOS] See the Squirrel.Mac README for more information:
  /// https://github.com/Squirrel/Squirrel.Mac
  abstract serverType: AutoUpdateFeedServerType with get, set

[<StringEnum; RequireQualifiedAccess>]
type FileIconSize =
  /// 16x16
  | Small
  /// 32x32
  | Normal
  /// 48x48 on Linux, 32x32 on Windows, unsupported on macOS.
  | Large

type FileIconOptions =
  abstract size: FileIconSize with get, set

type GetCookiesFilter =
  /// Retrieves cookies which are associated with url. Empty implies retrieving
  /// cookies of all urls.
  abstract url: string with get, set
  /// Filters cookies by name.
  abstract name: string with get, set
  /// Retrieves cookies whose domains match or are subdomains of `domain`.
  abstract domain: string with get, set
  /// Retrieves cookies whose path matches `path`.
  abstract path: string with get, set
  /// Filters cookies by their Secure property.
  abstract secure: bool with get, set
  /// Filters out session or persistent cookies.
  abstract session: bool with get, set

type FindInPageOptions =
  /// Whether to search forward or backward, defaults to true.
  abstract forward: bool with get, set
  /// Whether the operation is first request or a follow up, defaults to false.
  abstract findNext: bool with get, set
  /// Whether search should be case-sensitive, defaults to false.
  abstract matchCase: bool with get, set
  /// Whether to look only at the start of words. defaults to false.
  abstract wordStart: bool with get, set
  /// When combined with wordStart, accepts a match in the middle of a word if
  /// the match begins with an uppercase letter followed by a lowercase or
  /// non-letter. Accepts several other intra-word matches, defaults to false.
  abstract medialCapitalAsWordStart: bool with get, set

type FromPartitionOptions =
  /// Whether to enable cache.
  abstract cache: bool with get, set

type HeapStatistics =
  abstract totalHeapSize: int with get, set
  abstract totalHeapSizeExecutable: int with get, set
  abstract totalPhysicalSize: int with get, set
  abstract totalAvailableSize: int with get, set
  abstract usedHeapSize: int with get, set
  abstract heapSizeLimit: int with get, set
  abstract mallocedMemory: int with get, set
  abstract peakMallocedMemory: int with get, set
  abstract doesZapGarbage: bool with get, set

type IgnoreMouseEventsOptions =
  /// [macOS, Windows] If true, forwards mouse move messages to Chromium,
  /// enabling mouse related events such as mouseleave. Only used when ignore is
  /// true. If ignore is false, forwarding is always disabled regardless of this
  /// value.
  abstract forward: bool with get, set

type ImportCertificateOptions =
  /// Path for the pkcs12 file.
  abstract certificate: string with get, set
  /// Passphrase for the certificate.
  abstract password: string with get, set

type IsolatedWorldInfo =
  /// Security origin for the isolated world.
  abstract securityOrigin: string with get, set
  /// Content Security Policy for the isolated world. If this is specified, then
  /// `securityOrigin` also has to be specified.
  abstract csp: string with get, set
  /// Name for isolated world. Useful in devtools.
  abstract name: string with get, set

type BeforeInputEventData =
  /// Either keyUp or keyDown.
  abstract ``type``: string with get, set
  /// Equivalent to .
  abstract key: string with get, set
  /// Equivalent to .
  abstract code: string with get, set
  /// Equivalent to .
  abstract isAutoRepeat: bool with get, set
  /// Equivalent to .
  abstract shift: bool with get, set
  /// Equivalent to .
  abstract control: bool with get, set
  /// Equivalent to .
  abstract alt: bool with get, set
  /// Equivalent to .
  abstract meta: bool with get, set

type InterceptBufferProtocolRequest =
  abstract url: string with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type InterceptFileProtocolRequest =
  abstract url: string with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type InterceptHttpProtocolRequest =
  abstract url: string with get, set
  abstract headers: obj with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type InterceptStreamProtocolRequest =
  abstract url: string with get, set
  abstract headers: obj with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type InterceptStringProtocolRequest =
  abstract url: string with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type DraggedItem =
  /// The absolute path to the file being dragged. Mutually exclusive with `files`.
  abstract file: string with get, set
  /// The absolute paths to the files being dragged. Mutually exclusive with `file`.
  abstract files: string [] with get, set
  /// The image showing under the cursor when dragging. Must be non-empty on
  /// macOS.
  abstract icon: NativeImage with get, set

type JumpListSettings =
  /// The minimum number of items that will be shown in the Jump List. For
  /// details, see
  /// https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nf-shobjidl_core-icustomdestinationlist-beginlist
  abstract minItems: int with get, set
  /// Items that the user has explicitly removed from custom categories in the
  /// Jump List. These items must not be re-added to the Jump List in the next
  /// call to app.setJumpList(). Windows will not display any custom category
  /// that contains any of the removed items.
  abstract removedItems: JumpListItem [] with get, set

type LoadFileOptions =
  /// Passed to url.format().
  abstract query: obj with get, set
  /// Passed to url.format().
  abstract search: string with get, set
  /// Passed to url.format().
  abstract hash: string with get, set

type LoadURLOptions =
  /// An HTTP Referrer url.
  abstract httpReferrer: U2<string, Referrer> with get, set
  /// A user agent originating the request.
  abstract userAgent: string with get, set
  /// Extra headers separated by "\n"
  abstract extraHeaders: string with get, set
  abstract postData: U3<UploadRawData [], UploadFile [], UploadBlob []> with get, set
  /// Base url (with trailing path separator) for files to be loaded by the data
  /// url. This is needed only if the specified url is a data url and needs to
  /// load other files.
  abstract baseURLForDataURL: string with get, set

type LoginItemSettings =
  /// True if the app is set to open at login.
  abstract openAtLogin: bool with get, set
  /// [macOS] True if the app is set to open as hidden at login. This setting is
  /// not available on Mac App Store builds.
  abstract openAsHidden: bool with get, set
  /// True if the app was opened at login automatically. This setting is not
  /// available on Mac App Store builds.
  abstract wasOpenedAtLogin: bool with get, set
  /// True if the app was opened as a hidden login item. This indicates that the
  /// app should not open any windows at startup. This setting is not available
  /// on Mac App Store builds.
  abstract wasOpenedAsHidden: bool with get, set
  /// True if the app was opened as a login item that should restore the state
  /// from the previous session. This indicates that the app should restore the
  /// windows that were open the last time the app was closed. This setting is
  /// not available on Mac App Store builds.
  abstract restoreState: bool with get, set

type GetLoginItemSettingsOptions =
  /// [Windows] The executable path to compare against. Defaults to
  /// process.execPath.
  abstract path: string with get, set
  /// [Windows] The command-line arguments to compare against. Defaults to an
  /// empty array.
  abstract args: string [] with get, set

[<StringEnum; RequireQualifiedAccess>]
type MenuItemRole =
  | Undo
  | Redo
  | Cut
  | Copy
  | Paste
  | PasteAndMatchStyle
  | SelectAll
  | Delete
  /// Minimize current window.
  | Minimize
  /// Close current window.
  | Close
  /// Quit the application.
  | Quit
  /// Reload the current window.
  | Reload
  /// Reload the current window ignoring the cache.
  | ForceReload
  /// Toggle developer tools in the current window.
  | ToggleDevTools
  /// Toggle full screen mode on the current window.
  | ToggleFullScreen
  /// Reset the focused page's zoom level to the original size.
  | ResetZoom
  /// Zoom in the focused page by 10%.
  | ZoomIn
  /// Zoom out the focused page by 10%.
  | ZoomOut
  /// Whole default "File" menu (Close / Quit)
  | FileMenu
  /// Whole default "Edit" menu (Undo, Copy, etc.).
  | EditMenu
  /// Whole default "View" menu (Reload, Toggle Developer Tools, etc.)
  | ViewMenu
  /// Whole default "Window" menu (Minimize, Zoom, etc.).
  | WindowMenu
  /// [macOS] Whole default "App" menu (About, Services, etc.)
  | AppMenu
  /// [macOS] Map to the orderFrontStandardAboutPanel action.
  | About
  /// [macOS] Map to the `hide` action.
  | Hide
  /// [macOS] Map to the `hideOtherApplications` action.
  | HideOthers
  /// [macOS] Map to the `unhideAllApplications` action.
  | Unhide
  /// [macOS] Map to the `startSpeaking` action.
  | StartSpeaking
  /// [macOS] Map to the `stopSpeaking` action.
  | StopSpeaking
  /// [macOS] Map to the `arrangeInFront` action.
  | Front
  /// [macOS] Map to the `performZoom` action.
  | Zoom
  /// [macOS] Map to the `toggleTabBar` action.
  | ToggleTabBar
  /// [macOS] Map to the `selectNextTab` action.
  | SelectNextTab
  /// [macOS] Map to the `selectPreviousTab` action.
  | SelectPreviousTab
  /// [macOS] Map to the `mergeAllWindows` action.
  | MergeAllWindows
  /// [macOS] Map to the `moveTabToNewWindow` action.
  | MoveTabToNewWindow
  /// [macOS] The submenu is a "Window" menu.
  | Window
  /// [macOS] The submenu is a "Help" menu.
  | Help
  /// [macOS] The submenu is a "Services" menu. This is only intended for use in
  /// the Application Menu and is not the same as the "Services" submenu used in
  /// context menus in macOS apps, which is not implemented in Electron.
  | Services
  /// [macOS] The submenu is an "Open Recent" menu.
  | RecentDocuments
  /// [macOS] Map to the `clearRecentDocuments` action.
  | ClearRecentDocuments

[<StringEnum; RequireQualifiedAccess>]
type MenuItemType =
  | Normal
  | Separator
  | [<CompiledName("submenu")>] SubMenu
  | Checkbox
  | Radio

type MenuItemOptions =
  /// Will be called when the menu item is clicked.
  abstract click: (MenuItem -> BrowserWindow -> Event -> unit) with get, set
  /// The action of the menu item, when specified the `click` property will be
  /// ignored. More information: https://electronjs.org/docs/api/menu-item#roles
  abstract role: MenuItemRole with get, set
  abstract ``type``: MenuItemType with get, set
  abstract label: string with get, set
  abstract sublabel: string with get, set
  abstract accelerator: string with get, set
  abstract icon: U2<NativeImage, string> with get, set
  /// If false, the menu item will be greyed out and unclickable.
  abstract enabled: bool with get, set
  /// [macOS] Default is `true`, and when `false` will prevent the accelerator
  /// from triggering the item if the item is not visible. This property is only
  /// usable on macOS High Sierra 10.13 or newer. On Windows and Linux this has
  /// no effect, since accelerators always work when items are hidden.
  abstract acceleratorWorksWhenHidden: bool with get, set
  /// If false, the menu item will be entirely hidden.
  abstract visible: bool with get, set
  /// Should only be specified for MenuItemType.Checkbox and MenuItemType.Radio
  /// type menu items.
  abstract ``checked``: bool with get, set
  /// If false, the accelerator won't be registered with the system, but it will
  /// still be displayed. Defaults to true.
  abstract registerAccelerator: bool with get, set
  /// Should be specified for MenuItemType.SubMenu type menu items. If this
  /// property is set, then the `type` property may be omitted.
  abstract submenu: U2<MenuItemOptions [], Menu> with get, set
  /// Unique within a single menu. If defined then it can be used as a reference
  /// to this item by the position attribute.
  abstract id: string with get, set
  /// Inserts this item before the item with the specified label. If the
  /// referenced item doesn't exist the item will be inserted at the end of the
  /// menu. Also implies that the menu item in question should be placed in the
  /// same “group” as the item.
  abstract before: string [] with get, set
  /// Inserts this item after the item with the specified label. If the
  /// referenced item doesn't exist the item will be inserted at the end of the
  /// menu.
  abstract after: string [] with get, set
  /// Provides a means for a single context menu to declare the placement of
  /// their containing group before the containing group of the item with the
  /// specified label.
  abstract beforeGroupContaining: string [] with get, set
  /// Provides a means for a single context menu to declare the placement of
  /// their containing group after the containing group of the item with the
  /// specified label.
  abstract afterGroupContaining: string [] with get, set

[<StringEnum; RequireQualifiedAccess>]
type MessageBoxType =
  | None
  | Info
  /// On macOS, by default uses the same icon as Warning.
  | Error
  /// On Windows, by default uses the same icon as Info.
  | Question
  /// On macOS, by default uses the same icon as Error.
  | Warning

type MessageBoxOptions =
  abstract ``type``: MessageBoxType with get, set
  /// Array of texts for buttons. On Windows, an empty array will result in one
  /// button labeled "OK".
  abstract buttons: string [] with get, set
  /// Index of the button in the buttons array which will be selected by default
  /// when the message box opens.
  abstract defaultId: int with get, set
  /// Title of the message box, some platforms will not show it.
  abstract title: string with get, set
  /// Content of the message box.
  abstract message: string with get, set
  /// Extra information of the message.
  abstract detail: string with get, set
  /// If provided, the message box will include a checkbox with the given label.
  abstract checkboxLabel: string with get, set
  /// Initial checked state of the checkbox. `false` by default.
  abstract checkboxChecked: bool with get, set
  abstract icon: NativeImage with get, set
  /// The index of the button to be used to cancel the dialog, via the Esc key.
  /// By default this is assigned to the first button with "cancel" or "no" as
  /// the label. If no such labeled buttons exist and this option is not set, 0
  /// will be used as the return value or callback response.
  abstract cancelId: int with get, set
  /// On Windows Electron will try to figure out which one of the buttons are
  /// common buttons (like "Cancel" or "Yes"), and show the others as command
  /// links in the dialog. This can make the dialog appear in the style of
  /// modern Windows apps. If you don't like this behavior, you can set noLink
  /// to true.
  abstract noLink: bool with get, set
  /// Normalize the keyboard access keys across platforms. Default is false.
  /// Enabling this assumes & is used in the button labels for the placement of
  /// the keyboard shortcut access key and labels will be converted so they work
  /// correctly on each platform, & characters are removed on macOS, converted
  /// to _ on Linux, and left untouched on Windows. For example, a button label
  /// of Vie&w will be converted to Vie_w on Linux and View on macOS and can be
  /// selected via Alt-W on Windows and Linux.
  abstract normalizeAccessKeys: bool with get, set

type NotificationOptions =
  /// A title for the notification, which will be shown at the top of the
  /// notification window when it is shown.
  abstract title: string with get, set
  /// [macOS] A subtitle for the notification, which will be displayed below the
  /// title.
  abstract subtitle: string with get, set
  /// The body text of the notification, which will be displayed below the title
  /// or subtitle.
  abstract body: string with get, set
  /// Whether or not to emit an OS notification noise when showing the
  /// notification.
  abstract silent: bool with get, set
  /// An icon to use in the notification.
  abstract icon: U2<string, NativeImage> with get, set
  /// [macOS] Whether or not to add an inline reply option to the notification.
  abstract hasReply: bool with get, set
  /// [macOS] The placeholder to write in the inline reply input field.
  abstract replyPlaceholder: string with get, set
  /// [macOS] The name of the sound file to play when the notification is shown.
  abstract sound: string with get, set
  /// [macOS] Actions to add to the notification. Please read the available
  /// actions and limitations in the NotificationAction documentation.
  abstract actions: NotificationAction [] with get, set
  /// [macOS] A custom title for the close button of an alert. An empty string
  /// will cause the default localized text to be used.
  abstract closeButtonText: string with get, set

type OnBeforeRedirectDetails =
  abstract id: int with get, set
  abstract url: string with get, set
  abstract method: string with get, set
  abstract webContentsId: int option with get, set
  abstract resourceType: string with get, set
  abstract referrer: string with get, set
  abstract timestamp: float with get, set
  abstract redirectURL: string with get, set
  abstract statusCode: int with get, set
  /// The server IP address that the request was actually sent to.
  abstract ip: string option with get, set
  abstract fromCache: bool with get, set
  abstract responseHeaders: obj with get, set

type OnBeforeRedirectFilter =
  /// Array of URL patterns that will be used to filter out the requests that do
  /// not match the URL patterns.
  abstract urls: string [] with get, set

type OnBeforeRequestDetails =
  abstract id: int with get, set
  abstract url: string with get, set
  abstract method: string with get, set
  abstract webContentsId: int option with get, set
  abstract resourceType: string with get, set
  abstract referrer: string with get, set
  abstract timestamp: float with get, set
  abstract uploadData: UploadData [] with get, set

type OnBeforeRequestFilter =
  /// Array of URL patterns that will be used to filter out the requests that do
  /// not match the URL patterns.
  abstract urls: string [] with get, set

type OnBeforeSendHeadersDetails =
  abstract id: int with get, set
  abstract url: string with get, set
  abstract method: string with get, set
  abstract webContentsId: int option with get, set
  abstract resourceType: string with get, set
  abstract referrer: string with get, set
  abstract timestamp: float with get, set
  abstract requestHeaders: obj with get, set

type OnBeforeSendHeadersFilter =
  /// Array of URL patterns that will be used to filter out the requests that do
  /// not match the URL patterns.
  abstract urls: string [] with get, set

type OnBeforeSendHeadersResponse =
  abstract cancel: bool with get, set
  /// When provided, request will be made with these headers.
  abstract requestHeaders: obj with get, set

type OnCompletedDetails =
  abstract id: int with get, set
  abstract url: string with get, set
  abstract method: string with get, set
  abstract webContentsId: int option with get, set
  abstract resourceType: string with get, set
  abstract referrer: string with get, set
  abstract timestamp: float with get, set
  abstract responseHeaders: obj with get, set
  abstract fromCache: bool with get, set
  abstract statusCode: int with get, set
  abstract statusLine: string with get, set

type OnCompletedFilter =
  /// Array of URL patterns that will be used to filter out the requests that do
  /// not match the URL patterns.
  abstract urls: string [] with get, set

type OnErrorOccurredDetails =
  abstract id: int with get, set
  abstract url: string with get, set
  abstract method: string with get, set
  abstract webContentsId: int option with get, set
  abstract resourceType: string with get, set
  abstract referrer: string with get, set
  abstract timestamp: float with get, set
  abstract fromCache: bool with get, set
  /// The error description.
  abstract error: string with get, set

type OnErrorOccurredFilter =
  /// Array of URL patterns that will be used to filter out the requests that do
  /// not match the URL patterns.
  abstract urls: string [] with get, set

type OnHeadersReceivedDetails =
  abstract id: int with get, set
  abstract url: string with get, set
  abstract method: string with get, set
  abstract webContentsId: int option with get, set
  abstract resourceType: string with get, set
  abstract referrer: string with get, set
  abstract timestamp: float with get, set
  abstract statusLine: string with get, set
  abstract statusCode: int with get, set
  abstract responseHeaders: obj with get, set

type OnHeadersReceivedFilter =
  /// Array of URL patterns that will be used to filter out the requests that do
  /// not match the URL patterns.
  abstract urls: string [] with get, set

type OnHeadersReceivedResponse =
  abstract cancel: bool with get, set
  /// When provided, the server is assumed to have responded with these headers.
  abstract responseHeaders: obj with get, set
  /// Should be provided when overriding responseHeaders to change header status
  /// otherwise original response header's status will be used.
  abstract statusLine: string with get, set

type OnResponseStartedDetails =
  abstract id: int with get, set
  abstract url: string with get, set
  abstract method: string with get, set
  abstract webContentsId: int option with get, set
  abstract resourceType: string with get, set
  abstract referrer: string with get, set
  abstract timestamp: float with get, set
  abstract responseHeaders: obj with get, set
  /// Indicates whether the response was fetched from disk cache.
  abstract fromCache: bool with get, set
  abstract statusCode: int with get, set
  abstract statusLine: string with get, set

type OnResponseStartedFilter =
  /// Array of URL patterns that will be used to filter out the requests that do
  /// not match the URL patterns.
  abstract urls: string [] with get, set

type OnSendHeadersDetails =
  abstract id: int with get, set
  abstract url: string with get, set
  abstract method: string with get, set
  abstract webContentsId: int option with get, set
  abstract resourceType: string with get, set
  abstract referrer: string with get, set
  abstract timestamp: float with get, set
  abstract requestHeaders: obj with get, set

type OnSendHeadersFilter =
  /// Array of URL patterns that will be used to filter out the requests that do
  /// not match the URL patterns.
  abstract urls: string [] with get, set

[<StringEnum; RequireQualifiedAccess>]
type DevToolsDockMode =
  | Right
  | Bottom
  | Undocked
  | Detach

type OpenDevToolsOptions =
  /// Opens the devtools with specified dock state. Defaults to last used dock
  /// state. In DevToolsDockMode.Undocked mode it's possible to dock back. In
  /// DevToolsDockMode.Detach mode it's not.
  abstract mode: DevToolsDockMode with get, set
  /// Whether to bring the opened devtools window to the foreground. The default
  /// is true.
  abstract activate: bool with get, set

[<StringEnum; RequireQualifiedAccess>]
type DialogFeature =
  /// Allow files to be selected. Note: On Windows and Linux, can not be
  /// combined with OpenDirectory. OpenDirectory will take precedence.
  | OpenFile
  /// Allow directories to be selected. Note: On Windows and Linux, can not be
  /// combined with OpenFile. OpenDirectory will take precedence.
  | OpenDirectory
  /// Allow multiple paths to be selected.
  | MultiSelections
  /// Show hidden files in dialog.
  | ShowHiddenFiles
  /// [macOS] Allow creating new directories from dialog.
  | CreateDirectory
  /// [Windows] Prompt for creation if the file path entered in the dialog does
  /// not exist. This does not actually create the file at the path but allows
  /// non-existent paths to be returned that should be created by the
  /// application.
  | PromptToCreate
  /// [macOS] Disable the automatic alias (symlink) path resolution. Selected
  /// aliases will now return the alias path instead of their target path.
  | NoResolveAliases
  /// [macOS] Treat packages, such as .app folders, as a directory instead of a
  /// file.
  | TreatPackageAsDirectory

type OpenDialogOptions =
  abstract title: string with get, set
  abstract defaultPath: string with get, set
  /// Custom label for the confirmation button, when left empty the default
  /// label will be used.
  abstract buttonLabel: string with get, set
  /// Filters the file types that are displayed or selected in the dialog.
  abstract filters: FileFilter [] with get, set
  /// Contains which features the dialog should use. The following values are
  /// supported:
  abstract properties: DialogFeature [] with get, set
  /// [macOS] Message to display above input boxes.
  abstract message: string with get, set
  /// [Mac App Store] Create security scoped bookmarks when packaged for the Mac
  /// App Store.
  abstract securityScopedBookmarks: bool with get, set

type OpenExternalOptions =
  /// [macOS] true to bring the opened application to the foreground. The
  /// default is true.
  abstract activate: bool with get, set
  /// [Windows] The working directory.
  abstract workingDirectory: string with get, set

[<StringEnum; RequireQualifiedAccess>]
type DeviceEmulationScreenPosition =
  | Desktop
  | Mobile

type DeviceEmulationParameters =
  /// Specify the screen type to emulate (default:
  /// DeviceEmulationScreenPosition.Desktop):
  abstract screenPosition: DeviceEmulationScreenPosition with get, set
  /// Set the emulated screen size (screenPosition ==
  /// DeviceEmulationScreenPosition.Mobile).
  abstract screenSize: Size with get, set
  /// Position the view on the screen (screenPosition ==
  /// DeviceEmulationScreenPosition.Mobile) (default: { x: 0, y: 0 }).
  abstract viewPosition: Point with get, set
  /// Set the device scale factor (if zero defaults to original device scale
  /// factor) (default: 0).
  abstract deviceScaleFactor: int with get, set
  /// Set the emulated view size (empty means no override)
  abstract viewSize: Size with get, set
  /// Scale of emulated view inside available space (not in fit to view mode)
  /// (default: 1).
  abstract scale: float with get, set

type Payment =
  /// The identifier of the purchased product.
  abstract productIdentifier: string with get, set
  /// The quantity purchased.
  abstract quantity: int with get, set

[<StringEnum; RequireQualifiedAccess>]
type PermissionCheckMediaType =
  | Video
  | Audio
  | Unknown

[<StringEnum; RequireQualifiedAccess>]
type PermissionRequestMediaType =
  | Video
  | Audio

type PermissionCheckHandlerDetails =
  /// The security orign of the `media` check.
  abstract securityOrigin: string with get, set
  /// The type of media access being requested.
  abstract mediaType: PermissionCheckMediaType with get, set
  /// The last URL the requesting frame loaded
  abstract requestingUrl: string with get, set
  /// Whether the frame making the request is the main frame
  abstract isMainFrame: bool with get, set

type PermissionRequestHandlerDetails =
  /// The url of the openExternal request.
  abstract externalURL: string option with get, set
  /// The types of media access being requested
  abstract mediaTypes: PermissionRequestMediaType [] option with get, set
  /// The last URL the requesting frame loaded
  abstract requestingUrl: string with get, set
  /// Whether the frame making the request is the main frame
  abstract isMainFrame: bool with get, set

type PopupOptions =
  /// Default is the focused window.
  abstract window: BrowserWindow with get, set
  /// Default is the current mouse cursor position. Must be set if y is set.
  abstract x: int with get, set
  /// Default is the current mouse cursor position. Must be set if x is set.
  abstract y: int with get, set
  /// [macOS] The index of the menu item to be positioned under the mouse cursor
  /// at the specified coordinates. Default is -1.
  abstract positioningItem: int with get, set
  /// Called when menu is closed.
  abstract callback: (unit -> unit) with get, set

type PrintOptions =
  /// Don't ask user for print settings. Default is false.
  abstract silent: bool with get, set
  /// Also prints the background color and image of the web page. Default is
  /// false.
  abstract printBackground: bool with get, set
  /// Set the printer device name to use. Default is "".
  abstract deviceName: string with get, set

[<StringEnum; RequireQualifiedAccess>]
type PrintToPDFSize =
  | [<CompiledName("A3")>] A3
  | [<CompiledName("A4")>] A4
  | [<CompiledName("A5")>] A5
  | [<CompiledName("Legal")>] Legal
  | [<CompiledName("Letter")>] Letter
  | [<CompiledName("Tabloid")>] Tabloid

type PrintToPDFOptions =
  /// Specifies the type of margins to use. Uses 0 for default margin, 1 for no
  /// margin, and 2 for minimum margin. Default 0.
  abstract marginsType: int with get, set
  /// Specify page size of the generated PDF. Can be A3, A4, A5, Legal, Letter,
  /// Tabloid or an object containing height and width in microns.
  abstract pageSize: U2<PrintToPDFSize, Size> with get, set
  /// Whether to print CSS backgrounds. Default false.
  abstract printBackground: bool with get, set
  /// Whether to print selection only. Default false.
  abstract printSelectionOnly: bool with get, set
  /// true for landscape, false for portrait. Default false.
  abstract landscape: bool with get, set

type CustomSchemePrivileges =
  /// Default false.
  abstract standard: bool with get, set
  /// Default false.
  abstract secure: bool with get, set
  /// Default false.
  abstract bypassCSP: bool with get, set
  /// Default false.
  abstract allowServiceWorkers: bool with get, set
  /// Default false.
  abstract supportFetchAPI: bool with get, set
  /// Default false.
  abstract corsEnabled: bool with get, set

type ProcessMemoryInfo =
  /// [Linux, Windows] The amount of memory currently pinned to actual physical
  /// RAM in Kilobytes.
  ///
  /// Chromium does not provide `residentSet` value for macOS. This is because
  /// macOS performs in-memory compression of pages that haven't been recently
  /// used. As a result the resident set size value is not what one would
  /// expect. `private` memory is more representative of the actual
  /// pre-compression memory usage of the process on macOS.
  abstract residentSet: int with get, set
  /// The amount of memory not shared by other processes, such as JS heap or
  /// HTML content in Kilobytes.
  abstract ``private``: int with get, set
  /// The amount of memory shared between processes, typically memory consumed
  /// by the Electron code itself in Kilobytes.
  abstract shared: int with get, set

[<StringEnum; RequireQualifiedAccess>]
type ProgressBarMode =
  | None
  | Normal
  | Indeterminate
  | Error
  | Paused

type ProgressBarOptions =
  /// [Windows] Mode for the progress bar. Can be none, normal, indeterminate,
  /// error or paused.
  abstract mode: ProgressBarMode with get, set

type SpellCheckProvider =
  /// First argument is the words words to spellcheck. Second argument is a
  /// callback that must be called with misspelt words.
  abstract spellCheck: (string [] -> (string [] -> unit) -> unit) with get, set

type ClipboardBookmark =
  /// The title of the bookmark in the clipboard. Will be the empty string when
  /// the bookmark is unavailable.
  abstract title: string with get, set
  /// The URL of the bookmark in the clipboard. Will be the empty string when
  /// the bookmark is unavailable.
  abstract url: string with get, set

type RedirectRequestUploadData =
  /// MIME type of the content.
  abstract contentType: string with get, set
  /// Content to be sent.
  abstract data: string with get, set

type RedirectRequest =
  abstract url: string with get, set
  abstract method: string with get, set
  abstract session: Session option with get, set
  abstract uploadData: RedirectRequestUploadData with get, set

type RegisterBufferProtocolRequest =
  abstract url: string with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type RegisterFileProtocolRequest =
  abstract url: string with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type RegisterHttpProtocolRequest =
  abstract url: string with get, set
  abstract headers: obj with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type RegisterStreamProtocolRequest =
  abstract url: string with get, set
  abstract headers: obj with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type RegisterStringProtocolRequest =
  abstract url: string with get, set
  abstract referrer: string with get, set
  abstract method: string with get, set
  abstract uploadData: UploadData [] with get, set

type RelaunchOptions =
  abstract args: string [] with get, set
  abstract execPath: string with get, set

type LoginRequest =
  abstract method: string with get, set
  abstract url: string with get, set
  abstract referrer: string with get, set

[<StringEnum; RequireQualifiedAccess>]
type ResizeQuality =
  | Good
  | Better
  | Best

type ResizeOptions =
  /// Defaults to the image's width.
  abstract width: int with get, set
  /// Defaults to the image's height.
  abstract height: int with get, set
  /// The desired quality of the resize image. These values express a desired
  /// quality/speed tradeoff. They are translated into an algorithm-specific
  /// method that depends on the capabilities (CPU, GPU) of the underlying
  /// platform. It is possible for all three methods to be mapped to the same
  /// algorithm on a given platform.
  abstract quality: ResizeQuality with get, set

type ResourceUsage =
  abstract images: MemoryUsageDetails with get, set
  abstract scripts: MemoryUsageDetails with get, set
  abstract cssStyleSheets: MemoryUsageDetails with get, set
  abstract xslStyleSheets: MemoryUsageDetails with get, set
  abstract fonts: MemoryUsageDetails with get, set
  abstract other: MemoryUsageDetails with get, set

type OnBeforeRequestResponse =
  abstract cancel: bool with get, set
  /// The original request is prevented from being sent or completed and is
  /// instead redirected to the given URL.
  abstract redirectURL: string with get, set

type SaveDialogOptions =
  abstract title: string with get, set
  /// Absolute directory path, absolute file path, or file name to use by
  /// default.
  abstract defaultPath: string with get, set
  /// Custom label for the confirmation button, when left empty the default
  /// label will be used.
  abstract buttonLabel: string with get, set
  /// Filters the file types that are displayed in the dialog.
  abstract filters: FileFilter [] with get, set
  /// [macOS] Message to display above text fields.
  abstract message: string with get, set
  /// [macOS] Custom label for the text displayed in front of the filename text
  /// field.
  abstract nameFieldLabel: string with get, set
  /// [macOS] Show the tags input box, defaults to true.
  abstract showsTagField: bool with get, set
  /// [Mac App Store] Create a security scoped bookmark when packaged for the
  /// Mac App Store. If this option is enabled and the file doesn't already
  /// exist a blank file will be created at the chosen path.
  abstract securityScopedBookmarks: bool with get, set

type SetLoginItemSettings =
  /// True to open the app at login, false to remove the app as a login item.
  /// Defaults to false.
  abstract openAtLogin: bool with get, set
  /// [macOS] True to open the app as hidden. Defaults to false. The user can
  /// edit this setting from the System Preferences, so
  /// app.getLoginItemSettings().wasOpenedAsHidden should be checked when the
  /// app is opened to know the current value. This setting is not available on
  /// Mac App Store builds.
  abstract openAsHidden: bool with get, set
  /// [Windows] The executable to launch at login. Defaults to process.execPath.
  abstract path: string with get, set
  /// [Windows] The command-line arguments to pass to the executable. Defaults
  /// to an empty array. Take care to wrap paths in quotes.
  abstract args: string [] with get, set

[<RequireQualifiedAccess; StringEnum>]
type DesktopCapturerSourceType =
  | Screen
  | Window

type GetDesktopCapturerSourcesOptions =
  /// An array of Strings that lists the types of desktop sources to be
  /// captured.
  abstract types: DesktopCapturerSourceType [] with get, set
  /// The size that the media source thumbnail should be scaled to. Default is
  /// 150 x 150. Set width or height to 0 when you do not need the thumbnails.
  /// This will save the processing time required for capturing the content of
  /// each window and screen.
  abstract thumbnailSize: Size with get, set
  /// Set to true to enable fetching window icons. The default value is false.
  /// When false the appIcon property of the sources return null. Same if a
  /// source has the type screen.
  abstract fetchWindowIcons: bool with get, set

type SystemMemoryInfo =
  /// The total amount of physical memory in Kilobytes available to the system.
  abstract total: int with get, set
  /// The total amount of memory not being used by applications or disk cache.
  abstract free: int with get, set
  /// [Windows, Linux] The total amount of swap memory in Kilobytes available to
  /// the system.
  abstract swapTotal: int with get, set
  /// [Windows, Linux] The free amount of swap memory in Kilobytes available to
  /// the system.
  abstract swapFree: int with get, set

type ToBitmapOptions =
  /// Defaults to 1.0.
  abstract scaleFactor: float with get, set

type ToDataURLOptions =
  /// Defaults to 1.0.
  abstract scaleFactor: float with get, set

type ToPNGOptions =
  /// Defaults to 1.0.
  abstract scaleFactor: float with get, set

[<StringEnum; RequireQualifiedAccess>]
type TouchBarButtonIconPosition =
  | Left
  | Right
  | Overlay

type TouchBarButtonOptions =
  /// Button text.
  abstract label: string with get, set
  /// Button background color in hex format, i.e #ABCDEF.
  abstract backgroundColor: string with get, set
  /// Button icon.
  abstract icon: NativeImage with get, set
  abstract iconPosition: TouchBarButtonIconPosition with get, set
  /// Function to call when the button is clicked.
  abstract click: (unit -> unit) with get, set

type TouchBarColorPickerOptions =
  /// Array of hex color strings to appear as possible colors to select.
  abstract availableColors: string [] with get, set
  /// The selected hex color in the picker, i.e #ABCDEF.
  abstract selectedColor: string with get, set
  /// Function to call when a color is selected. Called with the color that the
  /// user selected from the picker.
  abstract change: (string -> unit) with get, set

type TouchBarOptions =
  abstract items: ITouchBarItem [] with get, set
  abstract escapeItem: ITouchBarItem option with get, set

type TouchBarGroupOptions =
  /// Items to display as a group.
  abstract items: TouchBar with get, set

type TouchBarLabelOptions =
  /// Text to display.
  abstract label: string with get, set
  /// Hex color of text, i.e #ABCDEF.
  abstract textColor: string with get, set

type TouchBarPopoverOptions =
  /// Popover button text.
  abstract label: string with get, set
  /// Popover button icon.
  abstract icon: NativeImage with get, set
  /// Items to display in the popover.
  abstract items: TouchBar with get, set
  /// true to display a close button on the left of the popover, false to not
  /// show it. Default is true.
  abstract showCloseButton: bool with get, set

type TouchBarScrubberOptions =
  /// An array of items to place in this scrubber.
  abstract items: ScrubberItem [] with get, set
  /// Called when the user taps an item that was not the last tapped item.
  /// Called with the index of the item the user selected.
  abstract select: (int -> unit) with get, set
  /// Called when the user taps any item. Called with the index of the item the
  /// user touched
  abstract highlight: (int -> unit) with get, set
  /// Selected item style. Defaults to None.
  abstract selectedStyle: TouchBarScrubberStyle option with get, set
  /// Selected overlay item style. Defaults to None.
  abstract overlayStyle: TouchBarScrubberStyle option with get, set
  /// Defaults to false.
  abstract showArrowButtons: bool with get, set
  /// Defaults to TouchBarScrubberMode.Free.
  abstract mode: TouchBarScrubberMode with get, set
  /// Defaults to true.
  abstract continuous: bool with get, set

[<StringEnum; RequireQualifiedAccess>]
type TouchBarSegmentedControlSegmentStyle =
  /// The appearance of the segmented control is automatically determined based
  /// on the type of window in which the control is displayed and the position
  /// within the window.
  | Automatic
  /// The control is displayed using the rounded style.
  | Rounded
  /// The control is displayed using the textured rounded style.
  | [<CompiledName("textured-rounded")>] TexturedRounded
  /// The control is displayed using the round rect style.
  | [<CompiledName("round-rect")>] RoundRect
  /// The control is displayed using the textured square style.
  | [<CompiledName("textured-square")>] TexturedSquare
  /// The control is displayed using the capsule style.
  | Capsule
  /// The control is displayed using the small square style.
  | [<CompiledName("small-square")>] SmallSquare
  /// The segments in the control are displayed very close to each other but not
  /// touching.
  | Separated

[<StringEnum; RequireQualifiedAccess>]
type TouchBarSegmentedControlMode =
  /// One item selected at a time, selecting one deselects the previously
  /// selected item.
  | Single
  /// Multiple items can be selected at a time.
  | Multiple
  /// Make the segments act as buttons, each segment can be pressed and released
  /// but never marked as active.
  | Buttons

type TouchBarSegmentedControlOptions =
  /// Style of the segments. Default is
  /// TouchBarSegmentedControlSegmentStyle.Automatic.
  abstract segmentStyle: TouchBarSegmentedControlSegmentStyle with get, set
  /// The selection mode of the control. Default is
  /// TouchBarSegmentedControlMode.Single.
  abstract mode: TouchBarSegmentedControlMode with get, set
  /// An array of segments to place in this control.
  abstract segments: SegmentedControlSegment [] with get, set
  /// The index of the currently selected segment, will update automatically
  /// with user interaction. When the `mode` is
  /// TouchBarSegmentedControlMode.Multiple it will be the last selected item.
  abstract selectedIndex: int option with get, set
  /// Called when the user selects a new segment. Called with the index of the
  /// segment the user selected, and whether as a result of user selection the
  /// segment is selected or not.
  abstract change: (int -> bool -> unit) with get, set

type TouchBarSliderOptions =
  /// Label text.
  abstract label: string with get, set
  /// Selected value.
  abstract value: int with get, set
  /// Minimum value.
  abstract minValue: int with get, set
  /// Maximum value.
  abstract maxValue: int with get, set
  /// Function to call when the slider is changed. Called with the value that
  /// the user selected on the Slider.
  abstract change: (int -> unit) with get, set

[<StringEnum; RequireQualifiedAccess>]
type TouchBarSpacerSize =
  /// Small space between items.
  | Small
  /// Large space between items.
  | Large
  /// Take up all available space.
  | Flexible

type TouchBarSpacerOptions =
  /// Size of spacer.
  abstract size: TouchBarSpacerSize with get, set

type UploadProgress =
  /// Whether the request is currently active. If this is false no other
  /// properties will be set
  abstract active: bool with get, set
  /// Whether the upload has started. If this is false both `current` and
  /// `total` will be set to 0.
  abstract started: bool with get, set
  /// The number of bytes that have been uploaded so far
  abstract current: int with get, set
  /// The number of bytes that will be uploaded this request
  abstract total: int with get, set

type VisibleOnAllWorkspacesOptions =
  /// Sets whether the window should be visible above fullscreen windows
  abstract visibleOnFullScreen: bool with get, set

type ContextMenuEditFlags =
  /// Whether the renderer believes it can undo.
  abstract canUndo: bool with get, set
  /// Whether the renderer believes it can redo.
  abstract canRedo: bool with get, set
  /// Whether the renderer believes it can cut.
  abstract canCut: bool with get, set
  /// Whether the renderer believes it can copy
  abstract canCopy: bool with get, set
  /// Whether the renderer believes it can paste.
  abstract canPaste: bool with get, set
  /// Whether the renderer believes it can delete.
  abstract canDelete: bool with get, set
  /// Whether the renderer believes it can select all.
  abstract canSelectAll: bool with get, set

type FoundInPageResult =
  abstract requestId: int with get, set
  /// Position of the active match.
  abstract activeMatchOrdinal: int with get, set
  /// Number of Matches.
  abstract matches: int with get, set
  /// Coordinates of first match region.
  abstract selectionArea: obj with get, set
  abstract finalUpdate: bool with get, set

type ContextMenuMediaFlags =
  /// Whether the media element has crashed.
  abstract inError: bool with get, set
  /// Whether the media element is paused.
  abstract isPaused: bool with get, set
  /// Whether the media element is muted.
  abstract isMuted: bool with get, set
  /// Whether the media element has audio.
  abstract hasAudio: bool with get, set
  /// Whether the media element is looping.
  abstract isLooping: bool with get, set
  /// Whether the media element's controls are visible.
  abstract isControlsVisible: bool with get, set
  /// Whether the media element's controls are toggleable.
  abstract canToggleControls: bool with get, set
  /// Whether the media element can be rotated.
  abstract canRotate: bool with get, set

[<StringEnum; RequireQualifiedAccess>]
type AutoplayPolicy =
  | [<CompiledName("no-user-gesture-required")>] NoUserGestureRequired
  | [<CompiledName("user-gesture-required")>] UserGestureRequired
  | [<CompiledName("document-user-activation-required")>] DocumentUserActivationRequired

type WebPreferences =
  /// Whether to enable DevTools. If it is set to false, can not use
  /// BrowserWindow.webContents.openDevTools() to open DevTools. Default is
  /// true.
  abstract devTools: bool with get, set
  /// Whether node integration is enabled. Default is false.
  abstract nodeIntegration: bool with get, set
  /// Whether node integration is enabled in web workers. Default is false. More
  /// about this can be found here: https://electronjs.org/docs/tutorial/multithreading
  abstract nodeIntegrationInWorker: bool with get, set
  /// Experimental option for enabling NodeJS support in sub-frames such as
  /// iframes. All your preloads will load for every iframe, you can use
  /// process.isMainFrame to determine if you are in the main frame or not.
  abstract nodeIntegrationInSubFrames: bool with get, set
  /// Specifies a script that will be loaded before other scripts run in the
  /// page. This script will always have access to node APIs no matter whether
  /// node integration is turned on or off. The value should be the absolute
  /// file path to the script. When node integration is turned off, the preload
  /// script can reintroduce Node global symbols back to the global scope.
  abstract preload: string with get, set
  /// If set, this will sandbox the renderer associated with the window, making
  /// it compatible with the Chromium OS-level sandbox and disabling the Node.js
  /// engine. This is not the same as the `nodeIntegration` option and the APIs
  /// available to the preload script are more limited. Read more about the
  /// option here: https://electronjs.org/docs/api/sandbox-option.
  ///
  /// Note: This option is currently experimental and may change or be
  /// removed in future Electron releases.
  abstract sandbox: bool with get, set
  /// Whether to enable the `remote` module. Default is true.
  abstract enableRemoteModule: bool with get, set
  /// Sets the session used by the page. Instead of passing the Session object
  /// directly, you can also choose to use the `partition` option instead, which
  /// accepts a partition string. When both session and partition are provided,
  /// session will be preferred. Default is the default session.
  abstract session: Session with get, set
  /// Sets the session used by the page according to the session's partition
  /// string. If partition starts with `persist:`, the page will use a
  /// persistent session available to all pages in the app with the same
  /// partition. If there is no persist: prefix, the page will use an in-memory
  /// session. By assigning the same partition, multiple pages can share the
  /// same session. Default is the default session.
  abstract partition: string with get, set
  /// When specified, web pages with the same affinity will run in the same
  /// renderer process. Note that due to reusing the renderer process, certain
  /// webPreferences options will also be shared between the web pages even when
  /// you specified different values for them, including but not limited to
  /// preload, sandbox and nodeIntegration. So it is suggested to use exact same
  /// webPreferences for web pages with the same affinity.
  abstract affinity: string with get, set
  /// The default zoom factor of the page, 3.0 represents 300%. Default is 1.0.
  abstract zoomFactor: float with get, set
  /// Enables JavaScript support. Default is true.
  abstract javascript: bool with get, set
  /// When false, it will disable the same-origin policy (usually using testing
  /// websites by people), and set allowRunningInsecureContent to true if this
  /// options has not been set by user. Default is true.
  abstract webSecurity: bool with get, set
  /// Allow an https page to run JavaScript, CSS or plugins from http URLs.
  /// Default is false.
  abstract allowRunningInsecureContent: bool with get, set
  /// Enables image support. Default is true.
  abstract images: bool with get, set
  /// Make TextArea elements resizable. Default is true.
  abstract textAreasAreResizable: bool with get, set
  /// Enables WebGL support. Default is true.
  abstract webgl: bool with get, set
  /// Enables WebAudio support. Default is true.
  abstract webaudio: bool with get, set
  /// Whether plugins should be enabled. Default is false.
  abstract plugins: bool with get, set
  /// Enables Chromium's experimental features. Default is false.
  abstract experimentalFeatures: bool with get, set
  /// Enables scroll bounce (rubber banding) effect on macOS. Default is false.
  abstract scrollBounce: bool with get, set
  /// A list of feature strings to enable separated by comma, like
  /// CSSVariables,KeyboardEventKey. The full list of supported feature strings
  /// can be found here:
  /// https://cs.chromium.org/chromium/src/third_party/blink/renderer/platform/runtime_enabled_features.json5?l=70
  abstract enableBlinkFeatures: string with get, set
  /// A list of feature strings to disable separated by comma, like
  /// CSSVariables,KeyboardEventKey. The full list of supported feature strings
  /// can be found here:
  /// https://cs.chromium.org/chromium/src/third_party/blink/renderer/platform/runtime_enabled_features.json5?l=70
  abstract disableBlinkFeatures: string with get, set
  /// Sets the default font for the font-family.
  abstract defaultFontFamily: DefaultFontFamily with get, set
  /// Defaults to 16.
  abstract defaultFontSize: int with get, set
  /// Defaults to 13.
  abstract defaultMonospaceFontSize: int with get, set
  /// Defaults to 0.
  abstract minimumFontSize: int with get, set
  /// Defaults to ISO-8859-1.
  abstract defaultEncoding: string with get, set
  /// Whether to throttle animations and timers when the page becomes
  /// background. This also affects the Page Visibility API. Defaults to true.
  abstract backgroundThrottling: bool with get, set
  /// Whether to enable offscreen rendering for the browser window. Defaults to
  /// false. See here for more details:
  /// https://electronjs.org/docs/tutorial/offscreen-rendering
  abstract offscreen: bool with get, set
  /// Whether to run Electron APIs and the specified preload script in a
  /// separate JavaScript context. Defaults to false. The context that the
  /// preload script runs in will still have full access to the document and
  /// window globals but it will use its own set of JavaScript builtins (Array,
  /// Object, JSON, etc.) and will be isolated from any changes made to the
  /// global environment by the loaded page. The Electron API will only be
  /// available in the preload script and not the loaded page. This option
  /// should be used when loading potentially untrusted remote content to ensure
  /// the loaded content cannot tamper with the preload script and any Electron
  /// APIs being used. This option uses the same technique used by Chrome
  /// Content Scripts. You can access this context in the dev tools by selecting
  /// the 'Electron Isolated Context' entry in the combo box at the top of the
  /// Console tab.
  abstract contextIsolation: bool with get, set
  /// Whether to use native window.open(). Defaults to false. Child windows will
  /// always have node integration disabled. Note: This option is currently
  /// experimental.
  abstract nativeWindowOpen: bool with get, set
  /// A list of strings that will be appended to process.argv in the renderer
  /// process of this app. Useful for passing small bits of data down to
  /// renderer process preload scripts.
  abstract additionalArguments: string [] with get, set
  /// Whether to enable browser style consecutive dialog protection. Default is
  /// false.
  abstract safeDialogs: bool with get, set
  /// The message to display when consecutive dialog protection is triggered. If
  /// not defined the default message would be used, note that currently the
  /// default message is in English and not localized.
  abstract safeDialogsMessage: string with get, set
  /// Whether dragging and dropping a file or link onto the page causes a
  /// navigation. Default is false.
  abstract navigateOnDragDrop: bool with get, set
  /// Autoplay policy to apply to content in the window.
  abstract autoplayPolicy: AutoplayPolicy with get, set
  /// Whether to prevent the window from resizing when entering HTML Fullscreen.
  /// Default is `false`.
  abstract disableHtmlFullscreenWindowResize: bool with get, set

type DefaultFontFamily =
  /// Defaults to Times New Roman.
  abstract standard: string with get, set
  /// Defaults to Times New Roman.
  abstract serif: string with get, set
  /// Defaults to Arial.
  abstract sansSerif: string with get, set
  /// Defaults to Courier New.
  abstract monospace: string with get, set
  /// Defaults to Script.
  abstract cursive: string with get, set
  /// Defaults to Impact.
  abstract fantasy: string with get, set


module Helpers =

  open Fable.Core.JsInterop

  [<StringEnum; RequireQualifiedAccess>]
  type Modifier =
    /// macOS only. Use CmdOrCtrl instead.
    | [<CompiledName("Command")>] Command
    /// Alias for Command
    | [<CompiledName("Cmd")>] Cmd
    | [<CompiledName("Control")>] Control
    /// Alias for Control
    | [<CompiledName("Ctrl")>] Ctrl
    | [<CompiledName("CommandOrControl")>] CommandOrControl
    /// Alias for CommandOrControl
    | [<CompiledName("CmdOrCtrl")>] CmdOrCtrl
    | [<CompiledName("Alt")>] Alt
    /// macOS only. Use Alt instead.
    | [<CompiledName("Option")>] Option
    | [<CompiledName("AltGr")>] AltGr
    | [<CompiledName("Shift")>] Shift
    /// Mapped to the Windows key on Windows and Linux and the Cmd key on macOS.
    | [<CompiledName("Super")>] Super

  [<StringEnum; RequireQualifiedAccess>]
  type Key =
    /// The number 0 (not NumPad).
    | [<CompiledName("0")>] N0
    /// The number 1.
    | [<CompiledName("1")>] N1
    /// The number 2.
    | [<CompiledName("2")>] N2
    /// The number 3.
    | [<CompiledName("3")>] N3
    /// The number 4.
    | [<CompiledName("4")>] N4
    /// The number 5.
    | [<CompiledName("5")>] N5
    /// The number 6.
    | [<CompiledName("6")>] N6
    /// The number 7.
    | [<CompiledName("7")>] N7
    /// The number 8.
    | [<CompiledName("8")>] N8
    /// The number 9.
    | [<CompiledName("9")>] N9
    /// The letter A.
    | [<CompiledName("A")>] A
    /// The letter B.
    | [<CompiledName("B")>] B
    /// The letter C.
    | [<CompiledName("C")>] C
    /// The letter D.
    | [<CompiledName("D")>] D
    /// The letter E.
    | [<CompiledName("E")>] E
    /// The letter F.
    | [<CompiledName("F")>] F
    /// The letter G.
    | [<CompiledName("G")>] G
    /// The letter H.
    | [<CompiledName("H")>] H
    /// The letter I.
    | [<CompiledName("I")>] I
    /// The letter J.
    | [<CompiledName("J")>] J
    /// The letter K.
    | [<CompiledName("K")>] K
    /// The letter L.
    | [<CompiledName("L")>] L
    /// The letter M.
    | [<CompiledName("M")>] M
    /// The letter N.
    | [<CompiledName("N")>] N
    /// The letter O.
    | [<CompiledName("O")>] O
    /// The letter P.
    | [<CompiledName("P")>] P
    /// The letter Q.
    | [<CompiledName("Q")>] Q
    /// The letter R.
    | [<CompiledName("R")>] R
    /// The letter S.
    | [<CompiledName("S")>] S
    /// The letter T.
    | [<CompiledName("T")>] T
    /// The letter U.
    | [<CompiledName("U")>] U
    /// The letter V.
    | [<CompiledName("V")>] V
    /// The letter W.
    | [<CompiledName("W")>] W
    /// The letter X.
    | [<CompiledName("X")>] X
    /// The letter Y.
    | [<CompiledName("Y")>] Y
    /// The letter Z.
    | [<CompiledName("Z")>] Z
    /// Function key 1.
    | [<CompiledName("F1")>] F1
    /// Function key 2.
    | [<CompiledName("F2")>] F2
    /// Function key 3.
    | [<CompiledName("F3")>] F3
    /// Function key 4.
    | [<CompiledName("F4")>] F4
    /// Function key 5.
    | [<CompiledName("F5")>] F5
    /// Function key 6.
    | [<CompiledName("F6")>] F6
    /// Function key 7.
    | [<CompiledName("F7")>] F7
    /// Function key 8.
    | [<CompiledName("F8")>] F8
    /// Function key 9.
    | [<CompiledName("F9")>] F9
    /// Function key 10.
    | [<CompiledName("F10")>] F10
    /// Function key 11.
    | [<CompiledName("F11")>] F11
    /// Function key 12.
    | [<CompiledName("F12")>] F12
    /// Function key 13.
    | [<CompiledName("F13")>] F13
    /// Function key 14.
    | [<CompiledName("F14")>] F14
    /// Function key 15.
    | [<CompiledName("F15")>] F15
    /// Function key 16.
    | [<CompiledName("F16")>] F16
    /// Function key 17.
    | [<CompiledName("F17")>] F17
    /// Function key 18.
    | [<CompiledName("F18")>] F18
    /// Function key 19.
    | [<CompiledName("F19")>] F19
    /// Function key 20.
    | [<CompiledName("F20")>] F20
    /// Function key 21.
    | [<CompiledName("F21")>] F21
    /// Function key 22.
    | [<CompiledName("F22")>] F22
    /// Function key 23.
    | [<CompiledName("F23")>] F23
    /// Function key 24.
    | [<CompiledName("F24")>] F24
    /// )
    | [<CompiledName(")")>] RParen
    /// (
    | [<CompiledName("(")>] LParen
    /// !
    | [<CompiledName("!")>] Exclamation
    /// ?
    | [<CompiledName("?")>] Question
    /// @
    | [<CompiledName("@")>] At
    /// #
    | [<CompiledName("#")>] Hash
    /// $
    | [<CompiledName("$")>] Dollar
    /// %
    | [<CompiledName("%")>] Percent
    /// ^
    | [<CompiledName("^")>] Caret
    /// &
    | [<CompiledName("&")>] Ampersand
    /// *
    | [<CompiledName("*")>] Asterisk
    /// :
    | [<CompiledName(":")>] Colon
    /// ;
    | [<CompiledName(";")>] Semicolon
    /// =
    | [<CompiledName("=")>] Equals
    /// <
    | [<CompiledName("<")>] LessThan
    /// >
    | [<CompiledName(">")>] GreaterThan
    /// ,
    | [<CompiledName(",")>] Comma
    /// _
    | [<CompiledName("_")>] Underscore
    /// -
    | [<CompiledName("-")>] Dash
    /// Alias for Dash
    | [<CompiledName("-")>] Hyphen
    /// .
    | [<CompiledName(".")>] Dot
    /// /
    | [<CompiledName(".")>] ForwardSlash
    /// \
    | [<CompiledName("\\")>] Backslash
    /// ~
    | [<CompiledName("~")>] Tilde
    /// `
    | [<CompiledName("`")>] Backtick
    /// {
    | [<CompiledName("{")>] LBrace
    /// }
    | [<CompiledName("}")>] RBrace
    /// [
    | [<CompiledName("[")>] LBracket
    /// ]
    | [<CompiledName("]")>] RBracket
    /// |
    | [<CompiledName("|")>] Pipe
    /// '
    | [<CompiledName("'")>] SingleQuote
    /// Alias for SingleQuote
    | [<CompiledName("'")>] Apostrophe
    /// "
    | [<CompiledName("\"")>] DoubleQuote
    | [<CompiledName("Plus")>] Plus
    | [<CompiledName("Space")>] Space
    | [<CompiledName("Tab")>] Tab
    | [<CompiledName("Capslock")>] Capslock
    | [<CompiledName("Numlock")>] Numlock
    | [<CompiledName("Scrolllock")>] ScrollLock
    | [<CompiledName("Backspace")>] Backspace
    | [<CompiledName("Delete")>] Delete
    | [<CompiledName("Insert")>] Insert
    | [<CompiledName("Return")>] Return
    /// Alias for Return
    | [<CompiledName("Enter")>] Enter
    | [<CompiledName("Up")>] Up
    | [<CompiledName("Down")>] Down
    | [<CompiledName("Left")>] Left
    | [<CompiledName("Right")>] Right
    | [<CompiledName("Home")>] Home
    | [<CompiledName("End")>] End
    | [<CompiledName("PageUp")>] PageUp
    | [<CompiledName("PageDown")>] PageDown
    | [<CompiledName("Escape")>] Escape
    /// Alias for Escape
    | [<CompiledName("Esc")>] Esc
    | [<CompiledName("VolumeUp")>] VolumeUp
    | [<CompiledName("VolumeDown")>] VolumeDown
    | [<CompiledName("VolumeMute")>] VolumeMute
    | [<CompiledName("MediaNextTrack")>] MediaNextTrack
    | [<CompiledName("MediaPreviousTrack")>] MediaPreviousTrack
    | [<CompiledName("MediaStop")>] MediaStop
    | [<CompiledName("MediaPlayPause")>] MediaPlayPause
    | [<CompiledName("PrintScreen")>] PrintScreen
    /// Numpad 0
    | [<CompiledName("num0")>] Num0
    /// Numpad 1
    | [<CompiledName("num1")>] Num1
    /// Numpad 2
    | [<CompiledName("num2")>] Num2
    /// Numpad 3
    | [<CompiledName("num3")>] Num3
    /// Numpad 4
    | [<CompiledName("num4")>] Num4
    /// Numpad 5
    | [<CompiledName("num5")>] Num5
    /// Numpad 6
    | [<CompiledName("num6")>] Num6
    /// Numpad 7
    | [<CompiledName("num7")>] Num7
    /// Numpad 8
    | [<CompiledName("num8")>] Num8
    /// Numpad 9
    | [<CompiledName("num9")>] Num9
    /// Numpad decimal
    | [<CompiledName("numdec")>] NumDec
    /// Numpad +
    | [<CompiledName("numadd")>] NumAdd
    /// Numpad -
    | [<CompiledName("numsub")>] NumSub
    /// Numpad *
    | [<CompiledName("nummult")>] NumMult
    /// Numpad /
    | [<CompiledName("numdiv")>] NumDiv


  /// Returns an accelerator string that can be used to register shortcuts.
  let createAccelerator (modifiers: Modifier list) (key: Key) =
    modifiers, !!key ||> List.foldBack (fun (m: Modifier) acc -> !!m + "+" + acc)
