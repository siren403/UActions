UActions
===

[![update Actions.md](https://github.com/qkrsogusl3/UActions/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/qkrsogusl3/UActions/actions/workflows/dotnet.yml)

Getting started
---

install YamlDotNet package.
- [YamlDotNet](https://github.com/aaubry/YamlDotNet)
- [YamlDotNet-UPM (fork repo)](https://github.com/qkrsogusl3/YamlDotNet-UPM)
    
    add package from git url
    ```
    https://github.com/qkrsogusl3/YamlDotNet-UPM.git?path=YamlDotNet.Unity/Packages/YamlDotNet#upm
    ```

add package from git url

```
https://github.com/qkrsogusl3/UActions.git?path=Packages/UActions
```

create [workflow.yaml](./workflow.yaml) file.

> basic hierarchy
```yaml
env: # using values in workflows. <KEY>: <value>
  COMPANY: company
  PRODUCT_NAME: product
steps: # action definitions
  # <name>:
  #   uses: <action-name>
  #   with:
  #     <key>: <value>
  injection:
    uses: injection
    with:
      path: Assets/InjectSample.asset
      data:
        url: "https://google.com"
        key: 333=-====Snd3d30_3433
        number: !!int 11111
jobs:
  build-apk: # <name>
    platform: android # ex) android || ios
    steps: # actions array
      - uses: print
        with:
          message: build apk start
      - name: injection # defined action
      - uses: player-settings
        with:
          company-name: $(COMPANY) # using env value
          product-name: $(PRODUCT_NAME)
          preset: Assets/PlayerSettings
      - uses: player-settings-android
        with:
          package-name: com.$(COMPANY).$(PRODUCT_NAME)
          architectures: !architectures
            - "ARMv7"
            - "ARM64"
      - uses: build
        with:
          path: $(PROJECT_PATH)/Build/$(PLATFORM)/$(PRODUCT_NAME)
```

```bash
<UnityEditor> -batchmode -quit -buildTarget Android -projectPath <path> -executeMethod UActions.Bootstrap.Run -job build-apk
```

run build

> [Unity Editor command line arguments](https://docs.unity3d.com/Manual/EditorCommandLineArguments.html)

```bash
<UnityEditor> -batchmode -quit -buildTarget <platform> -projectPath <path> -executeMethod UActions.Bootstrap.Run -job <jobName>
```
    
check [actions](./Actions.md)

JsonSchema
---

use remote schema this url
```
https://raw.githubusercontent.com/qkrsogusl3/UActions/main/Documents/workflow_schema.json
```

