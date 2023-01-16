[build](Documents/Actions/build.md)
---

```yaml
uses: build
with:
  path: <String>
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
  data: <String>
```

[player-settings](Documents/Actions/player-settings.md)
---

```yaml
uses: player-settings
with:
  preset: <String>
  company-name: <String>
  product-name: <String>
  version: <String>
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
  app-bundle: <Boolean>
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

