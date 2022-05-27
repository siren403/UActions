
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

---

```yaml
uses: get-version
```

```
$(VERSION)
```

---

```yaml
uses: injection
with:
  path: <String>
  data: <Dictionary`2>
```

---

```yaml
uses: player-settings
with:
  company-name: <String>
  product-name: <String>
  version: <String?>
  preset: <String?>
```

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

---

```yaml
uses: player-settings-ios
with:
  target-sdk: !iossdkversion <iOSSdkVersion?>
```

---

```yaml
uses: print
with:
  message: <String>
```

---

```yaml
uses: process
with:
  file-name: <String>
```

---

```yaml
uses: semantic-versioning
with:
  type: <VersionType>
```
