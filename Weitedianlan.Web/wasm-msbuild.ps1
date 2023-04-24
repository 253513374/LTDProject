# 使用项目的解决方案文件 (.sln) 替换 <SolutionFile>
# 使用发布配置文件的名称替换 <PublishProfile1> 和 <PublishProfile2>
$projectFile  = "Wtdl.Wasm\Wtdl.Wasm.csproj"
$publishProfile1 = "Wtdl.Wasm\Properties\PublishProfiles\47.112.189.81-负载服务器B.pubxml"
$publishProfile2 = "Wtdl.Wasm\Properties\PublishProfiles\120.78.174.245-负载服务器A.pubxml"

# 获取 Visual Studio 2022 msbuild 路径
$msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"

# 发布到第一个 IIS 服务器
& $msbuildPath $solutionFile /p:DeployOnBuild=true /p:PublishProfile=$publishProfile1

# 发布到第二个 IIS 服务器
& $msbuildPath $solutionFile /p:DeployOnBuild=true /p:PublishProfile=$publishProfile2

Write-Host "发布完成"
