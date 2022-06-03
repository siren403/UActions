auto-increment-version-code
---

```yaml
uses: auto-increment-version-code
with:
  a: <Int32>
  b: <String>
  c: <Int32?10>
```
[build](Documents/Actions/build.md)
---

```yaml
uses: build
with:
  path: <String>
```
command
---

```yaml
uses: command
with:
  args: <String>
  working-directory: <String?>
```
[fastlane](Documents/Actions/fastlane.md)
---

```yaml
uses: fastlane
with:
  platform: <String>
  lane: <String>
  directory: <String>
```
get-version
---

```yaml
uses: get-version
```

```
$(VERSION)
```
injection
---

```yaml
uses: injection
with:
  path: <String>
  data: <Dictionary`2>
```
[player-settings](Documents/Actions/player-settings.md)
---

```yaml
uses: player-settings
with:
  company-name: <String>
  product-name: <String>
  version: <String?>
  preset: <String?>
```
player-settings-android
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
player-settings-ios
---

```yaml
uses: player-settings-ios
with:
  identifier: <String?>
  increment-version-code: <Boolean?>
  target-sdk: !iossdkversion <iOSSdkVersion?>
  ios-version: <String?>
```
print
---

```yaml
uses: print
with:
  message: <String>
```
process
---

```yaml
uses: process
with:
  file-name: <String>
```
semantic-versioning
---

```yaml
uses: semantic-versioning
with:
  type: <VersionType>
```
