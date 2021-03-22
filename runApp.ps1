$p = Start-Process -FilePath ".\BernardeSaturdayCakes\bin\Release\netcoreapp3.1\BernardeSaturdayCakes.exe" -Wait -PassThru

if($p.ExitCode -ne 0)
{
    throw "Error Code returned: $($p.ExitCode)"
}

Read-Host -Prompt "Press Enter to exit"