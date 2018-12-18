$version = "1.0.1"


$today = Get-Date
$date = $today.ToString('yyyy-MM-dd')
Remove-Item ./bin/* -Force -Recurse

Set-Location ./ScorpioIni
$fileData = @"
namespace Scorpio.Ini {
    public static class Version {
        public const string version = "$version";
        public const string date = "$date";
    }
}
"@
$fileData | Out-File -Encoding utf8 ./src/Version.cs
dotnet restore
dotnet build
dotnet pack -p:PackageVersion=$version -o ../bin

Set-Location ../
