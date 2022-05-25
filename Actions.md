AutoIncrementVersionCode
---

```yaml
uses: auto-increment-version-code
with:
  a: <Int32>
  b: <String>
  c: <Int32?10>
```
Build
---

```yaml
uses: build
with:
  path: <String>
```
ExecuteShellCommand
---

```yaml
uses: command
with:
  args: <String>
```
GetVersion
---

```yaml
uses: get-version
```

```
$(VERSION)
```
Injection
---

```yaml
uses: injection
with:
  path: <String>
  data: <Dictionary`2>
```
Log
---

```yaml
uses: log
with:
  message: <String>
```
PlayerSettingsAction
---

```yaml
uses: player-settings
with:
  company-name: <String>
  product-name: <String>
  version: <String?>
  preset: <String?>
```
PlayerSettingsAndroid
---

```yaml
uses: player-settings-android
with:
  package-name: <String?>
  architectures: !architectures <AndroidArchitecture[]?>
  keystore: !keystore
    path: <path>
    passwd: <passwd>
    alias: <alias>
    aliasPasswd: <aliasPasswd>
  increment-version-code: <Boolean?>
  optimized-frame-pacing: <Boolean?>
```
PlayerSettingsiOS
---

```yaml
uses: player-settings-ios
with:
  target-sdk: !iossdkversion <iOSSdkVersion?>
```
Process
---

```yaml
uses: process
with:
  file-name: <String>
```
SemanticVersioning
---

```yaml
uses: semantic-versioning
with:
  type: <VersionType>
```
