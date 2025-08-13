param(
  [String]$Audience,
  [String]$Issuer,
  [Int]$Lifetime,
  [String]$SigningSecret,
  [String]$JwtEncryptionSecret,
  [HashTable]$Claims
)

if ($PSVersionTable.PSEdition -ne 'Core') {
  throw 'Please use PowerShell Core to run this script.'
}

$dependenciesPath = "$PSScriptRoot/bin"

function InstallDependencies() {
  New-Item -Path $dependenciesPath -ItemType Directory | Out-Null
  Write-Host "Installing dependencies to $dependenciesPath."

  $packages = @(
    @{
      Name    = 'Microsoft.IdentityModel.JsonWebTokens';
      Version = '6.23.1'
    },
    @{
      Name    = 'Microsoft.IdentityModel.Tokens';
      Version = '6.23.1'
    },
    @{
      Name    = 'Microsoft.IdentityModel.Logging';
      Version = '6.23.1'
    },
    @{
      Name    = 'Microsoft.IdentityModel.Abstractions';
      Version = '6.23.1'
    }
  )

  foreach ($package in $packages) {
    Write-Host "Installing NuGet package $($package.Name).$($package.Version)."
    $folderPath = "$dependenciesPath/$($package.Name).$($package.Version)"
    $filePath = "$folderPath.nupkg"
    Invoke-WebRequest -Uri "https://www.nuget.org/api/v2/package/$($package.Name)/$($package.Version)" -OutFile $filePath
    Expand-Archive -Path $filePath -DestinationPath $folderPath
    Move-Item -Path $filePath -Destination $folderPath
  }
}

function ConvertToDictionary([Hashtable]$hashtable) {
  if ($null -eq $hashtable) {
    return $null
  }
  [System.Collections.Generic.Dictionary[string, object]] $dictionary = [System.Collections.Generic.Dictionary[string, string]]::new()
  foreach ($item in $hashtable.GetEnumerator()) {
    $dictionary.Add($item.Name, $item.Value)
  }
  $dictionary
}

if (-Not (Test-Path -Path $dependenciesPath -PathType Container)) {
  $userInput = (Read-Host -Prompt "Need to install NuGet dependencies from nuget.org. Continue? (Y/n)").Trim()
  if (($userInput -ne '') -and ($userInput -ine 'y')) {
    Exit
  }
  InstallDependencies
}

Add-Type -Path "$PSScriptRoot/bin/Microsoft.IdentityModel.Abstractions.6.23.1/lib/netstandard2.0/Microsoft.IdentityModel.Abstractions.dll"
Add-Type -Path "$PSScriptRoot/bin/Microsoft.IdentityModel.Logging.6.23.1/lib/netstandard2.0/Microsoft.IdentityModel.Logging.dll"
Add-Type -Path "$PSScriptRoot/bin/Microsoft.IdentityModel.Tokens.6.23.1/lib/netstandard2.0/Microsoft.IdentityModel.Tokens.dll"
Add-Type -Path "$PSScriptRoot/bin/Microsoft.IdentityModel.JsonWebTokens.6.23.1/lib/netstandard2.0/Microsoft.IdentityModel.JsonWebTokens.dll"

$jwtDescriptor = [Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor]::new()

[Byte[]]$SigningSecret = [System.Text.Encoding]::ASCII.GetBytes($SigningSecret)
if ($SigningSecret.Length -gt 0) {
  $jwtDescriptor.SigningCredentials = [Microsoft.IdentityModel.Tokens.SigningCredentials]::new(`
      [Microsoft.IdentityModel.Tokens.SymmetricSecurityKey]::new($SigningSecret), `
      [Microsoft.IdentityModel.Tokens.SecurityAlgorithms]::HmacSha256Signature)
}

[Byte[]]$JwtEncryptionSecret = [System.Text.Encoding]::ASCII.GetBytes($JwtEncryptionSecret)
if ($JwtEncryptionSecret.Length -gt 0) {
  $jwtDescriptor.EncryptingCredentials = [Microsoft.IdentityModel.Tokens.EncryptingCredentials]::new(`
      [Microsoft.IdentityModel.Tokens.SymmetricSecurityKey]::new($JwtEncryptionSecret), `
      [Microsoft.IdentityModel.Tokens.SecurityAlgorithms]::Aes256KW, `
      [Microsoft.IdentityModel.Tokens.SecurityAlgorithms]::Aes256CbcHmacSha512)
}

$jwtDescriptor.Expires = [System.DateTime]::Now.AddMinutes($Lifetime)
$jwtDescriptor.Claims = ConvertToDictionary $Claims
$jwtDescriptor.Issuer = $Issuer
$jwtDescriptor.Audience = $Audience

$jwtHandler = [Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler]::new()

$jwtHandler.CreateToken($jwtDescriptor)