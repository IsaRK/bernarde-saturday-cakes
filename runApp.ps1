$p = Start-Process -FilePath "BernardeSaturdayCakes.exe" -WorkingDirectory "C:\Users\Isabelle\source\repos\BernardeSaturdayCakes\BernardeSaturdayCakes\bin\Release\netcoreapp3.1"  -Wait -PassThru

if($p.ExitCode -ne 0)
{
    throw "Error Code returned: $($p.ExitCode)"
}

Read-Host -Prompt "Press Enter to exit"