$buildNumber = Read-Host -Prompt 'Build number'
$output = "c:\nugetlocal"

& dotnet pack StoryTeller.Portal.ResultsAggregator --version-suffix alpha-$buildNumber --output $output
& dotnet pack StoryTeller.Portal.ResultsAggregator.Client --version-suffix alpha-$buildNumber --output $output
& dotnet pack StoryTeller.ResultAggregation.Models --version-suffix alpha-$buildNumber --output $output