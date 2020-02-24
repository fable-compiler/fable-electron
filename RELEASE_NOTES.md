### 5.1.0

* Minor update based on updated Electron docs

### 5.0.0

* Updated for Electron 8

### 4.3.0

* Small breaking change according to Electron docs update: Renamed `LoginRequest` to `AuthenticationResponseDetails` and removed its `method` and `referrer` properties  (relevant for the `login` event on `App` and `WebContents`)

### 4.2.0

Changed according to Electron docs update:
 - Breaking (small): `ProtocolResponseUploadData.data` is changed from `string` to `U2<string, Buffer>`
 - Added `BrowserWindow.setIcon` overload accepting `string`

### 4.1.0

* Updated for Electron 7.1

### 4.0.0

* Updated for Electron 7.0

### 3.0.0

* Breaking bugfix: Use explicit Action wrappers for `MenuItem.click`, `MenuItemOptions.click`, `SpellCheckProvider.spellCheck`, and `TouchBarSegmentedControlOptions.change`

### 2.0.0

* Tiny breaking change based on updated Electron docs: In `showMessageBox`, made `MessageBoxOptions.icon` accept both `NativeImage` and `string`.


### 1.0.0

* First release
