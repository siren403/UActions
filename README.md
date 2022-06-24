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
env: # const values 
  COMPANY: company
input: # default values
  PRODUCT_NAME: product
groups:
  first:
    - print:
        message: first-1
    - print:
        message: first-2
  second:
    - print:
        message: s
works:
  build-apk:
    platform: android
    steps:
      - player-settings:
          company-name: $(COMPANY)
          product-name: $(PRODUCT_NAME)
      - player-settings-android:
          package-name: com.$(COMPANY).$(PRODUCT_NAME)
          architectures: !architectures
            - "ARMv7"
            - "ARM64"
      - build:
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

