UActions
===

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

run build

> [Unity Editor command line arguments](https://docs.unity3d.com/Manual/EditorCommandLineArguments.html)

```bash
Unity.exe -batchmode -quit -buildTarget <name> -projectPath <path> -executeMethod UActions.Bootstrap.Run -job <UActions.jobName>
```
    
check [actions](./Actions.md)
