{
  description = "Rain world multiplyer mod";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
    flake-utils = { url = "github:numtide/flake-utils"; };
  };

  outputs = { self, nixpkgs, flake-utils}: 
     flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs { inherit system; };

        dotnet = pkgs.dotnet-sdk;
      in
      {
        devShell = pkgs.mkShell {
          packages = [
            dotnet
          ];
        };
      });
}
