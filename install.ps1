$ModuleName = 'Y360Management'
$Path = $Env:Programfiles+'\PowerShell\7\Modules\'+$Modulename+'\'
Copy-Item -Path $PSScriptRoot -Destination $Path -Recurse -Force