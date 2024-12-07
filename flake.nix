{
  description = "Rain world multiplyer mod";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
    flake-utils = { url = "github:numtide/flake-utils"; };

    roslyn-language-server = {
      url = "github:sofusa/roslyn-language-server";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };

  outputs = { self, nixpkgs, flake-utils, roslyn-language-server}: 
     flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs { inherit system; };

        dotnet = pkgs.dotnet-sdk;
      in
      {
        devShell = pkgs.mkShell {
          packages = [
            dotnet
            roslyn-language-server.packages.${system}.roslyn-language-server
          ];
        };
      });

}
